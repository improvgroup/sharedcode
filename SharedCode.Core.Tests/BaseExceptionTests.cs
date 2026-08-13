namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="BaseException"/> class.
/// </summary>
public class BaseExceptionTests
{
	[Test]
	public async Task DefaultConstructor_CreatesException()
	{
		var ex = new BaseException();
		await Assert.That(ex.InnerException is null).IsTrue();
	}

	[Test]
	public async Task MessageConstructor_SetsMessage()
	{
		var ex = new BaseException("test message");
		await Assert.That(ex.Message).IsEqualTo("test message");
	}

	[Test]
	public async Task MessageAndInnerExceptionConstructor_SetsMessageAndInner()
	{
		var inner = new InvalidOperationException("inner error");
		var ex = new BaseException("outer message", inner);
		await Assert.That(ex.Message).IsEqualTo("outer message");
		await Assert.That(ex.InnerException).IsSameReferenceAs(inner);
	}

	[Test]
	public async Task InnerExceptionAndDataConstructor_SetsMessageFromInnerAndData()
	{
		var inner = new InvalidOperationException("inner error");
		var data = new Hashtable { { "key", "value" } };
		var ex = new BaseException(inner, data);
		await Assert.That(ex.Message).IsEqualTo("inner error");
		await Assert.That(ex.InnerException).IsSameReferenceAs(inner);
		await Assert.That(ex.Data.Contains("key")).IsTrue();
	}

	[Test]
	public async Task MessageInnerExceptionAndDataConstructor_SetsAll()
	{
		var inner = new InvalidOperationException("inner");
		var data = new Hashtable { { "errorCode", "42" } };
		var ex = new BaseException("outer message", inner, data);
		await Assert.That(ex.Message).IsEqualTo("outer message");
		await Assert.That(ex.InnerException).IsSameReferenceAs(inner);
		await Assert.That(ex.Data.Contains("errorCode")).IsTrue();
	}

	[Test]
	public async Task BaseException_IsException()
	{
		var ex = new BaseException("test");
		await Assert.That(ex).IsTypeOf<Exception>();
	}
}
