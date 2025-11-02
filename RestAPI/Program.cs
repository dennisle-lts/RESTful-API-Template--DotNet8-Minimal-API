using RestAPI.Database;
using RestAPI.Repositories.Implementations;
using RestAPI.Repositories.Interfaces;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        // Call a repository method
        var users = await repo.GetAllAsync(10, 0);
        users.ForEach(user =>
        {
            Console.WriteLine(user.Id);
        });
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

app.UseHttpsRedirection();

app.Run();
