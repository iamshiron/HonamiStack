using DotNetEnv;
using Scalar.AspNetCore;
using Shiron.HonamiCore;
using Shiron.HonamiCore.Sandbox.DB;
using Shiron.HonamiCore.Sandbox.DB.Schema;

Env.TraversePath().Load();
var builder = HonamiApp.CreateBuilder("Sandbox API", args);
builder.AddPostgres<SandboxDb>("SANDBOX");
builder.AddIdentity<User, Guid, SandboxDb>()
    .ConfigureCookie()
    .DisablePasswordRules()
    .RequireUniqueEmail();

builder.AddMinIO("MINIO")
    .UseSsl(false);

// Adds the reference API to the app
// This will NOT map the endpoints, it just configures the reference API
builder.AddApiReference()
    .SetRoute("/api/v1")
    .SetTheme(ScalarTheme.Purple);

builder.Services.AddOpenApi();

var app = builder.Build();

// WIP: This will later not be required but is a current workaround to shorten calls to the actual web app
var handle = app.AppHandle;

if (handle.Environment.IsDevelopment()) {
    handle.MapOpenApi();

    // This explicitly maps the reference API to the app
    app.MapApiReference();
}

handle.MapGet("/health", () => new { Status = "Ok" });
handle.Run();
