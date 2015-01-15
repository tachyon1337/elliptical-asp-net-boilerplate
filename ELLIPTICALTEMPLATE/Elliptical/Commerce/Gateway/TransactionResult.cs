namespace Elliptical.Mvc.Commerce.Gateway
{
    /// <summary>
    ///     Transaction Result enumerator list
    /// </summary>
    public enum TransactionResult
    {
        Approved,
        Declined,
        Pending,
        Failed,
        Fraud,
        ValidationSuccess,
        ValidationError
    }
}