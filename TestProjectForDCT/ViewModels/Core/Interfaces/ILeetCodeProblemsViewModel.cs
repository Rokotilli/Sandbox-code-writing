using System.Collections.ObjectModel;
using System.Windows.Input;
using TestProjectForDCT.Models.LeetCodeModels;

namespace TestProjectForDCT.ViewModels.Core.Interfaces;

public interface ILeetCodeProblemsViewModel
{
    ObservableCollection<StatStatusPairs> DisplayedProblems { get; set; }
    int CurrentPage { get; set; }
    int TotalPages { get; set; }
    bool IsNextPageButtonEnabled { get; set; }
    bool IsPreviousPageButtonEnabled { get; set; }
    object CurrentViewModel { get; set; }
    StatStatusPairs SelectedProblem { get; set; }

    ICommand NextPageCommand { get; }
    ICommand PreviousPageCommand { get; }
    ICommand ListItemClickedCommand { get; }
    ICommand CloseCurrentViewModelCommand { get; }
    ICommand OrderCommand { get; }

    Task InitializeData();
}
