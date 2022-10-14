using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Invoices.Events
{
    public class GrossAmountValue : IEventState
    {
        private const double GROSSVALUE = 1.2;
        public string DateIssued { get; set; }
        public double GrossAmount { get; set; }

        public GrossAmountValue(double grossAmount)
        {
            DateIssued = DateTime.UtcNow.ToString("dd/MM/yyyy");
            GrossAmount = grossAmount * GROSSVALUE;
        }
    }
}
