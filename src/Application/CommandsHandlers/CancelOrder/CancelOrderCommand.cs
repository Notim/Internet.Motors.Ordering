using MediatR;
using Notim.Outputs;

namespace Core.Application.CommandsHandlers.CancelOrder;

public record CancelOrderCommand : IRequest<Output>
{

    public Guid OrderId { get; set; }

    public string? Reason { get; set; }

}