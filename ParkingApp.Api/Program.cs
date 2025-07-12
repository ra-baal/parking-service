using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkingApp.Infrastructure.RavenDb;
using ParkingApp.Infrastructure.Repositories;
using Raven.Client.Documents;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureRavenDb(builder.Services, builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add endpoints/controllers here

app.Run();

static void ConfigureRavenDb(IServiceCollection services, IConfiguration configuration)
{
    string ravenUrl = configuration["RavenDb:Url"] ?? "http://localhost:8080";
    string databaseName = configuration["RavenDb:Database"] ?? "ParkingDb";

    IDocumentStore documentStore = RavenDbFactory.Create(ravenUrl, databaseName);
    RavenDbInitializer.EnsureDatabaseExists(documentStore, databaseName);

    services.AddSingleton<IDocumentStore>(documentStore);
    services.AddScoped<IParkingAreaRepository, ParkingAreaRepository>();
}
