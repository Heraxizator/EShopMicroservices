using System;

namespace Aggregator.WebApi.Order;

public record struct OrderDTO
(
    long ProductId,
    long CustomerId,
    DateTime Date,
    int OrderStatus
);
