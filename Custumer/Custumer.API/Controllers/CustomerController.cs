using Customers.Application.Commands;
using Customers.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var query = new GetCustomerByIdQuery(id);

        var customer = await _mediator.Send(query);

        if (customer == null) return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomers([FromBody] AddCustomers command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetCustomerById), new { id }, command);
    }
}
