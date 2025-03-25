using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestProjectForDCT.Models.LeetCodeModels;
using TestProjectForDCT.Services.Interfaces;
using TestProjectForDCT.ViewModels.Core;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT.ViewModels;

public class LeetCodeProblemsViewModel : BaseViewModel, ILeetCodeProblemsViewModel
{
    private readonly ILeetCodeService _leetCodeService;
    private readonly IDetailsProblemViewModel _detailsProblemViewModel;
    private readonly IEnteringPersonalDataViewModel _enteringPersonalDataViewModel;
    private readonly Config _config;
    private readonly ILogger<ILeetCodeProblemsViewModel> _logger;

    private object _currentViewModel;
    private ObservableCollection<StatStatusPairs> _allProblems;
    private ObservableCollection<StatStatusPairs> _displayedProblems;
    private StatStatusPairs _selectedProblem;
    private bool _isNextPageButtonEnabled = false;
    private bool _isPreviousPageButtonEnabled = false;
    private bool _isOrderedByDescendingTitle = true;
    private bool _isOrderedByDescendingDifficulty = true;
    private bool _isOrderedByDescendingPaidOnly = true;
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
    public ICommand OrderCommand { get; }

    public LeetCodeProblemsViewModel(
        ILeetCodeService leetCodeService,
        IDetailsProblemViewModel detailsProblemViewModel,
        IEnteringPersonalDataViewModel enteringPersonalDataViewModel,
        IOptions<Config> config,
        ILogger<ILeetCodeProblemsViewModel> logger)
    {
        _leetCodeService = leetCodeService;
        _detailsProblemViewModel = detailsProblemViewModel;
        _enteringPersonalDataViewModel = enteringPersonalDataViewModel;
        _logger = logger;
        _config = config.Value;

        NextPageCommand = new HandleCommand(obj => CurrentPage++);
        PreviousPageCommand = new HandleCommand(obj => CurrentPage--);
        CloseCurrentViewModelCommand = new HandleCommand(obj => CurrentViewModel = null);
        OrderCommand = new HandleCommand(obj => OrderAllProblems(obj));
        ListItemClickedCommand = new HandleCommand(async obj => await ListItemClicked(obj));

        detailsProblemViewModel.NavigateLeetCodeProblemsView = CloseCurrentViewModelCommand;
        enteringPersonalDataViewModel.NavigateLeetCodeProblemsView = CloseCurrentViewModelCommand;
    }

    public async Task InitializeData()
    {
        try
        {
            _logger.LogInformation("Initializing data");

            var problems = await _leetCodeService.GetProblemsAsync();

            _allProblems = new ObservableCollection<StatStatusPairs>(problems.stat_status_pairs);

            TotalPages = (_allProblems.Count + _itemsPerPage - 1) / _itemsPerPage;

            UpdateDisplayedProblems();

            _logger.LogInformation("Data initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing data");
        }        
    }

    private void OrderAllProblems(object parameter)
    {
        if (parameter is string order)
        {
            switch (order)
            {
                case "Difficulty":
                    OrderByDifficulty();
                    break;
                case "PaidOnly":
                    OrderByPaidOnly();
                    break;
                case "Title":
                    OrderByTitle();
                    break;
            }
        }        
    }

    private void OrderByTitle()
    {
        try
        {
            _logger.LogInformation("Ordering by title");

            if (_isOrderedByDescendingTitle)
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderBy(x => x.stat.question__title));
            }
            else
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderByDescending(x => x.stat.question__title));
            }
            _isOrderedByDescendingTitle = !_isOrderedByDescendingTitle;
            UpdateDisplayedProblems();

            _logger.LogInformation("Ordered by title successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ordering by title");
        }        
    }

    private void OrderByPaidOnly()
    {
        try
        {
            _logger.LogInformation("Ordering by paid only");

            if (_isOrderedByDescendingPaidOnly)
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderBy(x => x.paid_only));
            }
            else
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderByDescending(x => x.paid_only));
            }
            _isOrderedByDescendingPaidOnly = !_isOrderedByDescendingPaidOnly;
            UpdateDisplayedProblems();

            _logger.LogInformation("Ordered by paid only successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ordering by paid only");
        }        
    }

    private void OrderByDifficulty()
    {
        try
        {
            _logger.LogInformation("Ordering by difficulty");

            if (_isOrderedByDescendingDifficulty)
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderBy(x => x.difficulty.level));
            }
            else
            {
                _allProblems = new ObservableCollection<StatStatusPairs>(_allProblems.OrderByDescending(x => x.difficulty.level));
            }
            _isOrderedByDescendingDifficulty = !_isOrderedByDescendingDifficulty;
            UpdateDisplayedProblems();

            _logger.LogInformation("Ordered by difficulty successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ordering by difficulty");
        }        
    }

    private async Task ListItemClicked(object parameter)
    {
        try
        {
            _logger.LogInformation("Clicking list item");

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

            _logger.LogInformation("List item clicked successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clicking list item");
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
