using Confluent.Kafka;
using Core.Application.CommandsHandlers.CancelOrder;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Consumer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Presentation.Consumers.HostedServices;

public class CancelOrderConsumer : KafkaConsumer<CancelOrderCommand>
{

    private readonly ILogger<KafkaConsumer<CancelOrderCommand>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CancelOrderConsumer(
        IOptions<ConsumerConfig> consumerConfig, 
        ILogger<KafkaConsumer<CancelOrderCommand>> logger,
        IServiceProvider serviceProvider
    )
        : base(consumerConfig, logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override string GetTopic() => Topics.PaymentExpiredTopic;

    protected override async Task HandleAsync(Envelop<CancelOrderCommand> envelop, CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                var output = await mediatr.Send(envelop.Value!, cancellationToken);
                _logger.LogInformation("response from use case {@Output} {@Errors}", output.Messages, output.FaultMessages);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occurred during handling of events.");
        }
    }

}