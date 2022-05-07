// <copyright file="ObservableCollectionExtensions.cs" company="improvGroup, LLC">
//     Copyright © improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Collections
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// The observable collection extensions class.
	/// </summary>
	public static class ObservableCollectionExtensions
	{
		/// <summary>
		/// Adds the specified enumerable to the observable collection.
		/// </summary>
		/// <typeparam name="T">The type of items in the collection.</typeparam>
		/// <param name="observableCollection">The observable collection.</param>
		/// <param name="enumerable">The enumerable to add.</param>
		/// <returns>The observable collection.</returns>
		/// <exception cref="ArgumentNullException">collection</exception>
		public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> enumerable)
		{
			_ = observableCollection ?? throw new ArgumentNullException(nameof(observableCollection));
			_ = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
			Contract.Ensures(Contract.Result<ObservableCollection<T>>() != null);

			foreach (var item in enumerable)
			{
				observableCollection.Add(item);
			}

			return observableCollection;
		}
	}
}
