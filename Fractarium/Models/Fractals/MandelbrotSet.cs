namespace Fractarium.Models.Fractals
{
	public class MandelbrotSet : Fractal
	{
		public MandelbrotSet(BaseParameters parameters) : base(parameters) { }

		public override int IteratePoint(double r, double i, out double nextR, out double nextI)
		{
			double firstR = r;
			double firstI = i;
			nextR = 0;
			nextI = 0;
			int iteration = 0;
			for(; iteration < P.IterationLimit; iteration++)
			{
				nextR = r * r - i * i + firstR;
				nextI = 2 * r * i + firstI;
				r = nextR;
				i = nextI;
				if(r * r + i * i > DivergenceLimit)
					break;
			}
			return iteration;
		}
	}
}