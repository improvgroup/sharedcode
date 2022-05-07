// <copyright file="PageViewModelBase.cs" company="improvGroup, LLC">
//     Copyright © 2022 improvGroup, LLC. All Rights Reserved.
// </copyright>

namespace SharedCode.Windows.WPF.ViewModels
{
	using SharedCode;
	using SharedCode.Threading.Tasks;
	using SharedCode.Windows.WPF.Commands;

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// The page view model base class. Implements the <see cref="ViewModelBase" />. Implements the
	/// <see cref="IPersistable" />. Implements the <see cref="IPageViewModel" />.
	/// </summary>
	/// <seealso cref="ViewModelBase" />
	/// <seealso cref="IPersistable" />
	/// <seealso cref="IPageViewModel" />
	public abstract class PageViewModelBase : ViewModelBase, IPersistable, IPageViewModel
	{
		private ICommand? closeAllPagesButThisCommand;
		private ICommand? closeAllPagesCommand;
		private ICommand? closePageCommand;
		private ICommand? editTitleCommand;
		private ICommand? finishTabTextCommand;
		private bool isEditingTitle;
		private bool isSelected;
		private ICommand? saveCommand;
		private ICommand? selectPageCommand;
		private string? subTitle;
		private string? tabIcon;
		private string? tabTitle;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageViewModelBase" /> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="selectPageCommand">The select page command.</param>
		protected PageViewModelBase(string? title = null, ICommand? selectPageCommand = null)
		{
			this.TabTitle = title ?? string.Empty;
			this.SelectPageCommand = selectPageCommand ??= new AsyncCommand(this.SelectPage, this.CanSelectPage);
		}

		/// <summary>
		/// Occurs when [all but this closed].
		/// </summary>
		public event EventHandler? AllButThisClosed;

		/// <summary>
		/// Occurs when [all closed].
		/// </summary>
		public event EventHandler? AllClosed;

		/// <summary>
		/// Occurs when [closed].
		/// </summary>
		public event EventHandler? Closed;

		/// <summary>
		/// Occurs when [order swapped].
		/// </summary>
		public event EventHandler<EventArgs<PageViewModelBase>>? OrderSwapped;

		/// <inheritdoc />
		public event EventHandler? Reverted;

		/// <inheritdoc />
		public event EventHandler? Reverting;

		/// <inheritdoc />
		public event EventHandler? Saved;

		/// <inheritdoc />
		public event EventHandler? Saving;

		/// <summary>
		/// Occurs when [selected].
		/// </summary>
		public event EventHandler? Selected;

		/// <summary>
		/// Gets or sets the dragging page.
		/// </summary>
		/// <value>The dragging page.</value>
		public static PageViewModelBase? DraggingPage { get; set; }

		/// <summary>
		/// Gets the close all pages but this command.
		/// </summary>
		/// <value>The close all pages but this command.</value>
		public ICommand CloseAllPagesButThisCommand => this.closeAllPagesButThisCommand ??= new AsyncCommand(this.CloseAllPagesButThis, this.CanCloseAllPagesButThis);

		/// <summary>
		/// Gets the close all pages command.
		/// </summary>
		/// <value>The close all pages command.</value>
		public ICommand CloseAllPagesCommand => this.closeAllPagesCommand ??= new AsyncCommand(this.CloseAllPages, this.CanCloseAllPages);

		/// <summary>
		/// Gets the close page command.
		/// </summary>
		/// <value>The close page command.</value>
		public ICommand ClosePageCommand => this.closePageCommand ??= new AsyncCommand(this.ClosePage, this.CanClosePage);

		/// <inheritdoc />
		public virtual IEnumerable<IPersistable> Components => Enumerable.Empty<IPersistable>();

		/// <summary>
		/// Gets the edit title command.
		/// </summary>
		/// <value>The edit title command.</value>
		public ICommand EditTitleCommand => this.editTitleCommand ??= new AsyncCommand(this.EditTitle, this.CanEditTitle);

		/// <summary>
		/// Gets the finish tab text command.
		/// </summary>
		/// <value>The finish tab text command.</value>
		public ICommand FinishTabTextCommand => this.finishTabTextCommand ??= new AsyncCommand(this.FinishTabText, this.CanFinishTabText);

