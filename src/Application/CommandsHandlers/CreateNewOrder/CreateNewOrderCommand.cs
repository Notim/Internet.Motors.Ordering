using MediatR;
using Notim.Outputs;

namespace Core.Application.CommandsHandlers.CreateNewOrder;

public record CreateNewOrderCommand : IRequest<Output>
{

    public Guid VehicleId { get; private set; }

    public Guid OrderId { get; private set; }

    public string CustomerDocument { get; private set; } = string.Empty;

    public string CarName { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public DateTime OrderedAt { get; private set; }

}