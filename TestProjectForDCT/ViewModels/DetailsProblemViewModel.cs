using HTMLConverter;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace TestProjectForDCT.ViewModels;

public class DetailsProblemViewModel : BaseViewModel
{    
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

    public DetailsProblemViewModel()
    {
    }

    private void UpdateFlowDocument()
    {
        string xaml = HtmlToXamlConverter.ConvertHtmlToXaml(_htmlContent, true);
        using (var stringReader = new StringReader(xaml))
        using (var xmlReader = XmlReader.Create(stringReader))
        {
            ProblemDescription = XamlReader.Load(xmlReader) as FlowDocument;
        }
    }
}
