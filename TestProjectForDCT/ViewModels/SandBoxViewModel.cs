using ICSharpCode.AvalonEdit.Highlighting;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TestProjectForDCT.Models.HackerearthModels;
using TestProjectForDCT.ViewModels.CommandHandler;

namespace TestProjectForDCT.ViewModels;

public class SandBoxViewModel : BaseViewModel
{
    private readonly CodeEvaluationService _codeEvaluationService;
    private Dictionary<string, string> _languages;
    private string _codeText;
    private string _memoryLimit;
    private string _timeLimit;
    private string _resultText;
    private string _selectedLanguage;
    private bool _isCheckStatusButtonEnabled;
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
            string language = _selectedLanguage switch
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

            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(language);
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

    public SandBoxViewModel(CodeEvaluationService codeEvaluationService)
    {
        _codeEvaluationService = codeEvaluationService;
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
        _memoryLimit = "262144";
        _timeLimit = "5";
        IsCheckStatusButtonEnabled = false;        

        RunCodeCommand = new HandleCommand(async obj => await RunCodeAsync());
        CheckCodeEvaluationCommand = new HandleCommand(async obj => await CheckCodeEvaluationStatus());
    }

    public async Task RunCodeAsync()
    {
        try
        {
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
        }
        catch (Exception ex)
        {
            ResultText = ex.Message;
        }
    }

    public async Task CheckCodeEvaluationStatus()
    {
        try
        {
            var result = await _codeEvaluationService.GetResultCodeEvaluation(he_id);

            var runStatus = result.result.run_status;

            var resultMessage = new StringBuilder();

            resultMessage.AppendLine($"Compile status: {result.result.compile_status}");

            switch (runStatus.status)
            {
                case "NA":
                    resultMessage.AppendLine("Executing...");
                    break;

                case "AC":
                    resultMessage.AppendLine($"Memory used: {runStatus.memory_used} bytes");
                    resultMessage.AppendLine($"Time used: {runStatus.time_used}");
                    resultMessage.AppendLine($"Exit code: {runStatus.exit_code}");
                    resultMessage.AppendLine($"Output: {runStatus.output}");
                    break;

                case "MLE":
                    resultMessage.AppendLine("Memory Limit Exceeded: Execution used more memory than the allowed limit.");
                    break;

                case "TLE":
                    resultMessage.AppendLine("Time Limit Exceeded: Execution took more time than the allowed limit.");
                    break;

                case "RE":
                    resultMessage.AppendLine(GetRuntimeErrorDescription(runStatus.status_detail));
                    break;

                default:
                    resultMessage.AppendLine("Unknown status received.");
                    break;
            }

            ResultText = resultMessage.ToString();
        }
        catch (Exception ex)
        {
            ResultText = ex.Message;
        }
    }

    private string GetRuntimeErrorDescription(string statusDetail)
    {
        return statusDetail switch
        {
            "SIGXFSZ" => "SIGXFSZ: The output file size exceeded the allowed system value.",
            "SIGSEGV" => "SIGSEGV: Invalid memory reference or segmentation fault.",
            "SIGFPE" => "SIGFPE: Division by zero.",
            "SIGBUS" => "SIGBUS: Accessed memory location that has not been physically mapped.",
            "SIGABRT" => "SIGABRT: Aborting due to a fatal error.",
            "NZEC" or "OTHER" => "NZEC / OTHER: User code stopped due to an unexpected reason.",
            _ => "Unknown runtime error."
        };
    }
}
