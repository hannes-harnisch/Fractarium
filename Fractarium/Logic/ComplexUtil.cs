﻿using System.Numerics;
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
			= new(@"^(([-\+]?(\d+(\.\d+)?)?i)|([-\+]?\d+(\.\d+)?)([-\+](\d+(\.\d+)?)?i)?)$");

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
				if(im == "")
					im = "0";
				if(im.Length < 3)
					im = im.Replace('i', '1');

				result = new(double.Parse(re, App.Locale), double.Parse(im.TrimEnd('i'), App.Locale));
			}
			else
				result = Complex.Zero;
			return match.Success;
		}

		/// <summary>
		/// Generates a concise string representation of a complex number.
		/// </summary>
		/// <param name="c">The complex input.</param>
		/// <returns>The string representation.</returns>
		public static string ToMathString(this Complex c)
		{
			if(c == Complex.Zero)
				return "0";
			string s = "";
			string re = c.Real.ToString(Format, App.Locale);
			string im = c.Imaginary.ToString(Format, App.Locale);
			if(c.Real != 0)
			{
				s += re;
				if(c.Imaginary > 0)
					s += "+";
			}
			if(c.Imaginary != 0)
			{
				if(c.Imaginary == 1)
					s += "i";
				else if(c.Imaginary == -1)
					s += "-i";
				else
					s += im + "i";
			}
			return s;
		}
		private static readonly string Format = "0." + new string('#', 339);
	}
}
