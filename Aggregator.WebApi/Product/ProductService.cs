using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator.WebApi.Product;

public sealed class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _productServiceUrl;

    public ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _productServiceUrl = configuration["MicroserviceUrls:ProductService"] ?? "http://localhost:5003";
    }

    public async Task<ProductApi> GetProductByIdAsync(int productId, int cancelTimeout)
    {
        using CancellationTokenSource cancellationTokenSource = new(cancelTimeout);

        var response = await _httpClient.GetAsync($"{_productServiceUrl}/api/v1/Product/{productId}", cancellationTokenSource.Token);
        return JsonConvert.DeserializeObject<ProductApi>(await response.Content.ReadAsStringAsync(cancellationTokenSource.Token));
    }
}
