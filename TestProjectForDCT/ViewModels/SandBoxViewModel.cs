using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TestProjectForDCT.Models.HackerearthModels;
using TestProjectForDCT.Views;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using TestProjectForDCT.ViewModels.Core;
using System.Net.Http;
using Microsoft.Extensions.Options;
using TestProjectForDCT.ViewModels.Core.Interfaces;
using TestProjectForDCT.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace TestProjectForDCT.ViewModels;

public class SandBoxViewModel : BaseViewModel, ISandBoxViewModel
{
    private readonly ICodeEvaluationService _codeEvaluationService;
    private readonly Config _config;
    private readonly LocalizationManager _localizationManager;
    private readonly ILogger<ISandBoxViewModel> _logger;

    private Dictionary<string, string> _languages;
    private string _codeText;
    private string _memoryLimit = "262144";
    private string _timeLimit = "5";
    private string _resultText;
    private string _selectedLanguage;
    private bool _isCheckStatusButtonEnabled = false;
    private IHighlightingDefinition _syntaxHighlighting;
    private string he_id;    

    public string CodeText
    {
        get => _codeText;
        set
        {
            _codeText = value;
            OnPropertyChanged();
        }
    }

    public string MemoryLimit
    {
        get => _memoryLimit;
        set
        {
            _memoryLimit = value;
            OnPropertyChanged();
        }
    }

    public string TimeLimit
    {
        get => _timeLimit;
        set
        {
            _timeLimit = value;
            OnPropertyChanged();
        }
    }

