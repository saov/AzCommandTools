namespace SAOV.CommandTools.AzTools.Commands.AzAccountSubscriptionList
{
    using System.Text.Json.Serialization;

    internal class AzAccountSubscriptionListEntity
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("subscriptionId")]
        public string? SubscriptionId { get; set; }
    }
}
