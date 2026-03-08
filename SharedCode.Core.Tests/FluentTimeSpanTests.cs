namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="FluentTimeSpan"/> struct.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class FluentTimeSpanTests
{
	[TestMethod]
	public void ImplicitConversionToTimeSpan_ReturnsCorrectTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(2) };
		TimeSpan ts = fts;
		Assert.AreEqual(TimeSpan.FromHours(2), ts);
	}

	[TestMethod]
	public void ImplicitConversionFromTimeSpan_ReturnsFluentTimeSpan()
	{
		FluentTimeSpan fts = TimeSpan.FromDays(3);
		Assert.AreEqual(TimeSpan.FromDays(3), fts.TimeSpan);
	}

	[TestMethod]
	public void Years_ConvertedToTimeSpan_UsesDaysPerYear()
	{
		FluentTimeSpan fts = new() { Years = 1 };
		TimeSpan ts = fts;
		Assert.AreEqual(FluentTimeSpan.DaysPerYear, ts.Days);
	}

	[TestMethod]
	public void Months_ConvertedToTimeSpan_Uses30DaysPerMonth()
	{
		FluentTimeSpan fts = new() { Months = 2 };
		TimeSpan ts = fts;
		Assert.AreEqual(60, ts.Days);
	}

	[TestMethod]
	public void Add_TwoFluentTimeSpans_ReturnsSummedFluentTimeSpan()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2), Months = 2, Years = 2 };
		var result = a + b;
		Assert.AreEqual(TimeSpan.FromHours(3), result.TimeSpan);
		Assert.AreEqual(3, result.Months);
		Assert.AreEqual(3, result.Years);
	}

	[TestMethod]
	public void Subtract_TwoFluentTimeSpans_ReturnsDifference()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(5), Months = 3, Years = 2 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2), Months = 1, Years = 1 };
		var result = a - b;
		Assert.AreEqual(TimeSpan.FromHours(3), result.TimeSpan);
		Assert.AreEqual(2, result.Months);
		Assert.AreEqual(1, result.Years);
	}

	[TestMethod]
	public void Negate_ReturnsNegatedFluentTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		var negated = -fts;
		Assert.AreEqual(-TimeSpan.FromHours(1), negated.TimeSpan);
	}

	[TestMethod]
	public void Equals_TwoEqualFluentTimeSpans_ReturnsTrue()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1), Months = 1, Years = 1 };
		Assert.IsTrue(a == b);
		Assert.IsFalse(a != b);
		Assert.IsTrue(a.Equals(b));
	}

	[TestMethod]
	public void Equals_TwoDifferentFluentTimeSpans_ReturnsFalse()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(2) };
		Assert.IsFalse(a == b);
		Assert.IsTrue(a != b);
	}

	[TestMethod]
	public void LessThan_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan small = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan large = new() { TimeSpan = TimeSpan.FromHours(2) };
		Assert.IsTrue(small < large);
		Assert.IsFalse(large < small);
	}

	[TestMethod]
	public void GreaterThan_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan small = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan large = new() { TimeSpan = TimeSpan.FromHours(2) };
		Assert.IsTrue(large > small);
		Assert.IsFalse(small > large);
	}

	[TestMethod]
	public void LessThanOrEqual_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.IsTrue(a <= b);
		Assert.IsTrue(b <= a);
	}

	[TestMethod]
	public void GreaterThanOrEqual_FluentTimeSpan_ReturnsExpected()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(2) };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.IsTrue(a >= b);
		Assert.IsTrue(b <= a);
	}

	[TestMethod]
	public void Clone_ReturnsEqualFluentTimeSpan()
	{
		FluentTimeSpan original = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		var clone = (FluentTimeSpan)original.Clone();
		Assert.AreEqual(original, clone);
	}

	[TestMethod]
	public void ToString_ReturnsTimeSpanString()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.AreEqual(TimeSpan.FromHours(1).ToString(), fts.ToString());
	}

	[TestMethod]
	public void GetHashCode_EqualFluentTimeSpans_ReturnSameHashCode()
	{
		FluentTimeSpan a = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		FluentTimeSpan b = new() { TimeSpan = TimeSpan.FromHours(1), Months = 2, Years = 3 };
		Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
	}

	[TestMethod]
	public void CompareTo_TimeSpan_ReturnsExpected()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.AreEqual(0, fts.CompareTo(TimeSpan.FromHours(1)));
		Assert.IsTrue(fts.CompareTo(TimeSpan.FromHours(2)) < 0);
		Assert.IsTrue(fts.CompareTo(TimeSpan.FromMinutes(30)) > 0);
	}

	[TestMethod]
	public void DaysPerYear_Is365()
	{
		Assert.AreEqual(365, FluentTimeSpan.DaysPerYear);
	}

	[TestMethod]
	public void Properties_ReturnCorrectValues()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(25).Add(TimeSpan.FromMinutes(30).Add(TimeSpan.FromSeconds(45))) };
		Assert.AreEqual(1, fts.Days);
		Assert.AreEqual(1, fts.Hours);
		Assert.AreEqual(30, fts.Minutes);
		Assert.AreEqual(45, fts.Seconds);
	}

	[TestMethod]
	public void ToFluentTimeSpan_ReturnsSelf()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.AreEqual(fts, fts.ToFluentTimeSpan());
	}

	[TestMethod]
	public void ToTimeSpan_ReturnsEquivalentTimeSpan()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(2) };
		Assert.AreEqual(TimeSpan.FromHours(2), fts.ToTimeSpan());
	}

	[TestMethod]
	public void Equals_WithObject_WorksCorrectly()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		object boxed = fts;
		Assert.IsTrue(fts.Equals(boxed));
		Assert.IsFalse(fts.Equals(null));
		Assert.IsFalse(fts.Equals("not a time span"));
	}

	[TestMethod]
	public void CompareTo_Object_InvalidType_ThrowsArgumentException()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		_ = Assert.ThrowsExactly<ArgumentException>(() => fts.CompareTo("invalid"));
	}

	[TestMethod]
	public void CompareTo_Object_Null_Returns1()
	{
		FluentTimeSpan fts = new() { TimeSpan = TimeSpan.FromHours(1) };
		Assert.AreEqual(1, fts.CompareTo(null));
	}
}
