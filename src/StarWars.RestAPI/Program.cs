using FluentValidation.AspNetCore;
using NLog;
using NLog.Web;
using StarWars.Infra.Data;
using StarWars.RestAPI;
using StarWars.RestAPI.AutoMapper;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapperSetup();
    builder.Services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // IoC registers
    DependencyInjectorStartup.Register(builder.Services, builder.Configuration);

    var app = builder.Build();

    // Apply EF Migrations
    await MigrationManager.ApplyMigrationAsync(app.Services);

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
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}