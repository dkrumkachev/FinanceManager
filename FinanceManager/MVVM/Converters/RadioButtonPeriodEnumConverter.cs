using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FinanceManager.MVVM.Converters
{
	public class RadioButtonValueConverter(object optionValue) : MarkupExtension, IValueConverter
	{
		private readonly object _optionValue = optionValue;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(_optionValue);
		}

		public object ConvertBack(object isChecked, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)isChecked ? _optionValue : Binding.DoNothing;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
