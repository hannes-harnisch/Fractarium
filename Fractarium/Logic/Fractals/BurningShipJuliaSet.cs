using System;
using System.Numerics;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Burning Ship Julia set.
	/// </summary>
	public class BurningShipJuliaSet : Fractal
	{
		private Complex JuliaConst;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="juliaConstant">The Julia constant parameter.</param>
		public BurningShipJuliaSet(BaseParameters parameters, Complex juliaConstant) : base(parameters)
		{
			JuliaConst = juliaConstant;
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
			for(; iteration < P.IterationLimit; iteration++)
			{
				nextR = r * r - i * i + JuliaConst.Real;
				nextI = 2 * Math.Abs(r * i) - JuliaConst.Imaginary;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}