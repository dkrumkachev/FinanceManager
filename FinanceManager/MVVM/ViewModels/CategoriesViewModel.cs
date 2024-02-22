using FinanceManager.Data.Models;
using FinanceManager.MVVM.Core;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.Services.Interfaces;
using System.Collections.ObjectModel;

namespace FinanceManager.MVVM.ViewModels
{
	public class CategoriesViewModel : ErrorsViewModel
	{

		private ObservableCollection<Category> _expenseCategories = [];
		public ObservableCollection<Category> ExpenseCategories
		{
			get => _expenseCategories;
			set
			{
				_expenseCategories = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Category> _incomeCategories = [];
		public ObservableCollection<Category> IncomeCategories
		{
			get => _incomeCategories;
			set
			{
				_incomeCategories = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand DeleteCategoryCommand { get; set; }

		public RelayCommand EditCategoryCommand { get; set; }

		public RelayCommand NewCategoryCommand { get; set; }

		public RelayCommand SaveCategoryCommand { get; set; }

		public RelayCommand AddCategoryCommand { get; set; }

		public Category CategoryToFill { get; set; }

		public CategoriesViewModel(ICategoryService categoryService, IDialogService dialogService)
		{
			Task.Run(async () =>
			{
				ExpenseCategories = new(await categoryService.GetCategoriesAsync(expenses: true));
				IncomeCategories = new(await categoryService.GetCategoriesAsync(expenses: false));
			});

			DeleteCategoryCommand = new RelayCommand(async (obj) =>
			{
				if (obj is Category category)
				{
					if (category.IsForExpenses)
					{
						ExpenseCategories.Remove(category);
					}
					else
					{
						IncomeCategories.Remove(category);
					}
					await categoryService.DeleteCategoryAsync(category.CategoryId);
				}
			}, (_) => true);

			EditCategoryCommand = new RelayCommand(obj =>
			{
				if (obj is Category category)
				{
					CategoryToFill = new Category(category);
					dialogService.ShowEditCategoryDialog(this);

				}
			}, (_) => true);

			SaveCategoryCommand = new RelayCommand(async _ =>
			{
				try
				{
					await categoryService.UpdateCategoryAsync(CategoryToFill);
				}
				catch (Exception e)
				{
					ErrorMessage = e.Message;
					ShowErrorMessage();
					return;
				}
				HideErrorMessage();
				RequestClose();

				void replaceCategoryInCollection(ObservableCollection<Category> categories)
				{
					var editedCategory = categories.First(c => c.CategoryId == CategoryToFill.CategoryId);
					var i = categories.IndexOf(editedCategory);
					categories[i] = CategoryToFill;
				}
				replaceCategoryInCollection(CategoryToFill.IsForExpenses ? ExpenseCategories : IncomeCategories);
			}, _ => true);

			NewCategoryCommand = new RelayCommand(obj =>
			{
				if (obj is bool isForExpense)
				{
					CategoryToFill = new Category() { IsForExpenses = isForExpense };
					dialogService.ShowAddCategoryDialog(this);
				}
			}, (_) => true);

			AddCategoryCommand = new RelayCommand(async _ =>
			{
				try
				{
					await categoryService.CreateCategoryAsync(CategoryToFill);
				}
				catch (Exception e)
				{
					ErrorMessage = e.Message;
					ShowErrorMessage();
					return;
				}
				HideErrorMessage();
				RequestClose();
				if (CategoryToFill.IsForExpenses)
				{
					ExpenseCategories.Add(CategoryToFill);
				}
				else
				{
					IncomeCategories.Add(CategoryToFill);
				}
			}, _ => true);
		}


	}
}
