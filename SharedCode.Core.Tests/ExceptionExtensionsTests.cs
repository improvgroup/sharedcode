namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="ExceptionExtensions"/>.
/// </summary>
public class ExceptionExtensionsTests
{
	[Test]
	public async Task AddData_PopulatesExceptionData()
	{
		var exception = new InvalidOperationException("test");
		var dictionary = new Hashtable { { "key1", "value1" }, { "key2", "value2" } };
		exception.AddData(dictionary);
		await Assert.That(exception.Data.Contains("key1")).IsTrue();
		await Assert.That(exception.Data.Contains("key2")).IsTrue();
	}

	[Test]
	public async Task AddData_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		await Assert.That(() => ex!.AddData(new Hashtable())).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AddData_NullDictionary_DoesNotThrow()
	{
		var exception = new InvalidOperationException("test");
		exception.AddData(null!);
		await Assert.That(exception.Data.Count).IsEqualTo(0);
	}

	[Test]
	public async Task AddOrUpdateData_AddsNewKey()
	{
		var exception = new InvalidOperationException("test");
		exception.AddOrUpdateData("key1", "value1");
		await Assert.That(exception.Data.Contains("key1")).IsTrue();
	}

	[Test]
	public async Task AddOrUpdateData_UpdatesExistingKey()
	{
		var exception = new InvalidOperationException("test");
		exception.AddOrUpdateData("key1", "value1");
		exception.AddOrUpdateData("key1", "value2");
		var values = exception.Data["key1"] as List<string>;
		await Assert.That(values is not null).IsTrue();
		await Assert.That(values!.Count).IsEqualTo(2);
		await Assert.That(values.Contains("value1")).IsTrue();
		await Assert.That(values.Contains("value2")).IsTrue();
	}

	[Test]
	public async Task AddOrUpdateData_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		await Assert.That(() => ex!.AddOrUpdateData("key", "value")).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task DataEquals_BothEmpty_ReturnsTrue()
	{
		var ex1 = new InvalidOperationException("test");
		var ex2 = new InvalidOperationException("test");
		await Assert.That(ex1.DataEquals(ex2.Data)).IsTrue();
	}

	[Test]
	public async Task DataEquals_NullDictionary_EmptyData_ReturnsTrue()
	{
		var ex = new InvalidOperationException("test");
		await Assert.That(ex.DataEquals(null)).IsTrue();
	}

	[Test]
	public async Task DataEquals_NullDictionary_WithData_ReturnsFalse()
	{
		var ex = new InvalidOperationException("test");
		ex.AddOrUpdateData("key1", "value1");
		await Assert.That(ex.DataEquals(null)).IsFalse();
	}

	[Test]
	public async Task SameExceptionAs_SameExceptions_ReturnsTrue()
	{
		var ex1 = new InvalidOperationException("test message");
		var ex2 = new InvalidOperationException("test message");
		await Assert.That(ex1.SameExceptionAs(ex2)).IsTrue();
	}

	[Test]
	public async Task SameExceptionAs_DifferentMessages_ReturnsFalse()
	{
		var ex1 = new InvalidOperationException("message 1");
		var ex2 = new InvalidOperationException("message 2");
		await Assert.That(ex1.SameExceptionAs(ex2)).IsFalse();
	}

	[Test]
	public async Task SameExceptionAs_DifferentTypes_ReturnsFalse()
	{
		var ex1 = new InvalidOperationException("test");
		var ex2 = new ArgumentException("test");
		await Assert.That(ex1.SameExceptionAs(ex2)).IsFalse();
	}

	[Test]
	public async Task SameExceptionAs_BothNull_ReturnsTrue()
	{
		await Assert.That(((Exception?)null)!.SameExceptionAs(null!)).IsTrue();
	}

	[Test]
	public async Task ThrowIfContainsErrors_WithData_ThrowsException()
	{
		var ex = new InvalidOperationException("test");
		ex.Data.Add("key", "value");
		await Assert.That(() => ex.ThrowIfContainsErrors()).ThrowsExactly<InvalidOperationException>();
	}

	[Test]
	public void ThrowIfContainsErrors_WithoutData_DoesNotThrow()
	{
		var ex = new InvalidOperationException("test");
		ex.ThrowIfContainsErrors(); // Should not throw
	}

	[Test]
	public async Task ThrowIfContainsErrors_NullException_ThrowsArgumentNullException()
	{
		Exception? ex = null;
		await Assert.That(() => ex!.ThrowIfContainsErrors()).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task ToLogString_WithMessage_ContainsExceptionMessage()
	{
		var ex = new InvalidOperationException("test error");
		var log = ex.ToLogString("additional message");
		await Assert.That(log.Contains("test error", StringComparison.Ordinal)).IsTrue();
		await Assert.That(log.Contains("additional message", StringComparison.Ordinal)).IsTrue();
	}

	[Test]
	public async Task ToLogString_NullException_ReturnsNonNullString()
	{
		var log = ((Exception?)null).ToLogString("message");
		await Assert.That(log.Contains("message", StringComparison.Ordinal)).IsTrue();
	}

	[Test]
	public async Task ToLogString_NoAdditionalMessage_ReturnsExceptionInfo()
	{
		var ex = new InvalidOperationException("error message");
		var log = ex.ToLogString(null);
		await Assert.That(log.Contains("error message", StringComparison.Ordinal)).IsTrue();
	}
}
