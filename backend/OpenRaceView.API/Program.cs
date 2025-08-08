using Microsoft.EntityFrameworkCore;
using OpenRaceView.Infrastructure.Data;
using OpenRaceView.Application.Commands.Laps;
using MediatR;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Data Source=laps.db"));

// Configure MediatR
builder.Services.AddMediatR(typeof(CreateLapCommand).Assembly);

// Configure options
builder.Services.Configure<TelemetryOptions>(
    builder.Configuration.GetSection("Telemetry"));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Auto-migrate database in development
    using IServiceScope scope = app.Services.CreateScope();
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
