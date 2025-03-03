namespace Core.Application.Services.CreatePaymentLink;

public record CreatePaymentLink
{

    public CreatePaymentLink(
        Guid orderId,
        string customerDocument, 
        decimal totalAmount, 
        string description
    )
    {
        OrderId = orderId;
        CustomerDocument = customerDocument;
        TotalAmount = totalAmount;
        Description = description;
    }

    public Guid OrderId { get; private set; }

    public string CustomerDocument { get; private set; }

    public decimal TotalAmount { get; private set; }

    public string Description { get; private set; }

}