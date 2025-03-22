using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TestProjectForDCT.Models.LeetCodeModels;

namespace TestProjectForDCT.Services;

public class LeetCodeService
{
    private readonly HttpClient _httpClient;

    public LeetCodeService(IHttpClientFactory httpClientFactory, Config config)
    {
        _httpClient = httpClientFactory.CreateClient(config.LeetCode.httpClientName);
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

    public async Task<DetailsProblemModel> GetDetailsProblemAsync(string ProblemSlug, string SessionToken, string CsrfToken)
    {
        _httpClient.DefaultRequestHeaders.Add("cookie", $"{SessionToken}; csrftoken={CsrfToken}");
        _httpClient.DefaultRequestHeaders.Add("x-csrftoken", CsrfToken);

        var queryObj = new
        {
            operationName = "questionData",
            variables = new { titleSlug = ProblemSlug },
            query = @"
                query questionData($titleSlug: String!) {
                    question(titleSlug: $titleSlug) {
                        questionId
                        title
                        content
                        difficulty
                    }
                }
            "
        };

        var json = JsonConvert.SerializeObject(queryObj);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("graphql/", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed getting details of problem from LeetCode: " + response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var details = JsonConvert.DeserializeObject<DetailsProblemModel>(responseContent);

        return details;
    }
}
