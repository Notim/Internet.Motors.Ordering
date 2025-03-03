using Core.Application.Services.NotifyOrderFinalized;
using Core.Domain;
using MediatR;
using Notim.Outputs;

namespace Core.Application.CommandsHandlers.FinalizeOrder;

public class FinalizeOrderCommandHandler : IRequestHandler<FinalizeOrderCommand, Output>
{

    private readonly IOrderRepository _orderRepository;
    private readonly INotifyOrderFinalizedService _notifyOrderFinalizedService;

    public FinalizeOrderCommandHandler(
        IOrderRepository orderRepository, 
        INotifyOrderFinalizedService notifyOrderFinalizedService
    )
    {
        _orderRepository = orderRepository;
        _notifyOrderFinalizedService = notifyOrderFinalizedService;
    }

    public async Task<Output> Handle(FinalizeOrderCommand request, CancellationToken cancellationToken)
    {
        var output = new Output();
        
        var order = await _orderRepository.GetOrderByOrderId(request.OrderId, cancellationToken);
        if (order == null)
        {
            output.AddFault(new Fault(FaultType.ResourceNotFound, "Order not found"));
            return output;
        }

        order.FinalizeOrder();

        var updateSuccess = await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        if (!updateSuccess)
        {
            output.AddFault(new Fault(FaultType.GenericError, "Failed to update order"));
            return output;
        }
        
        await _notifyOrderFinalizedService.NotifyOrderFinalizedAsync(
            new OrderFinalized(
                vehicleId: order.Vehicle.VehicleId,
                orderId: order.OrderId
            ), 
            cancellationToken
        );
        
        output.AddMessage("Order finalized with success");
        
        return output;
    }

}