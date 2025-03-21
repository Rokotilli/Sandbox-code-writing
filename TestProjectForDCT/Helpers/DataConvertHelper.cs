using System.Globalization;
using System.Windows.Data;

namespace TestProjectForDCT.Helpers;

public class DataConvertHelper : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string data)
        {
            return data switch
            {
                "1" => "Easy",
                "2" => "Medium",
                "3" => "Hard",
                "true" => "Yes",
                "false" => "No",
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
