namespace SharedCode.Tests.Linq;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Linq;

using System;

/// <summary>
/// Tests for <see cref="Predicates" />.
/// </summary>
public class PredicatesTests
{
	[Test]
	public async Task AfterShouldReturnTheProperValueForDateTimeOffsets()
	{
		var testDateTimeOffset = DateTimeOffset.Now;
		var afterDateTimeOffset = testDateTimeOffset.AddMilliseconds(1);

		// testDateTime should not be after afterDateTime
		var resultDateTimeOffset = Predicates.After(afterDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsFalse();

		// afterDateTime should be after testDateTime
		resultDateTimeOffset = Predicates.After(testDateTimeOffset).Compile().Invoke(afterDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsTrue();

		// single value should not be after itself
		resultDateTimeOffset = Predicates.After(testDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsFalse();
	}

	[Test]
	public async Task AfterShouldReturnTheProperValueForDateTimes()
	{
		var testDateTime = DateTime.Now;
		var afterDateTime = testDateTime.AddMilliseconds(1);

		// testDateTime should not be after afterDateTime
		var resultDateTime = Predicates.After(afterDateTime).Compile().Invoke(testDateTime);
		await Assert.That(resultDateTime).IsFalse();

		// afterDateTime should be after testDateTime
		resultDateTime = Predicates.After(testDateTime).Compile().Invoke(afterDateTime);
		await Assert.That(resultDateTime).IsTrue();

		// single value should not be after itself
		resultDateTime = Predicates.After(testDateTime).Compile().Invoke(testDateTime);
		await Assert.That(resultDateTime).IsFalse();
	}

	[Test]
	public async Task BeforeShouldReturnTheProperValueForDateTimeOffsets()
	{
		var testDateTimeOffset = DateTimeOffset.Now;
		var beforeDateTimeOffset = testDateTimeOffset.AddMilliseconds(-1);

		// testDateTime should not be before beforeDateTime
		var resultDateTimeOffset = Predicates.Before(beforeDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsFalse();

		// beforeDateTime should be before testDateTime
		resultDateTimeOffset = Predicates.Before(testDateTimeOffset).Compile().Invoke(beforeDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsTrue();

		// single value should not be before itself
		resultDateTimeOffset = Predicates.Before(testDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		await Assert.That(resultDateTimeOffset).IsFalse();
	}

	[Test]
	public async Task BeforeShouldReturnTheProperValueForDateTimes()
	{
		var testDateTime = DateTime.Now;
		var beforeDateTime = testDateTime.AddMilliseconds(-1);

		// testDateTime should not be before beforeDateTime
		var resultDateTime = Predicates.Before(beforeDateTime).Compile().Invoke(testDateTime);
		await Assert.That(resultDateTime).IsFalse();

		// beforeDateTime should be before testDateTime
		resultDateTime = Predicates.Before(testDateTime).Compile().Invoke(beforeDateTime);
		await Assert.That(resultDateTime).IsTrue();

		// single value should not be before itself
		resultDateTime = Predicates.Before(testDateTime).Compile().Invoke(testDateTime);
		await Assert.That(resultDateTime).IsFalse();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForEdgesOfDateRange()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusiveEdgeStart = Predicates.Between(start, end, true).Compile().Invoke(start);
		var betweenStartAndEndExclusiveEdgeStart = Predicates.Between(start, end, false).Compile().Invoke(start);
		var betweenStartAndEndExclusiveImplicitEdgeStart = Predicates.Between(start, end).Compile().Invoke(start);

		await Assert.That(betweenStartAndEndInclusiveEdgeStart).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveEdgeStart).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveImplicitEdgeStart).IsTrue();

		var betweenStartAndEndInclusiveEdgeEnd = Predicates.Between(start, end, true).Compile().Invoke(end);
		var betweenStartAndEndExclusiveEdgeEnd = Predicates.Between(start, end, false).Compile().Invoke(end);
		var betweenStartAndEndExclusiveImplicitEdgeEnd = Predicates.Between(start, end).Compile().Invoke(end);

		await Assert.That(betweenStartAndEndInclusiveEdgeEnd).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveEdgeEnd).IsFalse();
		await Assert.That(betweenStartAndEndExclusiveImplicitEdgeEnd).IsFalse();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForEdgesOfDateTimeOffsetRange()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusiveEdgeStart = Predicates.Between(start, end, true).Compile().Invoke(start);
		var betweenStartAndEndExclusiveEdgeStart = Predicates.Between(start, end, false).Compile().Invoke(start);
		var betweenStartAndEndExclusiveImplicitEdgeStart = Predicates.Between(start, end).Compile().Invoke(start);

		await Assert.That(betweenStartAndEndInclusiveEdgeStart).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveEdgeStart).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveImplicitEdgeStart).IsTrue();

		var betweenStartAndEndInclusiveEdgeEnd = Predicates.Between(start, end, true).Compile().Invoke(end);
		var betweenStartAndEndExclusiveEdgeEnd = Predicates.Between(start, end, false).Compile().Invoke(end);
		var betweenStartAndEndExclusiveImplicitEdgeEnd = Predicates.Between(start, end).Compile().Invoke(end);

		await Assert.That(betweenStartAndEndInclusiveEdgeEnd).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveEdgeEnd).IsFalse();
		await Assert.That(betweenStartAndEndExclusiveImplicitEdgeEnd).IsFalse();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForEqualStartAndEndDates()
	{
		var date = DateTime.Now;

		var inclusiveResult = Predicates.Between(date, date, true).Compile().Invoke(date);
		var exclusiveResult = Predicates.Between(date, date, false).Compile().Invoke(date);

		await Assert.That(inclusiveResult).IsTrue();
		await Assert.That(exclusiveResult).IsFalse();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForEqualStartAndEndDateTimeOffsets()
	{
		var date = DateTimeOffset.Now;

		var inclusiveResult = Predicates.Between(date, date, true).Compile().Invoke(date);
		var exclusiveResult = Predicates.Between(date, date, false).Compile().Invoke(date);

		await Assert.That(inclusiveResult).IsTrue();
		await Assert.That(exclusiveResult).IsFalse();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForValuesBetweenTheStartAndEndDatesWhenEndIsBeforeStart()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenEndAndStartInclusive = Predicates.Between(end, start, true).Compile().Invoke(test);
		var betweenEndAndStartExclusive = Predicates.Between(end, start, false).Compile().Invoke(test);
		var betweenEndAndStartExclusiveImplicit = Predicates.Between(end, start).Compile().Invoke(test);

		await Assert.That(betweenEndAndStartInclusive).IsTrue();
		await Assert.That(betweenEndAndStartExclusive).IsTrue();
		await Assert.That(betweenEndAndStartExclusiveImplicit).IsTrue();
	}

	[Test]
	public async Task BetweenShouldHandleInclusiveProperlyForValuesBetweenTheStartAndEndDateTimeOffsetsWhenEndIsBeforeStart()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenEndAndStartInclusive = Predicates.Between(end, start, true).Compile().Invoke(test);
		var betweenEndAndStartExclusive = Predicates.Between(end, start, false).Compile().Invoke(test);
		var betweenEndAndStartExclusiveImplicit = Predicates.Between(end, start).Compile().Invoke(test);

		await Assert.That(betweenEndAndStartInclusive).IsTrue();
		await Assert.That(betweenEndAndStartExclusive).IsTrue();
		await Assert.That(betweenEndAndStartExclusiveImplicit).IsTrue();
	}

	[Test]
	public async Task BetweenShouldHandleInclusivePropertyForValuesBetweenTheStartAndEndDate()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusive = Predicates.Between(start, end, true).Compile().Invoke(test);
		var betweenStartAndEndExclusive = Predicates.Between(start, end, false).Compile().Invoke(test);
		var betweenStartAndEndExclusiveImplicit = Predicates.Between(start, end).Compile().Invoke(test);

		await Assert.That(betweenStartAndEndInclusive).IsTrue();
		await Assert.That(betweenStartAndEndExclusive).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveImplicit).IsTrue();
	}

	[Test]
	public async Task BetweenShouldHandleInclusivePropertyForValuesBetweenTheStartAndEndDateTimeOffset()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusive = Predicates.Between(start, end, true).Compile().Invoke(test);
		var betweenStartAndEndExclusive = Predicates.Between(start, end, false).Compile().Invoke(test);
		var betweenStartAndEndExclusiveImplicit = Predicates.Between(start, end).Compile().Invoke(test);

		await Assert.That(betweenStartAndEndInclusive).IsTrue();
		await Assert.That(betweenStartAndEndExclusive).IsTrue();
		await Assert.That(betweenStartAndEndExclusiveImplicit).IsTrue();
	}
}
