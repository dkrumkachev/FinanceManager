using FinanceManager.Data.Models;
using FinanceManager.MVVM.Core;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.Services.Interfaces;
using System.Collections.ObjectModel;

namespace FinanceManager.MVVM.ViewModels
{
	public class MainViewModel : ErrorsViewModel
	{
		public const decimal AmountMaxValue = Transaction.MaxAmount;

		private Transaction _newTransaction;
		public Transaction NewTransaction
		{
			get => _newTransaction;
			set
			{
				_newTransaction = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Category> _shownCategories;
		public ObservableCollection<Category> ShownCategories
		{
			get => _shownCategories;
			set
			{
				_shownCategories = value;
				OnPropertyChanged();
			}
		}

		private INavigationService _navigationService;
		public INavigationService NavigationService
		{
			get => _navigationService;
			set
			{
				_navigationService = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand NavigateExpensesCommand { get; set; }

		public RelayCommand NavigateIncomeCommand { get; set; }

		public RelayCommand NavigateCategoriesCommand { get; set; }

		public RelayCommand ShowAddTransactionWindowCommand { get; set; }

		public RelayCommand ChangeTransactionTypeCommand { get; set; }

		public RelayCommand AddNewTransactionCommand { get; set; }

		public RelayCommand CloseAddTransactionWindowCommand { get; set; }

		public bool IsAddTransactionWindowOpen { get; set; }


		public MainViewModel(INavigationService navigationService, IDialogService dialogService,
			ITransactionService transactionService, ICategoryService categoryService)
		{
			_navigationService = navigationService;

			NavigateExpensesCommand = new RelayCommand(async _ =>
			{
				navigationService.Navigate<ExpensesDetailsViewModel>();
				var expensesViewModel = (ExpensesDetailsViewModel)navigationService.CurrentViewModel;
				expensesViewModel.Categories = await categoryService.GetCategoriesAsync(expenses: true);
				expensesViewModel.ChangePeriodCommand.Execute(null);

			}, _ => true);

			NavigateIncomeCommand = new RelayCommand(async _ =>
			{
				navigationService.Navigate<IncomeDetailsViewModel>();
				var incomeViewModel = (IncomeDetailsViewModel)navigationService.CurrentViewModel;
				incomeViewModel.Categories = await categoryService.GetCategoriesAsync(expenses: false);
				incomeViewModel.ChangePeriodCommand.Execute(null);
			}, _ => true);

			NavigateCategoriesCommand = new RelayCommand(_ => navigationService.Navigate<CategoriesViewModel>(), _ => true);

			ShowAddTransactionWindowCommand = new RelayCommand(async _ =>
			{
				IsAddTransactionWindowOpen = true;
				NewTransaction = new Transaction();
				ShownCategories = new(await categoryService.GetCategoriesAsync(NewTransaction.IsExpense));
				dialogService.ShowAddTransactionDialog(this);
			},
			canExecute: _ => !IsAddTransactionWindowOpen);

			ChangeTransactionTypeCommand = new RelayCommand(async _ =>
			{
				ShownCategories = new(await categoryService.GetCategoriesAsync(NewTransaction.IsExpense));
			}, _ => true);

			AddNewTransactionCommand = new RelayCommand(async _ =>
			{
				try
				{
					await transactionService.CreateTransactionAsync(NewTransaction);
				}
				catch (Exception e)
				{
					ErrorMessage = e.Message;
					ShowErrorMessage();
					return;
				}
				HideErrorMessage();
				RequestClose();
				var currentViewModelType = navigationService.CurrentViewModel.GetType();
				if (NewTransaction.IsExpense && currentViewModelType == typeof(ExpensesDetailsViewModel))
				{
					NavigateExpensesCommand.Execute(null);
				}
				else if (!NewTransaction.IsExpense && currentViewModelType == typeof(IncomeDetailsViewModel))
				{
					NavigateIncomeCommand.Execute(null);
				}

			}, _ => true);

		}

	}

}
