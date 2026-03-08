namespace SharedCode.Tests.Domain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Domain;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the Domain result types.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class ResultTests
{
	[TestMethod]
	public void Result_ParameterlessConstructor_HasFalseSuccess()
	{
		// Note: for readonly record struct, new Result() calls the parameterless struct
		// constructor which zero-initializes all fields. To get Success=true, you must
		// pass the parameter explicitly.
		var result = new Result();
		Assert.IsFalse(result.Success);
	}

	[TestMethod]
	public void Result_SuccessTrue_IsSuccessful()
	{
		var result = new Result(Success: true);
		Assert.IsTrue(result.Success);
	}

	[TestMethod]
	public void Result_SuccessFalse_IsNotSuccessful()
	{
		var result = new Result(Success: false);
		Assert.IsFalse(result.Success);
	}

	[TestMethod]
	public void ResultT_WithValue_IsSuccessful()
	{
		var result = new Result<string>("hello");
		Assert.IsTrue(result.Success);
		Assert.AreEqual("hello", result.Value);
	}

	[TestMethod]
	public void ResultT_DirectConstructorWithNullValue_UsesDefaultSuccessTrue()
	{
		// When calling the constructor directly with null, the default success=true is used
		var result = new Result<string>((string?)null);
		Assert.IsTrue(result.Success);
		Assert.IsNull(result.Value);
	}

	[TestMethod]
	public void ResultT_ToResult_NullValue_ReturnsFailed()
	{
		var result = Result<string>.ToResult(null);
		Assert.IsFalse(result.Success);
		Assert.IsNull(result.Value);
	}

	[TestMethod]
	public void ResultT_ImplicitConversion_FromValue_IsSuccessful()
	{
		Result<int> result = 42;
		Assert.IsTrue(result.Success);
		Assert.AreEqual(42, result.Value);
	}

	[TestMethod]
	public void ResultT_ToResult_NonNullValue_ReturnsSuccess()
	{
		var result = Result<string>.ToResult("value");
		Assert.IsTrue(result.Success);
		Assert.AreEqual("value", result.Value);
	}

	[TestMethod]
	public void ResultT_ExplicitFailure_IsNotSuccessful()
	{
		var result = new Result<string>("value", success: false);
		Assert.IsFalse(result.Success);
	}

	[TestMethod]
	public void Error_WithCodeAndDetails_SetsProperties()
	{
		var error = new Error("E001", "Something went wrong");
		Assert.AreEqual("E001", error.Code);
		Assert.AreEqual("Something went wrong", error.Details);
	}

	[TestMethod]
	public void Error_WithDetailsOnly_CodeIsNull()
	{
		var error = new Error("Something went wrong");
		Assert.IsNull(error.Code);
		Assert.AreEqual("Something went wrong", error.Details);
	}

	[TestMethod]
	public void ValidationError_SetsPropertyName()
	{
		var error = new ValidationError("Name", "Name is required");
		Assert.AreEqual("Name", error.PropertyName);
		Assert.AreEqual("Name", error.Code);
		Assert.AreEqual("Name is required", error.Details);
	}

	[TestMethod]
	public void ErrorResult_WithMessage_IsNotSuccessful()
	{
		var result = new ErrorResult("An error occurred");
		Assert.IsFalse(result.Success);
		Assert.AreEqual("An error occurred", result.Message);
		Assert.AreEqual(0, result.Errors.Count);
	}

	[TestMethod]
	public void ErrorResult_WithErrors_ContainsErrors()
	{
		var errors = new List<Error> { new("E001", "Error 1"), new("E002", "Error 2") };
		var result = new ErrorResult("Multiple errors", errors);
		Assert.IsFalse(result.Success);
		Assert.AreEqual(2, result.Errors.Count);
	}

	[TestMethod]
	public void ErrorResult_NullErrors_UsesEmptyCollection()
	{
		var result = new ErrorResult("An error", null!);
		Assert.IsNotNull(result.Errors);
		Assert.AreEqual(0, result.Errors.Count);
	}

	[TestMethod]
	public void ValidationErrorResult_WithMessage_IsNotSuccessful()
	{
		var result = new ValidationErrorResult("Validation failed");
		Assert.IsFalse(result.Success);
		Assert.AreEqual("Validation failed", result.Message);
	}

	[TestMethod]
	public void ValidationErrorResult_WithValidationErrors_ContainsErrors()
	{
		var errors = new List<ValidationError>
		{
			new("Name", "Name is required"),
			new("Email", "Invalid email"),
		};
		var result = new ValidationErrorResult("Validation failed", errors);
		Assert.AreEqual(2, result.Errors.Count);
	}
}
