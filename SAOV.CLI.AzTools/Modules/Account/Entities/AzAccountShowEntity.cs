namespace SAOV.CLI.AzTools.Modules.Account.Entities
{
    using System.Text.Json.Serialization;

    internal class AzAccountShowEntity
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("tenantDisplayName")]
        public string? TenantDisplayName { get; set; }

        [JsonPropertyName("tenantId")]
        public string? TenantId { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        [JsonPropertyName("userType")]
        public string? UserType { get; set; }
    }
}
