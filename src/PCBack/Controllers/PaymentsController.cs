using Microsoft.AspNetCore.Mvc;
using PCBack.Models;
using PCBack.Services;

namespace PCBack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Initiates checkout for paying to generate/access a report (MVP: mock URL only).
    /// </summary>
    [HttpPost("checkout")]
    [ProducesResponseType(typeof(PaymentCheckoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public async Task<ActionResult<PaymentCheckoutResponse>> Checkout([FromBody] PaymentCheckoutRequest? request)
    {
        if (request == null || request.ReportId == Guid.Empty)
            return BadRequest("A valid reportId is required.");

        try
        {
            var url = await _paymentService.CreateCheckoutSessionAsync(request.ReportId);
            if (url == null)
                return NotFound();

            return Ok(new PaymentCheckoutResponse { CheckoutUrl = url });
        }
        catch (NotSupportedException ex)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, new { message = ex.Message });
        }
    }
}
