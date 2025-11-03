using Microsoft.AspNetCore.Diagnostics;
using RestAPI.Endpoints.User;
using RestAPI.Infrastructure.Database;
using RestAPI.Repositories.Implementations;
using RestAPI.Repositories.Interfaces;
using RestAPI.Services.Implementations;
using RestAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory>(sp => new DbConnectionFactory(
    builder.Configuration.GetConnectionString("DefaultConnection")!
));

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services
builder.Services.AddScoped<IUserService, UserService>();

// Add problem details and global exception handler
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add problem details middleware
app.UseExceptionHandler();

// Add endpoints
app.MapUserEndpoints();

app.Run();
