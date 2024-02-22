using System.Windows;
using System.Windows.Input;

namespace FinanceManager.MVVM.Views
{
	public partial class MainWindow : Window
	{
		private bool isMaximized = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				if (isMaximized)
				{
					WindowState = WindowState.Normal;
					Width = 1280;
					Height = 720;
					isMaximized = false;
				}
				else
				{
					WindowState = WindowState.Maximized;
					isMaximized = true;
				}
			}
		}
	}
}