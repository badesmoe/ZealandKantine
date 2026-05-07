using System.Text.Json;
namespace ZealandKantine.Helpers
{
    public static class ConnectionString
    {
        static string JsonString = File.ReadAllText("utility.json");

        public static string GetConnectionString()
        {
            var jsonDoc = JsonDocument.Parse(JsonString);
            var root = jsonDoc.RootElement;
            var connectionString = root.GetProperty("ConnectionString").GetString();
            return connectionString;
        }
    }
}
