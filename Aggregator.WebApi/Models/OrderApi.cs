using System;

namespace Aggregator.WebApi.Models;


public class OrderApiList
{
    public OrderApi[] Source { get; set; }
}

public class OrderApi
{
    public int productId { get; set; }
    public int customerId { get; set; }
    public DateTime date { get; set; }
    public int orderStatus { get; set; }
    public int id { get; set; }
}

