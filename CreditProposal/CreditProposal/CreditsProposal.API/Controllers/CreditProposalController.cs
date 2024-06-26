using CreditsProposal.Application.InputModels;
using CreditsProposal.Application.Services;
using CreditsProposal.Core.Events;
using Microsoft.AspNetCore.Mvc;

namespace CreditsProposal.API.Controllers;

[Route("api/creditproposal")]
[ApiController]
public class CreditProposalController : ControllerBase
{
    private readonly ILogger<CreditProposalController> logger;
    private readonly ICreditProposalService _creditProposalService;

    public CreditProposalController(ILogger<CreditProposalController> logger, ICreditProposalService creditProposalService)
    {
        this.logger = logger;
        _creditProposalService = creditProposalService;
    }
    [HttpGet]
    public async Task<IActionResult> GetCreditProposalById(int id)
    {
        var result = await _creditProposalService.GetById(id);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCreditProposal(CancellationToken cancellationToken, [FromBody] ProposalInputModel inputModel)
    {
        var customerCreated = new CustomerCreated(inputModel.Id, inputModel.FullName, inputModel.Email, inputModel.Income);
        await _creditProposalService.HandlerProposalCreateAsync(customerCreated, cancellationToken);

        return CreatedAtAction(nameof(GetCreditProposalById), new { id = customerCreated.Id }, inputModel);
    }
}
