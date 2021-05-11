using System;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Burning Ship set.
	/// </summary>
	public class BurningShipSet : Fractal
	{
		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="power">Exponent required for the generalized fractal equation.</param>
		public BurningShipSet(BaseParameters parameters, Palette palette, double power)
			: base(parameters, palette, power) { }

		/// <summary>
		/// Iterates a complex point according to a specific fractal type's equation.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <returns>The color of the given point based on the iteration.</returns>
		public override int Iterate(double r, double i)
		{
			double nextR;
			double nextI;
			double firstR = r;
			double firstI = i;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = r * r - i * i + firstR;
				nextI = 2 * Math.Abs(r * i) + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}

		/// <summary>
		/// Iterates a complex point according to the generalized fractal equation with a different exponent.
		/// </summary>
		/// <param name="r">Real component of the complex point to be iterated.</param>
		/// <param name="i">Imaginary component of the complex point to be iterated.</param>
		/// <returns>The color of the given point based on the iteration.</returns>
		public override int IterateWithExponent(double r, double i)
		{
			double nextR;
			double nextI;
			double firstR = r;
			double firstI = i;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = Math.Pow(r * r + i * i, Power / 2) * Math.Cos(Power * Math.Atan2(i, r)) + firstR;
				nextI = Math.Abs(Math.Pow(r * r + i * i, Power / 2) * Math.Sin(Power * Math.Atan2(i, r))) + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}
	}
}
