using System;
using System.Globalization;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Fractarium.UserInterface;

namespace Fractarium
{
	/// <summary>
	/// Encapsulates the Fractarium desktop app.
	/// </summary>
	public class App : Application
	{
		/// <summary>
		/// Gets a static reference to the running instance's window.
		/// </summary>
		public static readonly MainWindow Window = new MainWindow();

		/// <summary>
		/// Returns the value by which the screen DPI is enhanced through the display settings.
		/// </summary>
		public static float ScreenEnhancement => System.Drawing.Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;

		/// <summary>
		/// Determines the global locale for the app.
		/// </summary>
		public static CultureInfo CI => CultureInfo.InvariantCulture;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		/// <summary>
		/// Returns an Avalonia bitmap fitted to the platform configuration.
		/// </summary>
		/// <param name="width">The width in pixels.</param>
		/// <param name="height">The height in pixels.</param>
		/// <param name="ptr">A handle from an array encoding the pixels.</param>
		/// <returns>A bitmap with an image.</returns>
		public static unsafe Bitmap MakeDefaultBitmap(int width, int height, int* ptr)
		{
			var size = new PixelSize(width, height);
			var dpi = new Vector(96, 96);
			return new Bitmap(PixelFormat.Bgra8888, (IntPtr)ptr, size, dpi, 4 * width);
		}
	}
}