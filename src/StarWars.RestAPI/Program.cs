using FluentValidation.AspNetCore;
using StarWars.Infra.Data;
using StarWars.RestAPI;
using StarWars.RestAPI.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapperSetup();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

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
