using FinanceManager.Data.Models;
using FinanceManager.MVVM.Core;
using FinanceManager.MVVM.Enums;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.Services.Interfaces;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;

namespace FinanceManager.MVVM.ViewModels
{
	public abstract class DetailsViewModel : ErrorsViewModel
	{
		private readonly ITransactionService _transactionService;
		private readonly ICategoryService _categoryService;
		private readonly IDialogService _dialogService;

		public const decimal AmountMaxValue = Transaction.MaxAmount;

		private Transaction _transactionToEdit;
		public Transaction TransactionToEdit
		{
			get => _transactionToEdit;
			set
			{
				_transactionToEdit = value;
				OnPropertyChanged();
			}
		}

		private IEnumerable<Category> _categories;
		public IEnumerable<Category> Categories
		{
			get => _categories;
			set
			{
				_categories = value;
				OnPropertyChanged();
			}
		}

		protected ObservableCollection<Transaction> _transactions = [];
		public ObservableCollection<Transaction> Transactions
		{
			get => _transactions;
			set
			{
				_transactions = value;
				OnPropertyChanged();
			}
		}

		protected SeriesCollection _transactionsByCategories = [];
		public SeriesCollection TransactionsByCategories
		{
			get => _transactionsByCategories;
			set
			{
				_transactionsByCategories = value;
				OnPropertyChanged();
			}
		}

		private decimal _totalAmount;
		public decimal TotalAmount
		{
			get => _totalAmount;
			set
			{
				_totalAmount = value;
				OnPropertyChanged();
			}
		}

		private Period _lastSelectedPeriod;
		private Period _selectedPeriod = Period.Today;
		public Period SelectedPeriod
		{
			get => _selectedPeriod;
			set
			{
				_lastSelectedPeriod = _selectedPeriod;
				_selectedPeriod = value;
				OnPropertyChanged();
			}
		}

		public DateTime CustomPeriodStart { get; set; } = DateTime.Today;
		public DateTime CustomPeriodEnd { get; set; } = DateTime.Today;


		public Action TransactionsChanged { get; set; }

		public abstract RelayCommand ChangePeriodCommand { get; set; }

		public RelayCommand DeleteTransactionCommand { get; set; }

		public RelayCommand EditTransactionCommand { get; set; }

		public RelayCommand SaveTransactionCommand { get; set; }

		public RelayCommand SelectCustomPeriodCommand { get; set; }

		public DetailsViewModel(ITransactionService transactionService, ICategoryService categoryService, IDialogService dialogService)
		{
			_transactionService = transactionService;
			_categoryService = categoryService;
			_dialogService = dialogService;

			SelectCustomPeriodCommand = new RelayCommand(_ =>
			{
				if (dialogService.ShowSelectDatesDialog(this) == true && CustomPeriodStart <= CustomPeriodEnd)
				{
					ChangePeriodCommand.Execute(null);
				}
				else
				{
					SelectedPeriod = _lastSelectedPeriod;
					(CustomPeriodStart, CustomPeriodEnd) = (DateTime.Today, DateTime.Today);
				}
			}, _ => true);

			DeleteTransactionCommand = new RelayCommand(async (obj) =>
			{
				if (obj is Transaction transaction)
				{
					await transactionService.DeleteTransactionAsync(transaction.TransactionId);
					Transactions.Remove(transaction);
					GroupTransactionsByCategories();
				}
			}, (_) => true);

			EditTransactionCommand = new RelayCommand(obj =>
			{
				if (obj is Transaction transaction)
				{
					TransactionToEdit = new Transaction(transaction);
					dialogService.ShowEditTransactionDialog(this);
				}
			}, (_) => true);

			SaveTransactionCommand = new RelayCommand(async _ =>
			{
				try
				{
					await transactionService.UpdateTransactionAsync(TransactionToEdit);
				}
				catch (Exception e)
				{
					ErrorMessage = e.Message;
					ShowErrorMessage();
					return;
				}
				HideErrorMessage();
				RequestClose();
				var editedTransaction = Transactions.First(t => t.TransactionId == TransactionToEdit.TransactionId);
				var i = Transactions.IndexOf(editedTransaction);
				Transactions[i] = TransactionToEdit;
				TransactionsChanged?.Invoke();
				GroupTransactionsByCategories();
			}, _ => true);

		}

		protected async Task GetTransactionsForPeriod(bool expenses)
		{
			(DateTime start, DateTime end) = GetDatesOfPeriod(SelectedPeriod);
			Transactions = new(await _transactionService.GetTransactionsAsync(start, end, expenses));
			GroupTransactionsByCategories();
		}

		protected void GroupTransactionsByCategories()
		{
			decimal totalAmount = 0;
			TransactionsByCategories.Clear();
			foreach (Category category in Categories)
			{
				var sumAmount = Transactions.Where(t => t.Category.CategoryId == category.CategoryId).Sum(t => t.Amount);
				if (sumAmount > 0)
				{
					TransactionsByCategories.Add(new PieSeries()
					{
						Title = category.Name,
						Values = new ChartValues<decimal>() { sumAmount }
					});
					totalAmount += sumAmount;
				}
			}
			TotalAmount = totalAmount;
		}

		protected (DateTime, DateTime) GetDatesOfPeriod(Period period)
		{
			return period switch
			{
				Period.Today => (DateTime.Today, DateTime.Today),
				Period.Week => (DateTime.Today.AddDays(-7), DateTime.Today),
				Period.Month => (DateTime.Today.AddMonths(-1), DateTime.Today),
				Period.Year => (DateTime.Today.AddYears(-1), DateTime.Today),
				Period.Custom => (CustomPeriodStart, CustomPeriodEnd),
				_ => (DateTime.Today, DateTime.Today),
			};
		}
	}

}
