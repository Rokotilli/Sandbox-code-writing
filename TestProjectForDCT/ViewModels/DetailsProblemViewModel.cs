using HTMLConverter;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using TestProjectForDCT.ViewModels.Core;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT.ViewModels;

public class DetailsProblemViewModel : BaseViewModel, IDetailsProblemViewModel
{    
    private readonly ILogger<IDetailsProblemViewModel> _logger;
    private string _htmlContent;    
    private FlowDocument _problemDescription;    

    public FlowDocument ProblemDescription
    {
        get => _problemDescription;
        set
        {
            _problemDescription = value;
            OnPropertyChanged();
        }
    }

    public string HtmlContent
    {
        get => _htmlContent;
        set
        {
            _htmlContent = value;
            UpdateFlowDocument();
            OnPropertyChanged();
        }
    }

    public ICommand NavigateLeetCodeProblemsView { get; set; }

    public DetailsProblemViewModel(ILogger<IDetailsProblemViewModel> logger)
    {
        _logger = logger;
    }

    private void UpdateFlowDocument()
    {
        try
        {
            _logger.LogInformation("Updating FlowDocument");

            string xaml = HtmlToXamlConverter.ConvertHtmlToXaml(_htmlContent, true);
            using (var stringReader = new StringReader(xaml))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                ProblemDescription = XamlReader.Load(xmlReader) as FlowDocument;
            }

            _logger.LogInformation("FlowDocument updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating FlowDocument");
        }        
    }
}
