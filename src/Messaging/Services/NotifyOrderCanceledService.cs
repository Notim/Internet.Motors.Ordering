using Core.Application.Services.NotifyOrderCanceled;
using Infrastructure.Messaging.Producer;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Services;

public class NotifyOrderCanceledService : INotifyOrderCanceledService
{

    private readonly ILogger<NotifyOrderCanceledService> _logger;
    private readonly IKafkaProducer<OrderCanceled> _kafkaProducer;

    public NotifyOrderCanceledService(
        ILogger<NotifyOrderCanceledService> logger, 
        IKafkaProducer<OrderCanceled> kafkaProducer
    )
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }

    public async Task NotifyOrderCanceledAsync(OrderCanceled orderCanceled, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting to notify order cancellation. OrderId: {OrderId}", orderCanceled.OrderId);

            var sent = await _kafkaProducer.ProduceAsync(
                new Envelop<OrderCanceled>(
                    key: orderCanceled.OrderId.ToString(),
                    topic: Topics.OrderCanceledTopic,
                    value: orderCanceled,
                    headers: new Dictionary<string, byte[]>()
                ),
                cancellationToken
            );

            if (!sent)
                throw new Exception($"Failed to publish the order cancellation notification. OrderId: {orderCanceled.OrderId}");

            _logger.LogInformation("Successfully published the order cancellation notification for OrderId: {OrderId}", orderCanceled.OrderId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Order cancellation notification operation was canceled. OrderId: {OrderId}", orderCanceled.OrderId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while notifying the order cancellation. OrderId: {OrderId}", orderCanceled.OrderId);
            throw;
        }
    }

}