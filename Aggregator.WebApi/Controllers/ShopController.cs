using Aggregator.WebApi.Customer;
using Aggregator.WebApi.Order;
using Aggregator.WebApi.Product;
using Microsoft.AspNetCore.Mvc;
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
    private readonly CustomerService _customerService = new();
    private readonly OrderService _orderService = new();
    private readonly ProductService _productService = new();

    [HttpGet("customers/{customerName}")]
    public async Task<IActionResult> GetAllByCustomerId(string customerName)
    {
        List<ProductApi> productApis = new();

        CustomerApi customerApi = await _customerService.GetCustomerByNameAsync(customerName, 5000);

        if (customerApi is null)
        {
            return NoContent();
        }

        List<OrderApi> orderApis = await _orderService.GetOrdersByCustomerIdAsync(customerApi.Id, 5000);

        foreach (OrderApi orderApi in orderApis)
        {
            productApis.Add(await _productService.GetProductByIdAsync(orderApi.productId, 5000));
        }

        return Ok(productApis);
    }
}