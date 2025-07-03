using System.Text.Json.Serialization;

namespace Aggregator.WebApi.Product;

public class ProductApi
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("rate")]
    public int Rate { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}

