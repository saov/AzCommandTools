namespace SAOV.CLI.AzTools.Modules.Vnet.Entities
{
    using System.Text.Json.Serialization;

    internal class AzVnetListEntity
    {
        public AzVnetListEntity()
        {
            AddressSpace = [];
            Subnets = [];
            SubnetsDetails = [];
        }

        [JsonPropertyName("addressSpace")]
        public List<string> AddressSpace { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("provisioningState")]
        public string? ProvisioningState { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string? ResourceGroup { get; set; }

        [JsonPropertyName("subnets")]
        public List<string> Subnets { get; set; }

        [JsonPropertyName("subnetsDetails")]
        public List<AzSubnetEntity> SubnetsDetails { get; set; }
    }
}
