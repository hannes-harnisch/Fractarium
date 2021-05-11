using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The status bar at the bottom of the user interface that shows relevant information about
	/// the current program state.
	/// </summary>
	public class StatusBar : UserControl
	{
		/// <summary>
		/// Sets the information about the real component of the currently hovered over complex point.
		/// </summary>
		public string Coordinates
		{
			set => this.Find<TextBlock>("Coordinates").Text = value;
		}

		/// <summary>
		/// Initializes XAML elements.
		/// </summary>
		public StatusBar()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
