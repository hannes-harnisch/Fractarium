using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The main menu tab where the color palette can be manipulated.
	/// </summary>
	public class ColorTab : UserControl
	{
		public string[] ComboBoxEntries { get; set; }

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public ColorTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}
	}
}