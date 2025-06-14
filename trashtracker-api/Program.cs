using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using trashtracker_api.Repositories;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
// Add services to the container.
var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<Program>();

// Gets SQL connection string from the user secrets
var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

if (string.IsNullOrWhiteSpace(sqlConnectionString))
{
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
}

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services
    .AddIdentityApiEndpoints<IdentityUser>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = sqlConnectionString;
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Adding Swagger Service for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapGet("/", () => "Hello World! The API is up and everything works great!! :D");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
