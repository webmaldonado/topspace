using System;
using TopSpaceMAUI.DAL;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class VisitCount : Syncable<Model.VisitCountTemp, DAL.VisitCount>
	{
		public VisitCount (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityVisitCount");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_VISIT_COUNT, DAL.Token.Current.Username);
		}
	}
}

