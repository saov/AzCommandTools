namespace SAOV.CLI.AzTools.Modules.KeyVault.Entities
{
    using System.Text.Json.Serialization;

    internal class AzKeyVaultEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string? ResourceGroup { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("bypass")]
        public string? NetworkRuleSetBypass { get; set; }

        [JsonPropertyName("defaultAction")]
        public string? NetworkRuleSetDefaultAction { get; set; }
    }
}
