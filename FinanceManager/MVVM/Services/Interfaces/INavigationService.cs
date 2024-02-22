using FinanceManager.MVVM.ViewModels;

namespace FinanceManager.MVVM.Services.Interfaces
{
	public interface INavigationService
	{
		ViewModelBase CurrentViewModel { get; }

		void Navigate<T>() where T : ViewModelBase;

	}
}
