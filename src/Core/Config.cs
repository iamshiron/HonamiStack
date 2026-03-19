using System.Text.Json.Serialization;

namespace Shiron.HonamiStack.Core;

public record Config(string Name, List<Module> Modules);

[JsonDerivedType(typeof(BackendModule))]
[JsonDerivedType(typeof(FrontendModule))]
public abstract class Module(string name, string location) {
    public string Name { get; } = name;
    public string Location { get; } = location;
}

public class BackendModule(string name, string location) : Module(name, location) {
}

public class FrontendModule(string name, string location) : Module(name, location) {
}
