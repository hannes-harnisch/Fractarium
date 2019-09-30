using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The main menu of the user interface, holding various tabs.
	/// </summary>
	public class MainMenu : UserControl
	{
		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public MainMenu()
		{
			AvaloniaXamlLoader.Load(this);
		}

		/// <summary>
		/// Starts the rendering procedure that updates the displayed fractal image.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnRenderButton(object sender, RoutedEventArgs e)
		{
			App.Context.Render();
		}
	}
}