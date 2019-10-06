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
			App.Context.FractalType = FractalTypes.TypeByName(((ComboBox)sender).SelectedItem.ToString());
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
					widthTextBox.Text = ((uint)bounds.Width).ToString();
					OnPositiveIntInput(widthTextBox, null);
					var heightTextBox = this.Find<TextBox>("Height");
					heightTextBox.Text = ((uint)bounds.Height).ToString();
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
			SetTextBoxState((TextBox)sender, parsed);
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
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as long positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnLongInput(object sender, KeyEventArgs e)
		{
			bool parsed = ulong.TryParse(((TextBox)sender).Text, out ulong result) && result != 0;
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				App.Context.Params.Scale = result;
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as complex numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnComplexInput(object sender, KeyEventArgs e)
		{
			bool parsed = ComplexUtil.TryParse(((TextBox)sender).Text, out var result);
			SetTextBoxState((TextBox)sender, parsed);
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
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as floating-point numbers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnFloatingPointInput(object sender, KeyEventArgs e)
		{
			bool parsed = double.TryParse(((TextBox)sender).Text, out double result);
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				App.Context.MultibrotExponent = result;
		}

		/// <summary>
		/// Sets a text box's styling to the appropriate style depending on the correctness of user input.
		/// </summary>
		/// <param name="box">The box where user input occurred.</param>
		/// <param name="parsed">Whether the input could be parsed correctly.</param>
		public void SetTextBoxState(TextBox box, bool parsed)
		{
			box.Classes = new Classes(parsed ? "" : "Error");
		}
	}
}