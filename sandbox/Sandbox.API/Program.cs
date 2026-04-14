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

var route = app.MapGroup("/api");
route.MapGet("/health", () => new { Status = "Ok" });
route.MapPost("/get/{id:int}", (int id) => $"Hello {id}");
route.MapPut("/update/{id:int}", (int id) => $"Updated {id}");
route.MapDelete("/delete/{id:int}", (int id) => $"Deleted {id}");
route.MapPatch("/patch/{id:int}", (int id) => $"Patched {id}");
app.Run();
