namespace SharedCode.Tests.Threading;

using SharedCode.Threading.Tasks;

using System;
using System.Threading.Tasks;

using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="TaskExtensions" />.
/// </summary>
public class TaskExtensionsTests
{
    [Test]
    public async Task SafeFireAndForgetAsync_WithCompletedTask_DoesNotInvokeCallback()
    {
        // Arrange
        var invoked = false;

        // Act
        Task.CompletedTask.SafeFireAndForgetAsync(onException: _ => invoked = true);
        await Task.Delay(50).ConfigureAwait(false);

        // Assert
        await Assert.That(invoked).IsFalse();
    }

    [Test]
    public async Task SafeFireAndForgetAsync_WithMatchingGenericException_InvokesCallback()
    {
        // Arrange
        var completion = new TaskCompletionSource<InvalidOperationException>();
        var task = Task.FromException(new InvalidOperationException("boom"));

        // Act
        task.SafeFireAndForgetAsync<InvalidOperationException>(
            continueOnCapturedContext: false,
            onException: exception => completion.TrySetResult(exception));

        var result = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        // Assert
        await Assert.That(result.Message).IsEqualTo("boom");
    }

    [Test]
    public async Task SafeFireAndForgetAsync_WithException_InvokesNonGenericCallback()
    {
        // Arrange
        var completion = new TaskCompletionSource<Exception>();
        var task = Task.FromException(new InvalidOperationException("boom"));

        // Act
        task.SafeFireAndForgetAsync(
            continueOnCapturedContext: false,
            onException: exception => completion.TrySetResult(exception));

        var result = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        // Assert
        await Assert.That(result.Message).IsEqualTo("boom");
        await Assert.That(result).IsTypeOf<InvalidOperationException>();
    }

    [Test]
    public async Task SafeFireAndForgetAsync_WithNullTask_InvokesMatchingGenericCallback()
    {
        // Arrange
        var completion = new TaskCompletionSource<ArgumentNullException>();
        Task? task = null;

        // Act
#pragma warning disable CS8604 // Intentional null task test
        task!.SafeFireAndForgetAsync<ArgumentNullException>(
            continueOnCapturedContext: false,
            onException: exception => completion.TrySetResult(exception));
#pragma warning restore CS8604

        var result = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        // Assert
        await Assert.That(result.ParamName).IsEqualTo("task");
    }
}
