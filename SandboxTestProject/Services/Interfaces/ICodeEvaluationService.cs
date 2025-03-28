using SandboxTestProject.Models.HackerearthModels;

namespace SandboxTestProject.Services.Interfaces;

public interface ICodeEvaluationService
{
    Task<ResponseCodeEvaluationModel> SendCodeEvaluation(PostCodeEvaluationModel model);
    Task<ResultCodeEvaluationModel> GetResultCodeEvaluation(string he_id);
    Task<string> GetOutputStringFromFile(string url);
}
