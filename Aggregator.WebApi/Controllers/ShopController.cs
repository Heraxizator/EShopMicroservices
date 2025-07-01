using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.DTOs;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
    {
        var response = await _httpClient.GetAsync($"/api/v1/Basket/{productDTO.Name}");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpGet("products/{productId}")]
    public async Task<IActionResult> GetAllByProductId(long productId)
    {
        return Ok();
    }

    [HttpGet("customers/{customerId}")]
    public async Task<IActionResult> GetAllByCustomerId(long customerId)
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDTO productDTO)
    {
        return Ok();
    }
}