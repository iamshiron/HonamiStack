using DotNetEnv;
using Scalar.AspNetCore;
using Shiron.HonamiCore;
using Shiron.HonamiCore.Sandbox.DB;
using Shiron.HonamiCore.Sandbox.DB.Schema;

Env.TraversePath().Load();
var builder = HonamiApp.CreateBuilder<User, Guid, SandboxDb>("Sandbox API", args);
builder.AddIdentity()
    .ConfigureCookie()
    .DisablePasswordRules()
    .RequireUniqueEmail();

builder.AddPostgres("SANDBOX");
builder.AddReference()
    .SetRoute("/api/v1")
    .SetTheme(ScalarTheme.Purple);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[] {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () => {
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
})
    .WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}
