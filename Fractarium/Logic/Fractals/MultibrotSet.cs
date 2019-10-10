using System;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Multibrot set.
	/// </summary>
	public class MultibrotSet : Fractal
	{
		private double Exp;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="exponent">Exponent required for the Multibrot set.</param>
		public MultibrotSet(BaseParameters parameters, Palette palette, double exponent) : base(parameters, palette)
		{
			Exp = exponent;
		}

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's formula.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <param name="nextR">Real component of the next complex number after break condition was reached.</param>
		/// <param name="nextI">Imaginary component of the next complex number after break condition was reached.</param>
		/// <returns>How many iterations were cycled through.</returns>
		public override int IteratePoint(double r, double i, out double nextR, out double nextI)
		{
			nextR = 0;
			nextI = 0;
			double firstR = r;
			double firstI = i;
			int iteration = 0;
			for(; iteration < Params.IterationLimit; iteration++)
			{
				nextR = Math.Pow(r * r + i * i, Exp / 2) * Math.Cos(Exp * Math.Atan2(i, r)) + firstR;
				nextI = Math.Pow(r * r + i * i, Exp / 2) * Math.Sin(Exp * Math.Atan2(i, r)) + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}