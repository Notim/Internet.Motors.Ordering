namespace Core.Application.Services.NotifyOrderCanceled;

public record OrderCanceled
{

    public OrderCanceled(
        Guid orderId,
        Guid vehicleId,
        string? reason
    )
    {
        OrderId = orderId;
        VehicleId = vehicleId;
        Reason = reason;
    }

    public Guid OrderId { get; private set; }

    public Guid VehicleId { get; private set; }

    public string? Reason { get; private set; }

}