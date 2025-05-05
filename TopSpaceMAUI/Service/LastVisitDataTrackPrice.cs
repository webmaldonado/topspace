using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LastVisitDataTrackPrice: Syncable<Model.LastVisitDataTrackPriceTemp, DAL.LastVisitDataTrackPrice>
	{
		public LastVisitDataTrackPrice (string whenUtc) : base (whenUtc)
		{
		}

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityLastVisitDataTrackPrice");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LAST_VISIT_DATA_TRACK_PRICE, DAL.Token.Current.Username);
		}
	}
}

