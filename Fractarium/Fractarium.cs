using Avalonia;
using Avalonia.Logging.Serilog;

using Fractarium.ViewModels;
using Fractarium.Views;

namespace Fractarium
{
	public class Fractarium
	{
		public static void Main(string[] args)
		{
			BuildAvaloniaApp().Start((app, appArgs) =>
			{
				app.Run(new MainWindow { DataContext = new MainWindowViewModel() });
			}, args);
		}

		public static AppBuilder BuildAvaloniaApp()
		{
			return AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().UseReactiveUI();
		}
	}
}