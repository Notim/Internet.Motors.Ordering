namespace Core.Domain;

public class Order
{

    public Order(
        long id,
        Guid orderId,
        string customerDocument, 
        DateTime orderedAt,
        Vehicle vehicle, 
        OrderStatus status,
        string? observations
    )
    {
        Id = id;
        OrderId = orderId;
        CustomerDocument = customerDocument;
        OrderedAt = orderedAt;
        Vehicle = vehicle;
        Status = status;
        Observations = observations;
    }

    public long Id { get; private set; }

    public Guid OrderId { get; private set; }

    public string CustomerDocument { get; private set; }

    public DateTime OrderedAt { get; private set; }

    public Vehicle Vehicle { get; private set; }

    public OrderStatus Status { get; private set; }

    public string? Observations { get; private set; }
    
    public void CancelOrder(string? reason)
    {
        Status = OrderStatus.Cancelled;
        Observations = reason;
    } 
    
    public void FinalizeOrder()
    {
        Status = OrderStatus.Finished;
        Observations = $"order finalized at {DateTime.UtcNow:O}";
    } 

}

