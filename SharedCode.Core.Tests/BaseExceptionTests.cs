namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="BaseException"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class BaseExceptionTests
{
	[TestMethod]
	public void DefaultConstructor_CreatesException()
	{
		var ex = new BaseException();
		Assert.IsNotNull(ex);
		Assert.IsNull(ex.InnerException);
	}

	[TestMethod]
	public void MessageConstructor_SetsMessage()
	{
		var ex = new BaseException("test message");
		Assert.AreEqual("test message", ex.Message);
	}

	[TestMethod]
	public void MessageAndInnerExceptionConstructor_SetsMessageAndInner()
	{
		var inner = new InvalidOperationException("inner error");
		var ex = new BaseException("outer message", inner);
		Assert.AreEqual("outer message", ex.Message);
		Assert.AreSame(inner, ex.InnerException);
	}

	[TestMethod]
	public void InnerExceptionAndDataConstructor_SetsMessageFromInnerAndData()
	{
		var inner = new InvalidOperationException("inner error");
		var data = new Hashtable { { "key", "value" } };
		var ex = new BaseException(inner, data);
		Assert.AreEqual("inner error", ex.Message);
		Assert.AreSame(inner, ex.InnerException);
		Assert.IsTrue(ex.Data.Contains("key"));
	}

	[TestMethod]
	public void MessageInnerExceptionAndDataConstructor_SetsAll()
	{
		var inner = new InvalidOperationException("inner");
		var data = new Hashtable { { "errorCode", "42" } };
		var ex = new BaseException("outer message", inner, data);
		Assert.AreEqual("outer message", ex.Message);
		Assert.AreSame(inner, ex.InnerException);
		Assert.IsTrue(ex.Data.Contains("errorCode"));
	}

	[TestMethod]
	public void BaseException_IsException()
	{
		var ex = new BaseException("test");
		Assert.IsInstanceOfType<Exception>(ex);
	}
}
