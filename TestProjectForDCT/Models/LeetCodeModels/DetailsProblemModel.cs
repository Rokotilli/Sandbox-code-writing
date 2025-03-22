namespace TestProjectForDCT.Models.LeetCodeModels;

public class DetailsProblemModel
{
    public Data data { get; set; }
}

public class Data
{
    public Question question { get; set; }
}

public class Question
{
    public string questionId { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public string difficulty { get; set; }
}
