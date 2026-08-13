namespace SharedCode.Tests;

using System.Threading.Tasks;
using TUnit.Assertions;
using TUnit.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Tests for the <see cref="ValueObject"/> class.
/// </summary>
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

	[Test]
	public async Task Equals_SameValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		await Assert.That(a.Equals(b)).IsTrue();
	}

	[Test]
	public async Task Equals_DifferentValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		await Assert.That(a.Equals(b)).IsFalse();
	}

	[Test]
	public async Task Equals_DifferentCurrency_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "EUR");
		await Assert.That(a.Equals(b)).IsFalse();
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of Equals.")]
	public async Task Equals_Null_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		await Assert.That(a.Equals((ValueObject?)null)).IsFalse();
	}

	[Test]
	public async Task Equals_DifferentType_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new Address("123 Main St", "Springfield");
		await Assert.That(a.Equals(b)).IsFalse();
	}

	[Test]
	public async Task OperatorEquals_SameValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		await Assert.That(a == b).IsTrue();
	}

	[Test]
	public async Task OperatorEquals_DifferentValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		await Assert.That(a == b).IsFalse();
	}

	[Test]
	public async Task OperatorNotEquals_SameValues_ReturnsFalse()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		await Assert.That(a != b).IsFalse();
	}

	[Test]
	public async Task OperatorNotEquals_DifferentValues_ReturnsTrue()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		await Assert.That(a != b).IsTrue();
	}

	[Test]
	public async Task GetHashCode_SameValues_ReturnsSameHashCode()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(100m, "USD");
		await Assert.That(b.GetHashCode()).IsEqualTo(a.GetHashCode());
	}

	[Test]
	public async Task GetHashCode_DifferentValues_ReturnsDifferentHashCode()
	{
		var a = new MoneyValue(100m, "USD");
		var b = new MoneyValue(200m, "USD");
		await Assert.That(b.GetHashCode()).IsNotEqualTo(a.GetHashCode());
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of operator==.")]
	public async Task OperatorEquals_BothNull_ReturnsTrue()
	{
		MoneyValue? a = null;
		MoneyValue? b = null;
#pragma warning disable CS8604 // Possible null reference argument.
		await Assert.That(a == b).IsTrue();
#pragma warning restore CS8604
	}

	[Test]
	[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Testing null handling of operator==.")]
	public async Task OperatorEquals_OneNull_ReturnsFalse()
	{
		MoneyValue? a = new(100m, "USD");
		MoneyValue? b = null;
#pragma warning disable CS8604 // Possible null reference argument.
		await Assert.That(a == b).IsFalse();
#pragma warning restore CS8604
	}
}
