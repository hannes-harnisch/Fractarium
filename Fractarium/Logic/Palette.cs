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
		/// Holds the palette colors as a 2D byte array.
		/// </summary>
		private byte[,] C;

		/// <summary>
		/// Indicates after how much of the iteration fraction a different color should be targeted
		/// for the gradient.
		/// </summary>
		private double Ratio => 1 / (double)(C.GetLength(0) - 2);

		/// <summary>
		/// Maximum amount of colors allowed in a palette excluding the set element color.
		/// </summary>
		public const int MaxColors = 64;

		/// <summary>
		/// Minimum amount of colors allowed in a palette excluding the set element color.
		/// </summary>
		public const int MinColors = 2;

		/// <summary>
		/// Amount of colors in the palette excluding the set element color.
		/// </summary>
		public int Size => C.GetLength(0) - 1;

		/// <summary>
		/// Holds the color representing a point that is part of a fractal's respective set.
		/// </summary>
		public int ElementColor => (C[0, 0] << 24) + (C[0, 1] << 16) + (C[0, 2] << 8) + C[0, 3];

		/// <summary>
		/// Instantiates a new palette from the given list of hexadecimal ARGB colors.
		/// </summary>
		/// <param name="colors">ARGB Colors given as hexadecimal strings.</param>
		public Palette(params string[] colors)
		{
			C = new byte[colors.Length, 4];
			for(int i = 0; i < colors.Length; i++)
				for(int j = 0; j < 4; j++)
					C[i, j] = byte.Parse(colors[i].Substring(j * 2, 2), NumberStyles.HexNumber);
		}

		/// <summary>
		/// Calculates a color between two palette colors based on a pixel's iteration count
		/// and the iteration limit.
		/// </summary>
		/// <param name="iterationFraction">How close the iteration count of a point is to
		/// the iteration limit.</param>
		/// <returns>An ARGB color as a 32-bit integer.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GradientColor(double iterationFraction)
		{
			int i = (int)Math.Ceiling(iterationFraction / Ratio);
			double v = (iterationFraction + Ratio * (1 - i)) / Ratio;
			unchecked
			{
				int a = (int)(C[i, 0] + Math.Round(v * (C[i + 1, 0] - C[i, 0]))) << 24;
				int r = (int)(C[i, 1] + Math.Round(v * (C[i + 1, 1] - C[i, 1]))) << 16;
				int g = (int)(C[i, 2] + Math.Round(v * (C[i + 1, 2] - C[i, 2]))) << 8;
				int b = (int)(C[i, 3] + Math.Round(v * (C[i + 1, 3] - C[i, 3])));
				return a + r + g + b;
			}
		}

		/// <summary>
		/// Gets the palette color given by the index. To get the set element color, use key 0.
		/// </summary>
		/// <param name="key">Index of the palette color.</param>
		/// <returns>The palette color as an array of 4 bytes.</returns>
		public byte[] this[int key] => new[] { C[key, 0], C[key, 1], C[key, 2], C[key, 3] };

		/// <summary>
		/// Allows the manipulation of individual bytes of the palette.
		/// </summary>
		/// <param name="color">Indexes the color of the palette.</param>
		/// <param name="colorByte">Indexes the byte of the ARGB color.</param>
		/// <returns>The value of the indexed color and color byte.</returns>
		public byte this[int color, int colorByte]
		{
			get => C[color, colorByte];
			set => C[color, colorByte] = value;
		}

		/// <summary>
		/// Appends a random new color to the end of the palette, unless it would exceed maximum size.
		/// </summary>
		/// <returns>Whether a color could be appended.</returns>
		public bool AppendRandom()
		{
			if(Size == MaxColors)
				return false;

			byte[,] newPalette = new byte[C.GetLength(0) + 1, 4];
			for(int i = 0; i < C.GetLength(0); i++)
				for(int j = 0; j < 4; j++)
					newPalette[i, j] = C[i, j];

			byte[] newColor = new byte[4];
			new Random().NextBytes(newColor);
			newColor[0] = 0xFF;
			for(int i = 0; i < 4; i++)
				newPalette[newPalette.GetLength(0) - 1, i] = newColor[i];

			C = newPalette;
			return true;
		}

		/// <summary>
		/// Inserts the color found at the specified index next to itself, unless it would exceed
		/// maximum size.
		/// </summary>
		/// <param name="index">Indexes the color to be duplicated.</param>
		/// <returns>Whether the color could be duplicated.</returns>
		public bool DuplicateAt(int index)
		{
			if(Size == MaxColors)
				return false;

			byte[,] newPalette = new byte[C.GetLength(0) + 1, 4];
			for(int i = 0; i < index + 1; i++)
				for(int j = 0; j < 4; j++)
					newPalette[i, j] = C[i, j];

			for(int i = index; i < C.GetLength(0); i++)
				for(int j = 0; j < 4; j++)
					newPalette[i + 1, j] = C[i, j];
			C = newPalette;
			return true;
		}

		/// <summary>
		/// Removes the specified color of the palette, unless there would be no colors left.
		/// </summary>
		/// <param name="index">Number of the color to be removed.</param>
		/// <returns>Whether the color could be removed.</returns>
		public bool RemoveAt(int index)
		{
			if(Size == MinColors)
				return false;

			byte[,] newPalette = new byte[C.GetLength(0) - 1, 4];
			for(int i = 0; i < index; i++)
				for(int j = 0; j < 4; j++)
					newPalette[i, j] = C[i, j];

			for(int i = index + 1; i < C.GetLength(0); i++)
				for(int j = 0; j < 4; j++)
					newPalette[i - 1, j] = C[i, j];
			C = newPalette;
			return true;
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
				{
					int* address = ptr + x + y * width;
					if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
						*address = ElementColor;
					else
						*address = GradientColor(x / (double)width);
				}
		}

		/// <summary>
		/// Draws a depiction of the palette with the colors visible as rectangles without blending.
		/// </summary>
		/// <param name="width">Width of the bitmap.</param>
		/// <param name="height">Height of the bitmap.</param>
		/// <param name="ptr">Handle to the array encoding the bitmap.</param>
		public unsafe void DrawDiscretePreview(int width, int height, int* ptr)
		{
			for(int x = 0; x < width; x++)
				for(int y = 0; y < height; y++)
				{
					int* address = ptr + x + y * width;
					if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
						*address = ElementColor;
					else
					{
						int i = (int)Math.Ceiling(x / (double)width * Size);
						*address = (C[i, 0] << 24) + (C[i, 1] << 16) + (C[i, 2] << 8) + C[i, 3];
					}
				}
		}
	}
}
