using TestProjectForDCT.Models.HackerearthModels.GenericModels;

namespace TestProjectForDCT.Models.HackerearthModels;

class ResultCodeEvaluationModel
{
    public RequestStatus request_status { get; set; }
    public string he_id { get; set; }
    public string status_update_url { get; set; }
    public string context { get; set; }
}
