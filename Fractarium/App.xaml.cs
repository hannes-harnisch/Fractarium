using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Fractarium.UserInterface;

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Fractarium
{
	/// <summary>
	/// Encapsulates the Fractarium desktop app and contains utility functions for the user interface.
	/// </summary>
	public class App : Application
	{
		/// <summary>
		/// Gets a static reference to the running instance's window.
		/// </summary>
		public static readonly MainWindow Window = new();

		/// <summary>
		/// Returns the value by which the screen DPI is enhanced through the display settings.
		/// </summary>
		public static float ScreenEnhancement => System.Drawing.Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;

		/// <summary>
		/// Determines the global locale for the app.
		/// </summary>
		public static CultureInfo Locale => CultureInfo.InvariantCulture;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
			Window.Show();
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
			PixelSize size = new(width, height);
			Vector dpi = new(96, 96);
			return new(PixelFormat.Bgra8888, AlphaFormat.Unpremul, (IntPtr)ptr, size, dpi, 4 * width);
		}

		/// <summary>
		/// Removes all whitespace from a string. To be used to ignore whitespace in text box inputs.
		/// </summary>
		/// <param name="text">A text box's text.</param>
		/// <returns>The input without whitespace.</returns>
		public static string PrepareInput(string text)
		{
			return Regex.Replace(text, @"\s+", "");
		}
	}
}