    public string ResultText
    {
        get => _resultText;
        set
        {
            _resultText = value;
            OnPropertyChanged();
        }
    }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            _selectedLanguage = value;
            UpdateSyntaxHighlighting();
            OnPropertyChanged();
        }
    }

    public IHighlightingDefinition SyntaxHighlighting
    {
        get => _syntaxHighlighting;
        set
        {
            _syntaxHighlighting = value;
            OnPropertyChanged();
        }
    }

    public bool IsCheckStatusButtonEnabled
    {
        get => _isCheckStatusButtonEnabled;
        set
        {
            _isCheckStatusButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Languages { get; } = new();

    public ICommand RunCodeCommand { get; set; }
    public ICommand CheckCodeEvaluationCommand { get; set; }
    public ICommand OpenLeetCodeProblemsCommand { get; set; }
    public ICommand NavigateToHomeViewCommand { get; set; }

    public SandBoxViewModel(ICodeEvaluationService codeEvaluationService, IServiceProvider serviceProvider, IOptions<Config> config, ILogger<ISandBoxViewModel> logger)
    {
        _codeEvaluationService = codeEvaluationService;
        _logger = logger;
        _config = config.Value;

        _localizationManager = LocalizationManager.GetInstance();

        _languages = new()
        {
            { "C", "C" },
            { "CPP14", "C++14" },
            { "CPP17", "C++17" },
            { "CLOJURE", "Clojure" },
            { "CSHARP", "C#" },
            { "GO", "Go" },
            { "HASKELL", "Haskell" },
            { "JAVA8", "Java 8" },
            { "JAVA14", "Java 14" },
            { "JAVASCRIPT_NODE", "JavaScript(Nodejs)" },
            { "KOTLIN", "Kotlin" },
            { "OBJECTIVEC", "Objective C" },
            { "PASCAL", "Pascal" },
            { "PERL", "Perl" },
            { "PHP", "PHP" },
            { "PYTHON", "Python 2" },
            { "PYTHON3", "Python 3" },
            { "PYTHON3_8", "Python 3.8" },
            { "R", "R" },
            { "RUBY", "Ruby" },
            { "RUST", "Rust" },
            { "SCALA", "Scala" },
            { "SWIFT", "Swift" },
            { "TYPESCRIPT", "TypeScript" }
        };

        foreach (var lang in _languages)
        {
            Languages.Add(lang.Value);
        }

        SelectedLanguage = Languages.ElementAt(0);

        RunCodeCommand = new HandleCommand(async obj => await RunCodeAsync());
        CheckCodeEvaluationCommand = new HandleCommand(async obj => await CheckCodeEvaluationStatus());
        OpenLeetCodeProblemsCommand = new HandleCommand(obj => OpenLeetCodeProblems(serviceProvider));
    }

    public void UpdateSyntaxHighlighting()
    {
        string language = SelectedLanguage switch
        {
            "C" or "C++14" or "C++17" or "Objective C" => "C++",
            "C#" => "C#",
            "Java 8" or "Java 14" => "Java",
            "JavaScript(Nodejs)" or "TypeScript" => "JavaScript",
            "Python 2" or "Python 3" or "Python 3.8" => "Python",
            "Ruby" => "Ruby",
            "Pascal" => "Pascal",
            "PHP" => "PHP",
            _ => ""
        };

        SyntaxHighlighting = _config.ApplicationTheme == "Dark" ? LoadDarkHighlighting(language) : HighlightingManager.Instance.GetDefinition(language);
    }

    private void OpenLeetCodeProblems(IServiceProvider serviceProvider)
    {
        try
        {
            _logger.LogInformation("Opening LeetCodeProblemsView");

            var _leetCodeProblemsView = serviceProvider.GetRequiredService<LeetCodeProblemsView>();
            _leetCodeProblemsView.Show();

            _logger.LogInformation("LeetCodeProblemsView opened successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening LeetCodeProblemsView");
        }        
    }

    private async Task RunCodeAsync()
    {
        try
        {
            _logger.LogInformation("Running code");

            var language = _languages.FirstOrDefault(x => x.Value == SelectedLanguage).Key;

            var postCodeEvaluationModel = new PostCodeEvaluationModel
            {
                lang = language,
                source = CodeText,
                input = "",
                memory_limit = MemoryLimit,
                time_limit = TimeLimit,
                context = "",
                callback = ""
            };

            var result = await _codeEvaluationService.SendCodeEvaluation(postCodeEvaluationModel);

            he_id = result.he_id;

            ResultText = result.result.compile_status;

            IsCheckStatusButtonEnabled = true;

            _logger.LogInformation("Code run successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running code");

            ResultText = ex.Message;
        }
    }

    private async Task CheckCodeEvaluationStatus()
    {
        try
        {
            _logger.LogInformation("Checking code evaluation status");

            var result = await _codeEvaluationService.GetResultCodeEvaluation(he_id);

            var runStatus = result.result.run_status;

            var resultMessage = new StringBuilder();

            resultMessage.AppendLine($"{_localizationManager["CompileStatus"]}{result.result.compile_status}");

            if (result.result.compile_status == "OK" && runStatus.status == "NA")
            {
                resultMessage.AppendLine(_localizationManager["Executing"]);
                ResultText = resultMessage.ToString();
                return;
            }

            switch (runStatus.status)
            {
                case "AC":
                    resultMessage.AppendLine($"{_localizationManager["MemoryUsed"]}{runStatus.memory_used} bytes");
                    resultMessage.AppendLine($"{_localizationManager["TimeUsed"]}{runStatus.time_used}");
                    if (runStatus.exit_code == "null") 
                    {
                        resultMessage.AppendLine($"{_localizationManager["ExitCode"]}{runStatus.exit_code}");
                    }
                    string output = await GetOutputStringFromFile(runStatus.output);
                    resultMessage.AppendLine($"{_localizationManager["Output"]}\n{output}");
                    break;

                case "MLE":
                    resultMessage.AppendLine(_localizationManager["MLE"]);
                    break;

                case "TLE":
                    resultMessage.AppendLine(_localizationManager["TLE"]);
                    break;

                case "RE":
                    resultMessage.AppendLine(GetRuntimeErrorDescription(runStatus.status_detail));
                    break;

                default:
                    resultMessage.AppendLine(_localizationManager["USR"]);
                    break;
            }

            _logger.LogInformation("Code evaluation status checked successfully");

            ResultText = resultMessage.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking code evaluation status");

            ResultText = ex.Message;
        }
    }

    private async Task<string> GetOutputStringFromFile(string url)
    {
        try
        {
            _logger.LogInformation("Getting output string from file");

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync(url);

                _logger.LogInformation("Output string received successfully");

                return result;
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting output string from file");
            return ex.Message;
        }        
    }

    private string GetRuntimeErrorDescription(string statusDetail)
    {
        return statusDetail switch
        {
            "SIGXFSZ" => _localizationManager["SIGXFSZ"],
            "SIGSEGV" => _localizationManager["SIGSEGV"],
            "SIGFPE" => _localizationManager["SIGFPE"],
            "SIGBUS" => _localizationManager["SIGBUS"],
            "SIGABRT" => _localizationManager["SIGABRT"],
            "NZEC" or "OTHER" => _localizationManager["NZEC/OTHER"],
            _ => _localizationManager["URE"]
        };
    }

    private IHighlightingDefinition LoadDarkHighlighting(string language)
    {
        try
        {
            _logger.LogInformation("Loading dark highlighting");

            var resourceName = language switch
            {
                "C++" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/CPP_Dark.xshd",
                "C#" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/CSHARP_Dark.xshd",
                "Java" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/Java_Dark.xshd",
                "JavaScript" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/JavaScript_Dark.xshd",
                "Pascal" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/Pascal_Dark.xshd",
                "PHP" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/PHP_Dark.xshd",
                "Python" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/Python_Dark.xshd",
                "Ruby" => $"{AppContext.BaseDirectory}/Themes/AvalonDarkTheme/Ruby_Dark.xshd",
                _ => null
            };

            using (var stream = File.OpenRead(resourceName))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    var xshd = HighlightingLoader.LoadXshd(reader);
                    var result = HighlightingLoader.Load(xshd, HighlightingManager.Instance);

                    _logger.LogInformation("Dark highlighting loaded successfully");

                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dark highlighting");
            return null;
        }        
    }
}
