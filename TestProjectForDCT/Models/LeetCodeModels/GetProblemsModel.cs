namespace TestProjectForDCT.Models.LeetCodeModels;

public class GetProblemsModel
{
    public string user_name { get; set; }
    public string num_solved { get; set; }
    public string num_total { get; set; }
    public string ac_easy { get; set; }
    public string ac_medium { get; set; }
    public string ac_hard { get; set; }
    public List<StatStatusPairs> stat_status_pairs { get; set; }
}

public class StatStatusPairs
{
    public Stat stat { get; set; }
    public string status { get; set; }
    public Difficulty difficulty { get; set; }
    public string paid_only { get; set; }
    public string is_favor { get; set; }
    public string frequency { get; set; }
    public string progress { get; set; }
}

public class Stat
{
    public string question_id { get; set; }
    public string question__article__live { get; set; }
    public string question__article__slug { get; set; }
    public string question__article__has_video_solution { get; set; }
    public string question__title { get; set; }
    public string question__title_slug { get; set; }
    public string question__hide { get; set; }
    public string total_acs { get; set; }
    public string total_submitted { get; set; }
    public string frontend_question_id { get; set; }
    public string is_new_question { get; set; }
}

public class Difficulty
{
    public string level { get; set; }
}