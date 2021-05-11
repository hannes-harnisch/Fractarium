using Avalonia.Media.Imaging;
using Fractarium.Logic;
using Fractarium.Logic.Fractals;
using System.Numerics;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Represents the data context for the entire user interface.
	/// </summary>
	public class AppContext
	{
		/// <summary>
		/// Holds a reference to the currently displayed fractal.
		/// </summary>
		public Fractal Fractal { get; set; }

		/// <summary>
		/// The currently selected fractal type.
		/// </summary>
		public FractalType FractalType { get; set; }

		/// <summary>
		/// How much the scale is multiplied after clicking on a point in the image.
		/// </summary>
		public int ZoomFactor { get; set; } = 2;

		/// <summary>
		/// The constant coefficient used for fractals related to the Julia set.
		/// </summary>
		public Complex JuliaConstant { get; set; } = Complex.Zero;

		/// <summary>
		/// The constant coefficient used in the Phoenix set.
		/// </summary>
		public Complex PhoenixConstant { get; set; } = Complex.Zero;

		/// <summary>
		/// The constant coefficient used for fractals related to the Multibrot set.
		/// </summary>
		public double Exponent { get; set; } = 2;

		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Params = new();

		/// <summary>
		/// Holds the currently selected color palette.
		/// </summary>
		public Palette Palette = new("FF000000", "FF0000FF", "FFFF00FF", "FFFF0000", "FFFFFF00", "FFFFFFFF");

		/// <summary>
		/// Uses current parameters to render a fractal image.
		/// </summary>
		/// <returns>An Avalonia bitmap holding the image.</returns>
		public unsafe Bitmap Render()
		{
			switch(FractalType)
			{
				case FractalType.MandelbrotSet:
					Fractal = new MandelbrotSet(Params, Palette, Exponent); break;
				case FractalType.JuliaSet:
					Fractal = new JuliaSet(Params, Palette, Exponent, JuliaConstant); break;
				case FractalType.PhoenixSet:
					Fractal = new PhoenixSet(Params, Palette, Exponent, JuliaConstant, PhoenixConstant); break;
				case FractalType.BurningShipSet:
					Fractal = new BurningShipSet(Params, Palette, Exponent); break;
				case FractalType.BurningShipJuliaSet:
					Fractal = new BurningShipJuliaSet(Params, Palette, Exponent, JuliaConstant); break;
				case FractalType.TricornSet:
					Fractal = new TricornSet(Params, Palette, Exponent); break;
			}

			fixed(int* ptr = &(new int[Params.Width * Params.Height])[0])
			{
				Fractal.DrawImage(ptr);
				return App.MakeDefaultBitmap(Params.Width, Params.Height, ptr);
			}
		}
	}
}
