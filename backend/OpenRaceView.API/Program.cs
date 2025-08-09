using Microsoft.EntityFrameworkCore;
using OpenRaceView.Infrastructure.Data;
using OpenRaceView.API.Configuration;
using System.Reflection;
using Scalar.AspNetCore;
using FastEndpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Data Source=laps.db"));

// Configure options
builder.Services.Configure<TelemetryOptions>(
    builder.Configuration.GetSection("Telemetry"));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    
    // Auto-migrate database in development
    using IServiceScope scope = app.Services.CreateScope();
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();
