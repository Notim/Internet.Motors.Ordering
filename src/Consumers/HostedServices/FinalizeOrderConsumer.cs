using Confluent.Kafka;
using Core.Application.CommandsHandlers.FinalizeOrder;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Consumer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Presentation.Consumers.HostedServices;

public class FinalizeOrderConsumer : KafkaConsumer<FinalizeOrderCommand>
{

    private readonly ILogger<KafkaConsumer<FinalizeOrderCommand>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public FinalizeOrderConsumer(
        IOptions<ConsumerConfig> consumerConfig, 
        ILogger<KafkaConsumer<FinalizeOrderCommand>> logger,
        IServiceProvider serviceProvider
    )
        : base(consumerConfig, logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override string GetTopic() => Topics.PaymentConfirmedTopic;

    protected override async Task HandleAsync(Envelop<FinalizeOrderCommand> envelop, CancellationToken cancellationToken)
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