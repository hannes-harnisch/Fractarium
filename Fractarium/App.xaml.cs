using Avalonia;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;

namespace Fractarium
{
	public class App : Application
	{
		public static BaseParameters Parameters = new BaseParameters();

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}