namespace SAOV.CLI.AzTools.Helpers
{
    using System.Text.Json;

    internal static class JsonHelper
    {
        public static string GetJson<T>(T obj)
        {
            return obj != null ?
                JsonSerializer.Serialize<T>(obj) :
                string.Empty;
        }
    }
}
