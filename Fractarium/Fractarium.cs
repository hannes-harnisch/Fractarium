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
			AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug().UseReactiveUI().Start(App, args);
		}

		private static void App(Application app, string[] args)
		{
			app.Run(new MainWindow { DataContext = new MainWindowViewModel() });
		}
	}
}