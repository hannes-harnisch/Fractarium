using Avalonia;
using Avalonia.Markup.Xaml;

using Fractarium.UserInterface;

namespace Fractarium
{
	/// <summary>
	/// Encapsulates the Fractarium desktop app.
	/// </summary>
	public class App : Application
	{
		/// <summary>
		/// Returns a static reference to the current app instance's global data context.
		/// </summary>
		public static Context Context => (Context)Current.MainWindow.DataContext;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}