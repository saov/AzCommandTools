namespace SAOV.CommandTools.AzTools.Commands.AzKeyVault
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
    }
}
