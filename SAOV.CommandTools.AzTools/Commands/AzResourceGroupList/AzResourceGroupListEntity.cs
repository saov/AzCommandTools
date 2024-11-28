namespace SAOV.CommandTools.AzTools.Commands.AzResourceGroupList
{
    using System.Text.Json.Serialization;

    internal class AzResourceGroupListEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("provisioningState")]
        public string ProvisioningState { get; set; }
    }
}
