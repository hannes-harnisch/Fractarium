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
				ParameterTab.Find<TextBox>("ZoomFactor").Text = Context.ZoomFactor.ToString();
				ParameterTab.Find<TextBox>("JuliaConstant").Text = Context.JuliaConstant.ProperString();
				ParameterTab.Find<TextBox>("PhoenixConstant").Text = Context.PhoenixConstant.ProperString();
				ParameterTab.Find<TextBox>("MultibrotExponent").Text = Context.MultibrotExponent.ToString();

				ColorTab.UpdateControls();
				ColorTab.ColorSelector.SelectedIndex = 0;

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

			AnyInvalidInput = Controls.Any(control => control.Classes.Contains("Error"));
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
		/// Zooms into the image by evaluating mouse position, updating parameters and re-rendering.
		/// </summary>
		/// <param name="sender">Source of the event.</param>
		/// <param name="e">Data associated with the event.</param>
		public void Zoom(object sender, PointerReleasedEventArgs e)
		{
			int x = (int)(e.GetPosition((Image)sender).X * App.ScreenEnhancement);
			int y = (int)(e.GetPosition((Image)sender).Y * App.ScreenEnhancement);

			var midpointBox = ParameterTab.Find<TextBox>("Midpoint");
			midpointBox.Text = Context.Fractal.GetPointFromPixel(x, y).ProperString();
			ParameterTab.OnComplexInput(midpointBox, null);

			int exp = e.MouseButton == MouseButton.Right ? -1 : 1;
			var scaleBox = ParameterTab.Find<TextBox>("Scale");
			scaleBox.Text = ((ulong)(Context.Params.Scale * Math.Pow(Context.ZoomFactor, exp))).ToString();
			ParameterTab.OnLongInput(scaleBox, null);

			InitRender(null, null);
		}
	}
}