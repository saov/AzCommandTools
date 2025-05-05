using System.Text.Json.Serialization;

namespace SAOV.CLI.AzTools.Modules.APIM.Entities
{
    internal class APIMOperationEntity
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string? ResourceGroup { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("statusCode")]
        public string? StatusCode { get; set; }
    }
}
