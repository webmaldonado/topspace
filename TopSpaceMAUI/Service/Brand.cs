using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class Brand : Syncable<Model.BrandTemp, DAL.Brand>
	{
		public Brand (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityBrand");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return TopSpaceMAUI.Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (TopSpaceMAUI.Config.URL_API_REQUEST_BRANDS, DAL.Token.Current.Username);
		}
	}
}