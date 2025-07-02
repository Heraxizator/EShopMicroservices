using Aggregator.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.Microservice.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Customer.Microservice.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ShopController()
    {
        _httpClient = new();
    }

    [HttpGet("customers/{customerName}")]
    public async Task<IActionResult> GetAllByCustomerId(string customerName)
    {
        List<ProductApi> productApis = [];

        var response = await _httpClient.GetAsync($"https://localhost:5001/api/v1/Customer/Nickname/{customerName}");

        CustomerApi customerApi = JsonConvert.DeserializeObject<CustomerApi>(await response.Content.ReadAsStringAsync());

        if (customerApi is null)
        {
            return NoContent();
        }

        response = await _httpClient.GetAsync($"https://localhost:44373/api/v1/Order/customers/{customerApi.Id}");

        List<OrderApi> orderApis = JsonConvert.DeserializeObject<List<OrderApi>>(await response.Content.ReadAsStringAsync());

        foreach (OrderApi orderApi in orderApis)
        {
            response = await _httpClient.GetAsync($"https://localhost:44337/api/v1/Product/{orderApi.productId}");

            string jsonResponse = await response.Content.ReadAsStringAsync();

            ProductApi productApi = JsonConvert.DeserializeObject<ProductApi>(jsonResponse);

            productApis.Add(productApi);
        }

        return Ok(productApis);
    }
}