using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator.WebApi.Customer;

public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient = new();

    public async Task<CustomerApi> GetCustomerByNameAsync(string customerName, int cancelTimeout)
    {
        using CancellationTokenSource cancellationTokenSource = new(cancelTimeout);

        var response = await _httpClient.GetAsync($"https://localhost:5001/api/v1/Customer/Nickname/{customerName}", cancellationTokenSource.Token);
        return JsonConvert.DeserializeObject<CustomerApi>(await response.Content.ReadAsStringAsync(cancellationTokenSource.Token));
    }
}
