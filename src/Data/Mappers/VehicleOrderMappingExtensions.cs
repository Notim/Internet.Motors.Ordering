using Core.Domain;
using Infrastructure.Data.Dao.Dto;

namespace Infrastructure.Data.Mappers
{

    public static class VehicleOrderMappingExtensions
    {

        public static Order ToDomain(this VehicleOrderDto dto)
        {
            var vehicle = new Vehicle(dto.VehicleId, dto.CarName, dto.Price);

            var status = Enum.TryParse<OrderStatus>(dto.Status, true, out var parsedStatus) ? parsedStatus : OrderStatus.Pending;

            return new Order(
                dto.Id,
                dto.OrderId,
                dto.CustomerDocument,
                dto.OrderedAt,
                vehicle,
                status,
                dto.Observation
            );
        }

        public static VehicleOrderDto ToDatabaseDto(this Order order)
        {
            return new VehicleOrderDto
            {
                Id = order.Id,
                OrderId = order.OrderId,
                CustomerDocument = order.CustomerDocument,
                OrderedAt = order.OrderedAt,
                VehicleId = order.Vehicle.VehicleId,
                CarName = order.Vehicle.CarName,
                Price = order.Vehicle.Price,
                Status = order.Status.ToString(),
                Observation = order.Observations
            };
        }

    }

}