using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class TagType : Syncable<Model.TagTypeTemp, DAL.TagType>
	{
		public TagType(string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagType");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_TAG_TYPE, DAL.Token.Current.Username);
		}
	}
}
