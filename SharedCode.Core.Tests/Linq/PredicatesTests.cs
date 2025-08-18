namespace SharedCode.Tests.Linq;

using AwesomeAssertions;

using SharedCode.Linq;

using System;

using Xunit;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class PredicatesTests
{
	[Fact]
	public void AfterShouldReturnTheProperValueForDateTimeOffsets()
	{
		var testDateTimeOffset = DateTimeOffset.Now;
		var afterDateTimeOffset = testDateTimeOffset.AddMilliseconds(1);

		// testDateTime should not be after afterDateTime
		var resultDateTimeOffset = Predicates.After(afterDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeFalse();

		// afterDateTime should be after testDateTime
		resultDateTimeOffset = Predicates.After(testDateTimeOffset).Compile().Invoke(afterDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeTrue();

		// single value should not be after itself
		resultDateTimeOffset = Predicates.After(testDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeFalse();
	}

	[Fact]
	public void AfterShouldReturnTheProperValueForDateTimes()
	{
		var testDateTime = DateTime.Now;
		var afterDateTime = testDateTime.AddMilliseconds(1);

		// testDateTime should not be after afterDateTime
		var resultDateTime = Predicates.After(afterDateTime).Compile().Invoke(testDateTime);
		_ = resultDateTime.Should().BeFalse();

		// afterDateTime should be after testDateTime
		resultDateTime = Predicates.After(testDateTime).Compile().Invoke(afterDateTime);
		_ = resultDateTime.Should().BeTrue();

		// single value should not be after itself
		resultDateTime = Predicates.After(testDateTime).Compile().Invoke(testDateTime);
		_ = resultDateTime.Should().BeFalse();
	}

	[Fact]
	public void BeforeShouldReturnTheProperValueForDateTimeOffsets()
	{
		var testDateTimeOffset = DateTimeOffset.Now;
		var beforeDateTimeOffset = testDateTimeOffset.AddMilliseconds(-1);

		// testDateTime should not be before beforeDateTime
		var resultDateTimeOffset = Predicates.Before(beforeDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeFalse();

		// beforeDateTime should be before testDateTime
		resultDateTimeOffset = Predicates.Before(testDateTimeOffset).Compile().Invoke(beforeDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeTrue();

		// single value should not be before itself
		resultDateTimeOffset = Predicates.Before(testDateTimeOffset).Compile().Invoke(testDateTimeOffset);
		_ = resultDateTimeOffset.Should().BeFalse();
	}

	[Fact]
	public void BeforeShouldReturnTheProperValueForDateTimes()
	{
		var testDateTime = DateTime.Now;
		var beforeDateTime = testDateTime.AddMilliseconds(-1);

		// testDateTime should not be before beforeDateTime
		var resultDateTime = Predicates.Before(beforeDateTime).Compile().Invoke(testDateTime);
		_ = resultDateTime.Should().BeFalse();

		// beforeDateTime should be before testDateTime
		resultDateTime = Predicates.Before(testDateTime).Compile().Invoke(beforeDateTime);
		_ = resultDateTime.Should().BeTrue();

		// single value should not be before itself
		resultDateTime = Predicates.Before(testDateTime).Compile().Invoke(testDateTime);
		_ = resultDateTime.Should().BeFalse();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForEdgesOfDateRange()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusiveEdgeStart = Predicates.Between(start, end, true).Compile().Invoke(start);
		var betweenStartAndEndExclusiveEdgeStart = Predicates.Between(start, end, false).Compile().Invoke(start);
		var betweenStartAndEndExclusiveImplicitEdgeStart = Predicates.Between(start, end).Compile().Invoke(start);

		_ = betweenStartAndEndInclusiveEdgeStart.Should().BeTrue();
		_ = betweenStartAndEndExclusiveEdgeStart.Should().BeTrue();
		_ = betweenStartAndEndExclusiveImplicitEdgeStart.Should().BeTrue();

		var betweenStartAndEndInclusiveEdgeEnd = Predicates.Between(start, end, true).Compile().Invoke(end);
		var betweenStartAndEndExclusiveEdgeEnd = Predicates.Between(start, end, false).Compile().Invoke(end);
		var betweenStartAndEndExclusiveImplicitEdgeEnd = Predicates.Between(start, end).Compile().Invoke(end);

		_ = betweenStartAndEndInclusiveEdgeEnd.Should().BeTrue();
		_ = betweenStartAndEndExclusiveEdgeEnd.Should().BeFalse();
		_ = betweenStartAndEndExclusiveImplicitEdgeEnd.Should().BeFalse();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForEdgesOfDateTimeOffsetRange()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusiveEdgeStart = Predicates.Between(start, end, true).Compile().Invoke(start);
		var betweenStartAndEndExclusiveEdgeStart = Predicates.Between(start, end, false).Compile().Invoke(start);
		var betweenStartAndEndExclusiveImplicitEdgeStart = Predicates.Between(start, end).Compile().Invoke(start);

		_ = betweenStartAndEndInclusiveEdgeStart.Should().BeTrue();
		_ = betweenStartAndEndExclusiveEdgeStart.Should().BeTrue();
		_ = betweenStartAndEndExclusiveImplicitEdgeStart.Should().BeTrue();

		var betweenStartAndEndInclusiveEdgeEnd = Predicates.Between(start, end, true).Compile().Invoke(end);
		var betweenStartAndEndExclusiveEdgeEnd = Predicates.Between(start, end, false).Compile().Invoke(end);
		var betweenStartAndEndExclusiveImplicitEdgeEnd = Predicates.Between(start, end).Compile().Invoke(end);

		_ = betweenStartAndEndInclusiveEdgeEnd.Should().BeTrue();
		_ = betweenStartAndEndExclusiveEdgeEnd.Should().BeFalse();
		_ = betweenStartAndEndExclusiveImplicitEdgeEnd.Should().BeFalse();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForEqualStartAndEndDates()
	{
		var date = DateTime.Now;

		var inclusiveResult = Predicates.Between(date, date, true).Compile().Invoke(date);
		var exclusiveResult = Predicates.Between(date, date, false).Compile().Invoke(date);

		_ = inclusiveResult.Should().BeTrue();
		_ = exclusiveResult.Should().BeFalse();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForEqualStartAndEndDateTimeOffsets()
	{
		var date = DateTimeOffset.Now;

		var inclusiveResult = Predicates.Between(date, date, true).Compile().Invoke(date);
		var exclusiveResult = Predicates.Between(date, date, false).Compile().Invoke(date);

		_ = inclusiveResult.Should().BeTrue();
		_ = exclusiveResult.Should().BeFalse();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForValuesBetweenTheStartAndEndDatesWhenEndIsBeforeStart()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenEndAndStartInclusive = Predicates.Between(end, start, true).Compile().Invoke(test);
		var betweenEndAndStartExclusive = Predicates.Between(end, start, false).Compile().Invoke(test);
		var betweenEndAndStartExclusiveImplicit = Predicates.Between(end, start).Compile().Invoke(test);

		_ = betweenEndAndStartInclusive.Should().BeTrue();
		_ = betweenEndAndStartExclusive.Should().BeTrue();
		_ = betweenEndAndStartExclusiveImplicit.Should().BeTrue();
	}

	[Fact]
	public void BetweenShouldHandleInclusiveProperlyForValuesBetweenTheStartAndEndDateTimeOffsetsWhenEndIsBeforeStart()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenEndAndStartInclusive = Predicates.Between(end, start, true).Compile().Invoke(test);
		var betweenEndAndStartExclusive = Predicates.Between(end, start, false).Compile().Invoke(test);
		var betweenEndAndStartExclusiveImplicit = Predicates.Between(end, start).Compile().Invoke(test);

		_ = betweenEndAndStartInclusive.Should().BeTrue();
		_ = betweenEndAndStartExclusive.Should().BeTrue();
		_ = betweenEndAndStartExclusiveImplicit.Should().BeTrue();
	}

	[Fact]
	public void BetweenShouldHandleInclusivePropertyForValuesBetweenTheStartAndEndDate()
	{
		var start = DateTime.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusive = Predicates.Between(start, end, true).Compile().Invoke(test);
		var betweenStartAndEndExclusive = Predicates.Between(start, end, false).Compile().Invoke(test);
		var betweenStartAndEndExclusiveImplicit = Predicates.Between(start, end).Compile().Invoke(test);

		_ = betweenStartAndEndInclusive.Should().BeTrue();
		_ = betweenStartAndEndExclusive.Should().BeTrue();
		_ = betweenStartAndEndExclusiveImplicit.Should().BeTrue();
	}

	[Fact]
	public void BetweenShouldHandleInclusivePropertyForValuesBetweenTheStartAndEndDateTimeOffset()
	{
		var start = DateTimeOffset.Now;
		var end = start.AddHours(1);
		var test = start.AddMinutes(30);

		var betweenStartAndEndInclusive = Predicates.Between(start, end, true).Compile().Invoke(test);
		var betweenStartAndEndExclusive = Predicates.Between(start, end, false).Compile().Invoke(test);
		var betweenStartAndEndExclusiveImplicit = Predicates.Between(start, end).Compile().Invoke(test);

		_ = betweenStartAndEndInclusive.Should().BeTrue();
		_ = betweenStartAndEndExclusive.Should().BeTrue();
		_ = betweenStartAndEndExclusiveImplicit.Should().BeTrue();
	}
}
