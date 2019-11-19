﻿// Copyright (c) 2019 Sergio Aquilini
// This code is licensed under MIT license (see LICENSE file for details)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Examples.Common.Messages;
using Silverback.Messaging;
using Silverback.Messaging.Configuration;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Publishing;

namespace Silverback.Examples.Main.UseCases.Advanced
{
    public class HeadersUseCase : UseCase
    {
        public HeadersUseCase() : base("Add message headers via custom behavior", 50)
        {
        }

        protected override void ConfigureServices(IServiceCollection services) => services
            .AddSilverback()
            .UseModel()
            .WithConnectionToKafka()
            .AddSingletonBehavior<CustomHeadersBehavior>();

        protected override void Configure(BusConfigurator configurator, IServiceProvider serviceProvider) =>
            configurator.Connect(endpoints => endpoints
                .AddOutbound<IIntegrationEvent>(new KafkaProducerEndpoint("silverback-examples-events")
                {
                    Configuration = new KafkaProducerConfig
                    {
                        BootstrapServers = "PLAINTEXT://localhost:9092",
                        ClientId = GetType().FullName
                    }
                }));

        protected override async Task Execute(IServiceProvider serviceProvider)
        {
            var publisher = serviceProvider.GetService<IEventPublisher>();

            await publisher.PublishAsync(new SimpleIntegrationEvent { Content = DateTime.Now.ToString("HH:mm:ss.fff") });
        }

        public class CustomHeadersBehavior : IBehavior
        {
            public async Task<IEnumerable<object>> Handle(IEnumerable<object> messages, MessagesHandler next)
            {
                foreach (var message in messages.OfType<IOutboundMessage>())
                {
                    message.Headers.Add("generated-by", "silverback");
                    message.Headers.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }

                return await next(messages);
            }
        }
    }
}