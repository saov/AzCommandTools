namespace SAOV.CommandTools.AzTools.Commands.AzKeyVault
{
    using System.Text.Json.Serialization;

    internal class AzKeyVaultSecretEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
