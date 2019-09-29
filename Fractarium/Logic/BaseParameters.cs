using System.Numerics;

namespace Fractarium.Logic
{
	public struct BaseParameters
	{
		public uint Width { get; set; }

		public uint Height { get; set; }

		public uint IterationLimit { get; set; }

		public uint ZoomFactor { get; set; }

		public ulong Scale { get; set; }

		public Complex Midpoint { get; set; }
	}
}