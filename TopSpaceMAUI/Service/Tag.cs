using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class Tag : Syncable<Model.TagTemp, DAL.Tag>
	{
		public Tag(string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTag");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_TAG, DAL.Token.Current.Username);
		}
	}
}
