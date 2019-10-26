using System;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using Fractarium.Logic;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The main menu tab where the color palette can be manipulated.
	/// </summary>
	public class ColorTab : UserControl
	{
		/// <summary>
		/// Holds the color of the color preview.
		/// </summary>
		public SolidColorBrush ColorPreview { get; set; } = new SolidColorBrush();

		private const int PreviewWidth = 150;

		private const int PreviewHeight = 40;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public ColorTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		/// <summary>
		/// Performs all UI updates for when a palette is set in the data context.
		/// </summary>
		public unsafe void UpdateControls()
		{
			this.Find<TextBlock>("PaletteSize").Text = App.Window.Context.Palette.Size.ToString();

			string[] items = new string[App.Window.Context.Palette.Size + 1];
			items[0] = "Set element color";
			for(int i = 1; i < items.Length; i++)
				items[i] = $"Color #{i}";
			this.Find<ComboBox>("ColorSelector").Items = items;

			var continuousPreview = this.Find<Image>("ContinuousPreview");
			fixed(int* ptr = &(new int[PreviewWidth * PreviewHeight])[0])
			{
				App.Window.Context.Palette.DrawContinuousPreview(PreviewWidth, PreviewHeight, ptr);
				continuousPreview.Source = App.MakeDefaultBitmap(PreviewWidth, PreviewHeight, ptr);
			}
			continuousPreview.InvalidateVisual();

			var discretePreview = this.Find<Image>("DiscretePreview");
			fixed(int* ptr = &(new int[PreviewWidth * PreviewHeight])[0])
			{
				App.Window.Context.Palette.DrawDiscretePreview(PreviewWidth, PreviewHeight, ptr);
				discretePreview.Source = App.MakeDefaultBitmap(PreviewWidth, PreviewHeight, ptr);
			}
			discretePreview.InvalidateVisual();
		}

		/// <summary>
		/// Handles the event for when the palette size spinner was used.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnPaletteSizeSpin(object sender, SpinEventArgs e)
		{
			var spinner = (TextBlock)((ButtonSpinner)sender).Content;
			int size = int.Parse(spinner.Text) + (e.Direction == SpinDirection.Increase ? 1 : -1);
			if(size > 0 && size < Palette.MaxColors)
			{
				if(e.Direction == SpinDirection.Increase)
				{
					byte[] newColor = new byte[4];
					new Random().NextBytes(newColor);
					newColor[0] = 0xFF;
					App.Window.Context.Palette.Add(newColor);
				}
				else
					App.Window.Context.Palette.RemoveLast();
				UpdateControls();
			}
		}

		/// <summary>
		/// Handles the event for when a color is selected in the color selector.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnColorSelected(object sender, SelectionChangedEventArgs e)
		{
			byte[] color = App.Window.Context.Palette[((ComboBox)sender).SelectedIndex];
			ColorPreview.Color = new Color(color[0], color[1], color[2], color[3]);

			this.Find<TextBox>("HexColor").Text = $"#{color[0]:X2}{color[1]:X2}{color[2]:X2}{color[3]:X2}";
			this.Find<TextBox>("Alpha").Text = color[0].ToString();
			this.Find<TextBox>("Red").Text = color[1].ToString();
			this.Find<TextBox>("Green").Text = color[2].ToString();
			this.Find<TextBox>("Blue").Text = color[3].ToString();
		}
	}
}