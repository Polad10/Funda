using System.Text.Json.Serialization;

namespace Funda.Models
{
    public class SaleObject
    {
        [JsonPropertyName("MakelaarNaam")]
        public string AgentName { get; set; }
    }
}
