using System.Collections.ObjectModel;
using System.Windows.Input;
using SandboxTestProject.Models.LeetCodeModels;

namespace SandboxTestProject.ViewModels.Core.Interfaces;

public interface ILeetCodeProblemsViewModel
{
    ObservableCollection<StatStatusPairs> DisplayedProblems { get; set; }
    int CurrentPage { get; set; }
    int TotalPages { get; set; }
    bool IsNextPageButtonEnabled { get; set; }
    bool IsPreviousPageButtonEnabled { get; set; }
    object CurrentViewModel { get; set; }

    ICommand NextPageCommand { get; }
    ICommand PreviousPageCommand { get; }
    ICommand ListItemClickedCommand { get; }
    ICommand CloseCurrentViewModelCommand { get; }
    ICommand OrderCommand { get; }

    Task InitializeData();
}
