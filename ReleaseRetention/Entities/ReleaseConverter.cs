using System.Text.Json;
using System.Text.Json.Serialization;

public class ReleaseConverter : JsonConverter<Release>
{
    private readonly static JsonConverter<Release> _defaultConverter = 
        (JsonConverter<Release>)JsonSerializerOptions.Default.GetConverter(typeof(Release));

    public override Release? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var release = JsonSerializer.Deserialize<Release>(ref reader, new JsonSerializerOptions());
        if (release != null)
        {
            release.Project = DataContext.Projects.FirstOrDefault(p => p.Id == release.ProjectId);
        }
        return release;
    }

    public override void Write(Utf8JsonWriter writer, Release value, JsonSerializerOptions options)
    {
        _defaultConverter.Write(writer, value, options);
    }
}