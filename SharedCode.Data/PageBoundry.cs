// <copyright file="PageBoundry.cs" company="improvGroup, LLC">
//     Copyright © 2021 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Data;

/// <summary>
/// The page boundry class.
/// </summary>
public class PageBoundry
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PageBoundry" /> class.
	/// </summary>
	/// <param name="firstItemZeroIndex">First index of the item zero.</param>
	/// <param name="lastItemZeroIndex">Last index of the item zero.</param>
	public PageBoundry(int firstItemZeroIndex, int lastItemZeroIndex)
	{
		this.FirstItemZeroIndex = firstItemZeroIndex;
		this.LastItemZeroIndex = lastItemZeroIndex;
	}

	/// <summary>
	/// Gets the first index of the item zero.
	/// </summary>
	/// <value>The first index of the item zero.</value>
	public int FirstItemZeroIndex { get; }

	/// <summary>
	/// Gets the last index of the item zero.
	/// </summary>
	/// <value>The last index of the item zero.</value>
	public int LastItemZeroIndex { get; }
}
