namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="EventHandlerExtensions" />.
/// </summary>
public class EventHandlerExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise(EventHandler, object)" /> invokes the
    /// handler with <see cref="EventArgs.Empty" />.
    /// </summary>
    [Test]
    public async Task EventHandler_InvokesHandlerWithEmptyArgs()
    {
        // Arrange
        object? capturedSender = null;
        EventArgs? capturedArgs = null;
        EventHandler handler = (s, e) =>
        {
            capturedSender = s;
            capturedArgs = e;
        };
        var sender = new object();

        // Act
        handler.Raise(sender);

        // Assert
        await Assert.That(capturedSender).IsSameReferenceAs(sender);
        await Assert.That(capturedArgs).IsSameReferenceAs(EventArgs.Empty);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise(EventHandler, object)" /> does not
    /// throw when the handler is null.
    /// </summary>
    [Test]
    public void EventHandler_NullHandler_DoesNotThrow()
    {
        // Arrange
        EventHandler? handler = null;

        // Act / Assert — should not throw
#pragma warning disable CS8604 // Possible null reference argument — intentional null test
        handler!.Raise(new object());
#pragma warning restore CS8604
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise{T}(EventHandler{EventArgs{T}}, object, T)" />
    /// invokes the handler with the expected value wrapped in <see cref="EventArgs{T}" />.
    /// </summary>
    [Test]
    public async Task EventHandlerOfEventArgsT_InvokesHandlerWithWrappedValue()
    {
        // Arrange
        int? capturedValue = null;
        EventHandler<EventArgs<int>> handler = (_, e) => capturedValue = e.Value;
        var sender = new object();

        // Act
        handler.Raise(sender, 42);

        // Assert
        await Assert.That(capturedValue).IsEqualTo(42);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise{T}(EventHandler{T}, object, T)" />
    /// invokes the handler with the supplied <see cref="EventArgs" />.
    /// </summary>
    [Test]
    public async Task EventHandlerOfT_InvokesHandlerWithSuppliedArgs()
    {
        // Arrange
        EventArgs? capturedArgs = null;
        var args = new EventArgs();
        EventHandler<EventArgs> handler = (_, e) => capturedArgs = e;

        // Act
        handler.Raise(new object(), args);

        // Assert
        await Assert.That(capturedArgs).IsSameReferenceAs(args);
    }
}
