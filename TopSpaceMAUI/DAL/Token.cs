using System;

using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class Token
	{
		private static Model.Token current = null;



		public Token ()
		{
		}



		public static Model.Token Current
		{
			get
			{
				if (current != null)
					return current;

				Model.Token tempToken = new Model.Token ();
				tempToken.Username = XNSUserDefaults.GetStringForKey(Config.KEY_USERNAME);
				tempToken.TokenID = XNSUserDefaults.GetStringForKey(Config.KEY_TOKEN);
				return Current = tempToken;
			}

			protected set
			{
				current = value;
			}
		}



		public Model.Token GetToken ()
		{
			return Current;
		}



		public void SaveUser (string username, string tokenID)
		{
			Current = null;

			// TODO: Simulate User
			//username = "EALMN";

			XNSUserDefaults.SetStringValueForKey (Config.KEY_USERNAME, username.Trim().ToUpper());
			XNSUserDefaults.SetStringValueForKey (Config.KEY_TOKEN, tokenID);
		}

		public void DeleteToken ()
		{
			Current = null;
			XNSUserDefaults.RemoveObject (Config.KEY_TOKEN);
		}
	}
}