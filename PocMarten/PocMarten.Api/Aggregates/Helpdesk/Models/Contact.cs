namespace PocMarten.Api.Aggregates.Helpdesk.Models
{
    public record Contact(
        ContactChannel ContactChannel,
        string? FirstName = null,
        string? LastName = null,
        string? EmailAddress = null,
        string? PhoneNumber = null
    );
}
