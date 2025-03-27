using System.Windows.Documents;
using System.Windows.Input;

namespace TestProjectForDCT.ViewModels.Core.Interfaces;

public interface IDetailsProblemViewModel
{
    FlowDocument ProblemDescription { get; set; }
    string HtmlContent { get; set; }
    ICommand NavigateLeetCodeProblemsView { get; set; }
}
