﻿using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS
{
    public interface ICommandHandler<in TCommand> where TCommand : class
    {
        Task HandleAsync(TCommand command);
    }
}
