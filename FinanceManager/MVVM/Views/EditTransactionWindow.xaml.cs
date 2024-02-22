using FinanceManager.MVVM.ViewModels;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinanceManager.MVVM.Views
{
	/// <summary>
	/// Interaction logic for EditTransactionWindow.xaml
	/// </summary>
	public partial class EditTransactionWindow : Window
	{
		public EditTransactionWindow()
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
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		[GeneratedRegex("^[.0-9]+")]
		private static partial Regex NumericRegex();

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var regex = NumericRegex();
			if (!regex.IsMatch(e.Text))
			{
				e.Handled = true;
				return;
			}
			var textBox = (TextBox)sender;
			var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
			e.Handled = !decimal.TryParse(fullText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
				CultureInfo.InvariantCulture, out decimal value);
			if (!e.Handled && (value.Scale > 2 || value > DetailsViewModel.AmountMaxValue))
			{
				e.Handled = true;
			}
		}

	}
}
