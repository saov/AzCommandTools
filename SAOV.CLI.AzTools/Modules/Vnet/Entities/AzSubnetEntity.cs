﻿namespace SAOV.CLI.AzTools.Modules.Vnet.Entities
{
    using System.Text.Json.Serialization;

    internal class AzSubnetEntity
    {
        public AzSubnetEntity()
        {
            AddressPrefixes = [];
        }

        [JsonPropertyName("addressPrefix")]
        public string? AddressPrefix { get; set; }

        [JsonPropertyName("addressPrefixes")]
        public List<string> AddressPrefixes { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("networkSecurityGroup")]
        public string? NetworkSecurityGroup { get; set; }

        [JsonPropertyName("provisioningState")]
        public string? ProvisioningState { get; set; }

        [JsonPropertyName("routeTable")]
        public string? RouteTable { get; set; }
    }
}
