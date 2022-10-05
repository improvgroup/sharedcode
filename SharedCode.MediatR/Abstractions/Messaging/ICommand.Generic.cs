namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The command interface. Implements the <see cref="IRequest{T}" />.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a marker interface for CQRS pattern.")]
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
