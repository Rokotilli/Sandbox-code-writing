using ICSharpCode.AvalonEdit.Highlighting;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TestProjectForDCT.ViewModels.Core.Interfaces;

public interface ISandBoxViewModel
{
    string CodeText { get; set; }
    string MemoryLimit { get; set; }
    string TimeLimit { get; set; }
    string ResultText { get; set; }
    string SelectedLanguage { get; set; }
    IHighlightingDefinition SyntaxHighlighting { get; set; }
    bool IsCheckStatusButtonEnabled { get; set; }
    ObservableCollection<string> Languages { get; }

    ICommand RunCodeCommand { get; }
    ICommand CheckCodeEvaluationCommand { get; }
    ICommand OpenLeetCodeProblemsCommand { get; }
    ICommand NavigateToHomeViewCommand { get; set; }

    void UpdateSyntaxHighlighting();
}
