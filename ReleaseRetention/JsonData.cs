using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonData
{
    public static IEnumerable<T> Read<T>(string path, params JsonConverter[] converters)
        where T : class
    {
        var options = new JsonSerializerOptions();
        foreach (var c in converters)
        {
            options.Converters.Add(c);
        }
        return JsonSerializer.Deserialize<T[]>(File.ReadAllBytes(path), options) ?? [];
    }
}
