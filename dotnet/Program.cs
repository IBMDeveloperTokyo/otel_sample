using System;
using System.Threading;
using System.Globalization;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "otel-dojo-dotnet";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
	.AddOtlpExporter()
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
	  .AddOtlpExporter()
          .AddConsoleExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
	  .AddOtlpExporter()
          .AddConsoleExporter());

var app = builder.Build();

string HandleRollDice(string? player)
{
    var rand = new System.Random();
    var result = rand.Next(1, 7);
    Thread.Sleep(result * 100);

    if (string.IsNullOrEmpty(player))
    {
        app.Logger.LogInformation("Anonymous player is rolling the dice: {result}", result);
    }
    else
    {
        app.Logger.LogInformation("{player} is rolling the dice: {result}", player, result);
    }

    if (result == 2)
    {
	app.Logger.LogError("2なのでErrorです");
	throw new Exception("2の時は例外を投げます");
    }

    return result.ToString(CultureInfo.InvariantCulture);
}

app.MapGet("/rolldice/{player?}", HandleRollDice);

app.Run();
