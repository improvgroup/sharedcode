namespace SharedCode.Tests.Domain;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Domain;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the Domain result types.
/// </summary>
public class ResultTests
{
	[Test]
	public async Task Result_ParameterlessConstructor_HasFalseSuccess()
	{
		// Note: for readonly record struct, new Result() calls the parameterless struct
		// constructor which zero-initializes all fields. To get Success=true, you must
		// pass the parameter explicitly.
		var result = new Result();
		await Assert.That(result.Success).IsFalse();
	}

	[Test]
	public async Task Result_SuccessTrue_IsSuccessful()
	{
		var result = new Result(Success: true);
		await Assert.That(result.Success).IsTrue();
	}

	[Test]
	public async Task Result_SuccessFalse_IsNotSuccessful()
	{
		var result = new Result(Success: false);
		await Assert.That(result.Success).IsFalse();
	}

	[Test]
	public async Task ResultT_WithValue_IsSuccessful()
	{
		var result = new Result<string>("hello");
		await Assert.That(result.Success).IsTrue();
		await Assert.That(result.Value).IsEqualTo("hello");
	}

	[Test]
	public async Task ResultT_DirectConstructorWithNullValue_UsesDefaultSuccessTrue()
	{
		// When calling the constructor directly with null, the default success=true is used
		var result = new Result<string>((string?)null);
		await Assert.That(result.Success).IsTrue();
		await Assert.That(result.Value is null).IsTrue();
	}

	[Test]
	public async Task ResultT_ToResult_NullValue_ReturnsFailed()
	{
		var result = Result<string>.ToResult(null);
		await Assert.That(result.Success).IsFalse();
		await Assert.That(result.Value is null).IsTrue();
	}

	[Test]
	public async Task ResultT_ImplicitConversion_FromValue_IsSuccessful()
	{
		Result<int> result = 42;
		await Assert.That(result.Success).IsTrue();
		await Assert.That(result.Value).IsEqualTo(42);
	}

	[Test]
	public async Task ResultT_ToResult_NonNullValue_ReturnsSuccess()
	{
		var result = Result<string>.ToResult("value");
		await Assert.That(result.Success).IsTrue();
		await Assert.That(result.Value).IsEqualTo("value");
	}

	[Test]
	public async Task ResultT_ExplicitFailure_IsNotSuccessful()
	{
		var result = new Result<string>("value", success: false);
		await Assert.That(result.Success).IsFalse();
	}

	[Test]
	public async Task Error_WithCodeAndDetails_SetsProperties()
	{
		var error = new Error("E001", "Something went wrong");
		await Assert.That(error.Code).IsEqualTo("E001");
		await Assert.That(error.Details).IsEqualTo("Something went wrong");
	}

	[Test]
	public async Task Error_WithDetailsOnly_CodeIsNull()
	{
		var error = new Error("Something went wrong");
		await Assert.That(error.Code is null).IsTrue();
		await Assert.That(error.Details).IsEqualTo("Something went wrong");
	}

	[Test]
	public async Task ValidationError_SetsPropertyName()
	{
		var error = new ValidationError("Name", "Name is required");
		await Assert.That(error.PropertyName).IsEqualTo("Name");
		await Assert.That(error.Code).IsEqualTo("Name");
		await Assert.That(error.Details).IsEqualTo("Name is required");
	}

	[Test]
	public async Task ErrorResult_WithMessage_IsNotSuccessful()
	{
		var result = new ErrorResult("An error occurred");
		await Assert.That(result.Success).IsFalse();
		await Assert.That(result.Message).IsEqualTo("An error occurred");
		await Assert.That(result.Errors.Count).IsEqualTo(0);
	}

	[Test]
	public async Task ErrorResult_WithErrors_ContainsErrors()
	{
		var errors = new List<Error> { new("E001", "Error 1"), new("E002", "Error 2") };
		var result = new ErrorResult("Multiple errors", errors);
		await Assert.That(result.Success).IsFalse();
		await Assert.That(result.Errors.Count).IsEqualTo(2);
	}

	[Test]
	public async Task ErrorResult_NullErrors_UsesEmptyCollection()
	{
		var result = new ErrorResult("An error", null!);
		await Assert.That(result.Errors.Count).IsEqualTo(0);
	}

	[Test]
	public async Task ValidationErrorResult_WithMessage_IsNotSuccessful()
	{
		var result = new ValidationErrorResult("Validation failed");
		await Assert.That(result.Success).IsFalse();
		await Assert.That(result.Message).IsEqualTo("Validation failed");
	}

	[Test]
	public async Task ValidationErrorResult_WithValidationErrors_ContainsErrors()
	{
		var errors = new List<ValidationError>
		{
			new("Name", "Name is required"),
			new("Email", "Invalid email"),
		};
		var result = new ValidationErrorResult("Validation failed", errors);
		await Assert.That(result.Errors.Count).IsEqualTo(2);
	}
}
