﻿using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Data.Documents.CosmosDB;
using Amido.Stacks.Data.Documents.CosmosDB.Extensions;
using Amido.Stacks.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure
{
    public static class DependencyRegistration
    {
        /// <summary>
        /// Register static services that does not change between environment or contexts(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureStaticDependencies(IServiceCollection services)
        {
            AddCommandHandlers(services);
            AddQueryHandlers(services);
        }

        /// <summary>
        /// Register dynamic services that changes between environments or context(i.e: tests)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureProductionDependencies(WebHostBuilderContext context, IServiceCollection services)
        {
            services.Configure<CosmosDbConfiguration>(context.Configuration.GetSection("CosmosDB"));

            services.AddSecrets();
            services.AddCosmosDB();

            //TODO: Evaluate if event publishers should be generic, probably not, EventHandler are generic tough
            AddEventPublishers(services);

            services.AddTransient<IMenuRepository, MenuRepository>();
            //services.AddTransient<IMenuRepository, InMemoryMenuRepository>();
        }

        private static void AddCommandHandlers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(ICommandHandler<>).Name}");
            var definitions = typeof(CreateMenuCommandHandler).Assembly.GetImplementationsOf(typeof(ICommandHandler<>), typeof(ICommandHandler<,>));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        private static void AddQueryHandlers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(IQueryHandler<,>).Name}");
            var definitions = typeof(GetMenuByIdQueryHandler).Assembly.GetImplementationsOf(typeof(IQueryHandler<,>));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

        private static void AddEventPublishers(IServiceCollection services)
        {
            System.Console.WriteLine($"Loading implementations of  {typeof(IApplicationEventPublisher).Name}");
            var definitions = typeof(DummyEventPublisher).Assembly.GetImplementationsOf(typeof(IApplicationEventPublisher));
            foreach (var definition in definitions)
            {
                System.Console.WriteLine($"Registering '{definition.implementation.FullName}' as implementation of '{definition.interfaceVariation.FullName}'");
                //TODO: maybe this should be singleton
                services.AddTransient(definition.interfaceVariation, definition.implementation);
            }
        }

    }
}
