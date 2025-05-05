using System;
using System.Resources;
using TopSpaceMAUI.Properties;
using Resources = TopSpaceMAUI.Properties.Resources;



namespace TopSpaceMAUI.Util
{
	public static class Localization
	{
		public static string TranslateText (string text)
		{
			string translated = "";
            //#if IOS
            //			//translated = NSBundle.MainBundle.LocalizedString (text, "");
            //#else
            //            translated = Resources.ResourceManager.GetString(text)??"";
            //#endif

            translated = Resources.ResourceManager.GetString(text) ?? "";

            return translated;
		}


		public static string TryTranslateText (string text)
		{
			if (string.IsNullOrEmpty (text))
				return text;

			string aux = TranslateText (text);

			if (string.IsNullOrEmpty (aux))
				return text;

			return aux;
		}



		public static string TryTranslateText (string text, string textAlternative)
		{
			if (string.IsNullOrEmpty (text))
				return TryTranslateText (textAlternative);

			string aux = TranslateText (text);

			if (string.IsNullOrEmpty (aux))
				return TryTranslateText (textAlternative);

			return aux;
		}
	}
}