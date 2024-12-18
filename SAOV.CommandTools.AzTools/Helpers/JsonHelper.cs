﻿namespace SAOV.CommandTools.AzTools.Helpers
{
    using System.Text.Json;

    internal static class JsonHelper
    {
        public static T GetEntity<T>((bool Success, string Output) input) where T : class
        {
            return input.Success ? JsonSerializer.Deserialize<T>(input.Output) : null;
        }

        public static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
