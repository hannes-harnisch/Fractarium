using System.Globalization;

using Avalonia.Controls;
using Avalonia.Input;
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
		public bool BindImageSizeToWindow { get; set; } = true;

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
			App.Window.Context.FractalType = FractalTypes.ByName(((ComboBox)sender).SelectedItem.ToString());
			switch(App.Window.Context.FractalType)
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
		/// Handles user input for parameters that are interpreted as positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnPositiveIntInput(object sender, KeyEventArgs e)
		{
			string text = App.PrepareInput(((TextBox)sender).Text);
			bool parsed = int.TryParse(text, out int result) && result > 0;
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Width":
						App.Window.Context.Params.Width = result; break;
					case "Height":
						App.Window.Context.Params.Height = result; break;
					case "IterationLimit":
						App.Window.Context.Params.IterationLimit = result; break;
					case "ZoomFactor":
						App.Window.Context.ZoomFactor = result; break;
				}
			App.Window.ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as long positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnLongInput(object sender, KeyEventArgs e)
		{
			string text = App.PrepareInput(((TextBox)sender).Text);
			bool parsed = ulong.TryParse(text, out ulong result) && result != 0;
			if(parsed)
				App.Window.Context.Params.Scale = result;
			App.Window.ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as complex numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnComplexInput(object sender, KeyEventArgs e)
		{
			string text = App.PrepareInput(((TextBox)sender).Text);
			bool parsed = ComplexUtil.TryParse(text, out var result);
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Midpoint":
						App.Window.Context.Params.Midpoint = result; break;
					case "JuliaConstant":
						App.Window.Context.JuliaConstant = result; break;
					case "PhoenixConstant":
						App.Window.Context.PhoenixConstant = result; break;
				}
			App.Window.ReactToTextBoxInput((TextBox)sender, parsed, e);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as floating-point numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnFloatingPointInput(object sender, KeyEventArgs e)
		{
			string text = App.PrepareInput(((TextBox)sender).Text);
			bool parsed = double.TryParse(text, NumberStyles.Any, App.CI, out double result) && result != 0;
			if(parsed)
				App.Window.Context.MultibrotExponent = result;
			App.Window.ReactToTextBoxInput((TextBox)sender, parsed, e);
		}
	}
}