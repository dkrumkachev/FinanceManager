using System.Windows;
using System.Windows.Input;

namespace FinanceManager.MVVM.Views
{
	/// <summary>
	/// Interaction logic for DatePeriodSelectionWindow.xaml
	/// </summary>
	public partial class DatePeriodSelectionWindow : Window
	{
		public DatePeriodSelectionWindow()
		{
			InitializeComponent();
			var mousePosition = Mouse.GetPosition(Owner);
			Left = mousePosition.X;
			Top = mousePosition.Y;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

	}
}
