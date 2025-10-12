using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Hubs;
using server.Models.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServerRepository, ServerRepository>();

var connString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BlackjackContext>(options =>
    options.UseNpgsql(connString));

builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 10;
});
builder.Services.AddSingleton<IServerCache, ServerCache>();

var clientUrl = builder.Configuration["Frontend"]
    ?? throw new InvalidOperationException("Frontend url was not found");

builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy", b =>
        b.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(clientUrl)));

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    using var db = scope.ServiceProvider.GetRequiredService<BlackjackContext>();
//    db.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.MapHub<GameHub>("/game");

app.Run();
