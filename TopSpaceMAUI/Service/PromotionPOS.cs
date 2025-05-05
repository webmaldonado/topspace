using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class PromotionPOS : Syncable<Model.PromotionPOSTemp, DAL.PromotionPOS>
	{
		public PromotionPOS(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPromotionPOS");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_PROMOTION_POS, DAL.Token.Current.Username);
		}
	}
}