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
		/// <param name="nextR">Real component of the next complex number after break condition was reached.</param>
		/// <param name="nextI">Imaginary component of the next complex number after break condition was reached.</param>
		/// <returns>How many iterations were cycled through.</returns>
		public override int IteratePoint(double r, double i, out double nextR, out double nextI)
		{
			nextR = 0;
			nextI = 0;
			int iteration = 0;
			for(; iteration < Params.IterationLimit; iteration++)
			{
				nextR = Math.Pow(r * r + i * i, Exp / 2) * Math.Cos(Exp * Math.Atan2(i, r)) + JConst.Real;
				nextI = Math.Pow(r * r + i * i, Exp / 2) * Math.Sin(Exp * Math.Atan2(i, r)) + JConst.Imaginary;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}