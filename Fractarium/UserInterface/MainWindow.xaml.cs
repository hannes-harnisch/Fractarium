using System;
using System.Linq;
using System.Numerics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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
		/// Holds a reference to the currently displayed fractal.
		/// </summary>
		public Fractal Fractal { get; set; }

		/// <summary>
		/// The currently selected fractal type.
		/// </summary>
		public FractalType FractalType { get; set; }

		/// <summary>
		/// Holds the parameter values most recently parsed from the parameter tab.
		/// </summary>
		public BaseParameters Params = new BaseParameters();

		/// <summary>
		/// How much the scale is multiplied after clicking on a point in the image.
		/// </summary>
		public int ZoomFactor { get; set; }

		/// <summary>
		/// The constant coefficient used for fractals related to the Julia set.
		/// </summary>
		public Complex JuliaConstant { get; set; }

		/// <summary>
		/// The constant coefficient used in the Phoenix set.
		/// </summary>
		public Complex PhoenixConstant { get; set; }

		/// <summary>
		/// The constant coefficient used for fractals related to the Multibrot set.
		/// </summary>
		public double MultibrotExponent { get; set; }

		/// <summary>
		/// Holds the currently selected color palette.
		/// </summary>
		public Palette Palette = new Palette(6);

		private bool MenuInitialized = false;

		private readonly Button RenderButton;

		private readonly ParameterTab ParameterTab;

		private readonly ColorTab ColorTab;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public MainWindow()
		{
			AvaloniaXamlLoader.Load(this);
#if DEBUG
			this.AttachDevTools();
#endif
			RenderButton = this.Find<Button>("RenderButton");
			ParameterTab = this.Find<ParameterTab>("ParameterTab");
			ColorTab = this.Find<ColorTab>("ColorTab");
		}

		/// <summary>
		/// Performs initialization operations for the user interface that can't be otherwise implemented.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void InitializeMenu(object sender, EventArgs e)
		{
			if(!MenuInitialized)
			{
				this.GetObservable(BoundsProperty).Subscribe(bounds =>
				{
					if(ParameterTab.BindImageSizeToWindow)
					{
						var width = ParameterTab.Find<TextBox>("Width");
						width.Text = ((int)(bounds.Width * App.ScreenEnhancement)).ToString();
						ParameterTab.OnPositiveIntInput(width, null);

						double h = (bounds.Height - this.Find<TabControl>("Menu").Bounds.Height) * App.ScreenEnhancement;
						var height = ParameterTab.Find<TextBox>("Height");
						height.Text = ((int)h).ToString();
						ParameterTab.OnPositiveIntInput(height, null);
					}
				});
				ParameterTab.Find<ComboBox>("FractalType").SelectedIndex = 0;

				var zoomFactor = ParameterTab.Find<TextBox>("ZoomFactor");
				zoomFactor.Text = "2";
				ParameterTab.OnPositiveIntInput(zoomFactor, null);

				MenuInitialized = true;
			}
		}

		/// <summary>
		/// Sets a text box's styling to the appropriate style and disables the render button depending on the
		/// correctness of user input. Also initiates rendering if the enter key was pressed.
		/// </summary>
		/// <param name="box">The box where user input occurred.</param>
		/// <param name="parsed">Whether the input could be parsed correctly.</param>
		/// <param name="e">Data associated with the key event from input.</param>
		public void ReactToTextBoxInput(TextBox box, bool parsed, KeyEventArgs e)
		{
			box.Classes = new Classes(parsed ? "" : "Error");

			var enabledBoxes = ParameterTab.Find<Grid>("Grid").Children.Where(c => c is TextBox t && t.IsEnabled).ToList();
			enabledBoxes.AddRange(ColorTab.Find<Grid>("Grid").Children.Where(c => c is TextBox));
			RenderButton.IsEnabled = !enabledBoxes.Any(t => t.Classes.Contains("Error"));

			if(e?.Key == Key.Enter)
				Render(null, null);
		}

		/// <summary>
		/// Uses current parameters to render a fractal image on the image's bitmap.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public unsafe void Render(object sender, RoutedEventArgs e)
		{
			if(!RenderButton.IsEnabled)
				return;

			switch(FractalType)
			{
				case FractalType.MandelbrotSet:
					Fractal = new MandelbrotSet(Params, Palette); break;
				case FractalType.JuliaSet:
					Fractal = new JuliaSet(Params, Palette, JuliaConstant); break;
				case FractalType.PhoenixSet:
					Fractal = new PhoenixSet(Params, Palette, JuliaConstant, PhoenixConstant); break;
				case FractalType.BurningShipSet:
					Fractal = new BurningShipSet(Params, Palette); break;
				case FractalType.BurningShipJuliaSet:
					Fractal = new BurningShipJuliaSet(Params, Palette, JuliaConstant); break;
				case FractalType.MultibrotSet:
					Fractal = new MultibrotSet(Params, Palette, MultibrotExponent); break;
				case FractalType.MultiJuliaSet:
					Fractal = new MultiJuliaSet(Params, Palette, MultibrotExponent, JuliaConstant); break;
				case FractalType.TricornSet:
					Fractal = new TricornSet(Params, Palette); break;
					//case FractalType.LyapunovFractal:
					//fractal = new LyapunovFractal(Params); break;
			}

			fixed(int* ptr = &(new int[Params.Width * Params.Height])[0])
			{
				Fractal.DrawImage(ptr);

				var size = new PixelSize(Params.Width, Params.Height);
				var dpi = new Avalonia.Vector(96, 96);
				int stride = 4 * Params.Width;

				var img = this.Find<Image>("Image");
				img.Source = new Bitmap(PixelFormat.Bgra8888, (IntPtr)ptr, size, dpi, stride);
				img.InvalidateVisual();
			}
		}

		/// <summary>
		/// Zooms into the image by evaluating mouse position, updating parameters and re-rendering.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void Zoom(object sender, PointerReleasedEventArgs e)
		{
			int x = (int)(e.GetPosition((Image)sender).X * App.ScreenEnhancement);
			int y = (int)(e.GetPosition((Image)sender).Y * App.ScreenEnhancement);

			var midpointBox = ParameterTab.Find<TextBox>("Midpoint");
			midpointBox.Text = ComplexUtil.ToString(Fractal.GetPointFromPixel(x, y));
			ParameterTab.OnComplexInput(midpointBox, null);

			int exp = e.MouseButton == MouseButton.Right ? -1 : 1;
			var scaleBox = ParameterTab.Find<TextBox>("Scale");
			scaleBox.Text = ((ulong)(Params.Scale * Math.Pow(ZoomFactor, exp))).ToString();
			ParameterTab.OnLongInput(scaleBox, null);

			Render(null, null);
		}
	}
}