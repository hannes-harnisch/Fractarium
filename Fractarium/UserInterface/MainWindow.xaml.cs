using System;
using System.Numerics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Holds the content of the main window.
	/// </summary>
	public class MainWindow : Window
	{
		/// <summary>
		/// The currently selected fractal type.
		/// </summary>
		public FractalType FractalType { get; set; }

		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Params = new BaseParameters();

		/// <summary>
		/// The constant coefficient used for fractals related to the Julia set.
		/// </summary>
		public Complex JuliaConstant { get; set; }

		/// <summary>
		/// The constant coefficient used in the Phoenix set.
		/// </summary>
		public Complex PhoenixConstant { get; set; }

		/// <summary>
		/// The constant coefficient used for fractals related to the Multibrot set.
		/// </summary>
		public double MultibrotExponent { get; set; }

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public MainWindow()
		{
			AvaloniaXamlLoader.Load(this);
#if DEBUG
			this.AttachDevTools();
#endif
		}

		/// <summary>
		/// Uses current parameters to render a fractal image on the image's bitmap.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public unsafe void Render(object sender, RoutedEventArgs e)
		{
			Fractal fractal = null;
			switch(FractalType)
			{
				case FractalType.MandelbrotSet:
					fractal = new MandelbrotSet(Params); break;
				case FractalType.JuliaSet:
					fractal = new JuliaSet(Params, JuliaConstant); break;
				case FractalType.PhoenixSet:
					fractal = new PhoenixSet(Params, JuliaConstant, PhoenixConstant); break;
				case FractalType.BurningShipSet:
					fractal = new BurningShipSet(Params); break;
				case FractalType.BurningShipJuliaSet:
					fractal = new BurningShipJuliaSet(Params, JuliaConstant); break;
				//case FractalType.MultibrotSet:
				//fractal = new MultibrotSet(Params, MultibrotExponent); break;
				//case FractalType.MultiJuliaSet:
				//fractal = new MultiJuliaSet(Params, MultibrotExponent, JuliaConstant); break;
				case FractalType.TricornSet:
					fractal = new TricornSet(Params); break;
					//case FractalType.LyapunovFractal:
					//fractal = new LyapunovFractal(Params); break;
			}

			fixed(uint* ptr = &(new uint[Params.Width * Params.Height])[0])
			{
				fractal.DrawImage(ptr);

				var size = new PixelSize(Params.Width, Params.Height);
				var dpi = new Avalonia.Vector(96, 96);
				int stride = 4 * Params.Width;

				var img = this.Find<Image>("Image");
				img.Source = new Bitmap(PixelFormat.Bgra8888, (IntPtr)ptr, size, dpi, stride);
				img.InvalidateVisual();
			}
		}
	}
}