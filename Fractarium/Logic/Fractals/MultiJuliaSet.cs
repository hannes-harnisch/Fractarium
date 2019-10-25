using System;
using System.Numerics;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Multi-Julia set.
	/// </summary>
	public class MultiJuliaSet : Fractal
	{
		private double Exp;

		private Complex JConst;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="exponent">Exponent required for the Multibrot set.</param>
		/// <param name="juliaConstant">The Julia constant parameter.</param>
		public MultiJuliaSet(BaseParameters parameters, Palette palette, double exponent, Complex juliaConstant)
			: base(parameters, palette)
		{
			Exp = exponent;
			JConst = juliaConstant;
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
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = Math.Pow(r * r + i * i, Exp / 2) * Math.Cos(Exp * Math.Atan2(i, r)) + JConst.Real;
				nextI = Math.Pow(r * r + i * i, Exp / 2) * Math.Sin(Exp * Math.Atan2(i, r)) + JConst.Imaginary;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					return Palette.GradientColor(Normalize(iter, nextR, nextI) / Params.IterationLimit);
			}
			return Palette.ElementColor;
		}
	}
}