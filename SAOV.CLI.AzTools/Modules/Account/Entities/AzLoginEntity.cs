namespace SAOV.CLI.AzTools.Modules.Account.Entities
{
    using System.Text.Json.Serialization;

    internal class AzLoginEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
