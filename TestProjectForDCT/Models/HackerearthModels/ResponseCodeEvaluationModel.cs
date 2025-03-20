using TestProjectForDCT.Models.HackerearthModels.GenericModels;

namespace TestProjectForDCT.Models.HackerearthModels;

public class ResponseCodeEvaluationModel
{
    public RequestStatus request_status { get; set; }
    public string he_id { get; set; }
    public Result result { get; set; }
    public string context { get; set; }
    public string status_update_url { get; set; }
}