using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using Warren.ContaCorrente.IoC;
using Warren.ContaCorrente.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDependecies(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(j =>
    {
        j.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddApiVersioning(config =>
{
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.DefaultApiVersion = new ApiVersion(DateTime.UtcNow, 1, 0);
});

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
.AddFluentMigratorCore()
        .ConfigureRunner(c => c.AddMySql5()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MYSQL_CONTACORRENTE"))
            .ScanIn(Assembly.GetAssembly(typeof(MigrationManager))).For.Migrations());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Esta api é responsável pela movimentação na conta corrente do cliente.",
        Version = "v1",

    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase().Run();
