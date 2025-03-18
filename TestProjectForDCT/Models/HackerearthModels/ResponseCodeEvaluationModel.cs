using TestProjectForDCT.Models.HackerearthModels.GenericModels;

namespace TestProjectForDCT.Models.HackerearthModels;

class ResponseCodeEvaluationModel
{
    public RequestStatus request_status { get; set; }
    public string he_id { get; set; }
    public Result result { get; set; }
    public string context { get; set; }
    public string status_update_url { get; set; }
}

class Result
{
    public RunStatus run_status { get; set; }
    public string compile_status { get; set; }
}

class RunStatus
{
    public string memory_used { get; set; }
    public string status { get; set; }
    public string time_used { get; set; }
    public string signal { get; set; }
    public string exit_code { get; set; }
    public string status_detail { get; set; }
    public string stderr { get; set; }
    public string output { get; set; }
    public string request_NOT_OK_reason { get; set; }
    public string request_OK { get; set; }
}
