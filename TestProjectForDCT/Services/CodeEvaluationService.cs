using Newtonsoft.Json;
using System.Net.Http;
using TestProjectForDCT.Helpers;
using TestProjectForDCT.Models.HackerearthModels;

namespace TestProjectForDCT.ViewModels;

public class CodeEvaluationService
{
    private readonly HttpClient httpClient;

    public CodeEvaluationService(IHttpClientFactory httpClientFactory, Config config)
    {
        httpClient = httpClientFactory.CreateClient(config.httpClientHackerearthAPIName);
    }

    public async Task<ResponseCodeEvaluationModel> SendCodeEvaluation(PostCodeEvaluationModel model)
    {
        var data = model.ToDictionary();

        var content = new FormUrlEncodedContent(data);

        var response = await httpClient.PostAsync("", content);

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception("Error sending code evaluation: " + response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ResponseCodeEvaluationModel>(responseContent);

        return result;
    }

    public async Task<ResultCodeEvaluationModel> GetResultCodeEvaluation(string he_id)
    {
        var response = await httpClient.GetAsync(he_id);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error getting code evaluation result: " + response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ResultCodeEvaluationModel>(responseContent);

        return result;
    }
}
