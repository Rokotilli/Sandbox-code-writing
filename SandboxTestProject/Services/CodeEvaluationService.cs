using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using SandboxTestProject.Helpers;
using SandboxTestProject.Models.HackerearthModels;
using SandboxTestProject.Services.Interfaces;

namespace SandboxTestProject.ViewModels;

public class CodeEvaluationService : ICodeEvaluationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ICodeEvaluationService> _logger;

    public CodeEvaluationService(IHttpClientFactory httpClientFactory, IOptions<Config> config, ILogger<ICodeEvaluationService> logger)
    {
        _httpClient = httpClientFactory.CreateClient(config.Value.HackerEarth.httpClientName);
        _logger = logger;
    }

    public async Task<ResponseCodeEvaluationModel> SendCodeEvaluation(PostCodeEvaluationModel model)
    {
        _logger.LogInformation("Sending code evaluation");

        var data = model.ToDictionary();

        var content = new FormUrlEncodedContent(data);

        var response = await _httpClient.PostAsync("", content);

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending code evaluation: " + response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ResponseCodeEvaluationModel>(responseContent);

        _logger.LogInformation("Code evaluation sent successfully");

        return result;
    }

    public async Task<ResultCodeEvaluationModel> GetResultCodeEvaluation(string he_id)
    {
        _logger.LogInformation("Getting code evaluation result");

        var response = await _httpClient.GetAsync(he_id);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error getting code evaluation result: " + response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ResultCodeEvaluationModel>(responseContent);

        _logger.LogInformation("Code evaluation result received successfully");

        return result;
    }

    public async Task<string> GetOutputStringFromFile(string url)
    {
        _logger.LogInformation("Getting output string from file");

        using HttpClient client = new HttpClient();
            
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error getting output string from file: " + response.StatusCode);
        }

        var result = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("Output string received successfully");

        return result;            
    }
}
