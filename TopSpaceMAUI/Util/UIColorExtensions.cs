using System;


namespace TopSpaceMAUI.Util
{
	public static class UIColorExtensions
	{
		public static Color FromHexString (this Color color, string hexValue, float alpha = 1.0f)
		{
			var colorString = hexValue.Trim ().Replace ("#", "");
			if (alpha > 1.0f) {
				alpha = 1.0f;
			} else if (alpha < 0.0f) {
				alpha = 0.0f;
			}

			float red, green, blue;

			switch (colorString.Length) {
			case 3:
				{
					// #RGB
					red = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (0, 1)), 16) / 255f;
					green = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (1, 1)), 16) / 255f;
					blue = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (2, 1)), 16) / 255f;
					return Color.FromRgba(red, green, blue, alpha);
				}
			case 6:
				{
					// #RRGGBB
					red = Convert.ToInt32 (colorString.Substring (0, 2), 16) / 255f;
					green = Convert.ToInt32 (colorString.Substring (2, 2), 16) / 255f;
					blue = Convert.ToInt32 (colorString.Substring (4, 2), 16) / 255f;
					return Color.FromRgba(red, green, blue, alpha);
				}   
			default :
				return Color.FromRgb(1, 152, 195);
			}
		}
	}
}

