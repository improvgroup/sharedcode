namespace SharedCode.Tests.Text;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Text;

using System.Diagnostics.CodeAnalysis;
using System.Text;

/// <summary>
/// Tests for <see cref="StringBuilderExtensions"/>.
/// </summary>
public class StringBuilderExtensionsTests
{
	[Test]
	public async Task AppendIf_ConditionTrue_AppendsValue()
	{
		var sb = new StringBuilder();
		var result = sb.AppendIf("hello", condition: true);
		await Assert.That(sb.ToString()).IsEqualTo("hello");
		await Assert.That(result).IsSameReferenceAs(sb);
	}

	[Test]
	public async Task AppendIf_ConditionFalse_DoesNotAppend()
	{
		var sb = new StringBuilder();
		var result = sb.AppendIf("hello", condition: false);
		await Assert.That(sb.ToString()).IsEqualTo(string.Empty);
		await Assert.That(result).IsSameReferenceAs(sb);
	}

	[Test]
	public async Task AppendIf_NullValue_ConditionTrue_AppendNothing()
	{
		var sb = new StringBuilder("prefix");
		_ = sb.AppendIf(null, condition: true);
		await Assert.That(sb.ToString()).IsEqualTo("prefix");
	}

	[Test]
	public async Task AppendIf_NullBuilder_ThrowsArgumentNullException()
	{
		StringBuilder? sb = null;
		await Assert.That(() => sb!.AppendIf("value", condition: true)).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AppendLineFormat_AppendsFormattedLine()
	{
		var sb = new StringBuilder();
		var result = sb.AppendLineFormat("Hello {0}, you are {1} years old", "Alice", 30);
		await Assert.That(result is not null).IsTrue();
		var content = sb.ToString();
		await Assert.That(content.Contains("Hello Alice, you are 30 years old", StringComparison.OrdinalIgnoreCase)).IsTrue();
	}

	[Test]
	public async Task AppendLineFormat_NullBuilder_ThrowsArgumentNullException()
	{
		StringBuilder? sb = null;
		await Assert.That(() => sb!.AppendLineFormat("format {0}", "arg")).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AppendLineFormat_NullFormat_ThrowsArgumentNullException()
	{
		var sb = new StringBuilder();
		await Assert.That(() => sb.AppendLineFormat(null!, "arg")).ThrowsExactly<ArgumentNullException>();
	}

	[Test]
	public async Task AppendLineFormat_NullArguments_ThrowsArgumentNullException()
	{
		var sb = new StringBuilder();
		await Assert.That(() => sb.AppendLineFormat("format", null!)).ThrowsExactly<ArgumentNullException>();
	}
}
