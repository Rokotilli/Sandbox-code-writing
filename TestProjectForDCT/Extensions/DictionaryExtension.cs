namespace TestProjectForDCT.Helpers;

static class DictionaryExtension
{
    public static Dictionary<string, string> ToDictionary(this object obj)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);

            if (value == null)
            {
                continue;
            }

            dictionary.Add(property.Name, value.ToString());                
        }

        return dictionary;
    }
}
