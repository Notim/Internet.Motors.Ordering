namespace Core.Application.Services.NotifyOrderFinalized;

public record OrderFinalized
{

    public OrderFinalized(
        Guid orderId, 
        Guid vehicleId
    )
    {
        OrderId = orderId;
        VehicleId = vehicleId;
    }

    public Guid OrderId { get; private set; }

    public Guid VehicleId { get; private set; }

}