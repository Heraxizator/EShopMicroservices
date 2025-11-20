using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator.WebApi.Customer;

public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly string _customerServiceUrl;

    public CustomerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _customerServiceUrl = configuration["MicroserviceUrls:CustomerService"] ?? "http://localhost:5001";
    }

    public async Task<CustomerApi> GetCustomerByNameAsync(string customerName, int cancelTimeout)
    {
        using CancellationTokenSource cancellationTokenSource = new(cancelTimeout);

        var response = await _httpClient.GetAsync($"{_customerServiceUrl}/api/v1/Customer/Nickname/{customerName}", cancellationTokenSource.Token);
        return JsonConvert.DeserializeObject<CustomerApi>(await response.Content.ReadAsStringAsync(cancellationTokenSource.Token));
    }
}
