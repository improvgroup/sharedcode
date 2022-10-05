namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

/// <summary>
/// The query interface. Implements the <see cref="IRequest{T}" />.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <seealso cref="IRequest{T}" />
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "This is a marker interface for the CQRS pattern.")]
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
