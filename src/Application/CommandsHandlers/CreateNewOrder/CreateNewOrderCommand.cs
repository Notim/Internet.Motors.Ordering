using MediatR;
using Notim.Outputs;

namespace Core.Application.CommandsHandlers.CreateNewOrder;

public record CreateNewOrderCommand : IRequest<Output>
{

    public Guid VehicleId { get; set; }

    public Guid OrderId { get; set; }

    public string CustomerDocument { get; set; } = string.Empty;

    public string CarName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime OrderedAt { get; set; }

}