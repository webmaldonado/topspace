using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class TagMerchandisingAcao : Syncable<Model.TagMerchandisingAcaoTemp, DAL.TagMerchandisingAcao>
	{
		public TagMerchandisingAcao(string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagMerchandisingAcao");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_TAG_MERCHANDISING_ACAO, DAL.Token.Current.Username);
		}
	}
}
