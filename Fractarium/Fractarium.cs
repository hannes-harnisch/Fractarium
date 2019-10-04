using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;

using Fractarium.UserInterface;

namespace Fractarium
{
	/// <summary>
	/// Entry point for the application.
	/// </summary>
	public static class Fractarium
	{
		/// <summary>
		/// Starts the application.
		/// </summary>
		/// <param name="args">Optional arguments given in the command line.</param>
		public static void Main(string[] args)
		{
			BuildAvaloniaApp().Start((app, appArgs) =>
			{
				var window = new Window { Content = new MainLayout(), Title = "Fractarium" };
#if DEBUG
				window.AttachDevTools();
#endif
				app.Run(window);
			}, args);
		}

		/// <summary>
		/// Method necessary for the designer to work. Prepares the application launch.
		/// </summary>
		/// <returns>Launcher object for the app.</returns>
		public static AppBuilder BuildAvaloniaApp()
		{
			return AppBuilder.Configure<App>().UsePlatformDetect().LogToDebug();
		}
	}
}