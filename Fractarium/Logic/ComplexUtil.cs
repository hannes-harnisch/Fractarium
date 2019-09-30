using System.Numerics;
using System.Text.RegularExpressions;

namespace Fractarium.Logic
{
	public static class ComplexUtil
	{
		private const string Pattern = @"^(-|\+)?(([0-9]+(\.[0-9]+)?)?i|[0-9]+(\.[0-9]+)?((-|\+)([0-9]+(\.[0-9]+)?)?i)?)$";

		public static bool TryParse(string s, out Complex result)
		{
			result = Complex.Zero;
			bool valid = Regex.IsMatch(s, Pattern);
			if(valid)
				result = new Complex(double.Parse(ParseReal(s)), double.Parse(ParseImaginary(s)));
			return valid;
		}

		private static string ParseReal(string s)
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
			return real;
		}

		private static string ParseImaginary(string s)
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
			return imaginary;
		}
	}
}