using FinanceManager.MVVM.ViewModels;
using System.Windows.Controls;

namespace FinanceManager.MVVM.Views
{
	/// <summary>
	/// Interaction logic for Details.xaml
	/// </summary>
	public partial class Details : UserControl
	{
		public Details()
		{
			InitializeComponent();
			Loaded += (s, e) =>
			{
				((DetailsViewModel)DataContext).TransactionsChanged = () => TransactionsDataGrid.Items.Refresh();
			};
		}

	}
}
