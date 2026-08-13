namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="FluentTimeSpan"/> struct.
/// </summary>
public class FluentTimeSpanTests
{
	[Test]
	public async Task ImplicitConversionToTimeSpan_ReturnsCorrectTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(2) };
		TimeSpan ts = fts;
		await Assert.That(ts).IsEqualTo(TimeSpan.FromHours(2));
	}

	[Test]
	public async Task ImplicitConversionFromTimeSpan_ReturnsFluentTimeSpan()
	{
		FluentTimeSpan fts = TimeSpan.FromDays(3);
		await Assert.That(fts.TimeSpan).IsEqualTo(TimeSpan.FromDays(3));
	}

	[Test]
	public async Task Years_ConvertedToTimeSpan_UsesDaysPerYear()
	{
		FluentTimeSpan fts = new() { Years = 1 };
		TimeSpan ts = fts;
		await Assert.That(ts.Days).IsEqualTo(FluentTimeSpan.DaysPerYear);
	}

	[Test]
	public async Task Months_ConvertedToTimeSpan_Uses30DaysPerMonth()
	{
		FluentTimeSpan fts = new() { Months = 2 };
		TimeSpan ts = fts;
		await Assert.That(ts.Days).IsEqualTo(60);
	}

	[Test]
	public async Task Add_TwoFluentTimeSpans_ReturnsSummedFluentTimeSpan()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2), Months = 2, Years = 2 };
		var result = a + b;
		await Assert.That(result.TimeSpan).IsEqualTo(TimeSpan.FromHours(3));
		await Assert.That(result.Months).IsEqualTo(3);
		await Assert.That(result.Years).IsEqualTo(3);
	}

	[Test]
	public async Task Subtract_TwoFluentTimeSpans_ReturnsDifference()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(5), Months = 3, Years = 2 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2), Months = 1, Years = 1 };
		var result = a - b;
		await Assert.That(result.TimeSpan).IsEqualTo(TimeSpan.FromHours(3));
		await Assert.That(result.Months).IsEqualTo(2);
		await Assert.That(result.Years).IsEqualTo(1);
	}

	[Test]
	public async Task Negate_ReturnsNegatedFluentTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		var negated = -fts;
		await Assert.That(negated.TimeSpan).IsEqualTo(-TimeSpan.FromHours(1));
	}

	[Test]
	public async Task Equals_TwoEqualFluentTimeSpans_ReturnsTrue()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		await Assert.That(a == b).IsTrue();
		await Assert.That(a != b).IsFalse();
		await Assert.That(a.Equals(b)).IsTrue();
	}

	[Test]
	public async Task Equals_TwoDifferentFluentTimeSpans_ReturnsFalse()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2) };
		await Assert.That(a == b).IsFalse();
		await Assert.That(a != b).IsTrue();
	}

	[Test]
	public async Task LessThan_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan small = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan large = new() { TimeSpan = TimeSpan.FromHours(2) };
		await Assert.That(small < large).IsTrue();
		await Assert.That(large < small).IsFalse();
	}

	[Test]
	public async Task GreaterThan_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan small = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan large = new() { TimeSpan = TimeSpan.FromHours(2) };
		await Assert.That(large > small).IsTrue();
		await Assert.That(small > large).IsFalse();
	}

	[Test]
	public async Task LessThanOrEqual_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(a <= b).IsTrue();
		await Assert.That(b <= a).IsTrue();
	}

	[Test]
	public async Task GreaterThanOrEqual_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(2) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(a >= b).IsTrue();
		await Assert.That(b <= a).IsTrue();
	}

	[Test]
	public async Task Clone_ReturnsEqualFluentTimeSpan()
	{
		FluentTimeSpan original = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		var clone = (FluentTimeSpan)original.Clone();
		await Assert.That(clone).IsEqualTo(original);
	}

	[Test]
	public async Task ToString_ReturnsTimeSpanString()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(fts.ToString()).IsEqualTo(TimeSpan.FromHours(1).ToString());
	}

	[Test]
	public async Task GetHashCode_EqualFluentTimeSpans_ReturnSameHashCode()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		await Assert.That(b.GetHashCode()).IsEqualTo(a.GetHashCode());
	}

	[Test]
	public async Task CompareTo_TimeSpan_ReturnsExpected()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(fts.CompareTo(TimeSpan.FromHours(1))).IsEqualTo(0);
		await Assert.That(fts.CompareTo(TimeSpan.FromHours(2)) < 0).IsTrue();
		await Assert.That(fts.CompareTo(TimeSpan.FromMinutes(30)) > 0).IsTrue();
	}

	[Test]
	public async Task DaysPerYear_Is365()
	{
		await Assert.That(FluentTimeSpan.DaysPerYear).IsEqualTo(365);
	}

	[Test]
	public async Task Properties_ReturnCorrectValues()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(25).Add(TimeSpan.FromMinutes(30).Add(TimeSpan.FromSeconds(45))) };
		await Assert.That(fts.Days).IsEqualTo(1);
		await Assert.That(fts.Hours).IsEqualTo(1);
		await Assert.That(fts.Minutes).IsEqualTo(30);
		await Assert.That(fts.Seconds).IsEqualTo(45);
	}

	[Test]
	public async Task ToFluentTimeSpan_ReturnsSelf()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(fts.ToFluentTimeSpan()).IsEqualTo(fts);
	}

	[Test]
	public async Task ToTimeSpan_ReturnsEquivalentTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(2) };
		await Assert.That(fts.ToTimeSpan()).IsEqualTo(TimeSpan.FromHours(2));
	}

	[Test]
	public async Task Equals_WithObject_WorksCorrectly()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		object boxed = fts;
		await Assert.That(fts.Equals(boxed)).IsTrue();
		await Assert.That(fts.Equals(null)).IsFalse();
		await Assert.That(fts.Equals("not a time span")).IsFalse();
	}

	[Test]
	public async Task CompareTo_Object_InvalidType_ThrowsArgumentException()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(() => fts.CompareTo("invalid")).ThrowsExactly<ArgumentException>();
	}

	[Test]
	public async Task CompareTo_Object_Null_Returns1()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		await Assert.That(fts.CompareTo(null)).IsEqualTo(1);
	}
}
