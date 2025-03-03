using Core.Domain;

namespace Core.Application.CommandsHandlers.CreateNewOrder;

public static class CreateNewOrderMappers
{

    public static Order MapToDomain(this CreateNewOrderCommand request)
    {
        var order = new Order(
            id: 0,
            orderId: request.OrderId,
            customerDocument: request.CustomerDocument,
            orderedAt: request.OrderedAt,
            vehicle: new Vehicle(
                vehicleId: request.VehicleId,
                carName: request.CarName, 
                price: request.Price
            ), 
            status: OrderStatus.Pending,
            observations: string.Empty
        );
        return order;
    }

}