		/// <inheritdoc />
		public virtual bool HasUnsavedChanges => this.Components?.Any(c => c.HasUnsavedChanges) ?? false;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is editing title.
		/// </summary>
		/// <value><c>true</c> if this instance is editing title; otherwise, <c>false</c>.</value>
		public bool IsEditingTitle
		{
			get => this.isEditingTitle;
			set => this.SetProperty(ref this.isEditingTitle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected
		{
			get => this.isSelected;
			set => this.SetProperty(ref this.isSelected, value);
		}

		/// <summary>
		/// Gets the route.
		/// </summary>
		/// <value>The route.</value>
		public abstract string Route { get; }

		/// <summary>
		/// Gets the route description.
		/// </summary>
		/// <value>The route description.</value>
		public abstract string RouteDescription { get; }

		/// <inheritdoc />
		public ICommand SaveCommand => this.saveCommand ??= new AsyncCommand(this.Save, this.CanSave);

		/// <summary>
		/// Gets the select page command.
		/// </summary>
		/// <value>The select page command.</value>
		public ICommand SelectPageCommand
		{
			get => this.selectPageCommand ??= new AsyncCommand(this.SelectPage, this.CanSelectPage);
			private init => this.selectPageCommand = value;
		}

		/// <summary>
		/// Gets or sets the sub title.
		/// </summary>
		/// <value>The sub title.</value>
		public string SubTitle
		{
			get => this.subTitle ?? string.Empty;
			set => this.SetProperty(ref this.subTitle, value);
		}

		/// <summary>
		/// Gets or sets the tab icon.
		/// </summary>
		/// <value>The tab icon.</value>
		public string TabIcon
		{
			get => this.tabIcon ?? string.Empty;
			set => this.SetProperty(ref this.tabIcon, value);
		}

		/// <summary>
		/// Gets or sets the tab title.
		/// </summary>
		/// <value>The tab title.</value>
		public string TabTitle
		{
			get => this.tabTitle ?? string.Empty;
			set => this.SetProperty(ref this.tabTitle, value);
		}

		/// <inheritdoc />
		public virtual IEnumerable<string?> UnsavedItems => Enumerable.Empty<string?>();

		/// <summary>
		/// Gets the children with the specified prefix.
		/// </summary>
		/// <param name="prefix">The filter prefix.</param>
		/// <returns>IEnumerable&lt;INavigable&gt;.</returns>
		public abstract IEnumerable<INavigable> Children(string prefix);

		/// <summary>
		/// Gets the child the routes.
		/// </summary>
		/// <param name="prefix">The filter prefix.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		public abstract IEnumerable<string> ChildRoutes(string prefix);

		/// <summary>
		/// Navigates the specified route.
		/// </summary>
		/// <param name="route">The route to navigate to.</param>
		public abstract void Navigate(params string[] route);

		/// <inheritdoc />
		public virtual async Task Save()
		{
			this.OnSaving();
			await Task.WhenAll(this.Components.Select(component => component.Save())).ConfigureAwait(false);
			this.OnSaved();
		}

		/// <summary>
		/// Selects the page.
		/// </summary>
		/// <returns>Task.</returns>
		public virtual Task SelectPage()
		{
			this.IsSelected = true;

			this.OnSelected(this, EventArgs.Empty);

			return Task.CompletedTask;
		}

		/// <summary>
		/// Uniques the prefixes.
		/// </summary>
		/// <param name="prefix">The string prefix.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		public abstract IEnumerable<string> UniquePrefixes(string prefix);

		/// <summary>
		/// Determines whether this instance [can close all pages] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can close all pages] the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanCloseAllPages(object? arg) => true;

		/// <summary>
		/// Determines whether this instance [can close all pages but this] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can close all pages but this] the specified argument;
		/// otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanCloseAllPagesButThis(object? arg) => true;

		/// <summary>
		/// Determines whether this instance [can close page] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can close page] the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanClosePage(object? arg) => true;

		/// <summary>
		/// Determines whether this instance [can edit title] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can edit title] the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanEditTitle(object? arg) => true;

		/// <summary>
		/// Determines whether this instance [can finish tab text] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can finish tab text] the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanFinishTabText(object? arg) => true;

		/// <summary>
		/// Determines whether this instance can save the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance can save the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanSave(object? arg) => true;

		/// <summary>
		/// Determines whether this instance [can select page] the specified argument.
		/// </summary>
		/// <param name="arg">The argument.</param>
		/// <returns>
		/// <c>true</c> if this instance [can select page] the specified argument; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool CanSelectPage(object? arg) => true;

		/// <summary>
		/// Handles the CloseAll event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnCloseAll(object sender, EventArgs e) => AllClosed?.Invoke(sender, e);

		/// <summary>
		/// Handles the CloseAllButThis event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnCloseAllButThis(object sender, EventArgs e) => AllButThisClosed?.Invoke(sender, e);

		/// <summary>
		/// Called when [close all but this].
		/// </summary>
		protected void OnCloseAllButThis() => this.OnCloseAllButThis(this, EventArgs.Empty);

		/// <summary>
		/// Called when [close all pages].
		/// </summary>
		protected void OnCloseAllPages() => this.OnCloseAll(this, EventArgs.Empty);

		/// <summary>
		/// Handles the <see cref="ClosePage" /> event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnClosePage(object sender, EventArgs e) => this.Closed?.Invoke(this, e);

		/// <summary>
		/// Called when [close page].
		/// </summary>
		protected void OnClosePage() => this.OnClosePage(this, EventArgs.Empty);

		/// <summary>
		/// Called when [order swapped].
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The arguments.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnOrderSwapped(object sender, EventArgs<PageViewModelBase> e) => this.OrderSwapped?.Invoke(sender, e);

		/// <summary>
		/// Called when [order swapped].
		/// </summary>
		/// <param name="pageBase">The page base.</param>
		protected void OnOrderSwapped(PageViewModelBase pageBase) => this.OnOrderSwapped(this, new EventArgs<PageViewModelBase>(pageBase));

		/// <summary>
		/// Handles the Reverted event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnReverted(object sender, EventArgs e) => this.Reverted?.Invoke(sender, e);

		/// <summary>
		/// Called when [reverted].
		/// </summary>
		protected void OnReverted() => this.OnReverted(this, EventArgs.Empty);

		/// <summary>
		/// Handles the Reverting event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnReverting(object sender, EventArgs e) => this.Reverting?.Invoke(sender, e);

		/// <summary>
		/// Called when [reverting].
		/// </summary>
		protected void OnReverting() => this.OnReverting(this, EventArgs.Empty);

		/// <summary>
		/// Handles the <see cref="Saved" /> event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnSaved(object sender, EventArgs e) => this.Saved?.Invoke(sender, e);

		/// <summary>
		/// Called when [saved].
		/// </summary>
		protected void OnSaved() => this.OnSaved(this, EventArgs.Empty);

		/// <summary>
		/// Handles the <see cref="Saving" /> event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnSaving(object sender, EventArgs e) => this.Saving?.Invoke(sender, e);

		/// <summary>
		/// Called when [saving].
		/// </summary>
		protected void OnSaving() => this.OnSaving(this, EventArgs.Empty);

		/// <summary>
		/// Handles the <see cref="Selected" /> event.
		/// </summary>
		/// <param name="sender">The page.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		[SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "Considered. Rejected.")]
		protected virtual void OnSelected(object sender, EventArgs e) => Selected?.Invoke(sender, e);

		/// <summary>
		/// Called when [selected].
		/// </summary>
		protected void OnSelected() => this.OnSelected(this, EventArgs.Empty);

		/// <summary>
		/// Closes all pages.
		/// </summary>
		private Task CloseAllPages()
		{
			Task.Run(() => this.OnCloseAllPages()).SafeFireAndForgetAsync();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Closes all pages but this.
		/// </summary>
		private Task CloseAllPagesButThis()
		{
			Task.Run(() => this.OnCloseAllButThis()).SafeFireAndForgetAsync();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Closes the page.
		/// </summary>
		private Task ClosePage()
		{
			Task.Run(() => this.OnClosePage()).SafeFireAndForgetAsync();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Edits the title.
		/// </summary>
		private Task EditTitle()
		{
			this.IsEditingTitle = true;
			return Task.CompletedTask;
		}

		/// <summary>
		/// Finishes the tab text.
		/// </summary>
		private Task FinishTabText()
		{
			this.IsEditingTitle = false;
			return Task.CompletedTask;
		}
	}
}
