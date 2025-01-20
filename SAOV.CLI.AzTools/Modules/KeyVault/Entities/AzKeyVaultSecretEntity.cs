namespace SAOV.CLI.AzTools.Modules.KeyVault.Entities
{
    using System.Text.Json.Serialization;

    internal class AzKeyVaultSecretEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
