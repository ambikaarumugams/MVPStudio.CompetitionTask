using System.Text.Json;

namespace qa_dotnet_cucumber.Helper
{
    public static class JsonHelper  
    {
        public static T LoadJson<T>(string fileName)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData",$"{fileName}.json");  //Build the full path to the json file in the TestData folder
            var jsonData = File.ReadAllText(jsonPath);  //Read the contents of the json file as a string
            return JsonSerializer.Deserialize<T>(jsonData); //Convert the strings into the object type CertificationsFeature
        }
    }
}
