using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	public class ParameterTab : UserControl
	{
		public string[] TypeEntries => FractalTypes.Names;

		public ParameterTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		public void BindImageSizeToWindow(object sender, RoutedEventArgs e)
		{
			System.Console.WriteLine(((CheckBox)sender).IsChecked);
		}

		public void SetTextBoxState(TextBox box, bool parsed)
		{
			box.Classes = new Classes(parsed ? "" : "Error");
		}

		public void OnPositiveIntInput(object sender, KeyEventArgs e)
		{
			bool parsed = uint.TryParse(((TextBox)sender).Text, out uint result);
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Width":
						App.Parameters.Width = result; break;
					case "Height":
						App.Parameters.Height = result; break;
					case "IterationLimit":
						App.Parameters.IterationLimit = result; break;
					case "ZoomFactor":
						App.Parameters.ZoomFactor = result; break;
				}
		}

		public void OnLongInput(object sender, KeyEventArgs e)
		{
			bool parsed = ulong.TryParse(((TextBox)sender).Text, out ulong result);
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				App.Parameters.Scale = result;
		}

		public void OnComplexInput(object sender, KeyEventArgs e)
		{
			bool parsed = ComplexUtil.TryParse(((TextBox)sender).Text, out var result);
			SetTextBoxState((TextBox)sender, parsed);
			if(parsed)
				switch(((TextBox)sender).Name)
				{
					case "Midpoint":
						App.Parameters.Midpoint = result; break;
				}
		}
	}
}