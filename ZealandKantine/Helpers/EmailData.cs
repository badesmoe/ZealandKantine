using System.Text.Json;

namespace ZealandKantine.Helpers
{
    public static class EmailData
    {
        static string JsonString = File.ReadAllText("utility.json");

        public static string GetEmailName()
        {
            var jsonDoc = JsonDocument.Parse(JsonString);
            var root = jsonDoc.RootElement;
            var emailNameString = root.GetProperty("EmailName").GetString();
            return emailNameString;
        }

        public static string GetEmailPassword()
        {
            var jsonDoc = JsonDocument.Parse(JsonString);
            var root = jsonDoc.RootElement;
            var emailPasswordString = root.GetProperty("EmailPassword").GetString();
            return emailPasswordString;
        }
    }
}
