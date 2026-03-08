namespace SharedCode.Tests.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Attributes;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the attribute classes.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class AttributeTests
{
	[TestMethod]
	public void StringValueAttribute_StoresValue()
	{
		var attr = new StringValueAttribute("my-value");
		Assert.AreEqual("my-value", attr.Value);
	}

	[TestMethod]
	public void StringValueAttribute_NullValue_ThrowsArgumentNullException()
	{
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => new StringValueAttribute(null!));
	}

	[TestMethod]
	public void DataFormatAttribute_StoresFormat()
	{
		var attr = new DataFormatAttribute("yyyy-MM-dd");
		Assert.AreEqual("yyyy-MM-dd", attr.Format);
	}

	[TestMethod]
	public void DataFormatAttribute_NullFormat_ThrowsArgumentNullException()
	{
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => new DataFormatAttribute(null!));
	}

	[TestMethod]
	public void DataWidthAttribute_StoresWidth()
	{
		var attr = new DataWidthAttribute(10);
		Assert.AreEqual(10, attr.Width);
	}

	[TestMethod]
	public void DataWidthAttribute_ZeroWidth_ThrowsArgumentException()
	{
		_ = Assert.ThrowsExactly<ArgumentException>(() => new DataWidthAttribute(0));
	}

	[TestMethod]
	public void DataWidthAttribute_NegativeWidth_ThrowsArgumentException()
	{
		_ = Assert.ThrowsExactly<ArgumentException>(() => new DataWidthAttribute(-5));
	}
}
