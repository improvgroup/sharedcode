namespace SharedCode.Windows.WPF.ViewModels;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

/// <summary>
/// The persistable interface.
/// </summary>
public interface IPersistable
{
	/// <summary>
	/// Called when changes have been saved.
	/// </summary>
	event EventHandler? Saved;

	/// <summary>
	/// Called when about to save changes.
	/// </summary>
	event EventHandler? Saving;

	/// <summary>
	/// Called when changes have been reverted.
	/// </summary>
	event EventHandler? Reverted;

	/// <summary>
	/// Called when about to revert changes.
	/// </summary>
	event EventHandler? Reverting;

	/// <summary>
	/// Gets a value indicating whether there are unsaved changes.
	/// </summary>
	/// <value>A value indicating whether there are unsaved changes.</value>
	bool HasUnsavedChanges { get; }

	/// <summary>
	/// Gets the save command.
	/// </summary>
	/// <value>The save command.</value>
	ICommand SaveCommand { get; }

	/// <summary>
	/// Gets the unsaved items.
	/// </summary>
	/// <value>The unsaved items.</value>
	IEnumerable<string?> UnsavedItems { get; }

	/// <summary>
	/// Gets the components.
	/// </summary>
	/// <value>The components.</value>
	IEnumerable<IPersistable> Components { get; }

	/// <summary>
	/// Saves the unsaved items.
	/// </summary>
	/// <returns>A <see cref="Task"/>.</returns>
	Task Save();
}
