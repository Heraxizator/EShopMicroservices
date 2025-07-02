using System;

namespace Aggregator.WebApi.DTOs;

public record struct CustomerDTO
(
    long ProductId,
    long CustomerId,
    DateTime Date,
    int OrderStatus
);
