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
		private readonly Palette Palette;

		private readonly int HalfWidth;

		private readonly int HalfHeight;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		protected Fractal(BaseParameters parameters, Palette palette)
		{
			Params = parameters;
			Palette = palette;
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
		public Complex GetPointFromPixel(int x, int y)
		{
			double r = (double)(x - HalfWidth) / Params.Scale + Params.Midpoint.Real;
			double i = (double)(y - HalfHeight) / Params.Scale + Params.Midpoint.Imaginary;
			return new Complex(r, i);
		}

		/// <summary>
		/// Draws the image of the fractal starting on a 32-bit unsigned integer pointer.
		/// </summary>
		/// <param name="bitmap">Pointer to the first bitmap pixel.</param>
		public virtual unsafe void DrawImage(int* bitmap)
		{
			Parallel.For(0, Params.Width * Params.Height, pixel =>
			{
				int x = pixel / Params.Height;
				int y = pixel % Params.Height;
				var c = GetPointFromPixel(x, y);
				int iteration = IteratePoint(c.Real, c.Imaginary, out double nextR, out double nextI);
				if(iteration == Params.IterationLimit)
					*(bitmap + x + y * Params.Width) = Palette.ElementColor;
				else
				{
					double normalized = iteration - Math.Log(Math.Log(Math.Sqrt(nextR * nextR + nextI * nextI), 2), 2);
					*(bitmap + x + y * Params.Width) = Palette.ColorFromFraction(normalized / Params.IterationLimit);
				}
			});
		}

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's formula.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <param name="nextR">Real component of the next complex number after break condition was reached.</param>
		/// <param name="nextI">Imaginary component of the next complex number after break condition was reached.</param>
		/// <returns>How many iterations were cycled through.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public abstract int IteratePoint(double r, double i, out double nextR, out double nextI);
	}
}