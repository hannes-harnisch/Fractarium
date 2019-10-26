using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Avalonia.Controls;
using Avalonia.Input;
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

		private static readonly Dictionary<string, int> ColorByteIndices = new Dictionary<string, int>()
		{
			["Alpha"] = 0,
			["Red"] = 1,
			["Green"] = 2,
			["Blue"] = 3,
		};

		private static readonly Regex HexColorRegex
			= new Regex(@"^#([A-Fa-f\d]{2})([A-Fa-f\d]{2})([A-Fa-f\d]{2})([A-Fa-f\d]{2})$");

		private readonly ComboBox ColorSelector;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public ColorTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
			ColorSelector = this.Find<ComboBox>("ColorSelector");
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
			ColorSelector.Items = items;

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
			if(ColorSelector.SelectedIndex == -1)
				ColorSelector.SelectedIndex = 0;
			else
			{
				byte[] color = App.Window.Context.Palette[ColorSelector.SelectedIndex];
				ColorPreview.Color = new Color(color[0], color[1], color[2], color[3]);

				this.Find<TextBox>("HexColor").Text = $"#{color[0]:X2}{color[1]:X2}{color[2]:X2}{color[3]:X2}";
				foreach(var i in ColorByteIndices)
					this.Find<TextBox>(i.Key).Text = color[i.Value].ToString();
			}
		}

		/// <summary>
		/// Handles user input in the hex color text box.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnHexColorInput(object sender, KeyEventArgs e)
		{
			var match = HexColorRegex.Match(((TextBox)sender).Text);
			if(match.Success)
			{
				foreach(int i in ColorByteIndices.Values)
				{
					byte value = byte.Parse(match.Groups[i + 1].Value, NumberStyles.HexNumber);
					App.Window.Context.Palette[ColorSelector.SelectedIndex, i] = value;
				}
				OnColorSelected(null, null);
			}
			App.Window.ReactToTextBoxInput((TextBox)sender, match.Success, e);
			UpdateControls();
		}

		/// <summary>
		/// Handles user input in the color component text boxes.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnColorComponentInput(object sender, KeyEventArgs e)
		{
			bool parsed = byte.TryParse(Clean(((TextBox)sender).Text), out byte result);
			if(parsed)
			{
				int byteIndex = ColorByteIndices[((TextBox)sender).Name];
				App.Window.Context.Palette[ColorSelector.SelectedIndex, byteIndex] = result;
				OnColorSelected(null, null);
			}
			App.Window.ReactToTextBoxInput((TextBox)sender, parsed, e);
			UpdateControls();
		}

		/// <summary>
		/// Removes all whitespace from a string. To be used to ignore whitespace in text box inputs.
		/// </summary>
		/// <param name="text">A text box's text.</param>
		/// <returns>The input without whitespace.</returns>
		private static string Clean(string text)
		{
			return Regex.Replace(text, @"\s+", "");
		}
	}
}