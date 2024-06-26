using CreditCards.Application.InputModels;
using CreditCards.Application.Services;
using CreditCards.Core.Events;
using Microsoft.AspNetCore.Mvc;

namespace CreditCards.API.Controllers;

[Route("api/creditCards")]
[ApiController]
public class CreditCardController : ControllerBase
{
    private readonly ILogger<CreditCardController> _logger;
    private readonly ICreditCardService _creditCardsService;

    public CreditCardController(ILogger<CreditCardController> logger, ICreditCardService CreditCardsService)
    {
        _logger = logger;
        _creditCardsService = CreditCardsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCreditCardsById(int id)
    {
        var result = await _creditCardsService.GetById(id);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCreditCards(CancellationToken cancellationToken, [FromBody] CardInputModel inputModel)
    {
        var customerCreated = new CustomerCreated(inputModel.Id, inputModel.FullName, inputModel.Email, inputModel.Income);
        await _creditCardsService.HandleCustomerCreatedAsync(customerCreated, cancellationToken);

        return CreatedAtAction(nameof(GetCreditCardsById), new { id = customerCreated.Id }, inputModel);
    }
}
