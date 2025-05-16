namespace SAOV.CLI.AzTools.Modules.ACR.Entities
{
        using System.Text.Json.Serialization;

    internal class ACREntity
    {
        [JsonPropertyName("anonymousPullEnabled")]
        public bool AnonymousPullEnabled { get; set; }

        [JsonPropertyName("dataEndpointHostNames")]
        public string? DataEndpointHostNames { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("loginServer")]
        public string LoginServer { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("networkRuleBypassOptions")]
        public string? NetworkRuleBypassOptions { get; set; }

        [JsonPropertyName("networkRuleSetDefaultAction")]
        public string? NetworkRuleSetDefaultAction { get; set; }

        [JsonPropertyName("provisioningState")]
        public string? ProvisioningState { get; set; }

        [JsonPropertyName("publicNetworkAccess")]
        public string? PublicNetworkAccess { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string? ResourceGroup { get; set; }

        [JsonPropertyName("skuName")]
        public string? SkuName { get; set; }

        [JsonPropertyName("skuTier")]
        public string? SkuTier { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("zoneRedundancy")]
        public string? ZoneRedundancy { get; set; }
    }
}
