using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
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
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

if (string.IsNullOrWhiteSpace(sqlConnectionString))
{
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
}

// Identity API with Dapper Stores
//builder.Services
//    .AddIdentityApiEndpoints<IdentityUser>()
//    .AddDapperStores(options =>
//    {
//        options.ConnectionString = sqlConnectionString;
//    });

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();

// Register direct DB connection
builder.Services.AddScoped<IDbConnection>(sp =>
{
    logger.LogInformation("🔗 Attempting to create a database connection...");
    return new SqlConnection(sqlConnectionString);
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILitterRepository, LitterRepository>();
builder.Services.AddScoped<IFavoriteLocationsRepository, FavoriteLocationsRepository>();

var app = builder.Build();

// Map default Identity endpoints under /auth.
//app.MapGroup("/auth").MapIdentityApi<IdentityUser>();

app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
