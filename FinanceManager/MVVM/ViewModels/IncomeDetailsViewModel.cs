using FinanceManager.MVVM.Core;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.Services.Interfaces;

namespace FinanceManager.MVVM.ViewModels
{
	public class IncomeDetailsViewModel : DetailsViewModel
	{
		public override RelayCommand ChangePeriodCommand { get; set; }

		public IncomeDetailsViewModel(ITransactionService transactionService, ICategoryService categoryService,
			IDialogService dialogService) : base(transactionService, categoryService, dialogService)
		{
			ChangePeriodCommand = new RelayCommand(async _ => await GetTransactionsForPeriod(expenses: false), _ => true);
			Task.Run(async () => Categories = await categoryService.GetCategoriesAsync(expenses: true)).Wait();
			ChangePeriodCommand.Execute(null);
		}

	}
}
