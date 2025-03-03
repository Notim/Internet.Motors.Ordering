using Core.Application.Services.CreatePaymentLink;
using Infrastructure.Messaging.Producer;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Services;

public class CreatePaymentLinkService : ICreatePaymentLinkService
{
    private readonly ILogger<CreatePaymentLinkService> _logger;
    private readonly IKafkaProducer<CreatePaymentLink> _kafkaProducer;

    public CreatePaymentLinkService(
        ILogger<CreatePaymentLinkService> logger,
        IKafkaProducer<CreatePaymentLink> kafkaProducer
    )
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
    }

    public async Task CreateNewPaymentLink(CreatePaymentLink createPaymentLink, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting to create a new payment link. PaymentLinkId: {PaymentLinkId}", createPaymentLink.OrderId);

            var sent = await _kafkaProducer.ProduceAsync(
                new Envelop<CreatePaymentLink>(
                    key: createPaymentLink.OrderId.ToString(),
                    topic: Topics.OrderCreatedTopic,
                    value: createPaymentLink,
                    headers: new Dictionary<string, byte[]>()
                ),
                cancellationToken
            );

            if (!sent)
                throw new Exception($"Failed to publish the payment link. PaymentLinkId: {createPaymentLink.OrderId}");

            _logger.LogInformation("Successfully published the payment link for Customer: {CustomerId}", createPaymentLink.OrderId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Payment link publishing operation was canceled. PaymentLinkId: {PaymentLinkId}", createPaymentLink.OrderId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while publishing the payment link. PaymentLinkId: {PaymentLinkId}", createPaymentLink.OrderId);
            throw;
        }
    }
}