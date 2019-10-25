using System.Numerics;
using System.Text.RegularExpressions;

namespace Fractarium.Logic
{
	/// <summary>
	/// Utilities for handling complex numbers.
	/// </summary>
	public static class ComplexUtil
	{
		/// <summary>
		/// Matches a complex number.
		/// </summary>
		private static readonly Regex ComplexRegex
			= new Regex(@"^(([-\+]?(\d+(\.\d+)?)?i)|([-\+]?\d+(\.\d+)?)([-\+](\d+(\.\d+)?)?i)?)$");

		/// <summary>
		/// Tries to convert the string representation of a number to its complex number equivalent.
		/// </summary>
		/// <param name="s">String representing a complex number.</param>
		/// <param name="result">A complex number parsed from the string or zero if parsing failed.</param>
		/// <returns>Whether the conversion succeeded or failed.</returns>
		public static bool TryParse(string s, out Complex result)
		{
			var match = ComplexRegex.Match(s);
			if(match.Success)
			{
				string re = match.Groups[5].Value;
				if(re == "")
					re = "0";

				string im = match.Groups[2].Value;
				if(im == "")
					im = match.Groups[7].Value;
				switch(im)
				{
					case "": im = "0"; break;
					case "i":
					case "+i": im = "1"; break;
					case "-i": im = "-1"; break;
				}
				result = new Complex(double.Parse(re, App.CI), double.Parse(im.TrimEnd('i'), App.CI));
			}
			else
				result = Complex.Zero;
			return match.Success;
		}

		/// <summary>
		/// Generates a string representation of a complex number.
		/// </summary>
		/// <param name="c">The complex input.</param>
		/// <returns>The string representation.</returns>
		public static string ProperString(this Complex c)
		{
			string s = c.Real.ToString(Format, App.CI);
			if(c.Imaginary >= 0)
				s += "+";
			return s + c.Imaginary.ToString(Format, App.CI) + "i";
		}
		private static readonly string Format = "0." + new string('#', 339);
	}
}