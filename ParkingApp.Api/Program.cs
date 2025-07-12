using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkingApp.Infrastructure.RavenDb;
using ParkingApp.Infrastructure.Repositories;
using ParkingApp.Infrastructure.Raven;
using Raven.Client.Documents;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureRavenDb(builder.Services, builder.Configuration);

// Kontrolery.
builder.Services.AddControllers();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Kontrolery.
app.MapControllers();

app.Run();

static void ConfigureRavenDb(IServiceCollection services, IConfiguration configuration)
{
    RavenDbSettings settings = configuration
        .GetSection("RavenDb")
        .Get<RavenDbSettings>() ?? throw new InvalidOperationException("Brak ustawieñ RavenDb.");

    services.AddSingleton(settings);
    services.AddSingleton<RavenDbFactory>();

    services.AddSingleton<IDocumentStore>(serviceProvider =>
    {
        var factory = serviceProvider.GetRequiredService<RavenDbFactory>();
        return factory.Create();
    });

    services.AddScoped<IParkingAreaRepository, ParkingAreaRepository>();
    services.AddHostedService<RavenDbStartupService>();
}
