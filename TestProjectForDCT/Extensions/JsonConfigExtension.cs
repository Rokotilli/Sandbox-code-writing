using System.IO;
using System.Text.Json;

namespace TestProjectForDCT.Extensions;

public static class JsonConfigExtension
{
    public static void SaveConfig(this Config config)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(config, options);
        File.WriteAllText(AppContext.BaseDirectory + "appconfig.json", json);
    }
}