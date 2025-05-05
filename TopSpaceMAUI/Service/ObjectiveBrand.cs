using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class ObjectiveBrand : Syncable<Model.ObjectiveBrandTemp, DAL.ObjectiveBrand>
	{
		public ObjectiveBrand (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityObjectiveBrand");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_OBJECTIVE_BRANDS, DAL.Token.Current.Username);
		}
	}
}