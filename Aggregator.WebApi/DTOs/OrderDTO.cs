using System;

namespace Aggregator.WebApi.DTOs;

public record struct OrderDTO
(
    long ProductId,
    long CustomerId,
    DateTime Date,
    int OrderStatus
);
