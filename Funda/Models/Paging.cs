using System.Text.Json.Serialization;

namespace Funda.Models
{
    public class Paging
    {
        [JsonPropertyName("AantalPaginas")]
        public int TotalPages { get; set; }
    }
}
