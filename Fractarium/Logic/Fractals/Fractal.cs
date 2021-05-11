using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Base class for all fractals based on escape time algorithms.
	/// </summary>
	public abstract class Fractal
	{
		/// <summary>
		/// For most fractals, when the sum of squares of a complex point's components after iteration surpasses 4,
		/// it is considered to be divergent.
		/// </summary>
		protected const int DivergenceLimit = 50;

		/// <summary>
		/// Basic parameters needed to generate a fractal image.
		/// </summary>
		protected readonly BaseParameters Params;

		/// <summary>
		/// Palette of colors with which different iteration counts are colored.
		/// </summary>
		protected readonly Palette Palette;

		/// <summary>
		/// The exponent the central variable in the fractal equation is raised to.
		/// </summary>
		protected readonly double Power;

		private readonly int HalfWidth;

		private readonly int HalfHeight;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="power">Exponent required for the generalized fractal equation.</param>
		protected Fractal(BaseParameters parameters, Palette palette, double power)
		{
			Params = parameters;
			Palette = palette;
			Power = power;
			HalfWidth = Params.Width / 2;
			HalfHeight = Params.Height / 2;
		}

		/// <summary>
		/// Calculates the complex point from the fractal parameters and given pixel coordinates.
		/// </summary>
		/// <param name="x">The pixel's X coordinate.</param>
		/// <param name="y">The pixel's Y coordinate.</param>
		/// <returns>The corresponding point on the complex plane.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Complex GetPointFromPixel(double x, double y)
		{
			double r = (x - HalfWidth) / Params.Scale + Params.Midpoint.Real;
			double i = (y - HalfHeight) / Params.Scale + Params.Midpoint.Imaginary;
			return new(r, i);
		}

		/// <summary>
		/// Draws the image of the fractal starting on a 32-bit unsigned integer pointer.
		/// </summary>
		/// <param name="bitmap">Pointer to the first bitmap pixel.</param>
		public unsafe void DrawImage(int* bitmap)
		{
			Func<double, double, int> iterate;
			if(Power == 2)
				iterate = Iterate;
			else
				iterate = IterateWithExponent;

			_ = Parallel.For(0, Params.Width * Params.Height, pixel =>
			{
				int x = pixel / Params.Height;
				int y = pixel % Params.Height;
				var c = GetPointFromPixel(x, y);
				*(bitmap + x + y * Params.Width) = iterate(c.Real, c.Imaginary);
			});
		}

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's equation.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <returns>The color of the given point based on the iteration.</returns>
		public abstract int Iterate(double r, double i);

		/// <summary>
		/// Iterates a complex point according to the generalized fractal equation with a different exponent.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <returns>The color of the given point based on the iteration.</returns>
		public abstract int IterateWithExponent(double r, double i);

		/// <summary>
		/// Normalizes the iteration value of a complex point to facilitate smoother coloring.
		/// </summary>
		/// <param name="iteration">The iteration value to be normalized.</param>
		/// <param name="nextR">Real component of the next complex number after break condition was reached.</param>
		/// <param name="nextI">Imaginary component of the next complex number after break condition was reached.</param>
		/// <returns>The normalized iteration value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static double Normalize(int iteration, double nextR, double nextI)
		{
			double value = iteration - Math.Log(Math.Log(Math.Sqrt(nextR * nextR + nextI * nextI), 2), 2);
			return value < 0 ? 0 : value;
		}
	}
}
