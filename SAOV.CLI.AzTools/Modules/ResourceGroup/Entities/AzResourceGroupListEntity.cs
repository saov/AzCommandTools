namespace SAOV.CLI.AzTools.Modules.ResourceGroup.Entities
{
    using System.Text.Json.Serialization;

    internal class AzResourceGroupListEntity
    {
        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("provisioningState")]
        public string? ProvisioningState { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
