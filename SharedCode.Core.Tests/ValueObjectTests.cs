namespace SharedCode.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="ValueObject"/> class.
/// </summary>
[TestClass]
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "<Pending>")]
public class ValueObjectTests
{
	private sealed class MoneyValue : ValueObject
	{
		public MoneyValue(decimal amount, string currency)
		{
			this.Amount = amount;
			this.Currency = currency;
		}

		public decimal Amount { get; }
		public string Currency { get; }
	}

	private sealed class Address : ValueObject
	{
		public Address(string street, string city)
		{
			this.Street = street;
			this.City = city;
		}

		public string Street { get; }
		public string City { get; }
	}

	[TestMethod]
	public void Equals_SameValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		Assert.IsTrue(a.Equals(b));
	}

	[TestMethod]
	public void Equals_DifferentValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		Assert.IsFalse(a.Equals(b));
	}

	[TestMethod]
	public void Equals_DifferentCurrency_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "EUR");
		Assert.IsFalse(a.Equals(b));
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of Equals.")]
	public void Equals_Null_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		Assert.IsFalse(a.Equals((ValueObject?)null));
	}

	[TestMethod]
	public void Equals_DifferentType_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new Address("123 Main St", "Springfield");
		Assert.IsFalse(a.Equals(b));
	}

	[TestMethod]
	public void OperatorEquals_SameValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		Assert.IsTrue(a == b);
	}

	[TestMethod]
	public void OperatorEquals_DifferentValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		Assert.IsFalse(a == b);
	}

	[TestMethod]
	public void OperatorNotEquals_SameValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		Assert.IsFalse(a != b);
	}

	[TestMethod]
	public void OperatorNotEquals_DifferentValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		Assert.IsTrue(a != b);
	}

	[TestMethod]
	public void GetHashCode_SameValues_ReturnsSameHashCode()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
	}

	[TestMethod]
	public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of operator==.")]
	public void OperatorEquals_BothNull_ReturnsTrue()
	{
		MoneyValue? a = null;
		MoneyValue? b = null;
#pragma warning disable CS8604 // Possible null reference argument.
		Assert.IsTrue(a == b);
#pragma warning restore CS8604
	}

	[TestMethod]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of operator==.")]
	public void OperatorEquals_OneNull_ReturnsFalse()
	{
		MoneyValue? a = new(100m, "USD");
		MoneyValue? b = null;
#pragma warning disable CS8604 // Possible null reference argument.
		Assert.IsFalse(a == b);
#pragma warning restore CS8604
	}
}
