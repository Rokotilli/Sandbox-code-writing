namespace TestProjectForDCT;

public class Config
{
    public HackerEarthConfig HackerEarth { get; set; }
    public LeetCodeConfig LeetCode { get; set; }
    public string ApplicationTheme { get; set; }
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
