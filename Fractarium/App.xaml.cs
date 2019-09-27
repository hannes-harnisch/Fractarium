using Avalonia;
using Avalonia.Markup.Xaml;

namespace Fractarium
{
	public class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}