using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Microservice.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Microservice.Entities;

namespace Customer.Microservice.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private IApplicationDbContext _context;

    public OrderController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order.Microservice.Entities.Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return Ok(order.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _context.Orders.ToListAsync();
        if (orders == null) return NotFound();
        return Ok(orders);
    }

    [HttpGet("Products/{productId}")]
    public async Task<IActionResult> GetAllByProductId(long productId)
    {
        var orders = await _context.Orders.Where(x => x.ProductId == productId).ToListAsync();
        if (orders == null) return NotFound();
        return Ok(orders);
    }

    [HttpGet("Customers/{customerId}")]
    public async Task<IActionResult> GetAllByCustomerId(long customerId)
    {
        var orders = await _context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        if (orders == null) return NotFound();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _context.Orders.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (order == null) return NotFound();
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return Ok(order.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Order.Microservice.Entities.Order orderData)
    {
        var order = _context.Orders.Where(a => a.Id == id).FirstOrDefault();

        if (order == null) return NotFound();
        else
        {
            order.OrderStatus = orderData.OrderStatus;
            await _context.SaveChangesAsync();
            return Ok(order.Id);
        }
    }
}