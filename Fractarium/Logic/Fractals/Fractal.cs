using System;
using System.Threading.Tasks;

namespace Fractarium.Logic.Fractals
{
	public abstract class Fractal
	{
		protected const int DivergenceLimit = 100;

		protected readonly BaseParameters P;

		protected Fractal(BaseParameters parameters)
		{
			P = parameters;
		}

		public abstract int IteratePoint(double r, double i, out double nextR, out double nextI);

		public unsafe void DrawImage(IntPtr bitmapPointer)
		{
			uint* bitmap = (uint*)bitmapPointer.ToPointer();
			Parallel.For(0, P.Width * P.Height, pixel =>
			{
				double r = ((double)pixel % P.Height - P.Width / 2) / P.Scale + P.Midpoint.Real;
				double i = ((double)pixel / P.Height - P.Height / 2) / P.Scale + P.Midpoint.Imaginary;
				if(IteratePoint(r, i, out double nextR, out double nextI) == P.IterationLimit)
					*(bitmap + pixel) = 0x0000FF;
				else
					*(bitmap + pixel) = 0x253434;
			});
		}
	}
}