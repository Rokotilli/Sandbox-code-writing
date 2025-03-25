using System.Globalization;
using System.Windows.Data;
using TestProjectForDCT.ViewModels.Core;

namespace TestProjectForDCT.Helpers;

public class DataConvertHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var localizationManager = LocalizationManager.GetInstance();
        if (value is string data)
        {
            return data switch
            {
                "1" => localizationManager["Easy"],
                "2" => localizationManager["Medium"],
                "3" => localizationManager["Hard"],
                "true" => localizationManager["Yes"],
                "false" => localizationManager["No"],
                _ => data
            };
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
