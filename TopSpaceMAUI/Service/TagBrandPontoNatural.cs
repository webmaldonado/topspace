using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class TagBrandPontoNatural : Syncable<Model.TagBrandPontoNaturalTemp, DAL.TagBrandPontoNatural>
	{
		public TagBrandPontoNatural(string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagBrandPontoNatural");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_TAG_BRAND_PONTO_NATURAL, DAL.Token.Current.Username);
		}
	}
}
