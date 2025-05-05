using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Util
{
	public class XUIAlertView
	{
		public static void ShowTranslated (string title, string message, Action del = null, string cancelButtonTitle = "OK", string[] otherButtons = null)
		{
			//string[] buttons = null;

			//if (otherButtons != null && otherButtons.Length > 0) {
			//	buttons = new string[otherButtons.Length];
			//	for (int i = 0; i < otherButtons.Length; i++)
			//		buttons [i] = Localization.TryTranslateText (otherButtons [i]);
			//}

			Application.Current.MainPage.DisplayAlert(
				Localization.TryTranslateText(title), 
				Localization.TryTranslateText(message),
                Localization.TryTranslateText(cancelButtonTitle));
			if (del != null) {
				del.Invoke();
			}
			//UIAlertView alert = new UIAlertView (
			//	Localization.TryTranslateText (title),
			//	,
			//	del,
			//	,
			//	buttons
			//	);

			//alert.Show ();

			//return alert;
		}
	}
}