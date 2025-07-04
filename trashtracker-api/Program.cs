﻿using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Data;
using trashtracker_api.Data;
using trashtracker_api.Repositories;
using trashtracker_api.Repositories.interfaces;
using trashtracker_api.Repositories.Interfaces;
using DotNetEnv;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        // Laad .env variabelen
        Env.Load();

        // Haal SQL connection string uit environment
        var sqlConnectionString = builder.Configuration.GetConnectionString("SQL_CONNECTION_STRING");
        var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

        if (!sqlConnectionStringFound)
        {
            throw new InvalidProgramException("Configuration variable SQL_CONNECTION_STRING not found");
        }

        // Logger setup
        var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<Program>();
       
        if (string.IsNullOrWhiteSpace(sqlConnectionString))
        {
            throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
        }

        // Register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ILitterRepository, LitterRepository>();
        builder.Services.AddScoped<IFavoriteLocationsRepository, FavoriteLocationsRepository>();
        builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();

        // Authorization - Identity API with Dapper Stores
        builder.Services.AddAuthorization();
        builder.Services
            .AddIdentityApiEndpoints<IdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
            })
            .AddDapperStores(options =>
            {
                options.ConnectionString = sqlConnectionString;
            });

        builder.Services
            .AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
            .Configure(options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
            });

        // Controllers
        builder.Services.AddControllers();

        // Swagger
        builder.Services.AddEndpointsApiExplorer(); 
        builder.Services.AddSwaggerGen();

        // Register direct DB connection
        builder.Services.AddScoped<IDbConnection>(sp =>
        {
            logger.LogInformation("🔗 Attempting to create a database connection...");
            return new SqlConnection(sqlConnectionString);
        });

        // Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

        var app = builder.Build();

        app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapScalarApiReference();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map default Identity endpoints under /auth.
        app.MapGroup("/auth")
            .MapIdentityApi<IdentityUser>();

        app.MapPost("/auth/logout",
            async (SignInManager<IdentityUser> signInManager,
            [FromBody] object empty) =>
            {
                if (empty != null)
                {
                    await signInManager.SignOutAsync();
                    return Results.Ok();
                }
                return Results.Unauthorized();
            })
            .RequireAuthorization();

        app.MapControllers()
            .RequireAuthorization();

        app.Run();
    }
}
