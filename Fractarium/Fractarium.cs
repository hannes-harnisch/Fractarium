using Avalonia;
using Avalonia.Logging.Serilog;

using Fractarium.UserInterface;

namespace Fractarium
{
	public class Fractarium
	{
		public static void Main(string[] args)
		{
			BuildAvaloniaApp().Start((app, appArgs) => app.Run(new MainWindow()), args);
		}

		public static AppBuilder BuildAvaloniaApp()
		{
			return AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug();
		}
	}
}