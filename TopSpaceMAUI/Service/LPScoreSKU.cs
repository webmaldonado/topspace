using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LPScoreSKU : Syncable<Model.LPScoreSKUTemp, DAL.LPScoreSKU>
	{
		public LPScoreSKU (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPScoreSKU");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LPSCORESKUS, DAL.Token.Current.Username);
		}
	}
}
