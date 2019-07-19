﻿using System;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Application.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers
{
    public class CreateMenuCommandHandler : ICommandHandler<CreateMenu, Guid>
    {
        private IMenuRepository repository;
        private IApplicationEventPublisher applicationEventPublisher;

        public CreateMenuCommandHandler(IMenuRepository repository, IApplicationEventPublisher applicationEventPublisher)
        {
            this.repository = repository;
            this.applicationEventPublisher = applicationEventPublisher;
        }

        public async Task<Guid> HandleAsync(CreateMenu command)
        {
            var id = Guid.NewGuid();

            //TODO: use a factory method for domain creation
            var newMenu = new Menu()
            {
                Id = id,
                Name = command.Name,
                Description = command.Description,
                RestaurantId = command.RestaurantId,
                Enabled = command.Enabled
            };

            await repository.SaveAsync(newMenu);

            await applicationEventPublisher.PublishAsync(new MenuCreated((OperationCode)command.OperationCode, command.CorrelationId, id));

            return id;
        }
    }
}
