using System.Collections.Generic;
using System.Linq;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Used for indicating which of the preprogrammed fractal types is selected.
	/// </summary>
	public enum FractalType
	{
		/// <summary>
		/// Indicates a fractal generated with the Mandelbrot set formula.
		/// </summary>
		MandelbrotSet,
		/// <summary>
		/// Indicates a fractal generated with the Julia set formula.
		/// </summary>
		JuliaSet,
		/// <summary>
		/// Indicates a fractal generated with the Phoenix set formula.
		/// </summary>
		PhoenixSet,
		/// <summary>
		/// Indicates a fractal generated with the Burning Ship set formula.
		/// </summary>
		BurningShipSet,
		/// <summary>
		/// Indicates a fractal generated with the Burning Ship Julia set formula.
		/// </summary>
		BurningShipJuliaSet,
		/// <summary>
		/// Indicates a fractal generated with the Tricorn set formula.
		/// </summary>
		TricornSet,
		/// <summary>
		/// Indicates a fractal generated with the Lyapunov fractal equation.
		/// </summary>
		LyapunovFractal
	}

	/// <summary>
	/// Encapsulates functions associated with the FractalType enum.
	/// </summary>
	public static class FractalTypes
	{
		private static readonly Dictionary<FractalType, string> names = new Dictionary<FractalType, string>()
		{
			[FractalType.MandelbrotSet] = "Mandelbrot set",
			[FractalType.JuliaSet] = "Julia set",
			[FractalType.PhoenixSet] = "Phoenix set",
			[FractalType.BurningShipSet] = "Burning Ship set",
			[FractalType.BurningShipJuliaSet] = "Burning Ship Julia set",
			[FractalType.TricornSet] = "Tricorn set",
			[FractalType.LyapunovFractal] = "Lyapunov fractal"
		};

		/// <summary>
		/// Returns the properly formatted names for all fractal types.
		/// </summary>
		public static string[] Names => names.Values.ToArray();

		/// <summary>
		/// Returns the enum value for the given name of a fractal type.
		/// </summary>
		/// <param name="name">Proper name of a fractal type.</param>
		/// <returns>The enum value associated with the name.</returns>
		public static FractalType ByName(string name)
		{
			return names.First(pair => pair.Value == name).Key;
		}
	}
}