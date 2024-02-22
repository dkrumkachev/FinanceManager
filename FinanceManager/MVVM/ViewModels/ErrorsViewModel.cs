namespace FinanceManager.MVVM.ViewModels
{
	public class ErrorsViewModel : ViewModelBase
	{
		public Action RequestClose { get; set; }

		public Action ShowErrorMessage { get; set; }

		public Action HideErrorMessage { get; set; }

		private string _errorMessage;
		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				OnPropertyChanged();
			}
		}

	}
}
