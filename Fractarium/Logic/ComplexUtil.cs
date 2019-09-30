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
		/// Regex pattern for matching complex numbers.
		/// </summary>
		private const string Pattern = @"^(-|\+)?(([0-9]+(\.[0-9]+)?)?i|[0-9]+(\.[0-9]+)?((-|\+)([0-9]+(\.[0-9]+)?)?i)?)$";

		/// <summary>
		/// Tries to convert the string representation of a number to its complex number equivalent.
		/// </summary>
		/// <param name="s">String representing a complex number.</param>
		/// <param name="result">A complex number parsed from the string or zero if parsing failed.</param>
		/// <returns>Whether the conversion succeeded or failed.</returns>
		public static bool TryParse(string s, out Complex result)
		{
			bool valid = Regex.IsMatch(s, Pattern);
			result = valid ? new Complex(ParseReal(s), ParseImaginary(s)) : Complex.Zero;
			return valid;
		}

		/// <summary>
		/// Extracts the real part of a string representing a complex number.
		/// </summary>
		/// <param name="s">String representing a complex number.</param>
		/// <returns>The real part of the complex number.</returns>
		private static double ParseReal(string s)
		{
			string real = "0";
			bool negativeStart = false;
			if(!s.Contains("i"))
				real = s;
			else
			{
				if(s.StartsWith("-"))
					negativeStart = true;
				if(s.StartsWith("-") || s.StartsWith("+"))
					s = s.Substring(1);
				if(s.Contains("+"))
				{
					real = s.Substring(0, s.IndexOf("+"));
					if(negativeStart)
						real = "-" + real;
				}
				else if(s.Contains("-"))
				{
					real = s.Substring(0, s.IndexOf("-"));
					if(negativeStart)
						real = "-" + real;
				}
			}
			return double.Parse(real);
		}

		/// <summary>
		/// Extracts the imaginary part of a string representing a complex number.
		/// </summary>
		/// <param name="s">String representing a complex number.</param>
		/// <returns>The imaginary part of the complex number.</returns>
		private static double ParseImaginary(string s)
		{
			string imaginary = "0";
			bool negativeStart = false;
			if(s.Contains("i"))
			{
				if(s.StartsWith("-"))
					negativeStart = true;
				if(s.StartsWith("-") || s.StartsWith("+"))
					s = s.Substring(1);
				if(s.Contains("+"))
				{
					if(string.IsNullOrEmpty(s.Substring(s.IndexOf("+") + 1, s.IndexOf("i") - s.IndexOf("+") - 1)))
						imaginary = "1";
					else
						imaginary = s.Substring(s.IndexOf("+") + 1, s.IndexOf("i") - s.IndexOf("+") - 1);
				}
				else if(s.Contains("-"))
				{
					if(string.IsNullOrEmpty(s.Substring(s.IndexOf("-") + 1, s.IndexOf("i") - s.IndexOf("-") - 1)))
						imaginary = "-1";
					else
						imaginary = s.Substring(s.IndexOf("-"), s.IndexOf("i") - s.IndexOf("-"));
				}
				else
				{
					if(string.IsNullOrEmpty(s.Substring(0, s.IndexOf("i"))))
						imaginary = "1";
					else
						imaginary = s.Substring(0, s.IndexOf("i"));
					if(negativeStart)
						imaginary = "-" + imaginary;
				}
			}
			return double.Parse(imaginary);
		}
	}
}