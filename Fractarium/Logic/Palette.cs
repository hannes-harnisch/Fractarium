using System;

namespace Fractarium.Logic
{
	/// <summary>
	/// Represents a list of ARGB colors that serves as the basis for coloring a fractal image.
	/// </summary>
	public struct Palette
	{
		public const int MaxColors = 20;

		private byte[,] Colors;

		private double Ratio;

		/// <summary>
		/// Holds the color representing a point that is part of a set-based fractal's respective set.
		/// </summary>
		public int ElementColor { get; private set; }

		public Palette(int size)
		{
			Colors = new byte[,]
			{
				{ 0xFF, 0x00, 0x00, 0x00 },
				{ 0xFF, 0x00, 0x00, 0xFF },
				{ 0xFF, 0xFF, 0x00, 0xFF },
				{ 0xFF, 0xFF, 0x00, 0x00 },
				{ 0xFF, 0xFF, 0xFF, 0x00 },
				{ 0xFF, 0xFF, 0xFF, 0xFF }
			};
			Ratio = 1 / (double)(Colors.GetLength(0) - 2);
			ElementColor = (Colors[0, 0] << 24) + (Colors[0, 1] << 16) + (Colors[0, 2] << 8) + Colors[0, 3];
		}

		/// <summary>
		/// Calculates a color between two palette colors based on a pixel's iteration count and the iteration limit.
		/// </summary>
		/// <param name="iterationFraction">How close the iteration count of a point is to the iteration limit.</param>
		/// <returns>An ARGB color as a 32-bit integer.</returns>
		public int ColorFromFraction(double iterationFraction)
		{
			int i = (int)Math.Ceiling(iterationFraction / Ratio);
			double v = (iterationFraction + Ratio * (1 - i)) / Ratio;
			int alpha = (int)(Colors[i, 0] + Math.Round(v * (Colors[i + 1, 0] - Colors[i, 0]))) << 24;
			int red = (int)(Colors[i, 1] + Math.Round(v * (Colors[i + 1, 1] - Colors[i, 1]))) << 16;
			int green = (int)(Colors[i, 2] + Math.Round(v * (Colors[i + 1, 2] - Colors[i, 2]))) << 8;
			int blue = (int)(Colors[i, 3] + Math.Round(v * (Colors[i + 1, 3] - Colors[i, 3])));
			return alpha + red + green + blue;
		}
	}
}