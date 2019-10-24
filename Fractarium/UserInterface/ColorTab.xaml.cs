using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;

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
		}

		/// <summary>
		/// Performs all UI updates for when a palette is set in the data context.
		/// </summary>
		/// <param name="palette">The palette on which the updates should be based.</param>
		public unsafe void Update(Palette palette)
		{
			this.Find<TextBlock>("PaletteSize").Text = palette.Size.ToString();
			SetColorSelectionEntries(palette.Size);

			int previewWidth = 100;
			int previewHeight = 35;

			var continuousPreview = this.Find<Image>("ContinuousPreview");
			fixed(int* ptr = &(new int[previewWidth * previewHeight])[0])
			{
				palette.DrawContinuousPreview(previewWidth, previewHeight, ptr);
				continuousPreview.Source = App.MakeDefaultBitmap(previewWidth, previewHeight, ptr);
				continuousPreview.InvalidateVisual();
			}

			var discretePreview = this.Find<Image>("DiscretePreview");
			fixed(int* ptr = &(new int[previewWidth * previewHeight])[0])
			{
				palette.DrawDiscretePreview(previewWidth, previewHeight, ptr);
				discretePreview.Source = App.MakeDefaultBitmap(previewWidth, previewHeight, ptr);
				discretePreview.InvalidateVisual();
			}
		}

		public void SetColorSelectionEntries(int paletteSize)
		{
			string[] colorSelectionEntries = new string[paletteSize + 1];
			colorSelectionEntries[0] = "Set element color";
			for(int i = 1; i < colorSelectionEntries.Length; i++)
				colorSelectionEntries[i] = $"Color #{i}";
			this.Find<ComboBox>("ColorSelector").Items = colorSelectionEntries;
		}

		public void OnPaletteSizeSpin(object sender, SpinEventArgs e)
		{
			var spinner = (TextBlock)((ButtonSpinner)sender).Content;
			int.TryParse(spinner.Text, out int result);
			int size = e.Direction == SpinDirection.Increase ? ++result : --result;
			if(size > 0 && size < Palette.MaxColors)
			{
				spinner.Text = size.ToString();
				SetColorSelectionEntries(size);
			}
		}

		public void OnColorSelected(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}