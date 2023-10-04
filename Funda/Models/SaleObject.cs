using System.Text.Json.Serialization;

namespace Funda.Models
{
    public class SaleObject
    {
        [JsonPropertyName("MakelaarNaam")]
        public string BrokerName { get; set; }
    }
}
