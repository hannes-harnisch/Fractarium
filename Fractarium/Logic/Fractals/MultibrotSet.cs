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
		/// <returns>The color of the given point based on the iteration.</returns>
		public override int IteratePoint(double r, double i)
		{
			double nextR;
			double nextI;
			double firstR = r;
			double firstI = i;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = Math.Pow(r * r + i * i, Exp / 2) * Math.Cos(Exp * Math.Atan2(i, r)) + firstR;
				nextI = Math.Pow(r * r + i * i, Exp / 2) * Math.Sin(Exp * Math.Atan2(i, r)) + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}
	}
}