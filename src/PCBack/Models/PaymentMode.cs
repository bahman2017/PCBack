namespace PCBack.Models;

/// <summary>
/// Payment provider strategy. MVP uses <see cref="Mock"/>; <see cref="Stripe"/> reserved for production.
/// </summary>
public enum PaymentMode
{
    Mock = 0,
    Stripe = 1
}
