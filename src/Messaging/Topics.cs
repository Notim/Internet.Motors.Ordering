namespace Messaging;

public static class Topics
{

    public static string CarReserved => "car-reserved";

    public static string OrderFinalizedTopic => "order-finalized";

    public static string OrderCanceledTopic => "order-canceled";

    public static string OrderCreatedTopic => "order-created";

    public static string PaymentConfirmedTopic => "payment-confirmed";

    public static string PaymentExpiredTopic => "payment-expired";

}