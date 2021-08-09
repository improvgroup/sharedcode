namespace SharedCode.Windows.WPF.ViewModels
{
	using SharedCode;
	using SharedCode.Windows.WPF.Commands;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;

	public abstract class PageViewModelBase : ViewModelBase, IPersistable, IPageViewModel
	{
		private ICommand? closePageCommand;
		private ICommand? closeAllPagesCommand;
		private ICommand? closeAllPagesButThisCommand;
		private ICommand? editTitleCommand;
		private ICommand? finishTabTextCommand;
		private bool isEditingTitle;
		private bool isSelected;
		private ICommand? saveCommand;
		private ICommand? selectPageCommand;
		private string? subTitle;
		private string? tabIcon;
		private string? tabTitle;

		protected PageViewModelBase(string? title = null, ICommand? selectPageCommand = null)
		{
			this.TabTitle = title ?? string.Empty;
			this.SelectPageCommand = selectPageCommand ??= new AsyncCommand(this.SelectPage, this.CanSelectPage);
		}

		public event EventHandler? AllButThisClosed;
		public event EventHandler? AllClosed;
		public event EventHandler? Closed;
		public event EventHandler<EventArgs<PageViewModelBase>>? OrderSwapped;
		public event EventHandler? Selected;
		public event EventHandler? Saved;
		public event EventHandler? Saving;
		public event EventHandler? Reverted;
		public event EventHandler? Reverting;

		public ICommand CloseAllPagesCommand => this.closeAllPagesCommand ??= new AsyncCommand(this.CloseAllPages, this.CanCloseAllPages);

		public ICommand CloseAllPagesButThisCommand => this.closeAllPagesButThisCommand ??= new AsyncCommand(this.CloseAllPagesButThis, this.CanCloseAllPagesButThis);

		public ICommand ClosePageCommand => this.closePageCommand ??= new AsyncCommand(this.ClosePage, this.CanClosePage);

		public ICommand EditTitleCommand => this.editTitleCommand ??= new AsyncCommand(this.EditTitle, this.CanEditTitle);

		public ICommand FinishTabTextCommand => this.finishTabTextCommand ??= new AsyncCommand(this.FinishTabText, this.CanFinishTabText);

		public ICommand SaveCommand => this.saveCommand ??= new AsyncCommand(this.Save, this.CanSave);

		public ICommand SelectPageCommand
		{
			get => this.selectPageCommand ??= new AsyncCommand(this.SelectPage, this.CanSelectPage);
			private init => this.selectPageCommand = value;
		}

		protected virtual bool CanCloseAllPages(object? arg) => true;
		protected virtual bool CanCloseAllPagesButThis(object? arg) => true;
		protected virtual bool CanClosePage(object? arg) => true;
		protected virtual bool CanEditTitle(object? arg) => true;
		protected virtual bool CanFinishTabText(object? arg) => true;
		protected virtual bool CanSave(object? arg) => true;
		protected virtual bool CanSelectPage(object? arg) => true;

		private async Task CloseAllPages() => this.OnCloseAllPages();
		private async Task CloseAllPagesButThis() => this.OnCloseAllButThis();
		private async Task ClosePage() => this.OnClosePage();
		private async Task EditTitle() => this.IsEditingTitle = true;
		private async Task FinishTabText() => this.IsEditingTitle = false;

		public static PageViewModelBase? DraggingPage { get; set; }

		public bool IsSelected
		{
			get => this.isSelected;
			set => this.SetProperty(ref this.isSelected, value);
		}

		public abstract string Route { get; }

		public abstract string RouteDescription { get; }


		public string SubTitle
		{
			get => this.subTitle ?? string.Empty;
			set => this.SetProperty(ref this.subTitle, value);
		}

		public string TabIcon
		{
			get => this.tabIcon ?? string.Empty;
			set => this.SetProperty(ref this.tabIcon, value);
		}

		public string TabTitle
		{
			get => this.tabTitle ?? string.Empty;
			set => this.SetProperty(ref this.tabTitle, value);
		}

		public bool IsEditingTitle
		{
			get => this.isEditingTitle;
			set => this.SetProperty(ref this.isEditingTitle, value);
		}

		public virtual bool HasUnsavedChanges => this.Components?.Any(c => c.HasUnsavedChanges) ?? false;

		public virtual IEnumerable<string?> UnsavedItems => Enumerable.Empty<string?>();

		public virtual IEnumerable<IPersistable> Components => Enumerable.Empty<IPersistable>();

		public abstract IEnumerable<INavigable> Children(string prefix);

		public abstract IEnumerable<string> ChildRoutes(string prefix);

		public abstract void Navigate(params string[] route);

		public virtual async Task Save()
		{
			this.OnSaving();
			await Task.WhenAll(this.Components.Select(component => component.Save())).ConfigureAwait(false);
			this.OnSaved();
		}

		public virtual Task SelectPage()
		{
			this.IsSelected = true;

			this.OnSelected(this, EventArgs.Empty);

			return Task.CompletedTask;
		}

		public abstract IEnumerable<string> UniquePrefixes(string prefix);

		protected void OnClosePage() => this.OnClosePage(this, EventArgs.Empty);

		protected virtual void OnClosePage(object sender, EventArgs e) => this.Closed?.Invoke(this, e);

		protected void OnCloseAllPages() => this.OnCloseAll(this, EventArgs.Empty);

		protected virtual void OnCloseAll(object sender, EventArgs e) => AllClosed?.Invoke(sender, e);

		protected void OnCloseAllButThis() => this.OnCloseAllButThis(this, EventArgs.Empty);

		protected virtual void OnCloseAllButThis(object sender, EventArgs e) => AllButThisClosed?.Invoke(sender, e);

		protected void OnReverted() => this.OnReverted(this, EventArgs.Empty);
		protected virtual void OnReverted(object sender, EventArgs e) => this.Reverted?.Invoke(sender, e);
		protected void OnReverting() => this.OnReverting(this, EventArgs.Empty);
		protected virtual void OnReverting(object sender, EventArgs e) => this.Reverting?.Invoke(sender, e);

		protected void OnSaved() => this.OnSaved(this, EventArgs.Empty);
		protected virtual void OnSaved(object sender, EventArgs e) => this.Saved?.Invoke(sender, e);
		protected void OnSaving() => this.OnSaving(this, EventArgs.Empty);
		protected virtual void OnSaving(object sender, EventArgs e) => this.Saving?.Invoke(sender, e);

		protected void OnSelected() => this.OnSelected(this, EventArgs.Empty);

		protected virtual void OnSelected(object sender, EventArgs e) => Selected?.Invoke(sender, e);

		protected void OnOrderSwapped(PageViewModelBase pageBase) => this.OnOrderSwapped(this, new EventArgs<PageViewModelBase>(pageBase));

		protected virtual void OnOrderSwapped(object sender, EventArgs<PageViewModelBase> e) => this.OrderSwapped?.Invoke(sender, e);
	}
}
