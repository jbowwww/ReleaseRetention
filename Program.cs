
using System.Text.Json;
using System.Text.Json.Serialization;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var projects = JsonSerializer.Deserialize<Project[]>(JsonDocument.Parse(File.ReadAllBytes("./Projects.json")));
Console.WriteLine($"Projects ({projects?.Length}): {string.Join("\n\t", projects?.Select(p => $"Project: Id=\"{p.Id}\" Name=\"{p.Name}\""))}");

class Project
{
    [JsonInclude]
    public string Id;
    [JsonInclude]
    public string Name;
}