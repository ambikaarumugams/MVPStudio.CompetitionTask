using System.Text.Json;

namespace qa_dotnet_cucumber.Helper
{
    public static class JsonHelper
    {
        public static T LoadJson<T>(string fileName)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"TestData\\{fileName}.json");
            var jsonData = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
