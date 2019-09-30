using Avalonia;
using Avalonia.Media.Imaging;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Encapsulates the main data context manipulated by the user interface.
	/// </summary>
	public class Context
	{
		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Parameters = new BaseParameters();

		/// <summary>
		/// Holds the bitmap for the currently displayed fractal image.
		/// </summary>
		public IBitmap Image { get; set; }

		/// <summary>
		/// Uses current parameters to render a fractal image on the context's bitmap.
		/// </summary>
		public unsafe void Render()
		{
			var bitmap = new WriteableBitmap(new PixelSize(700, 700), new Vector(96, 96));

			Parameters.Width = (uint)bitmap.Size.Width;
			Parameters.Height = (uint)bitmap.Size.Height;
			var fractal = new MandelbrotSet(Parameters);
			fractal.DrawImage(bitmap.Lock().Address);
			Image = bitmap;
		}
	}
}