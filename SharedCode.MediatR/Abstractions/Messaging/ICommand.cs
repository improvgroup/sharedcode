namespace SharedCode.MediatR.Abstractions.Messaging;

using global::MediatR;

using SharedCode.Domain;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The CQRS command interface. Implements the <see cref="IRequest{Result}" />.
/// </summary>
/// <seealso cref="IRequest{Result}" />
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used as a marker interface for CQRS pattern.")]
public interface ICommand : IRequest<Result>
{
}
