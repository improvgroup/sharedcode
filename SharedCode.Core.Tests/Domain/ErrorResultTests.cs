namespace SharedCode.Tests.Domain;

using SharedCode.Domain;

using System.Net;
using System.Threading.Tasks;

using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Additional tests for error result types.
/// </summary>
public class ErrorResultTests
{
    [Test]
    public async Task ErrorResult_WithNullErrors_UsesEmptyCollection()
    {
        // Arrange
        var result = new ErrorResult("message", null!);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Message).IsEqualTo("message");
        await Assert.That(result.Errors.Count).IsEqualTo(0);
    }

    [Test]
    public async Task GenericErrorResult_WithErrors_SetsProperties()
    {
        // Arrange
        Error[] errors = [new("E001", "first")];

        // Act
        var result = new ErrorResult<int>("message", errors);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Message).IsEqualTo("message");
        await Assert.That(result.Value).IsEqualTo(default(int));
        await Assert.That(result.Errors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task HttpErrorResult_WithErrors_SetsStatusCode()
    {
        // Arrange
        Error[] errors = [new("E001", "first")];

        // Act
        var result = new HttpErrorResult("message", errors, HttpStatusCode.BadRequest);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Message).IsEqualTo("message");
        await Assert.That(result.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(result.Errors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ValidationErrorResult_WithValidationErrors_PreservesErrors()
    {
        // Arrange
        ValidationError[] errors = [new("Name", "Required")];

        // Act
        var result = new ValidationErrorResult("validation failed", errors);

        // Assert
        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Message).IsEqualTo("validation failed");
        await Assert.That(result.Errors.Count).IsEqualTo(1);
        await Assert.That(result.Errors.First().Code).IsEqualTo("Name");
    }
}
