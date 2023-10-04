using System.Text.Json.Serialization;

namespace Funda.Models
{
    public class SalesData
    {
        [JsonPropertyName("Objects")]
        public List<SaleObject> SaleObjects { get; set; }
        public Paging Paging { get; set; }
    }
}
