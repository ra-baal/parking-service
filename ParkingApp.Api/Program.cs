using ParkingApp.Infrastructure.RavenDb;
using ParkingApp.Infrastructure.Repositories;
using ParkingApp.Infrastructure.Raven;
using Raven.Client.Documents;
using ParkingApp.Domain.Services;
using ParkingApp.Infrastructure.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureRavenDb(builder.Services, builder.Configuration);
ConfigureExchangeRatesApiClient(builder.Services, builder.Configuration);

// Kontrolery.
builder.Services.AddControllers();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}
else
{
    app.UseCors();
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

static void ConfigureExchangeRatesApiClient(IServiceCollection services, ConfigurationManager configuration)
{
    ExchangeRatesApiSettings settings = configuration
        .GetSection("ExchangeRatesApi")
        .Get<ExchangeRatesApiSettings>() ?? throw new InvalidOperationException("Missing settings");

    services.AddSingleton(settings);
    services.AddHttpClient<ICurrencyConverter, CurrencyConverter>();
}
