using System;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Fractarium.Logic;

namespace Fractarium.UserInterface
{
	/// <summary>
	/// Holds the content of the main window.
	/// </summary>
	public class MainWindow : Window
	{
		/// <summary>
		/// Holds the data context for the entire user interface.
		/// </summary>
		public readonly AppContext Context = new AppContext();

		private readonly Controls Controls = new Controls();

		private readonly ParameterTab ParameterTab;

		private readonly ColorTab ColorTab;

		private bool MenuInitialized = false;

		private bool AnyInvalidInput = false;

		private double ImageCursorX;

		private double ImageCursorY;

		private MouseButton ImageClickMouseButton;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public MainWindow()
		{
			AvaloniaXamlLoader.Load(this);
#if DEBUG
			this.AttachDevTools();
#endif
			ParameterTab = this.Find<ParameterTab>("ParameterTab");
			ColorTab = this.Find<ColorTab>("ColorTab");

			Controls.AddRange(ParameterTab.Find<Grid>("Grid").Children);
			Controls.AddRange(ColorTab.Find<Grid>("Grid").Children);
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
				this.GetObservable(BoundsProperty).Subscribe(SetSizeParametersFromBounds);
				ParameterTab.Find<ComboBox>("FractalType").SelectedIndex = 0;
				ParameterTab.Find<TextBox>("ZoomFactor").Text = Context.ZoomFactor.ToString();
				ParameterTab.Find<TextBox>("JuliaConstant").Text = Context.JuliaConstant.MathString();
				ParameterTab.Find<TextBox>("PhoenixConstant").Text = Context.PhoenixConstant.MathString();
				ParameterTab.Find<TextBox>("Exponent").Text = Context.Exponent.ToString();

				ColorTab.UpdateControls();
				ColorTab.ColorSelector.SelectedIndex = 0;

				MenuInitialized = true;
			}
		}

		/// <summary>
		/// If the option is enabled, sets the image width and height parameters from the window size.
		/// </summary>
		/// <param name="bounds">Bounds property of the window.</param>
		public void SetSizeParametersFromBounds(Rect bounds)
		{
			if(ParameterTab.BindImageSizeToWindow)
			{
				var widthBox = ParameterTab.Find<TextBox>("Width");
				widthBox.Text = ((int)(bounds.Width * App.ScreenEnhancement)).ToString();
				ParameterTab.OnPositiveIntInput(widthBox, null);

				double menuHeight = this.Find<TabControl>("Menu").Bounds.Height;
				double h = (bounds.Height - menuHeight) * App.ScreenEnhancement;
				var heightBox = ParameterTab.Find<TextBox>("Height");
				heightBox.Text = ((int)h).ToString();
				ParameterTab.OnPositiveIntInput(heightBox, null);
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
			if(parsed)
				box.Classes.Remove("Error");
			else
				box.Classes.Add("Error");

			AnyInvalidInput = Controls.Any(c => c.IsEnabled && c.Classes.Contains("Error"));
			this.Find<Button>("RenderButton").IsEnabled = !AnyInvalidInput;

			if(e?.Key == Key.Enter)
				InitRender(null, null);
		}

		/// <summary>
		/// Used to start the rendering process from the user interface.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void InitRender(object sender, RoutedEventArgs e)
		{
			if(!AnyInvalidInput)
			{
				var img = this.Find<Image>("Image");
				img.Source = Context.Render();
				img.InvalidateVisual();
			}
		}

		/// <summary>
		/// Tracks the image pixel position of the mouse when moved over the image.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void TrackMouseOnImage(object sender, PointerReleasedEventArgs e)
		{
			var img = (Image)sender;
			ImageCursorX = Context.Params.Width * e.GetPosition(img).X / img.Bounds.Width;
			ImageCursorY = Context.Params.Height * e.GetPosition(img).Y / img.Bounds.Height;
			ImageClickMouseButton = e.MouseButton;
		}

		/// <summary>
		/// Zooms into the image by evaluating tracked mouse input data, updating parameters and re-rendering.
		/// Cannot extract mouse input data from event parameter due to framework design.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void Zoom(object sender, RoutedEventArgs e)
		{
			var midpointBox = ParameterTab.Find<TextBox>("Midpoint");
			midpointBox.Text = Context.Fractal.GetPointFromPixel(ImageCursorX, ImageCursorY).MathString();
			ParameterTab.OnComplexInput(midpointBox, null);

			ulong newScale;
			if(ImageClickMouseButton == MouseButton.Right)
				newScale = Context.Params.Scale / (ulong)Context.ZoomFactor;
			else
				newScale = Context.Params.Scale * (ulong)Context.ZoomFactor;
			var scaleBox = ParameterTab.Find<TextBox>("Scale");
			scaleBox.Text = newScale.ToString();
			ParameterTab.OnLongInput(scaleBox, null);

			InitRender(null, null);
		}
	}
}