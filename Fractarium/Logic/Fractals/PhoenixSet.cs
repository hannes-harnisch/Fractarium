using System.Numerics;

namespace Fractarium.Logic.Fractals
{
	/// <summary>
	/// Represents a fractal image based on the Phoenix set.
	/// </summary>
	public class PhoenixSet : Fractal
	{
		private Complex JConst;

		private Complex PConst;

		/// <summary>
		/// Assigns all required parameters.
		/// </summary>
		/// <param name="parameters">Required base parameters.</param>
		/// <param name="palette">Required color palette.</param>
		/// <param name="juliaConstant">The Julia constant parameter.</param>
		/// <param name="phoenixConstant">The Phoenix constant parameter.</param>
		public PhoenixSet(BaseParameters parameters, Palette palette, Complex juliaConstant, Complex phoenixConstant)
			: base(parameters, palette)
		{
			JConst = juliaConstant;
			PConst = phoenixConstant;
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
			r = -r;
			double lastR = 0;
			double lastI = 0;
			for(int iter = 0; iter < Params.IterationLimit; iter++)
			{
				nextR = r * r - i * i + JConst.Real + PConst.Real * lastR - PConst.Imaginary * lastI;
				nextI = 2 * r * i - JConst.Imaginary + PConst.Real * lastI + PConst.Imaginary * lastR;
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