using System.Collections.Generic;
using System.Linq;

namespace Fractarium.Logic.Fractals
{
	public enum FractalType
	{
		MandelbrotSet,
		JuliaSet,
		PhoenixFractal,
		BurningShipFractal,
		BurningShipJuliaSet,
		MultibrotSet,
		MultiJuliaSet,
		Tricorn,
		LyapunovFractal
	}

	public static class FractalTypes
	{
		private static readonly Dictionary<FractalType, string> names = new Dictionary<FractalType, string>()
		{
			[FractalType.MandelbrotSet] = "Mandelbrot set",
			[FractalType.JuliaSet] = "Julia set",
			[FractalType.PhoenixFractal] = "Phoenix fractal",
			[FractalType.BurningShipFractal] = "Burning Ship fractal",
			[FractalType.BurningShipJuliaSet] = "Burning Ship Julia set",
			[FractalType.MultibrotSet] = "Multibrot set",
			[FractalType.MultiJuliaSet] = "Multi-Julia set",
			[FractalType.Tricorn] = "Tricorn",
			[FractalType.LyapunovFractal] = "Lyapunov fractal"
		};

		public static string[] Names => names.Values.ToArray();

		public static FractalType TypeByName(string name)
		{
			return names.First(pair => pair.Value == name).Key;
		}
	}
}