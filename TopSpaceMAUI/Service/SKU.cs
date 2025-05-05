using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class SKU : Syncable<Model.SKUTemp, DAL.SKU>
	{
		public SKU (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntitySKU");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_SKUS, DAL.Token.Current.Username);
		}
	}
}