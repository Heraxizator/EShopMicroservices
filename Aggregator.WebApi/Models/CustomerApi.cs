using System.Text.Json.Serialization;

namespace Aggregator.WebApi.Models;

public class CustomerApi
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}