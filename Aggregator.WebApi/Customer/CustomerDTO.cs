using System;

namespace Aggregator.WebApi.Customer;

public record struct CustomerDTO
(
    long ProductId,
    long CustomerId,
    DateTime Date,
    int OrderStatus
);
