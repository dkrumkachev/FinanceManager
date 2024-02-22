namespace FinanceManager.MVVM.Services.Interfaces
{
	public interface IDialogService
	{
		public bool? ShowEditCategoryDialog(object dataContext);

		public bool? ShowAddCategoryDialog(object dataContext);

		public bool? ShowEditTransactionDialog(object dataContext);

		public void ShowAddTransactionDialog(object dataContext);

		public bool? ShowSelectDatesDialog(object dataContext);

	}
}
