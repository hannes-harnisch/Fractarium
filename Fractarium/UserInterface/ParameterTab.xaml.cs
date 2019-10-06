using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;

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
		/// <param name="parsed">Whether the input could be parsed correctly.</param>
		public void SetTextBoxState(TextBox box, bool parsed)
		{
			box.Classes = new Classes(parsed ? "" : "Error");
		}
	}
}