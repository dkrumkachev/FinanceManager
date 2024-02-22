using FinanceManager.MVVM.Core;
using FinanceManager.MVVM.Services.Interfaces;
using FinanceManager.MVVM.ViewModels;

namespace FinanceManager.MVVM.Services.Implementation
{
	public class NavigationService(Func<Type, ViewModelBase> viewModelFactory) : ObservableObject, INavigationService
	{
		private readonly Func<Type, ViewModelBase> _viewModelFactory = viewModelFactory;

		private ViewModelBase _currentView;
		public ViewModelBase CurrentViewModel
		{
			get => _currentView;
			private set
			{
				_currentView = value;
				OnPropertyChanged();
			}
		}

		public void Navigate<TViewModel>() where TViewModel : ViewModelBase
		{
			CurrentViewModel = _viewModelFactory(typeof(TViewModel));
		}
	}
}
