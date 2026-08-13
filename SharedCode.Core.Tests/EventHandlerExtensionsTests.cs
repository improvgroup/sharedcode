namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for <see cref="EventHandlerExtensions" />.
/// </summary>
[TestClass]
public class EventHandlerExtensionsTests
{
    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise(EventHandler, object)" /> invokes the
    /// handler with <see cref="EventArgs.Empty" />.
    /// </summary>
    [TestMethod]
    public void RaiseNonGeneric_HandlerIsNotNull_InvokesHandler()
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
        Assert.AreSame(sender, capturedSender);
        Assert.AreSame(EventArgs.Empty, capturedArgs);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise(EventHandler, object)" /> does not
    /// throw when the handler is null.
    /// </summary>
    [TestMethod]
    public void RaiseNonGeneric_HandlerIsNull_DoesNotThrow()
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
    [TestMethod]
    public void RaiseGenericValue_HandlerIsNotNull_InvokesHandlerWithValue()
    {
        // Arrange
        int? capturedValue = null;
        EventHandler<EventArgs<int>> handler = (_, e) => capturedValue = e.Value;
        var sender = new object();

        // Act
        handler.Raise(sender, 42);

        // Assert
        Assert.AreEqual(42, capturedValue);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise{T}(EventHandler{T}, object, T)" />
    /// invokes the handler with the supplied <see cref="EventArgs" />.
    /// </summary>
    [TestMethod]
    public void RaiseGenericEventArgs_HandlerIsNotNull_InvokesHandler()
    {
        // Arrange
        EventArgs? capturedArgs = null;
        var args = new EventArgs();
        EventHandler<EventArgs> handler = (_, e) => capturedArgs = e;

        // Act
        handler.Raise(new object(), args);

        // Assert
        Assert.AreSame(args, capturedArgs);
    }
}
