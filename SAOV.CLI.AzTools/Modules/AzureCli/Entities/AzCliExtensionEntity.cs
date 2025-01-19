namespace SAOV.CLI.AzTools.Modules.AzureCli.Entities
{

    using System.Text.Json.Serialization;

    internal class AzCliExtensionEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }
}
