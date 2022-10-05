namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

/// <summary>
/// The command handler interface. Implements the <see cref="IRequestHandler{T}" />.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <seealso cref="IRequestHandler{T}" />
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}
