using System;
using TopSpaceMAUI.DAL;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LastVisit : Syncable<Model.LastVisitTemp, DAL.LastVisit>
	{
		public LastVisit (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityLastVisit");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LAST_VISIT, DAL.Token.Current.Username);
		}
	}
}

