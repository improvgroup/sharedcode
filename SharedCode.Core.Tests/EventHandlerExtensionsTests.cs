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
    public void Raise_HandlerIsNotNull_InvokesHandler()
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
        capturedSender.Should().BeSameAs(sender);
        capturedArgs.Should().BeSameAs(EventArgs.Empty);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise(EventHandler, object)" /> does not
    /// throw when the handler is null.
    /// </summary>
    [TestMethod]
    public void Raise_HandlerIsNull_DoesNotThrow()
    {
        // Arrange
        EventHandler? handler = null;

        // Act
        var act = () => handler.Raise(new object());

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise{T}(EventHandler{EventArgs{T}}, object, T)" />
    /// invokes the handler with the expected value wrapped in <see cref="EventArgs{T}" />.
    /// </summary>
    [TestMethod]
    public void Raise_Generic_HandlerIsNotNull_InvokesHandlerWithValue()
    {
        // Arrange
        int? capturedValue = null;
        EventHandler<EventArgs<int>> handler = (_, e) => capturedValue = e.Value;
        var sender = new object();

        // Act
        handler.Raise(sender, 42);

        // Assert
        capturedValue.Should().Be(42);
    }

    /// <summary>
    /// Tests that <see cref="EventHandlerExtensions.Raise{T}(EventHandler{T}, object, T)" />
    /// invokes the handler with the supplied <see cref="EventArgs" />.
    /// </summary>
    [TestMethod]
    public void Raise_GenericEventArgs_HandlerIsNotNull_InvokesHandler()
    {
        // Arrange
        EventArgs? capturedArgs = null;
        var args = new EventArgs();
        EventHandler<EventArgs> handler = (_, e) => capturedArgs = e;

        // Act
        handler.Raise(new object(), args);

        // Assert
        capturedArgs.Should().BeSameAs(args);
    }
}
