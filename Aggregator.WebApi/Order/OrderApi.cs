using System;
using System.Text.Json.Serialization;

namespace Aggregator.WebApi.Order;

public class OrderApi
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("orderStatus")]
    public int OrderStatus { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}

