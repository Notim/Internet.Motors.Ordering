namespace Core.Application.Services.NotifyOrderCanceled;

public interface INotifyOrderCanceledService
{

    Task NotifyOrderCanceledAsync(OrderCanceled orderCanceled, CancellationToken cancellationToken);

}