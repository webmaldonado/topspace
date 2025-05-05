using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LPScoreBrand : Syncable<Model.LPScoreBrandTemp, DAL.LPScoreBrand>
	{
		public LPScoreBrand (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPScoreBrand");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LPSCOREBRANDS, DAL.Token.Current.Username);
		}
	}
}
