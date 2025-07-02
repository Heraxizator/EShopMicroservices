using Aggregator.WebApi.Customer;
using Newtonsoft.Json;
using Ocelot.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator.WebApi.Order;

public sealed class OrderService : IOrderService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<OrderApi>> GetOrdersByCustomerIdAsync(int customerId, int cancelTimeout)
    {
        using CancellationTokenSource cancellationTokenSource = new(cancelTimeout);

        var response = await _httpClient.GetAsync($"https://localhost:44373/api/v1/Order/Customers/{customerId}", cancellationTokenSource.Token);
        return JsonConvert.DeserializeObject<List<OrderApi>>(await response.Content.ReadAsStringAsync(cancellationTokenSource.Token));
    }
}
