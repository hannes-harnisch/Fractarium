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

		/// <summary>
		/// Holds the palette colors as a 2D byte array.
		/// </summary>
		private byte[,] P;

		private double Ratio;

		/// <summary>
		/// Amount of colors in the palette excluding the set element color.
		/// </summary>
		public int Size => P.GetLength(0) - 1;

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
			P = new byte[colors.Length, 4];
			for(int i = 0; i < colors.Length; i++)
				for(int j = 0; j < 4; j++)
					P[i, j] = byte.Parse(colors[i].Substring(j * 2, 2), NumberStyles.HexNumber);

			Ratio = 1 / (double)(P.GetLength(0) - 2);
			ElementColor = (P[0, 0] << 24) + (P[0, 1] << 16) + (P[0, 2] << 8) + P[0, 3];
		}

		/// <summary>
		/// Calculates a color between two palette colors based on a pixel's iteration count and the iteration limit.
		/// </summary>
		/// <param name="iterationFraction">How close the iteration count of a point is to the iteration limit.</param>
		/// <returns>An ARGB color as a 32-bit integer.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GradientColor(double iterationFraction)
		{
			int i = (int)Math.Ceiling(iterationFraction / Ratio);
			double v = (iterationFraction + Ratio * (1 - i)) / Ratio;
			int a = (int)(P[i, 0] + Math.Round(v * (P[i + 1, 0] - P[i, 0]))) << 24;
			int r = (int)(P[i, 1] + Math.Round(v * (P[i + 1, 1] - P[i, 1]))) << 16;
			int g = (int)(P[i, 2] + Math.Round(v * (P[i + 1, 2] - P[i, 2]))) << 8;
			int b = (int)(P[i, 3] + Math.Round(v * (P[i + 1, 3] - P[i, 3])));
			return a + r + g + b;
		}

		/// <summary>
		/// Gets the palette color given by the index. To get the set element color, use key 0.
		/// </summary>
		/// <param name="key">Index of the palette color.</param>
		/// <returns>The palette color as an array of 4 bytes.</returns>
		public byte[] this[int key] => new[] { P[key, 0], P[key, 1], P[key, 2], P[key, 3] };

		/// <summary>
		/// Allows the manipulation of individual bytes of the palette.
		/// </summary>
		/// <param name="color">Indexes the color of the palette.</param>
		/// <param name="colorByte">Indexes the byte of the ARGB color.</param>
		/// <returns>The value of the indexed color and color byte.</returns>
		public byte this[int color, int colorByte]
		{
			get => P[color, colorByte];
			set
			{
				P[color, colorByte] = value;
				ElementColor = (P[0, 0] << 24) + (P[0, 1] << 16) + (P[0, 2] << 8) + P[0, 3];
			}
		}

		/// <summary>
		/// Adds the given color to the palette.
		/// </summary>
		/// <param name="color">The new color as an array of 4 bytes.</param>
		public void Add(byte[] color)
		{
			byte[,] newPalette = new byte[P.GetLength(0) + 1, 4];
			for(int i = 0; i < P.GetLength(0); i++)
				for(int j = 0; j < 4; j++)
					newPalette[i, j] = P[i, j];

			for(int i = 0; i < 4; i++)
				newPalette[newPalette.GetLength(0) - 1, i] = color[i];

			P = newPalette;
			Ratio = 1 / (double)(P.GetLength(0) - 2);
		}

		/// <summary>
		/// Removes the last color of the palette.
		/// </summary>
		public void RemoveLast()
		{
			byte[,] newPalette = new byte[P.GetLength(0) - 1, 4];
			for(int i = 0; i < newPalette.GetLength(0); i++)
				for(int j = 0; j < 4; j++)
					newPalette[i, j] = P[i, j];

			P = newPalette;
			Ratio = 1 / (double)(P.GetLength(0) - 2);
		}

		/// <summary>
		/// Draws a depiction of the palette with the colors blending together like a gradient.
		/// </summary>
		/// <param name="width">Width of the bitmap.</param>
		/// <param name="height">Height of the bitmap.</param>
		/// <param name="ptr">Handle to the array encoding the bitmap.</param>
		public unsafe void DrawContinuousPreview(int width, int height, int* ptr)
		{
			for(int x = 0; x < width; x++)
				for(int y = 0; y < height; y++)
					if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
						*(ptr + x + y * width) = ElementColor;
					else
						*(ptr + x + y * width) = GradientColor(x / (double)width);
		}

		/// <summary>
		/// Draws a depiction of the palette with the colors visible as rectangles without blending.
		/// </summary>
		/// <param name="width">Width of the bitmap.</param>
		/// <param name="height">Height of the bitmap.</param>
		/// <param name="ptr">Handle to the array encoding the bitmap.</param>
		public unsafe void DrawDiscretePreview(int width, int height, int* ptr)
		{
			double r = 1 / (double)(P.GetLength(0) - 1);
			for(int x = 0; x < width; x++)
				for(int y = 0; y < height; y++)
					if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
						*(ptr + x + y * width) = ElementColor;
					else
					{
						int i = (int)Math.Ceiling(x / (double)width / r);
						*(ptr + x + y * width) = (P[i, 0] << 24) + (P[i, 1] << 16) + (P[i, 2] << 8) + P[i, 3];
					}
		}
	}
}