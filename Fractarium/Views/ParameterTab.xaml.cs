using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Fractarium.ViewModels;

namespace Fractarium.Views
{
	public class ParameterTab : UserControl
	{
		public ParameterTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = new ParameterViewModel();
		}
	}
}