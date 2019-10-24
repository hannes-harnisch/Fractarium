using System;
using System.Drawing;

using Avalonia;
using Avalonia.Markup.Xaml;

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
		public static readonly MainWindow Context = new MainWindow();

		/// <summary>
		/// Returns the value by which the screen DPI is enhanced through the display settings.
		/// </summary>
		public static float ScreenEnhancement => Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;

		/// <summary>
		/// Initializes associated XAML objects.
		/// </summary>
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}