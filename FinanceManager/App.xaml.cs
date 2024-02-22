using FinanceManager.Data.Repositories.Implementations;
using FinanceManager.Data.Repositories.Interfaces;
using FinanceManager.MVVM.Services.Implementation;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.MVVM.ViewModels;
using FinanceManager.MVVM.Views;
using FinanceManager.Services.Implementations;
using FinanceManager.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FinanceManager
{
	public partial class App : Application
	{
		private readonly IServiceProvider _serviceProvider;

		public App()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddSingleton(serviceProvider => new MainWindow()
			{
				DataContext = serviceProvider.GetRequiredService<MainViewModel>()
			});

			services.AddSingleton<MainViewModel>();
			services.AddSingleton<ExpensesDetailsViewModel>();
			services.AddSingleton<IncomeDetailsViewModel>();
			services.AddTransient<CategoriesViewModel>();

			services.AddSingleton<IDialogService, DialogService>();
			services.AddSingleton<INavigationService, NavigationService>();

			services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider =>
				(viewModelType) => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

			services.AddSingleton<ICategoryRepository, CategoryRepository>();
			services.AddSingleton<ITransactionRepository, TransactionRepository>();
			services.AddSingleton<ICategoryService, CategoryService>();
			services.AddSingleton<ITransactionService, TransactionService>();

			_serviceProvider = services.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			_serviceProvider.GetRequiredService<INavigationService>().Navigate<ExpensesDetailsViewModel>();
			_serviceProvider.GetRequiredService<MainWindow>().Show();
			base.OnStartup(e);
		}
	}

}
