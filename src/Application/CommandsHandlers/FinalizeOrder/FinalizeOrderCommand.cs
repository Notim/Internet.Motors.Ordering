using MediatR;
using Notim.Outputs;

namespace Core.Application.CommandsHandlers.FinalizeOrder;

public record FinalizeOrderCommand : IRequest<Output>
{

    public Guid OrderId { get; set; }

}