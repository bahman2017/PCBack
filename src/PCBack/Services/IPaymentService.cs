namespace PCBack.Services;

public interface IPaymentService
{
    /// <summary>
    /// Starts a checkout session for the given persisted report id.
    /// Returns null if the report does not exist.
    /// </summary>
    Task<string?> CreateCheckoutSessionAsync(Guid reportId);
}
