using FinanceManager.MVVM.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FinanceManager.MVVM.Views
{
	/// <summary>
	/// Interaction logic for EditCategoryWindow.xaml
	/// </summary>
	public partial class EditCategoryWindow : Window
	{
		public EditCategoryWindow()
		{
			InitializeComponent();
			ErrorMessage.SizeChanged += (s, e) =>
			{
				if (e.HeightChanged)
				{
					var change = e.NewSize.Height - e.PreviousSize.Height;
					Height += change;
				}
			};
			Loaded += (s, e) =>
			{
				((ErrorsViewModel)DataContext).RequestClose = Close;
				((ErrorsViewModel)DataContext).ShowErrorMessage = () =>
				{
					ErrorMessage.Height = double.NaN;
				};
				((ErrorsViewModel)DataContext).HideErrorMessage = () =>
				{
					ErrorMessage.Height = 0;
				};
			};

			var mousePosition = Mouse.GetPosition(Owner);
			Left = mousePosition.X;
			Top = mousePosition.Y;
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

	}
}
