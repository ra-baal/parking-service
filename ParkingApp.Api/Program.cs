using ParkingApp.Infrastructure.RavenDb;
using ParkingApp.Infrastructure.Repositories;
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

builder.Services.AddScoped<PaymentCalculator>();
builder.Services.AddScoped<CurrencyConverter>();

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

app.MapControllers();

app.Run();

static void ConfigureRavenDb(IServiceCollection services, IConfiguration configuration)
{
    RavenDbSettings settings = configuration
        .GetSection("RavenDb")
        .Get<RavenDbSettings>() ?? throw new InvalidOperationException("No RavenDB settings");

    services.AddSingleton(settings);
    services.AddSingleton<RavenDbFactory>();

    services.AddSingleton<IDocumentStore>(serviceProvider =>
    {
        var factory = serviceProvider.GetRequiredService<RavenDbFactory>();
        return factory.Create();
    });

    services.AddScoped<IParkingAreaRepository, ParkingAreaRepository>();
}

static void ConfigureExchangeRatesApiClient(IServiceCollection services, ConfigurationManager configuration)
{
    ExchangeRatesApiSettings settings = configuration
        .GetSection("ExchangeRatesApi")
        .Get<ExchangeRatesApiSettings>() ?? throw new InvalidOperationException("Missing settings");

    services.AddSingleton(settings);
    services.AddHttpClient<ICurrencyRateProvider, CurrencyRateProvider>();
}
