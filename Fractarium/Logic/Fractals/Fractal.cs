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
		protected readonly BaseParameters P;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		protected Fractal(BaseParameters parameters)
		{
			P = parameters;
		}

		/// <summary>
		/// Draws the image of the fractal starting on a 32-bit unsigned integer pointer.
		/// </summary>
		/// <param name="bitmap">Pointer to the first bitmap pixel.</param>
		public unsafe void DrawImage(uint* bitmap)
		{
			Parallel.For(0, P.Width * P.Height, pixel =>
			{
				int x = pixel / P.Height;
				int y = pixel % P.Height;
				double r = (double)(x - P.Width / 2) / P.Scale + P.Midpoint.Real;
				double i = (double)(y - P.Height / 2) / P.Scale - P.Midpoint.Imaginary;
				if(IteratePoint(r, i, out double nextR, out double nextI) == P.IterationLimit)
					*(bitmap + x + y * P.Width) = 0xFF000000;
				else
					*(bitmap + x + y * P.Width) = 0xFFFFd800;
			});
		}
		// iteration - Math.Log(Math.Log(Math.Sqrt((double)(nextR * nextR + nextI * nextI))) / 0.693147f) / 0.693147f;

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