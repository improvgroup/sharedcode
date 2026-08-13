namespace SharedCode.Tests.Collections;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Collections.Generic;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="EnumerationUtilities"/>.
/// </summary>
public class EnumerationUtilitiesTests
{
	private enum TestEnum
	{
		First,
		Second,
		Third,
	}

	[Test]
	public async Task ToList_ReturnsAllEnumValues()
	{
		var list = EnumerationUtilities.ToList<TestEnum>();
		await Assert.That(list.Count).IsEqualTo(3);
		await Assert.That(list.Contains(TestEnum.First)).IsTrue();
		await Assert.That(list.Contains(TestEnum.Second)).IsTrue();
		await Assert.That(list.Contains(TestEnum.Third)).IsTrue();
	}

	[Test]
	public async Task ToList_DayOfWeek_ReturnsAllSevenDays()
	{
		var list = EnumerationUtilities.ToList<DayOfWeek>();
		await Assert.That(list.Count).IsEqualTo(7);
	}
}
