namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Mandelbrot set.
	/// </summary>
	public class MandelbrotSet : Fractal
	{
		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		public MandelbrotSet(BaseParameters parameters, Palette palette) : base(parameters, palette) { }

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's formula.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <returns>The color of the given point based on the iteration.</returns>
		public override int IteratePoint(double r, double i)
		{
			double nextR;
			double nextI;
			double firstR = r;
			double firstI = i;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = r * r - i * i + firstR;
				nextI = 2 * r * i + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}
	}
}