using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using Shiron.HonamiCore.EFCore.Entities;

namespace Shiron.HonamiCore;

public class HonamiApp(WebApplication app, IReadOnlyDictionary<Type, IAppBuilder[]> appBuilders) {
    public readonly WebApplication AppHandle = app;

    private static readonly string[] GetVerb = [
        HttpMethods.Get
    ];
    private static readonly string[] PostVerb = [
        HttpMethods.Post
    ];
    private static readonly string[] PutVerb = [
        HttpMethods.Put
    ];
    private static readonly string[] DeleteVerb = [
        HttpMethods.Delete
    ];
    private static readonly string[] PatchVerb = [
        HttpMethods.Patch
    ];

    public static HonamiBuilder CreateBuilder(string name, string[]? args = null) {
        return args == null
            ? new HonamiBuilder(name, WebApplication.CreateBuilder())
            : new HonamiBuilder(name, WebApplication.CreateBuilder(args));
    }
    public void MapApiReference() {
        var builders = appBuilders[typeof(HonamiReferenceBuilder)];
        foreach (var builder in builders) {
            builder.Process(AppHandle);
        }
    }

    public RouteGroupBuilder MapGroup([StringSyntax("Route")] string prefix) {
        return app.MapGroup(prefix);
    }
    public RouteGroupBuilder MapGroup(RoutePattern prefix) {
        return app.MapGroup(prefix);
    }
    public RouteHandlerBuilder MapGet([StringSyntax("Route")] string pattern, Delegate handler) {
        return app.MapMethods(pattern, GetVerb, handler);
    }
    public RouteHandlerBuilder MapPost([StringSyntax("Route")] string pattern, Delegate handler) {
        return app.MapMethods(pattern, PostVerb, handler);
    }
    public RouteHandlerBuilder MapPut([StringSyntax("Route")] string pattern, Delegate handler) {
        return app.MapMethods(pattern, PutVerb, handler);
    }
    public RouteHandlerBuilder MapDelete([StringSyntax("Route")] string pattern, Delegate handler) {
        return app.MapMethods(pattern, DeleteVerb, handler);
    }
    public RouteHandlerBuilder MapPatch([StringSyntax("Route")] string pattern, Delegate handler) {
        return app.MapMethods(pattern, PatchVerb, handler);
    }

    public void Run([StringSyntax("Uri")] string? url = null) {
        app.Run(url);
    }
}
