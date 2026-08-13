namespace SharedCode.Tests.Specifications;

using SharedCode.Specifications;
using SharedCode.Specifications.Builders;
using SharedCode.Specifications.Evaluators;
using SharedCode.Specifications.Exceptions;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TUnit.Assertions;
using TUnit.Core;

/// <summary>
/// Tests for <see cref="Specification{T}" /> and <see cref="Specification{T, TResult}" />.
/// </summary>
public class SpecificationTests
{
    [Test]
    public async Task Evaluate_WithWhereOrderPagingAndPostProcessing_ReturnsExpectedEntities()
    {
        // Arrange
        var specification = new SampleEntitySpecification();
        SampleEntity[] entities =
        [
            new(1, "third", true),
            new(2, "first", true),
            new(3, "ignored", false),
            new(4, "second", true),
        ];

        // Act
        var result = specification.Evaluate(entities).ToArray();

        // Assert
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0].Id).IsEqualTo(2);
        await Assert.That(result[1].Id).IsEqualTo(4);
        await Assert.That(specification.AsNoTracking).IsTrue();
        await Assert.That(specification.AsNoTrackingWithIdentityResolution).IsTrue();
        await Assert.That(specification.AsSplitQuery).IsTrue();
        await Assert.That(specification.IsPagingEnabled).IsTrue();
        await Assert.That(specification.Skip).IsEqualTo(0);
        await Assert.That(specification.Take).IsEqualTo(2);
        await Assert.That(specification.CacheEnabled).IsTrue();
        await Assert.That(specification.CacheKey).IsEqualTo("SampleEntitySpecification-2");
        await Assert.That(specification.IncludeStrings.Single()).IsEqualTo("Children");
        await Assert.That(specification.IncludeExpressions.Count()).IsEqualTo(1);
        await Assert.That(specification.OrderExpressions.Count()).IsEqualTo(2);
        await Assert.That(specification.WhereExpressions.Count()).IsEqualTo(1);
    }

    [Test]
    public async Task Evaluate_WithProjectionAndPostProcessing_ReturnsProjectedValues()
    {
        // Arrange
        var specification = new SampleProjectionSpecification();
        SampleEntity[] entities =
        [
            new(3, "Gamma", true),
            new(1, "Alpha", true),
            new(2, "Beta", false),
        ];

        // Act
        var result = specification.Evaluate(entities).ToArray();

        // Assert
        await Assert.That(result.Length).IsEqualTo(2);
        await Assert.That(result[0]).IsEqualTo("GAMMA");
        await Assert.That(result[1]).IsEqualTo("ALPHA");
        await Assert.That(specification.Selector is not null).IsTrue();
        await Assert.That(specification.PostProcessingAction is not null).IsTrue();
    }

    [Test]
    public async Task Evaluate_WithSearchCriteria_ThrowsNotSupportedException()
    {
        // Arrange
        var specification = new SearchOnlySpecification();

        // Act / Assert
        await Assert.That(() => specification.Evaluate([new SampleEntity(1, "match", true)]).ToArray())
            .ThrowsExactly<NotSupportedException>();
    }

    [Test]
    public async Task Evaluate_GenericSpecificationWithoutSelector_ThrowsSelectorNotFoundException()
    {
        // Arrange
        var specification = new MissingSelectorSpecification();

        // Act / Assert
        await Assert.That(() => specification.Evaluate([new SampleEntity(1, "match", true)]).ToArray())
            .ThrowsExactly<SelectorNotFoundException>();
    }

    [Test]
    public async Task Skip_WhenCalledTwice_ThrowsDuplicateSkipException()
    {
        // Arrange
        var specification = new DuplicateSkipSpecification();

        // Act / Assert
        await Assert.That(() => specification.ConfigureDuplicateSkip())
            .ThrowsExactly<DuplicateSkipException>();
    }

    [Test]
    public async Task Take_WhenCalledTwice_ThrowsDuplicateTakeException()
    {
        // Arrange
        var specification = new DuplicateTakeSpecification();

        // Act / Assert
        await Assert.That(() => specification.ConfigureDuplicateTake())
            .ThrowsExactly<DuplicateTakeException>();
    }

    [Test]
    public async Task EnableCache_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var specification = new BuilderExposingSpecification();

        // Act / Assert
        await Assert.That(() => specification.Builder.EnableCache(string.Empty))
            .ThrowsExactly<ArgumentException>();
    }

    private sealed record SampleEntity(int Id, string Name, bool IsActive)
    {
        public IReadOnlyList<string> Children { get; } = ["child"];
    }

    private sealed class SampleEntitySpecification : Specification<SampleEntity>
    {
        public SampleEntitySpecification()
        {
            this.Query
                .Where(entity => entity.IsActive)
                .OrderBy(entity => entity.Name)
                .ThenByDescending(entity => entity.Id)
                .Skip(0)
                .Take(2)
                .PostProcessingAction(items => items.ToArray())
                .AsNoTracking()
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .EnableCache(nameof(SampleEntitySpecification), 2);

            this.Query.Include(entity => entity.Children);
            this.Query.Include("Children");
        }
    }

    private sealed class SampleProjectionSpecification : Specification<SampleEntity, string>
    {
        public SampleProjectionSpecification()
        {
            this.Query.Where(entity => entity.IsActive);
            this.Query.OrderByDescending(entity => entity.Id);
            this.Query.Select(entity => entity.Name);
            this.Query.PostProcessingAction(items => items.Select(item => item.ToUpperInvariant()));
        }
    }

    private sealed class SearchOnlySpecification : Specification<SampleEntity>
    {
        public SearchOnlySpecification() => this.Query.Search(entity => entity.Name, "match");
    }

    private sealed class MissingSelectorSpecification : Specification<SampleEntity, string>
    {
    }

    private sealed class DuplicateSkipSpecification : Specification<SampleEntity>
    {
        public void ConfigureDuplicateSkip()
        {
            this.Query.Skip(1);
            this.Query.Skip(2);
        }
    }

    private sealed class DuplicateTakeSpecification : Specification<SampleEntity>
    {
        public void ConfigureDuplicateTake()
        {
            this.Query.Take(1);
            this.Query.Take(2);
        }
    }

    private sealed class BuilderExposingSpecification : Specification<SampleEntity>
    {
        public ISpecificationBuilder<SampleEntity> Builder => this.Query;
    }
}
