using Core.Domain;
using Infrastructure.Data.Dao;
using Infrastructure.Data.Mappers;

namespace Infrastructure.Data.Repositories;

public class OrderRepository : IOrderRepository
{

    private readonly IVehicleOrderDao _dao;

    public OrderRepository(IVehicleOrderDao dao)
    {
        _dao = dao;
    }

    public async Task<Order?> GetOrderByOrderId(Guid vehicleId, CancellationToken cancellationToken)
    {
        var dto = await _dao.GetOrderByVehicleIdAsync(vehicleId, cancellationToken);

        return dto?.ToDomain();
    }

    public async Task<int> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var dto = order.ToDatabaseDto();
        return await _dao.InsertVehicleOrderAsync(dto, cancellationToken);
    }

    public async Task<bool> UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var dto = order.ToDatabaseDto();
        return await _dao.UpdateVehicleOrderAsync(dto, cancellationToken);
    }

}