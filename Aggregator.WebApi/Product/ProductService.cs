using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator.WebApi.Product;

public sealed class ProductService : IProductService
{
    private readonly HttpClient _httpClient = new();

    public async Task<ProductApi> GetProductByIdAsync(int productId, int cancelTimeout)
    {
        using CancellationTokenSource cancellationTokenSource = new(cancelTimeout);

        var response = await _httpClient.GetAsync($"https://localhost:44337/api/v1/Product/{productId}", cancellationTokenSource.Token);
        return JsonConvert.DeserializeObject<ProductApi>(await response.Content.ReadAsStringAsync(cancellationTokenSource.Token));
    }
}
