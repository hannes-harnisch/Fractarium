using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The main menu tab where the color palette can be manipulated.
	/// </summary>
	public class ColorTab : UserControl
	{
		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public ColorTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		public void OnPaletteSizeSpin(object sender, SpinEventArgs e)
		{
			var spinner = (TextBlock)((ButtonSpinner)sender).Content;
			bool parsed = int.TryParse(spinner.Text, out int result);
			int size = result + (e.Direction == SpinDirection.Increase ? 1 : -1);
			if(parsed && size > 0 && size < 100)
			{
				spinner.Text = size.ToString();

				string[] colorSelectionEntries = new string[size + 1];
				colorSelectionEntries[0] = "Set element color";
				for(int i = 1; i < colorSelectionEntries.Length; i++)
					colorSelectionEntries[i] = $"Color #{i}";
				this.Find<ComboBox>("ColorSelector").Items = colorSelectionEntries;
			}
		}

		public void OnColorSelected(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}