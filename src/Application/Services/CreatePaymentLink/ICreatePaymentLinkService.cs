namespace Core.Application.Services.CreatePaymentLink;

public interface ICreatePaymentLinkService
{

    Task CreateNewPaymentLink(CreatePaymentLink createPaymentLink, CancellationToken cancellationToken = default);

}