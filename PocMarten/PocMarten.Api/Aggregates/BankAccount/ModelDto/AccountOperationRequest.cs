namespace PocMarten.Api.Aggregates.BankAccount.ModelDto
{
    public class AccountOperationRequest
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; } = 0;
        public string Description { get; set; }
        public AccountOperationType  OperationType { get; set; }
    }
}
