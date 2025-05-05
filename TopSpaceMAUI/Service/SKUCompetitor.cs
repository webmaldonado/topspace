using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class SKUCompetitor : Syncable<Model.SKUCompetitorTemp, DAL.SKUCompetitor>
	{
		public SKUCompetitor (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntitySKUCompetitor");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_SKU_COMPETITORS, DAL.Token.Current.Username);
		}
	}
}