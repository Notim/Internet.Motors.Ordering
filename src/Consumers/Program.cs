using System.Globalization;
using Confluent.Kafka;
using Core.Application.CommandsHandlers.CreateNewOrder;
using Core.Application.Services.CreatePaymentLink;
using Core.Application.Services.NotifyOrderCanceled;
using Core.Application.Services.NotifyOrderFinalized;
using Core.Domain;
using Infrastructure.Data.Configs;
using Infrastructure.Data.Dao;
using Infrastructure.Data.Repositories;
using Infrastructure.Messaging.Producer;
using Infrastructure.Messaging.Services;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Presentation.Consumers.HostedServices;
using Serilog;
using StackExchange.Redis;

namespace Presentation.Consumers;

internal class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog(
            (context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            }
        );

        builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(nameof(DatabaseConfig)));

        builder.Services.AddSingleton<IConnectionMultiplexer>(
            sp =>
            {
                var configuration = builder.Configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(configuration);
            }
        );

        builder.Services.AddScoped<IVehicleOrderDao, VehicleOrderDao>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
        builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

        builder.Services.AddScoped<IKafkaProducer<CreatePaymentLink>, MessageProducer<CreatePaymentLink>>();
        builder.Services.AddScoped<IKafkaProducer<OrderCanceled>, MessageProducer<OrderCanceled>>();
        builder.Services.AddScoped<IKafkaProducer<OrderFinalized>, MessageProducer<OrderFinalized>>();

        builder.Services.AddScoped<ICreatePaymentLinkService, CreatePaymentLinkService>();
        builder.Services.AddScoped<INotifyOrderFinalizedService, NotifyOrderFinalizedService>();
        builder.Services.AddScoped<INotifyOrderCanceledService, NotifyOrderCanceledService>();

        builder.Services.AddMediatR(typeof(CreateNewOrderCommandHandler).Assembly);

        builder.Services.AddHostedService<CreateNewOrderConsumer>();
        builder.Services.AddHostedService<CancelOrderConsumer>();
        builder.Services.AddHostedService<FinalizeOrderConsumer>();

        var app = builder.Build();

        var defaultCulture = "en-US";

        var supportedCultures = new[]
        {
            new CultureInfo(defaultCulture),
            new CultureInfo("pt-BR")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        app.UseRequestLocalization(localizationOptions);

        app.Run();
    }

}