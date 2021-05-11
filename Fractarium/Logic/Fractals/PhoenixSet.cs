using System;
using System.Numerics;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Phoenix set.
	/// </summary>
	public class PhoenixSet : Fractal
	{
		private Complex J;

		private Complex P;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="power">Exponent required for the generalized fractal equation.</param>
		/// <param name="juliaConstant">The Julia constant parameter.</param>
		/// <param name="phoenixConstant">The Phoenix constant parameter.</param>
		public PhoenixSet(BaseParameters parameters, Palette palette, double power, Complex juliaConstant,
			Complex phoenixConstant) : base(parameters, palette, power)
		{
			J = juliaConstant;
			P = phoenixConstant;
		}

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
			double lastR = 0;
			double lastI = 0;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = r * r - i * i + J.Real + P.Real * lastR - P.Imaginary * lastI;
				nextI = 2 * r * i - J.Imaginary + P.Real * lastI + P.Imaginary * lastR;
				lastR = r;
				lastI = i;
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
			double lastR = 0;
			double lastI = 0;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = Math.Pow(r * r + i * i, Power / 2) * Math.Cos(Power * Math.Atan2(i, r))
					+ J.Real + P.Real * lastR - P.Imaginary * lastI;
				nextI = Math.Pow(r * r + i * i, Power / 2) * Math.Sin(Power * Math.Atan2(i, r))
					- J.Imaginary + P.Real * lastI + P.Imaginary * lastR;
				lastR = r;
				lastI = i;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}
	}
}
