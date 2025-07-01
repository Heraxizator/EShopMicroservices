using Customer.Microservice.Entities;
using Order.Microservice.Enums;
using System;

namespace Order.Microservice.Entities;

public class Order : BaseEntity
{
    public long ProductId { get; set; }
    public long CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int OrderStatus { get; set; }
}
