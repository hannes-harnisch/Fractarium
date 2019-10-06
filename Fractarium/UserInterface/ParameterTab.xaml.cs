using System;

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
		/// Default iteration limit value to generate a typical fractal image.
		/// </summary>
		public const string DefaultIterationLimit = "100";

		/// <summary>
		/// Default scale value to generate a typical fractal image.
		/// </summary>
		public const string DefaultScale = "200";

		/// <summary>
		/// Default midpoint value to center the fractal image.
		/// </summary>
		public const string DefaultMidpoint = "0";

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
			var iterationLimit = this.Find<TextBox>("IterationLimit");
			iterationLimit.Text = DefaultIterationLimit;
			OnPositiveIntInput(iterationLimit, null);

			var scale = this.Find<TextBox>("Scale");
			scale.Text = DefaultScale;
			OnLongInput(scale, null);

			var midpoint = this.Find<TextBox>("Midpoint");
			midpoint.Text = DefaultMidpoint;
			OnComplexInput(midpoint, null);

			switch(App.Context.FractalType = FractalTypes.ByName(((ComboBox)sender).SelectedItem.ToString()))
			{
				case FractalType.JuliaSet:
				case FractalType.BurningShipJuliaSet:
					this.Find<TextBox>("JuliaConstant").IsEnabled = true;
					this.Find<TextBox>("PhoenixConstant").IsEnabled = false;
					this.Find<TextBox>("MultibrotExponent").IsEnabled = false;
					break;
				case FractalType.PhoenixSet:
					this.Find<TextBox>("JuliaConstant").IsEnabled = true;
					this.Find<TextBox>("PhoenixConstant").IsEnabled = true;
					this.Find<TextBox>("MultibrotExponent").IsEnabled = false;
					break;
				case FractalType.MultibrotSet:
					this.Find<TextBox>("JuliaConstant").IsEnabled = false;
					this.Find<TextBox>("PhoenixConstant").IsEnabled = false;
					this.Find<TextBox>("MultibrotExponent").IsEnabled = true;
					break;
				case FractalType.MultiJuliaSet:
					this.Find<TextBox>("JuliaConstant").IsEnabled = true;
					this.Find<TextBox>("PhoenixConstant").IsEnabled = false;
					this.Find<TextBox>("MultibrotExponent").IsEnabled = true;
					break;
				default:
					this.Find<TextBox>("JuliaConstant").IsEnabled = false;
					this.Find<TextBox>("PhoenixConstant").IsEnabled = false;
					this.Find<TextBox>("MultibrotExponent").IsEnabled = false;
					break;
			}
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
					var widthTextBox = this.Find<TextBox>("Width");
					widthTextBox.Text = ((int)bounds.Width).ToString();
					OnPositiveIntInput(widthTextBox, null);
					var heightTextBox = this.Find<TextBox>("Height");
					heightTextBox.Text = ((int)bounds.Height).ToString();
					OnPositiveIntInput(heightTextBox, null);
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
			bool parsed = int.TryParse(((TextBox)sender).Text, out int result) && result > 0;
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
						App.Context.Params.ZoomFactor = result; break;
				}
			HandleTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as long positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnLongInput(object sender, KeyEventArgs e)
		{
			bool parsed = ulong.TryParse(((TextBox)sender).Text, out ulong result) && result != 0;
			if(parsed)
				App.Context.Params.Scale = result;
			HandleTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as complex numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnComplexInput(object sender, KeyEventArgs e)
		{
			bool parsed = ComplexUtil.TryParse(((TextBox)sender).Text, out var result);
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
			HandleTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as floating-point numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnFloatingPointInput(object sender, KeyEventArgs e)
		{
			bool parsed = double.TryParse(((TextBox)sender).Text, out double result);
			if(parsed)
				App.Context.MultibrotExponent = result;
			HandleTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Sets a text box's styling to the appropriate style depending on the correctness of user input and
		/// initiates rendering if the enter key was pressed.
		/// </summary>
		/// <param name="box">The box where user input occurred.</param>
		/// <param name="parsed">Whether the input could be parsed correctly.</param>
		/// <param name="e">Data associated with the key event from input.</param>
		public void HandleTextBoxInput(TextBox box, bool parsed, KeyEventArgs e)
		{
			box.Classes = new Classes(parsed ? "" : "Error");
			if(e?.Key == Key.Enter)
				App.Context.Render(null, null);
		}
	}
}