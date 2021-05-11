using Avalonia;

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
			_ = BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		}

		/// <summary>
		/// Method necessary for the designer to work. Prepares the application launch.
		/// </summary>
		/// <returns>Object that initializes platform-specific services for the app.</returns>
		public static AppBuilder BuildAvaloniaApp()
		{
			return AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
		}
	}
}
