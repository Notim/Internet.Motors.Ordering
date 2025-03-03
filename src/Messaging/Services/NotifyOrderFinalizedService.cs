using Core.Application.Services.NotifyOrderFinalized;
using Infrastructure.Messaging.Producer;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Services;

public class NotifyOrderFinalizedService : INotifyOrderFinalizedService
{

    private readonly ILogger<NotifyOrderFinalizedService> _logger;
    private readonly IKafkaProducer<OrderFinalized> _kafkaProducer;

    public NotifyOrderFinalizedService(
        ILogger<NotifyOrderFinalizedService> logger, 
        IKafkaProducer<OrderFinalized> kafkaProducer
    )
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }

    public async Task NotifyOrderFinalizedAsync(OrderFinalized orderFinalized, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting to notify order finalized. OrderId: {OrderId}", orderFinalized.OrderId);

            var sent = await _kafkaProducer.ProduceAsync(
                new Envelop<OrderFinalized>(
                    key: orderFinalized.OrderId.ToString(),
                    topic: Topics.OrderFinalizedTopic,
                    value: orderFinalized,
                    headers: new Dictionary<string, byte[]>()
                ),
                cancellationToken
            );

            if (!sent)
                throw new Exception($"Failed to publish the order finalized notification. OrderId: {orderFinalized.OrderId}");

            _logger.LogInformation("Successfully published the order finalized notification for OrderId: {OrderId}", orderFinalized.OrderId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Order finalized notification operation was canceled. OrderId: {OrderId}", orderFinalized.OrderId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while notifying the order finalized. OrderId: {OrderId}", orderFinalized.OrderId);
            throw;
        }
    }

}