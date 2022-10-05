namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

/// <summary>
/// The command handler interface. Implements the <see cref="IRequestHandler{TCommand, Result}" />.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <seealso cref="IRequestHandler{TCommand, Result}" />
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}
