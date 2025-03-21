using System.Collections.ObjectModel;
using System.Windows.Input;
using TestProjectForDCT.Models.LeetCodeModels;
using TestProjectForDCT.Services;
using TestProjectForDCT.ViewModels.CommandHandler;

namespace TestProjectForDCT.ViewModels;

public class LeetCodeProblemsViewModel : BaseViewModel
{
    private readonly LeetCodeService _leetCodeService;
    private ObservableCollection<StatStatusPairs> _allProblems;
    private ObservableCollection<StatStatusPairs> _displayedProblems;
    private bool _isNextPageButtonEnabled = false;
    private bool _isPreviousPageButtonEnabled = false;
    private int _currentPage = 1;
    private int _itemsPerPage = 10;
    private int _totalPages;

    public ObservableCollection<StatStatusPairs> DisplayedProblems
    {
        get => _displayedProblems;
        set
        {
            _displayedProblems = value;
            OnPropertyChanged();
        }
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
            UpdateDisplayedProblems();
        }
    }

    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged();
        }
    }

    public bool IsNextPageButtonEnabled
    {
        get => _isNextPageButtonEnabled;
        set
        {
            _isNextPageButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool IsPreviousPageButtonEnabled
    {
        get => _isPreviousPageButtonEnabled;
        set
        {
            _isPreviousPageButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand ProblemSelectedCommand { get; }

    public LeetCodeProblemsViewModel(LeetCodeService leetCodeService)
    {
        _leetCodeService = leetCodeService;

        NextPageCommand = new HandleCommand(obj => CurrentPage++);
        PreviousPageCommand = new HandleCommand(obj => CurrentPage--);
    }

    public async Task InitializeData()
    {
        var problems = await _leetCodeService.GetProblemsAsync();

        _allProblems = new ObservableCollection<StatStatusPairs>(problems.stat_status_pairs);

        TotalPages = (_allProblems.Count + _itemsPerPage - 1) / _itemsPerPage;

        UpdateDisplayedProblems();
    }

    private void UpdateDisplayedProblems()
    {
        var problemsOnPage = _allProblems
            .Skip((CurrentPage - 1) * _itemsPerPage)
            .Take(_itemsPerPage);

        IsNextPageButtonEnabled = CurrentPage < TotalPages;
        IsPreviousPageButtonEnabled = CurrentPage > 1;

        DisplayedProblems = new ObservableCollection<StatStatusPairs>(problemsOnPage);
    }
}
