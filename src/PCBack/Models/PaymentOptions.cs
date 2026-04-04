namespace PCBack.Models;

public class PaymentOptions
{
    public const string SectionName = "Payment";

    public PaymentMode Mode { get; set; } = PaymentMode.Mock;
}
