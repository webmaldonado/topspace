using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LPMetricType : Syncable<Model.LPMetricTypeTemp, DAL.LPMetricType>
	{
		public LPMetricType (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPMetricType");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LPMETRICTYPES, DAL.Token.Current.Username);
		}
	}
}