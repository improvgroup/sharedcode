namespace SharedCode.Reflection;

using System;
using System.Runtime.CompilerServices;

internal class DeepCloneState
{
	private readonly object[] _baseFromTo = new object[6];
	private int _idx;
	private MiniDictionary? _loops;

	/// <summary>
	/// Adds the known reference.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	public void AddKnownRef(object from, object to)
	{
		if (this._idx < 3)
		{
			this._baseFromTo[this._idx] = from;
			this._baseFromTo[this._idx + 3] = to;
			this._idx++;
			return;
		}

		this._loops ??= new MiniDictionary();
		this._loops.Insert(from, to);
	}

	/// <summary>
	/// Gets the known reference.
	/// </summary>
	/// <param name="from">From.</param>
	/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
	public object? GetKnownRef(object from)
	{
		// this is faster than call Dictionary from begin also, small POCO objects do not have a lot
		// of references
		var baseFromTo = this._baseFromTo;
		if (ReferenceEquals(from, baseFromTo[0])) return baseFromTo[3];
		if (ReferenceEquals(from, baseFromTo[1])) return baseFromTo[4];
		if (ReferenceEquals(from, baseFromTo[2])) return baseFromTo[5];
		return this._loops?.FindEntry(from);
	}

	private class MiniDictionary
	{
		private static readonly int[] _primes =
		{
			3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
			1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
			17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
			187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
			1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
		};

		private int[] _buckets = Array.Empty<int>();

		private int _count;

		private Entry[] _entries = Array.Empty<Entry>();

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniDictionary" /> class.
		/// </summary>
		public MiniDictionary() : this(5)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniDictionary" /> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public MiniDictionary(int capacity)
		{
			if (capacity > 0)
			{
				this.Initialize(capacity);
			}
		}

		[SuppressMessage("Design", "GCop179:Do not hardcode numbers, strings or other values. Use constant fields, enums, config files or database as appropriate.", Justification = "<Pending>")]
		public object? FindEntry(object key)
		{
			if (this._buckets is not null)
			{
				var hashCode = RuntimeHelpers.GetHashCode(key) & 0x7FFFFFFF;
				Entry[] entries1 = this._entries;
				for (var i = this._buckets[hashCode % this._buckets.Length]; i >= 0; i = entries1![i].Next)
				{
					if (entries1[i].HashCode == hashCode && ReferenceEquals(entries1[i].Key, key))
					{
						return entries1[i].Value;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Inserts the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		[SuppressMessage("Design", "GCop179:Do not hardcode numbers, strings or other values. Use constant fields, enums, config files or database as appropriate.", Justification = "<Pending>")]
		public void Insert(object key, object value)
		{
			if (this._buckets is null)
			{
				this.Initialize(0);
			}

			var hashCode = RuntimeHelpers.GetHashCode(key) & 0x7FFFFFFF;
			var targetBucket = hashCode % this._buckets?.Length ?? 0;

			var entries1 = this._entries!;

			if (this._count == entries1.Length)
			{
				this.Resize();
				entries1 = this._entries!;
				targetBucket = hashCode % this._buckets?.Length ?? 0;
			}

			var index = this._count;
			this._count++;

			entries1[index].HashCode = hashCode;
			entries1[index].Next = this._buckets?[targetBucket] ?? 0;
			entries1[index].Key = key;
			entries1[index].Value = value;
			this._buckets![targetBucket] = index;
		}

		[SuppressMessage("Design", "GCop179:Do not hardcode numbers, strings or other values. Use constant fields, enums, config files or database as appropriate.", Justification = "<Pending>")]
		private static int ExpandPrime(int oldSize)
		{
			var newSize = 2 * oldSize;

			return (uint)newSize > 0x7FEFFFFD && 0x7FEFFFFD > oldSize ? 0x7FEFFFFD : GetPrime(newSize);
		}

		private static int GetPrime(int min)
		{
			for (var i = 0; i < _primes.Length; i++)
			{
				var prime = _primes[i];
				if (prime >= min)
				{
					return prime;
				}
			}

			// outside of our predefined table. compute the hard way.
			for (var i = min | 1; i < int.MaxValue; i += 2)
			{
				if (IsPrime(i) && (i - 1) % 101 != 0)
				{
					return i;
				}
			}

			return min;
		}

		private static bool IsPrime(int candidate)
		{
			if ((candidate & 1) != 0)
			{
				var limit = (int)Math.Sqrt(candidate);
				for (var divisor = 3; divisor <= limit; divisor += 2)
				{
					if ((candidate % divisor) == 0)
						return false;
				}

				return true;
			}

			return candidate == 2;
		}

		private void Initialize(int size)
		{
			this._buckets = new int[size];
			for (var i = 0; i < this._buckets.Length; i++)
			{
				this._buckets[i] = -1;
			}

			this._entries = new Entry[size];
		}

		private void Resize() => this.Resize(ExpandPrime(this._count));

		private void Resize(int newSize)
		{
			var newBuckets = new int[newSize];
			for (var i = 0; i < newBuckets.Length; i++)
			{
				newBuckets[i] = -1;
			}

			var newEntries = new Entry[newSize];
			Array.Copy(this._entries, 0, newEntries, 0, this._count);

			for (var i = 0; i < this._count; i++)
			{
				if (newEntries[i].HashCode >= 0)
				{
					var bucket = newEntries[i].HashCode % newSize;
					newEntries[i].Next = newBuckets[bucket];
					newBuckets[bucket] = i;
				}
			}

			this._buckets = newBuckets;
			this._entries = newEntries;
		}

		private struct Entry
		{
			public int HashCode;
			public object Key;
			public int Next;
			public object Value;
		}
	}
}
