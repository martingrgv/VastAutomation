using System.Diagnostics;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton<Orchestrator>();

builder.Services.Configure<RpaAgentOptions>(
        builder.Configuration.GetSection(nameof(RpaAgentOptions)));

var app = builder.Build();

app.MapPost("/startAutomation/{id}", (int id, Orchestrator orchestrator) =>
{
    var executedAgent = orchestrator.ExecuteAutomationAgent(id);
    return executedAgent ? Results.Ok(id) : Results.BadRequest();
});

app.Run("http://localhost:5001");

public class Orchestrator(ILogger<Orchestrator> logger, IOptions<RpaAgentOptions> options)
{
    public bool ExecuteAutomationAgent(int id)
    {
        logger.LogInformation("Starting automation agent {id}", id);

        var processInfo = new ProcessStartInfo
        {
            FileName = options.Value.FilePath,
            Arguments = options.Value.Args,
            RedirectStandardInput = false,
            RedirectStandardError = false,
            UseShellExecute = false
        };
        var process = Process.Start(processInfo);

        return process is not null;
    }
}

public class RpaAgentOptions
{
    public string FilePath { get; set; } = null!;
    public string Args { get; set; } = null!;
}
