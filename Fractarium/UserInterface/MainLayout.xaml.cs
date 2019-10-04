using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

using Fractarium.Logic;
using Fractarium.Logic.Fractals;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Holds the content of the main window.
	/// </summary>
	public class MainLayout : UserControl
	{
		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Parameters = new BaseParameters();

		/// <summary>
		/// Holds the bitmap for the currently displayed fractal image.
		/// </summary>
		public WriteableBitmap Bitmap { get; set; }

		/// <summary>
		/// Initializes associated XAML objects and data context.
		/// </summary>
		public MainLayout()
		{
			AvaloniaXamlLoader.Load(this);
			DataContext = this;
		}

		/// <summary>
		/// Uses current parameters to render a fractal image on the context's bitmap.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public unsafe void Render(object sender, RoutedEventArgs e)
		{
			var bitmap = new WriteableBitmap(new PixelSize((int)Parameters.Width, (int)Parameters.Height), new Vector(96, 96));
			var fractal = new MandelbrotSet(Parameters);
			fractal.DrawImage(bitmap.Lock().Address);
			Bitmap = bitmap;
		}
	}
}