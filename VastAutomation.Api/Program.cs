using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<RpaOrchestratorOptions>(
    builder.Configuration.GetSection(nameof(RpaOrchestratorOptions)));

var app = builder.Build();

app.MapPost("api/v1/automation/{id}", async (int id, IHttpClientFactory httpClientFactory, IOptions<RpaOrchestratorOptions> options) =>
{
    var client = httpClientFactory.CreateClient();
    var uri = new Uri($"{options.Value.Host}/startAutomation/{id}");

    var response = await client.PostAsync(uri, null);

    return response.IsSuccessStatusCode ? Results.Accepted() : Results.BadRequest();
});

app.Run("http://localhost:5000");

public class RpaOrchestratorOptions()
{
    public string Host { get; set; } = null!;
}
