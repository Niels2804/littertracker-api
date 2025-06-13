var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Gets SQL connection string from the user secrets
var sqlConnectionString = builder.Configuration["DefaultConnectionString"];

if (string.IsNullOrWhiteSpace(sqlConnectionString))
{
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
}

// builder.Services.AddTransient<IUserRepository>(o => new UserRepository(sqlConnectionString)); YOU NEED THIS LATER

builder.Services.AddControllers();

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
