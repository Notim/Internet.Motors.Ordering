namespace Core.Domain;

public interface IOrderRepository
{

    Task<Order?> GetOrderByOrderId(Guid vehicleId, CancellationToken cancellationToken);

    Task<int> InsertOrderAsync(Order vehicle, CancellationToken cancellationToken);

    Task<bool> UpdateOrderAsync(Order vehicle, CancellationToken cancellationToken);

}