namespace SAOV.CLI.AzTools.Modules.APIM.Entities
{
    using System.Text.Json.Serialization;

    internal class APIMEntity
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string? ResourceGroup { get; set; }

        [JsonPropertyName("serviceUrl")]
        public string? ServiceUrl { get; set; }

        [JsonPropertyName("subscriptionRequired")]
        public bool SubscriptionRequired { get; set; }

        public APIMOperationEntity[] Operations { get; set; }
    }
}
