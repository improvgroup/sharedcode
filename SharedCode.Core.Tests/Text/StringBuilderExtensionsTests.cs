namespace SharedCode.Tests.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Text;

using System.Diagnostics.CodeAnalysis;
using System.Text;

/// <summary>
/// Tests for <see cref="StringBuilderExtensions"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class StringBuilderExtensionsTests
{
	[TestMethod]
	public void AppendIf_ConditionTrue_AppendsValue()
	{
		var sb = new StringBuilder();
		var result = sb.AppendIf("hello", condition: true);
		Assert.AreEqual("hello", sb.ToString());
		Assert.AreSame(sb, result);
	}

	[TestMethod]
	public void AppendIf_ConditionFalse_DoesNotAppend()
	{
		var sb = new StringBuilder();
		var result = sb.AppendIf("hello", condition: false);
		Assert.AreEqual(string.Empty, sb.ToString());
		Assert.AreSame(sb, result);
	}

	[TestMethod]
	public void AppendIf_NullValue_ConditionTrue_AppendNothing()
	{
		var sb = new StringBuilder("prefix");
		_ = sb.AppendIf(null, condition: true);
		Assert.AreEqual("prefix", sb.ToString());
	}

	[TestMethod]
	public void AppendIf_NullBuilder_ThrowsArgumentNullException()
	{
		StringBuilder? sb = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => sb!.AppendIf("value", condition: true));
	}

	[TestMethod]
	public void AppendLineFormat_AppendsFormattedLine()
	{
		var sb = new StringBuilder();
		var result = sb.AppendLineFormat("Hello {0}, you are {1} years old", "Alice", 30);
		Assert.IsNotNull(result);
		var content = sb.ToString();
		Assert.IsTrue(content.Contains("Hello Alice, you are 30 years old", StringComparison.OrdinalIgnoreCase));
	}

	[TestMethod]
	public void AppendLineFormat_NullBuilder_ThrowsArgumentNullException()
	{
		StringBuilder? sb = null;
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => sb!.AppendLineFormat("format {0}", "arg"));
	}

	[TestMethod]
	public void AppendLineFormat_NullFormat_ThrowsArgumentNullException()
	{
		var sb = new StringBuilder();
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => sb.AppendLineFormat(null!, "arg"));
	}

	[TestMethod]
	public void AppendLineFormat_NullArguments_ThrowsArgumentNullException()
	{
		var sb = new StringBuilder();
		_ = Assert.ThrowsExactly<ArgumentNullException>(() => sb.AppendLineFormat("format", null!));
	}
}
