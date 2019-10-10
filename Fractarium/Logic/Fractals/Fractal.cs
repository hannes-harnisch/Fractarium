using System;
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
		protected const int DivergenceLimit = 4;

		/// <summary>
		/// Basic parameters needed to generate a fractal image.
		/// </summary>
		protected readonly BaseParameters Params;

		/// <summary>
		/// Palette of colors with which different iteration counts are colored.
		/// </summary>
		private readonly Palette Palette;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		protected Fractal(BaseParameters parameters, Palette palette)
		{
			Params = parameters;
			Palette = palette;
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
				double r = (double)(x - Params.Width / 2) / Params.Scale + Params.Midpoint.Real;
				double i = (double)(y - Params.Height / 2) / Params.Scale - Params.Midpoint.Imaginary;
				int iteration = IteratePoint(r, i, out double nextR, out double nextI);
				if(iteration == Params.IterationLimit)
					*(bitmap + x + y * Params.Width) = Palette.ElementColor;
				else
				{
					//double normalized = iteration - Math.Log(Math.Sqrt(nextR * nextR + nextI * nextI)) / 0.693147f;
					*(bitmap + x + y * Params.Width) = Palette.ColorFromFraction((double)iteration / Params.IterationLimit);
				}
			});
		}
		// 

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's formula.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <param name="nextR">Real component of the next complex number after break condition was reached.</param>
		/// <param name="nextI">Imaginary component of the next complex number after break condition was reached.</param>
		/// <returns>How many iterations were cycled through.</returns>
		public abstract int IteratePoint(double r, double i, out double nextR, out double nextI);
	}
}