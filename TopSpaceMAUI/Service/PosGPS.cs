using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class POSGps : Syncable<Model.POSGpsTemp, DAL.POSGps>
	{
		public POSGps(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOSGps");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_POSGPS, DAL.Token.Current.Username);
		}
	}
}