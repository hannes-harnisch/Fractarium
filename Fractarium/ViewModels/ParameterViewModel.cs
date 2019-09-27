using Fractarium.Models.Fractals;

namespace Fractarium.ViewModels
{
	public class ParameterViewModel : ViewModel
	{
		public string[] TypeEntries => FractalTypes.Names;

		public string Width
		{
			set
			{
			}
		}

		public string Height { get; set; }

		public string IterationLimit { get; set; }

		public string ZoomFactor { get; set; }

		public string Midpoint { get; set; }

		public string Scale { get; set; }

		public string JuliaConstant { get; set; }

		public string PhoenixConstant { get; set; }

		public string MultibrotExponent { get; set; }

		public void BindImageSizeToWindow(bool isChecked)
		{

		}
	}
}