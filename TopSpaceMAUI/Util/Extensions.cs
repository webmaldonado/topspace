using System;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

namespace TopSpaceMAUI.Util
{
	public static class Extensions
	{
		public static string RemoveAccents (this string s)
		{
			string stFormD = s.Normalize (NormalizationForm.FormD);
			StringBuilder sb = new StringBuilder ();

			for (int ich = 0; ich < stFormD.Length; ich++) {
				UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory (stFormD [ich]);
				if (uc != UnicodeCategory.NonSpacingMark) {
					sb.Append (stFormD [ich]);
				}
			}

			return (sb.ToString ().Normalize (NormalizationForm.FormC));
		}

		public static string Clean (this string s, bool emptyToNull = false, int? maxChar = null)
		{
			if (string.IsNullOrWhiteSpace (s) && emptyToNull)
				return null;

			s = s ?? "";

			s = s.Replace ("\n", " ");
			s = s.Replace ("\r", " ");

			s = s.Replace ("\t", " ");

			while (s.Contains ("  "))
				s = s.Replace ("  ", " ");

			s = s.Trim ();

			if (maxChar != null && s.Length > maxChar)
				return s.Substring (0, (int)maxChar - 1).Trim ();

					return s;   
		}

		public static string StripHTML (this string input)
		{
			return Regex.Replace (input, "<.*?>", " / ");
		}

		public static DateTime ToPeriod (this DateTime date)
		{
			return new DateTime (date.Year, date.Month, 1, 0, 0, 0, 0);
		}
	}

	public static class EnumExtensions
	{
		public static string Description (this Enum value)
		{
			var enumType = value.GetType ();
			var field = enumType.GetField (value.ToString ());
			var attributes = field.GetCustomAttributes (typeof (DescriptionAttribute),
								 false);
			return attributes.Length == 0
				? value.ToString ()
					: ((DescriptionAttribute)attributes [0]).Description;
		}
	}

	public static class LinqExtensions
	{
		public static IEnumerable<T> TakeLastItems<T> (this IEnumerable<T> collection,
												int n)
		{
			if (collection == null)
				throw new ArgumentNullException ("collection");
			if (n < 0)
				throw new ArgumentOutOfRangeException ("n", "n must be 0 or greater");

			LinkedList<T> temp = new LinkedList<T> ();

			foreach (var value in collection) {
				temp.AddLast (value);
				if (temp.Count > n)
					temp.RemoveFirst ();
			}

			return temp;
		}
	}
}