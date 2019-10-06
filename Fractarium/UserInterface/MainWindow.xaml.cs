using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Holds the content of the main window.
	/// </summary>
	public class MainWindow : Window
	{
		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Parameters = new BaseParameters();

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public MainWindow()
		{
			AvaloniaXamlLoader.Load(this);
#if DEBUG
			this.AttachDevTools();
#endif
		}

		/// <summary>
		/// Uses current parameters to render a fractal image on the image's bitmap.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public unsafe void Render(object sender, RoutedEventArgs e)
		{
			uint[] data = new uint[Parameters.Width * Parameters.Height];
			fixed(uint* ptr = &data[0])
			{
				var fractal = new MandelbrotSet(Parameters);
				fractal.DrawImage(ptr);

				var size = new PixelSize((int)Parameters.Width, (int)Parameters.Height);
				var dpi = new Vector(96, 96);
				int stride = 4 * (int)Parameters.Width;

				var img = this.Find<Image>("Image");
				img.Source = new Bitmap(PixelFormat.Bgra8888, (IntPtr)ptr, size, dpi, stride);
				img.InvalidateVisual();
			}
		}
	}
}