namespace SharedCode.Tests.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedCode.Models;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for <see cref="Entity"/> and <see cref="Entity{TKey}"/>.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class EntityTests
{
	[TestMethod]
	public void Entity_DefaultConstructor_HasNewGuidId()
	{
		var entity = new Entity();
		Assert.AreNotEqual(Guid.Empty, entity.Id);
	}

	[TestMethod]
	public void Entity_ConstructorWithId_HasSpecifiedId()
	{
		var id = Guid.NewGuid();
		var entity = new Entity(id);
		Assert.AreEqual(id, entity.Id);
	}

	[TestMethod]
	public void Entity_SameId_AreEqual()
	{
		var id = Guid.NewGuid();
		var entity1 = new Entity(id);
		var entity2 = new Entity(id);
		Assert.IsTrue(entity1.Equals(entity2));
	}

	[TestMethod]
	public void Entity_DifferentIds_AreNotEqual()
	{
		var entity1 = new Entity();
		var entity2 = new Entity();
		Assert.IsFalse(entity1.Equals(entity2));
	}

	[TestMethod]
	public void Entity_OperatorNotEquals_DifferentIds_ReturnsTrue()
	{
		var entity1 = new Entity();
		var entity2 = new Entity();
		Assert.IsTrue(entity1 != entity2);
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the == operator.")]
	public void Entity_OperatorEquals_BothNull_ReturnsTrue()
	{
		Entity? e1 = null;
		Entity? e2 = null;
		Assert.IsTrue(e1 == e2);
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the == operator.")]
	public void Entity_OperatorEquals_OneNull_ReturnsFalse()
	{
		Entity? e1 = new Entity();
		Entity? e2 = null;
		Assert.IsFalse(e1 == e2);
	}

	[TestMethod]
	public void Entity_ToString_ReturnsIdString()
	{
		var id = Guid.NewGuid();
		var entity = new Entity(id);
		Assert.AreEqual(id.ToString(), entity.ToString());
	}

	[TestMethod]
	public void Entity_Events_InitiallyEmpty()
	{
		var entity = new Entity();
		Assert.AreEqual(0, entity.Events.Count);
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of the Equals method.")]
	public void Entity_Equals_Null_ReturnsFalse()
	{
		var entity = new Entity();
		Assert.IsFalse(entity.Equals((Entity?)null));
	}

	[TestMethod]
	public void EntityT_WithIntKey_SameId_AreEqual()
	{
		var e1 = new Entity<int>(42);
		var e2 = new Entity<int>(42);
		Assert.IsTrue(e1.Equals(e2));
	}

	[TestMethod]
	public void EntityT_WithIntKey_DifferentId_AreNotEqual()
	{
		var e1 = new Entity<int>(1);
		var e2 = new Entity<int>(2);
		Assert.IsFalse(e1.Equals(e2));
	}
}
