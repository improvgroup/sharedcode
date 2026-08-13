namespace SharedCode.Tests.Attributes;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Attributes;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the attribute classes.
/// </summary>
public class AttributeTests
{
	[Test]
	public async Task StringValueAttribute_StoresValue()
	{
		var attr = new StringValueAttribute("my-value");
		await Assert.That(attr.Value).IsEqualTo("my-value");
	}

	[Test]
	public async Task StringValueAttribute_NullValue_ThrowsArgumentNullException()
	{
		await Assert.That(() => new StringValueAttribute(null!)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task DataFormatAttribute_StoresFormat()
	{
		var attr = new DataFormatAttribute("yyyy-MM-dd");
		await Assert.That(attr.Format).IsEqualTo("yyyy-MM-dd");
	}

	[Test]
	public async Task DataFormatAttribute_NullFormat_ThrowsArgumentNullException()
	{
		await Assert.That(() => new DataFormatAttribute(null!)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task DataWidthAttribute_StoresWidth()
	{
		var attr = new DataWidthAttribute(10);
		await Assert.That(attr.Width).IsEqualTo(10);
	}

	[Test]
	public async Task DataWidthAttribute_ZeroWidth_ThrowsArgumentException()
	{
		await Assert.That(() => new DataWidthAttribute(0)).ThrowsExactly<ArgumentException>();
	}

	[Test]
	public async Task DataWidthAttribute_NegativeWidth_ThrowsArgumentException()
	{
		await Assert.That(() => new DataWidthAttribute(-5)).ThrowsExactly<ArgumentException>();
	}
}
