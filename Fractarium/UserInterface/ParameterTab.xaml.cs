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
		/// Loads the names of preprogrammed fractal types.
		/// </summary>
		public string[] TypeEntries => FractalTypes.Names;

		/// <summary>
		/// Initializes associated XAML objects and data context.
		/// </summary>
		public ParameterTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		/// <summary>
		/// Makes the width and height parameters adapt to the window size when the box is checked.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void BindImageSizeToWindow(object sender, RoutedEventArgs e)
		{
			// TODO
			System.Console.WriteLine(((CheckBox)sender).IsChecked);
		}

		/// <summary>
		/// Handles user input for parameters that are interpreted as positive integers.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void OnPositiveIntInput(object sender, KeyEventArgs e)
		{
			bool parsed = uint.TryParse(((TextBox)sender).Text, out uint result) && result != 0;
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Width":
						App.Context.Parameters.Width = result; break;
					case "Height":
						App.Context.Parameters.Height = result; break;
					case "IterationLimit":
						App.Context.Parameters.IterationLimit = result; break;
					case "ZoomFactor":
						App.Context.Parameters.ZoomFactor = result; break;
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
				App.Context.Parameters.Scale = result;
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
						App.Context.Parameters.Midpoint = result; break;
				}
		}

		/// <summary>
		/// Sets a text box's styling to the appropriate style depending on the correctness of user input.
		/// </summary>
		/// <param name="box">The box where user input occurred.</param>
		/// <param name="parsed">Whether user input could be parsed correctly.</param>
		public void SetTextBoxState(TextBox box, bool parsed)
		{
			box.Classes = new Classes(parsed ? "" : "Error");
		}
	}
}