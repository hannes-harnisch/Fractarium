using System.Numerics;

namespace Fractarium.Logic
{
	/// <summary>
	/// Holds the basic parameters needed to generate a fractal image.
	/// </summary>
	public struct BaseParameters
	{
		/// <summary>
		/// Width of the fractal image.
		/// </summary>
		public uint Width { get; set; }

		/// <summary>
		/// Height of the fractal image.
		/// </summary>
		public uint Height { get; set; }

		/// <summary>
		/// Maximum amount of iterations possible for determining a pixel.
		/// </summary>
		public uint IterationLimit { get; set; }

		/// <summary>
		/// How much the scale is multiplied after clicking on a point in the image.
		/// </summary>
		public uint ZoomFactor { get; set; }

		/// <summary>
		/// How far the image is zoomed in.
		/// </summary>
		public ulong Scale { get; set; }

		/// <summary>
		/// Central point of the image.
		/// </summary>
		public Complex Midpoint { get; set; }
	}
}