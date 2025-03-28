using SandboxTestProject.Models.LeetCodeModels;

namespace SandboxTestProject.Services.Interfaces;

public interface ILeetCodeService
{
    Task<GetProblemsModel> GetProblemsAsync();
    Task<DetailsProblemModel> GetDetailsProblemAsync(string problemSlug, string sessionToken, string csrfToken);
}
