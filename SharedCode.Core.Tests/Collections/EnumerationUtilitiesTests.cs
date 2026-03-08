namespace SharedCode.Tests.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Collections.Generic;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="EnumerationUtilities"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class EnumerationUtilitiesTests
{
	private enum TestEnum
	{
		First,
		Second,
		Third,
	}

	[TestMethod]
	public void ToList_ReturnsAllEnumValues()
	{
		var list = EnumerationUtilities.ToList<TestEnum>();
		Assert.AreEqual(3, list.Count);
		Assert.IsTrue(list.Contains(TestEnum.First));
		Assert.IsTrue(list.Contains(TestEnum.Second));
		Assert.IsTrue(list.Contains(TestEnum.Third));
	}

	[TestMethod]
	public void ToList_DayOfWeek_ReturnsAllSevenDays()
	{
		var list = EnumerationUtilities.ToList<DayOfWeek>();
		Assert.AreEqual(7, list.Count);
	}
}
