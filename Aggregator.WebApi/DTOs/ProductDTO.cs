using System;

namespace Product.Microservice.DTOs;

public record struct ProductDTO
(
    long ProductId,
    long CustomerId,
    string Name,
    int Rate,
    DateTime Date,
    int OrderStatus 
);
