namespace Core.Application.Services.NotifyOrderFinalized;

public interface INotifyOrderFinalizedService
{

    Task NotifyOrderFinalizedAsync(OrderFinalized orderCanceled, CancellationToken cancellationToken);

}