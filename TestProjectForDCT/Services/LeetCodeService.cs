using Newtonsoft.Json;
using System.Net.Http;
using TestProjectForDCT.Models.LeetCodeModels;

namespace TestProjectForDCT.Services;

public class LeetCodeService
{
    private readonly HttpClient _httpClient;

    public LeetCodeService(IHttpClientFactory httpClientFactory, Config config)
    {
        _httpClient = httpClientFactory.CreateClient(config.httpClientLeetCodeAPIName);
    }

    public async Task<GetProblemsModel> GetProblemsAsync()
    {
        var response = await _httpClient.GetAsync("api/problems/all/");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed getting problems from LeetCode: " + response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();

        var problems = JsonConvert.DeserializeObject<GetProblemsModel>(content);

        return problems;
    }
}
