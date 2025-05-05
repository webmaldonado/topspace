using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class POS : Syncable<Model.POSTemp, DAL.POS>
	{
		public POS (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOS");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_POSS, DAL.Token.Current.Username);            
        }
	}
}