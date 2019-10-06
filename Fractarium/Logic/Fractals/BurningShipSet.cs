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
		public BurningShipSet(BaseParameters parameters) : base(parameters) { }

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
			for(; iteration < P.IterationLimit; iteration++)
			{
				nextR = r * r - i * i + firstR;
				nextI = 2 * Math.Abs(r * i) + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}