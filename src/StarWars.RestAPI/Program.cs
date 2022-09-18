using StarWars.Infra.Data;
using StarWars.Infra.IoC.RestAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IoC registers
IoCWrapper.Register(builder.Services, builder.Configuration);

var app = builder.Build();

// Apply EF Migrations
await MigrationManager.ApplyMigrationAsync(app.Services, builder.Configuration);

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
