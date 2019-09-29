using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	public class ParameterTab : UserControl
	{
		public string[] TypeEntries => FractalTypes.Names;

		public string FractalType { get; set; }

		public bool BindImageSizeToWindow
		{
			get => _bindImageSizeToWindow;
			set
			{
				_bindImageSizeToWindow = value;
				System.Console.WriteLine(value);
			}
		}
		private bool _bindImageSizeToWindow = true;

		public string ImageWidth
		{
			set
			{
				WidthError = uint.TryParse(value, out uint result);
				if(WidthError)
					App.Parameters.Width = result;
			}
		}

		public bool WidthError { get; set; }

		public string ImageHeight { get; set; }

		public string IterationLimit { get; set; }

		public string ZoomFactor { get; set; }

		public string Scale { get; set; }

		public string Midpoint { get; set; }

		public string JuliaConstant { get; set; }

		public string PhoenixConstant { get; set; }

		public string MultibrotExponent { get; set; }

		public ParameterTab()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}
	}
}