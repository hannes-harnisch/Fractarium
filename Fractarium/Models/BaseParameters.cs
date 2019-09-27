using System.Numerics;

namespace Fractarium.Models
{
	public struct BaseParameters
	{
		public int Width { get; }

		public int Height { get; }

		public int IterationLimit { get; }

		public long Scale { get; }

		public Complex Midpoint { get; }

		public BaseParameters(int width, int height, int iterationlimit, long scale, Complex midpoint)
		{
			Width = width;
			Height = height;
			IterationLimit = iterationlimit;
			Scale = scale;
			Midpoint = midpoint;
		}
	}
}