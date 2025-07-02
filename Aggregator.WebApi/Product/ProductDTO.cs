using System;

namespace Aggregator.WebApi.Product;

public record struct ProductDTO
(
    string Name,
    int Rate
);
