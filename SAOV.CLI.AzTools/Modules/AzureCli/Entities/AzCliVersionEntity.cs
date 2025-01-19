namespace SAOV.CLI.AzTools.Modules.AzureCli.Entities
{
    using System.Text.Json.Serialization;

    internal class AzCliVersionEntity
    {
        [JsonPropertyName("azure-cli")]
        public string? AzureCli { get; set; }

        [JsonPropertyName("azure-cli-core")]
        public string? AzureCliCore { get; set; }

        [JsonPropertyName("azure-cli-telemetry")]
        public string? AzureCliTelemetry { get; set; }
    }
}
