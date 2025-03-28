using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SandboxTestProject.Models.LeetCodeModels;
using SandboxTestProject.Services.Interfaces;

namespace SandboxTestProject.Services;

public class LeetCodeService : ILeetCodeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ILeetCodeService> _logger;

    public LeetCodeService(IHttpClientFactory httpClientFactory, IOptions<Config> config, ILogger<ILeetCodeService> logger)
    {
        _httpClient = httpClientFactory.CreateClient(config.Value.LeetCode.httpClientName);
        _logger = logger;
    }

    public async Task<GetProblemsModel> GetProblemsAsync()
    {
        _logger.LogInformation("Getting problems from LeetCode");

        var response = await _httpClient.GetAsync("api/problems/all/");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed getting problems from LeetCode: " + response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();

        var problems = JsonConvert.DeserializeObject<GetProblemsModel>(content);

        _logger.LogInformation("Problems received successfully");

        return problems;
    }

    public async Task<DetailsProblemModel> GetDetailsProblemAsync(string ProblemSlug, string SessionToken, string CsrfToken)
    {
        _logger.LogInformation($"Getting details of problem with slug {ProblemSlug} from LeetCode");

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

        _logger.LogInformation($"Details of problem with id {details.data.question.questionId} received successfully");

        return details;
    }
}
