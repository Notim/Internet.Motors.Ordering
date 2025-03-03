using Core.Application.Services.NotifyOrderCanceled;
using MediatR;
using Notim.Outputs;
using Core.Domain;

namespace Core.Application.CommandsHandlers.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Output>
{

    private readonly IOrderRepository _orderRepository;
    private readonly INotifyOrderCanceledService _notifyOrderCanceledService;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository, 
        INotifyOrderCanceledService notifyOrderCanceledService
    )
    {
        _orderRepository = orderRepository;
        _notifyOrderCanceledService = notifyOrderCanceledService;
    }

    public async Task<Output> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var output = new Output();
        
        var order = await _orderRepository.GetOrderByOrderId(request.OrderId, cancellationToken);
        if (order == null)
        {
            output.AddFault(new Fault(FaultType.ResourceNotFound, "Order not found"));
            return output;
        }

        order.CancelOrder(request.Reason);

        var updateSuccess = await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        if (!updateSuccess)
        {
            output.AddFault(new Fault(FaultType.GenericError, "Failed to update order"));
            return output;
        }
        
        await _notifyOrderCanceledService.NotifyOrderCanceledAsync(
            new OrderCanceled(
                vehicleId: order.Vehicle.VehicleId,
                orderId: order.OrderId,
                reason: order.Observations
            ), 
            cancellationToken
        );
        
        output.AddMessage("Order cancelled with success");
        
        return output;
    }

}