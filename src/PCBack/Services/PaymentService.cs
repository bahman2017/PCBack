using Microsoft.Extensions.Options;
using PCBack.Models;

namespace PCBack.Services;

public class PaymentService : IPaymentService
{
    private const string MockCheckoutBase = "https://fake-checkout.patentclarity.com/session";

    private readonly IReportService _reportService;
    private readonly PaymentOptions _options;

    public PaymentService(IReportService reportService, IOptions<PaymentOptions> options)
    {
        _reportService = reportService;
        _options = options.Value;
    }

    public async Task<string?> CreateCheckoutSessionAsync(Guid reportId)
    {
        var report = await _reportService.GetByIdAsync(reportId);
        if (report == null)
            return null;

        return _options.Mode switch
        {
            PaymentMode.Mock => $"{MockCheckoutBase}/{reportId}",
            PaymentMode.Stripe => throw new NotSupportedException(
                "Stripe checkout is not integrated yet. Set Payment:Mode to Mock for MVP."),
            _ => $"{MockCheckoutBase}/{reportId}"
        };
    }
}
