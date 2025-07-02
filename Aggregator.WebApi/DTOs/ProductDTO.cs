using System;

namespace Product.Microservice.DTOs;

public record struct ProductDTO
(
    string Name,
    int Rate
);
