using Core.Application.Services.CreatePaymentLink;
using Core.Domain;
using MediatR;
using Notim.Outputs;
using Microsoft.Extensions.Logging;

namespace Core.Application.CommandsHandlers.CreateNewOrder;

public class CreateNewOrderCommandHandler : IRequestHandler<CreateNewOrderCommand, Output>
{

    private readonly ILogger<CreateNewOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly ICreatePaymentLinkService _createPaymentLinkService;

    public CreateNewOrderCommandHandler(
        ILogger<CreateNewOrderCommandHandler> logger,
        IOrderRepository orderRepository,
        ICreatePaymentLinkService createPaymentLinkService
    )
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _createPaymentLinkService = createPaymentLinkService;
    }

    public async Task<Output> Handle(CreateNewOrderCommand request, CancellationToken cancellationToken)
    {
        var output = new Output();
        
        _logger.LogInformation("Handling CreateNewOrderCommand for OrderId: {OrderId}", request.OrderId);

        var order = request.MapToDomain();
        
        var orderId = await _orderRepository.InsertOrderAsync(order, cancellationToken);
        if (orderId >= 0)
        {
            output.AddFault(new Fault(FaultType.GenericError, "Could not create order"));
            return output;
        }
        
        _logger.LogInformation("Order created successfully for OrderId: {OrderId}", request.OrderId);

        var createPaymentLink = new CreatePaymentLink(
            orderId: order.OrderId,
            customerDocument: order.CustomerDocument, 
            totalAmount: order.Vehicle.Price, 
            description: $"Payment for {order.Vehicle.CarName} total amount {order.Vehicle.Price}"
        );
        
        await _createPaymentLinkService.CreateNewPaymentLink(createPaymentLink, cancellationToken);
        
        _logger.LogInformation("Payment creation message sent for OrderId: {OrderId}", request.OrderId);

        output.AddMessage($"Order created successfully {orderId}");
        
        return output;
    }

}