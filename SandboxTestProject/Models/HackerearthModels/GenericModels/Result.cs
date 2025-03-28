namespace SandboxTestProject.Models.HackerearthModels.GenericModels;

public class Result
{
    public RunStatus run_status { get; set; }
    public string compile_status { get; set; }    
}

public class RunStatus
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