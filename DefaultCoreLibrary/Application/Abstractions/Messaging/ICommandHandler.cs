using DefaultCoreLibrary.Core;

namespace DefaultCoreLibrary.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<VoidResult> Handle(TCommand command, CancellationToken cancellationToken);
}


public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}