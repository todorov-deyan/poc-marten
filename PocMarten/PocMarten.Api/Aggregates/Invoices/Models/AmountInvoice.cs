namespace PocMarten.Api.Aggregates.Invoices.Models
{
    public class AmountInvoice
    {
        public decimal Value { get; }

        public AmountInvoice(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException(nameof(amount));

            Value = amount;
        }
    }
}
