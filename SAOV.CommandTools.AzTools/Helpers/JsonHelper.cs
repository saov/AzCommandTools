﻿namespace SAOV.CommandTools.AzTools.Helpers
{
    using System.Text.Json;

    internal static class JsonHelper
    {
        public static T GetEntity<T>((bool Success, string Output) input) where T : class
        {
            return input.Success ? JsonSerializer.Deserialize<T>(input.Output) : null;
        }
    }
}