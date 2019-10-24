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
		public AppContext Context = new AppContext();

		private bool MenuInitialized = false;

		private bool ImageVisuallyOverlapped = false;

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

				ColorTab.Update(Context.Palette);

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
				InitRender(null, null);
		}

		/// <summary>
		/// Used to start the rendering process from the user interface.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void InitRender(object sender, RoutedEventArgs e)
		{
			if(!RenderButton.IsEnabled)
				return;

			var img = this.Find<Image>("Image");
			img.Source = Context.Render();
			img.InvalidateVisual();
		}

		/// <summary>
		/// Zooms into the image by evaluating mouse position, updating parameters and re-rendering.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void Zoom(object sender, PointerReleasedEventArgs e)
		{
			if(ImageVisuallyOverlapped)
				return;

			int x = (int)(e.GetPosition((Image)sender).X * App.ScreenEnhancement);
			int y = (int)(e.GetPosition((Image)sender).Y * App.ScreenEnhancement);

			var midpointBox = ParameterTab.Find<TextBox>("Midpoint");
			midpointBox.Text = ComplexUtil.ToString(Context.Fractal.GetPointFromPixel(x, y));
			ParameterTab.OnComplexInput(midpointBox, null);

			int exp = e.MouseButton == MouseButton.Right ? -1 : 1;
			var scaleBox = ParameterTab.Find<TextBox>("Scale");
			scaleBox.Text = ((ulong)(Context.Params.Scale * Math.Pow(Context.ZoomFactor, exp))).ToString();
			ParameterTab.OnLongInput(scaleBox, null);

			InitRender(null, null);
		}
	}
}