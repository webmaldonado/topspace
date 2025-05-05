using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class TagPresenca : Syncable<Model.TagPresencaTemp, DAL.TagPresenca>
	{
		public TagPresenca(string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagPresenca");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_TAG_PRESENCA, DAL.Token.Current.Username);
		}
	}
}
