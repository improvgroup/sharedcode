namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

/// <summary>
/// The query handler interface. Implements the <see cref="IRequestHandler{T}" />.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <seealso cref="IRequestHandler{T}" />
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}
