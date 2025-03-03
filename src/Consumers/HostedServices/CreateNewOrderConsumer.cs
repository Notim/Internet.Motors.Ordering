using Confluent.Kafka;
using Core.Application.CommandsHandlers.CreateNewOrder;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Consumer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Presentation.Consumers.HostedServices;

public class CreateNewOrderConsumer : KafkaConsumer<CreateNewOrderCommand>
{

    private readonly ILogger<KafkaConsumer<CreateNewOrderCommand>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CreateNewOrderConsumer(
        IOptions<ConsumerConfig> consumerConfig, 
        ILogger<KafkaConsumer<CreateNewOrderCommand>> logger,
        IServiceProvider serviceProvider
    )
        : base(consumerConfig, logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override string GetTopic() => Topics.CarReserved;

    protected override async Task HandleAsync(Envelop<CreateNewOrderCommand> envelop, CancellationToken cancellationToken)
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