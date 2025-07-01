using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customer.Microservice.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customer.Microservice.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private IApplicationDbContext _context;

    public CustomerController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Entities.Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return Ok(customer.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _context.Customers.ToListAsync();
        if (customers == null) return NotFound();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpGet("nickname/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var customer = await _context.Customers.Where(a => a.Name == name).FirstOrDefaultAsync();
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (customer == null) return NotFound();
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return Ok(customer.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Entities.Customer customerData)
    {
        var customer = _context.Customers.Where(a => a.Id == id).FirstOrDefault();

        if (customer == null) return NotFound();
        else
        {
            customer.City = customerData.City;
            customer.Name = customerData.Name;
            customer.Contact = customerData.Contact;
            customer.Email = customerData.Email;
            await _context.SaveChangesAsync();
            return Ok(customer.Id);
        }
    }
}