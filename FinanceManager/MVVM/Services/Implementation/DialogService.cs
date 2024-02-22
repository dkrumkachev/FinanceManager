using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.MVVM.Views;
using System.Windows;

namespace FinanceManager.MVVM.Services.Implementation
{
	public class DialogService : IDialogService
	{
		public bool? ShowAddCategoryDialog(object dataContext) =>
			ShowModalDialog(new NewCategoryWindow(), dataContext);

		public void ShowAddTransactionDialog(object dataContext) =>
			ShowModelessDialog(new NewTransactionWindow(), dataContext);

		public bool? ShowEditCategoryDialog(object dataContext) =>
			ShowModalDialog(new EditCategoryWindow(), dataContext);

		public bool? ShowEditTransactionDialog(object dataContext) =>
			ShowModalDialog(new EditTransactionWindow(), dataContext);

		public bool? ShowSelectDatesDialog(object dataContext) =>
			ShowModalDialog(new DatePeriodSelectionWindow(), dataContext);

		private static bool? ShowModalDialog(Window window, object dataContext)
		{
			window.DataContext = dataContext;
			return window.ShowDialog();
		}

		private static void ShowModelessDialog(Window window, object dataContext)
		{
			window.DataContext = dataContext;
			window.Show();
		}
	}
}
