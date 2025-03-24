using System.Collections.ObjectModel;
using System.Windows.Input;
using TestProjectForDCT.Models.LeetCodeModels;
using TestProjectForDCT.Services;
using TestProjectForDCT.ViewModels.Core;

namespace TestProjectForDCT.ViewModels;

public class LeetCodeProblemsViewModel : BaseViewModel
{
    private readonly LeetCodeService _leetCodeService;
    private readonly DetailsProblemViewModel _detailsProblemViewModel;
    private readonly EnteringPersonalDataViewModel _enteringPersonalDataViewModel;
    private readonly Config _config;
    private object _currentViewModel;
    private ObservableCollection<StatStatusPairs> _allProblems;
    private ObservableCollection<StatStatusPairs> _displayedProblems;
    private StatStatusPairs _selectedProblem;
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

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public StatStatusPairs SelectedProblem
    {
        get => _selectedProblem;
        set
        {
            _selectedProblem = value;
            OnPropertyChanged();
        }
    }

    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand ListItemClickedCommand { get; }
    public ICommand CloseCurrentViewModelCommand { get; }

    public LeetCodeProblemsViewModel(
        LeetCodeService leetCodeService,
        DetailsProblemViewModel detailsProblemViewModel,
        EnteringPersonalDataViewModel enteringPersonalDataViewModel,
        Config config)
    {
        _leetCodeService = leetCodeService;
        _detailsProblemViewModel = detailsProblemViewModel;
        _enteringPersonalDataViewModel = enteringPersonalDataViewModel;
        _config = config;

        NextPageCommand = new HandleCommand(obj => CurrentPage++);
        PreviousPageCommand = new HandleCommand(obj => CurrentPage--);
        CloseCurrentViewModelCommand = new HandleCommand(obj => CurrentViewModel = null);
        ListItemClickedCommand = new HandleCommand(async obj => await ListItemClicked(obj));

        detailsProblemViewModel.NavigateLeetCodeProblemsView = CloseCurrentViewModelCommand;
        enteringPersonalDataViewModel.NavigateLeetCodeProblemsView = CloseCurrentViewModelCommand;
    }

    public async Task InitializeData()
    {
        var problems = await _leetCodeService.GetProblemsAsync();

        _allProblems = new ObservableCollection<StatStatusPairs>(problems.stat_status_pairs);

        TotalPages = (_allProblems.Count + _itemsPerPage - 1) / _itemsPerPage;

        UpdateDisplayedProblems();
    }

    public async Task ListItemClicked(object parameter)
    {
        if (parameter is StatStatusPairs problem)
        {
            SelectedProblem = problem;         

            var content = await _leetCodeService.GetDetailsProblemAsync(problem.stat.question__title_slug, _config.LeetCode.session_token, _config.LeetCode.csrf_token);

            if (content.data.question.content == null)
            {
                CurrentViewModel = _enteringPersonalDataViewModel;
                return;
            }

            _detailsProblemViewModel.HtmlContent = content.data.question.content;

            CurrentViewModel = _detailsProblemViewModel;
        }
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
