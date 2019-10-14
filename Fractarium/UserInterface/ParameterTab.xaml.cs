using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// The main menu tab where image parameters can be set.
	/// </summary>
	public class ParameterTab : UserControl
	{
		/// <summary>
		/// Indicates whether width and height parameters should be adapted to the window size.
		/// </summary>
		public bool BindImageSizeToWindow { get; set; }

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public ParameterTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		/// <summary>
		/// Sets the fractal type parameter to the type selected in the combo box.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnFractalTypeSelected(object sender, SelectionChangedEventArgs e)
		{
			bool[] disableTextBoxes = new bool[3];
			switch(App.Context.FractalType = FractalTypes.ByName(((ComboBox)sender).SelectedItem.ToString()))
			{
				case FractalType.JuliaSet:
				case FractalType.BurningShipJuliaSet:
					disableTextBoxes = new[] { true, false, false }; break;
				case FractalType.PhoenixSet:
					disableTextBoxes = new[] { true, true, false }; break;
				case FractalType.MultibrotSet:
					disableTextBoxes = new[] { false, false, true }; break;
				case FractalType.MultiJuliaSet:
					disableTextBoxes = new[] { true, false, true }; break;
			}
			this.Find<TextBox>("JuliaConstant").IsEnabled = disableTextBoxes[0];
			this.Find<TextBox>("PhoenixConstant").IsEnabled = disableTextBoxes[1];
			this.Find<TextBox>("MultibrotExponent").IsEnabled = disableTextBoxes[2];

			var iterationLimit = this.Find<TextBox>("IterationLimit");
			iterationLimit.Text = "100";
			OnPositiveIntInput(iterationLimit, null);

			var scale = this.Find<TextBox>("Scale");
			scale.Text = "200";
			OnLongInput(scale, null);

			var midpoint = this.Find<TextBox>("Midpoint");
			midpoint.Text = "0";
			OnComplexInput(midpoint, null);
		}

		/// <summary>
		/// Attaches event handlers reacting to size changes of the main window for width and height parameters.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnBindImageSizeToWindow(object sender, RoutedEventArgs e)
		{
			App.Context.GetObservable(BoundsProperty).Subscribe(bounds =>
			{
				if(BindImageSizeToWindow)
				{
					var width = this.Find<TextBox>("Width");
					width.Text = ((int)(bounds.Width * App.ScreenEnhancement)).ToString();
					OnPositiveIntInput(width, null);

					double h = (bounds.Height - App.Context.Find<TabControl>("Menu").Bounds.Height) * App.ScreenEnhancement;
					var height = this.Find<TextBox>("Height");
					height.Text = ((int)h).ToString();
					OnPositiveIntInput(height, null);
				}
			});
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnPositiveIntInput(object sender, KeyEventArgs e)
		{
			bool parsed = int.TryParse(Prepare(((TextBox)sender).Text), out int result) && result > 0;
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Width":
						App.Context.Params.Width = result; break;
					case "Height":
						App.Context.Params.Height = result; break;
					case "IterationLimit":
						App.Context.Params.IterationLimit = result; break;
					case "ZoomFactor":
						App.Context.ZoomFactor = result; break;
				}
			ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as long positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnLongInput(object sender, KeyEventArgs e)
		{
			bool parsed = ulong.TryParse(Prepare(((TextBox)sender).Text), out ulong result) && result != 0;
			if(parsed)
				App.Context.Params.Scale = result;
			ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as complex numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnComplexInput(object sender, KeyEventArgs e)
		{
			bool parsed = ComplexUtil.TryParse(Prepare(((TextBox)sender).Text), out var result);
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Midpoint":
						App.Context.Params.Midpoint = result; break;
					case "JuliaConstant":
						App.Context.JuliaConstant = result; break;
					case "PhoenixConstant":
						App.Context.PhoenixConstant = result; break;
				}
			ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as floating-point numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnFloatingPointInput(object sender, KeyEventArgs e)
		{
			bool parsed = double.TryParse(Prepare(((TextBox)sender).Text), NumberStyles.Any,
				CultureInfo.InvariantCulture, out double result);
			if(parsed)
				App.Context.MultibrotExponent = result;
			ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Sets a text box's styling to the appropriate style and disables the render button depending on the
		/// correctness of user input. Also initiates rendering if the enter key was pressed.
		/// </summary>
		/// <param name="box">The box where user input occurred.</param>
		/// <param name="parsed">Whether the input could be parsed correctly.</param>
		/// <param name="e">Data associated with the key event from input.</param>
		public void ReactToTextBoxInput(TextBox box, bool parsed, KeyEventArgs e)
		{
			box.Classes = new Classes(parsed ? "" : "Error");

			var enabledBoxes = ((Grid)Content).Children.Where(c => c is TextBox t && t.IsEnabled);
			App.Context.Find<Button>("RenderButton").IsEnabled = !enabledBoxes.Any(t => t.Classes.Contains("Error"));

			if(e?.Key == Key.Enter)
				App.Context.Render(null, null);
		}

		/// <summary>
		/// Removes all whitespace from a string. To be used to ignore whitespace in text box inputs.
		/// </summary>
		/// <param name="text">A text box's text.</param>
		/// <returns>The input without whitespace.</returns>
		private static string Prepare(string text)
		{
			return Regex.Replace(text, @"\s+", "");
		}
	}
}