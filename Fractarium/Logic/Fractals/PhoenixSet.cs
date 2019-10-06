using System.Numerics;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Phoenix set.
	/// </summary>
	public class PhoenixSet : Fractal
	{
		private Complex JuliaConst;

		private Complex PhoenixConst;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="juliaConstant">The Julia constant parameter.</param>
		/// <param name="phoenixConstant">The Phoenix constant parameter.</param>
		public PhoenixSet(BaseParameters parameters, Complex juliaConstant, Complex phoenixConstant) : base(parameters)
		{
			JuliaConst = juliaConstant;
			PhoenixConst = phoenixConstant;
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
			r = -r;
			double lastR = 0;
			double lastI = 0;
			int iteration = 0;
			for(; iteration < P.IterationLimit; iteration++)
			{
				nextR = r * r - i * i + JuliaConst.Real + PhoenixConst.Real * lastR - PhoenixConst.Imaginary * lastI;
				nextI = 2 * r * i - JuliaConst.Imaginary + PhoenixConst.Real * lastI + PhoenixConst.Imaginary * lastR;
				lastR = r;
				lastI = i;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}