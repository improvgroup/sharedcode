namespace SharedCode.Tests.Models;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using SharedCode.Models;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="Entity"/> and <see cref="Entity{TKey}"/>.
/// </summary>
public class EntityTests
{
	[Test]
	public async Task Entity_DefaultConstructor_HasNewGuidId()
	{
		var entity = new Entity();
		await Assert.That(entity.Id).IsNotEqualTo(Guid.Empty);
	}

	[Test]
	public async Task Entity_ConstructorWithId_HasSpecifiedId()
	{
		var id = Guid.NewGuid();
		var entity = new Entity(id);
		await Assert.That(entity.Id).IsEqualTo(id);
	}

	[Test]
	public async Task Entity_SameId_AreEqual()
	{
		var id = Guid.NewGuid();
		var entity1 = new Entity(id);
		var entity2 = new Entity(id);
		await Assert.That(entity1.Equals(entity2)).IsTrue();
	}

	[Test]
	public async Task Entity_DifferentIds_AreNotEqual()
	{
		var entity1 = new Entity();
		var entity2 = new Entity();
		await Assert.That(entity1.Equals(entity2)).IsFalse();
	}

	[Test]
	public async Task Entity_OperatorNotEquals_DifferentIds_ReturnsTrue()
	{
		var entity1 = new Entity();
		var entity2 = new Entity();
		await Assert.That(entity1 != entity2).IsTrue();
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the == operator.")]
	public async Task Entity_OperatorEquals_BothNull_ReturnsTrue()
	{
		Entity? e1 = null;
		Entity? e2 = null;
		await Assert.That(e1 == e2).IsTrue();
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the == operator.")]
	public async Task Entity_OperatorEquals_OneNull_ReturnsFalse()
	{
		Entity? e1 = new Entity();
		Entity? e2 = null;
		await Assert.That(e1 == e2).IsFalse();
	}

	[Test]
	public async Task Entity_ToString_ReturnsIdString()
	{
		var id = Guid.NewGuid();
		var entity = new Entity(id);
		await Assert.That(entity.ToString()).IsEqualTo(id.ToString());
	}

	[Test]
	public async Task Entity_Events_InitiallyEmpty()
	{
		var entity = new Entity();
		await Assert.That(entity.Events.Count).IsEqualTo(0);
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the Equals method.")]
	public async Task Entity_Equals_Null_ReturnsFalse()
	{
		var entity = new Entity();
		await Assert.That(entity.Equals((Entity?)null)).IsFalse();
	}

	[Test]
	public async Task EntityT_WithIntKey_SameId_AreEqual()
	{
		var e1 = new Entity<int>(42);
		var e2 = new Entity<int>(42);
		await Assert.That(e1.Equals(e2)).IsTrue();
	}

	[Test]
	public async Task EntityT_WithIntKey_DifferentId_AreNotEqual()
	{
		var e1 = new Entity<int>(1);
		var e2 = new Entity<int>(2);
		await Assert.That(e1.Equals(e2)).IsFalse();
	}
}
