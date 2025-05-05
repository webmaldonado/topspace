using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class Promotion : Syncable<Model.PromotionTemp, DAL.Promotion>
	{
		public Promotion(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPromotion");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_PROMOTION, DAL.Token.Current.Username);
		}
	}
}