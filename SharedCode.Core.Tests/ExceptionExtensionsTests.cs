namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="ExceptionExtensions"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class ExceptionExtensionsTests
{
	[TestMethod]
	public void AddData_PopulatesExceptionData()
	{
		var exception = new InvalidOperationException("test");
		var dictionary = new Hashtable { { "key1", "value1" }, { "key2", "value2" } };
		exception.AddData(dictionary);
		Assert.IsTrue(exception.Data.Contains("key1"));
		Assert.IsTrue(exception.Data.Contains("key2"));
	}

	[TestMethod]
	public void AddData_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => ex!.AddData(new Hashtable()));
	}

	[TestMethod]
	public void AddData_NullDictionary_DoesNotThrow()
	{
		var exception = new InvalidOperationException("test");
		exception.AddData(null!);
		Assert.AreEqual(0, exception.Data.Count);
	}

	[TestMethod]
	public void AddOrUpdateData_AddsNewKey()
	{
		var exception = new InvalidOperationException("test");
		exception.AddOrUpdateData("key1", "value1");
		Assert.IsTrue(exception.Data.Contains("key1"));
	}

	[TestMethod]
	public void AddOrUpdateData_UpdatesExistingKey()
	{
		var exception = new InvalidOperationException("test");
		exception.AddOrUpdateData("key1", "value1");
		exception.AddOrUpdateData("key1", "value2");
		var values = exception.Data["key1"] as List<string>;
		Assert.IsNotNull(values);
		Assert.AreEqual(2, values!.Count);
		Assert.IsTrue(values.Contains("value1"));
		Assert.IsTrue(values.Contains("value2"));
	}

	[TestMethod]
	public void AddOrUpdateData_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => ex!.AddOrUpdateData("key", "value"));
	}

	[TestMethod]
	public void DataEquals_BothEmpty_ReturnsTrue()
	{
		var ex1 = new InvalidOperationException("test");
		var ex2 = new InvalidOperationException("test");
		Assert.IsTrue(ex1.DataEquals(ex2.Data));
	}

	[TestMethod]
	public void DataEquals_NullDictionary_EmptyData_ReturnsTrue()
	{
		var ex = new InvalidOperationException("test");
		Assert.IsTrue(ex.DataEquals(null));
	}

	[TestMethod]
	public void DataEquals_NullDictionary_WithData_ReturnsFalse()
	{
		var ex = new InvalidOperationException("test");
		ex.AddOrUpdateData("key1", "value1");
		Assert.IsFalse(ex.DataEquals(null));
	}

	[TestMethod]
	public void SameExceptionAs_SameExceptions_ReturnsTrue()
	{
		var ex1 = new InvalidOperationException("test message");
		var ex2 = new InvalidOperationException("test message");
		Assert.IsTrue(ex1.SameExceptionAs(ex2));
	}

	[TestMethod]
	public void SameExceptionAs_DifferentMessages_ReturnsFalse()
	{
		var ex1 = new InvalidOperationException("message 1");
		var ex2 = new InvalidOperationException("message 2");
		Assert.IsFalse(ex1.SameExceptionAs(ex2));
	}

	[TestMethod]
	public void SameExceptionAs_DifferentTypes_ReturnsFalse()
	{
		var ex1 = new InvalidOperationException("test");
		var ex2 = new ArgumentException("test");
		Assert.IsFalse(ex1.SameExceptionAs(ex2));
	}

	[TestMethod]
	public void SameExceptionAs_BothNull_ReturnsTrue()
	{
		Assert.IsTrue(((Exception?)null)!.SameExceptionAs(null!));
	}

	[TestMethod]
	public void ThrowIfContainsErrors_WithData_ThrowsException()
	{
		var ex = new InvalidOperationException("test");
		ex.Data.Add("key", "value");
		_ = Assert.ThrowsExactly<InvalidOperationException>(() => ex.ThrowIfContainsErrors());
	}

	[TestMethod]
	public void ThrowIfContainsErrors_WithoutData_DoesNotThrow()
	{
		var ex = new InvalidOperationException("test");
		ex.ThrowIfContainsErrors(); // Should not throw
	}

	[TestMethod]
	public void ThrowIfContainsErrors_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => ex!.ThrowIfContainsErrors());
	}

	[TestMethod]
	public void ToLogString_WithMessage_ContainsExceptionMessage()
	{
		var ex = new InvalidOperationException("test error");
		var log = ex.ToLogString("additional message");
		Assert.IsTrue(log.Contains("test error", StringComparison.Ordinal));
		Assert.IsTrue(log.Contains("additional message", StringComparison.Ordinal));
	}

	[TestMethod]
	public void ToLogString_NullException_ReturnsNonNullString()
	{
		var log = ((Exception?)null).ToLogString("message");
		Assert.IsNotNull(log);
		Assert.IsTrue(log.Contains("message", StringComparison.Ordinal));
	}

	[TestMethod]
	public void ToLogString_NoAdditionalMessage_ReturnsExceptionInfo()
	{
		var ex = new InvalidOperationException("error message");
		var log = ex.ToLogString(null);
		Assert.IsTrue(log.Contains("error message", StringComparison.Ordinal));
	}
}
