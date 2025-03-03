using Infrastructure.Data.Dao.Dto;

namespace Infrastructure.Data.Dao;

public interface IVehicleOrderDao
{

    Task<VehicleOrderDto?> GetOrderByVehicleIdAsync(Guid orderId, CancellationToken cancellationToken);

    Task<int> InsertVehicleOrderAsync(VehicleOrderDto dto, CancellationToken cancellationToken);

    Task<bool> UpdateVehicleOrderAsync(VehicleOrderDto dto, CancellationToken cancellationToken);

}