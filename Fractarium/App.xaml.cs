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
		/// Gets a static reference to the running instance's window.
		/// </summary>
		public static MainWindow Context => (MainWindow)Current.MainWindow;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}