using SandboxTestProject.Models.HackerearthModels.GenericModels;

namespace SandboxTestProject.Models.HackerearthModels;

public class ResultCodeEvaluationModel
{
    public RequestStatus request_status { get; set; }
    public string he_id { get; set; }
    public Result result { get; set; }
    public string status_update_url { get; set; }
    public string context { get; set; }
}
