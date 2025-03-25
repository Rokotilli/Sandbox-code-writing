using System.IO;
using System.Text.Json;

namespace TestProjectForDCT;

public class Config
{
    public HackerEarthConfig HackerEarth { get; set; }
    public LeetCodeConfig LeetCode { get; set; }
    public string ApplicationTheme { get; set; }
    public string ApplicationLanguage { get; set; }

    public void SaveConfig()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(AppContext.BaseDirectory + "appconfig.json", json);
    }
}

public class HackerEarthConfig
{
    public string client_secret { get; set; }
    public string url { get; set; }
    public string httpClientName { get; set; }
}

public class LeetCodeConfig
{
    public string url { get; set; }
    public string httpClientName { get; set; }
    public string session_token { get; set; }
    public string csrf_token { get; set; }
}
