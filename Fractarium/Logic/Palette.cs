using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Fractarium.Logic
{
	/// <summary>
	/// Represents a list of ARGB colors that serves as the basis for coloring a fractal image.
	/// </summary>
	public struct Palette
	{
		/// <summary>
		/// Maximum amount of colors allowed in a palette excluding the set element color.
		/// </summary>
		public const int MaxColors = 100;

		private byte[,] Colors;

		private double Ratio;

		/// <summary>
		/// Amount of colors in the palette excluding the set element color.
		/// </summary>
		public int Size => Colors.GetLength(0) - 1;

		/// <summary>
		/// Holds the color representing a point that is part of a set-based fractal's respective set.
		/// </summary>
		public int ElementColor { get; private set; }

		/// <summary>
		/// Instantiates a new palette from the given list of hexadecimal ARGB colors.
		/// </summary>
		/// <param name="colors">ARGB Colors given as hexadecimal strings.</param>
		public Palette(params string[] colors)
		{
			Colors = new byte[colors.Length, 4];
			for(int i = 0; i < colors.Length; i++)
				for(int j = 0; j < 4; j++)
					Colors[i, j] = byte.Parse(colors[i].Substring(j * 2, 2), NumberStyles.HexNumber);

			Ratio = 1 / (double)(Colors.GetLength(0) - 2);
			ElementColor = (Colors[0, 0] << 24) + (Colors[0, 1] << 16) + (Colors[0, 2] << 8) + Colors[0, 3];
		}

		/// <summary>
		/// Calculates a color between two palette colors based on a pixel's iteration count and the iteration limit.
		/// </summary>
		/// <param name="iterationFraction">How close the iteration count of a point is to the iteration limit.</param>
		/// <returns>An ARGB color as a 32-bit integer.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ColorFromFraction(double iterationFraction)
		{
			int i = (int)Math.Ceiling(iterationFraction / Ratio);
			double v = (iterationFraction + Ratio * (1 - i)) / Ratio;
			int a = (int)(Colors[i, 0] + Math.Round(v * (Colors[i + 1, 0] - Colors[i, 0]))) << 24;
			int r = (int)(Colors[i, 1] + Math.Round(v * (Colors[i + 1, 1] - Colors[i, 1]))) << 16;
			int g = (int)(Colors[i, 2] + Math.Round(v * (Colors[i + 1, 2] - Colors[i, 2]))) << 8;
			int b = (int)(Colors[i, 3] + Math.Round(v * (Colors[i + 1, 3] - Colors[i, 3])));
			return a + r + g + b;
		}

		public unsafe void DrawContinuousPreview(int width, int height, int* ptr)
		{

		}

		public unsafe void DrawDiscretePreview(int width, int height, int* ptr)
		{

		}
	}
}