using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class ImgLib : Syncable<Model.ImgLibTemp, DAL.ImgLib>
	{
		public string Category { get; set; }

		public ImgLib (string category, string whenUtc) : base (whenUtc)
		{
			Category = category;
		}

		public ImgLib (string whenUtc) : base (whenUtc)
		{
			Category = "BCO_IMG";
		}

		public override TopSpaceMAUI.DAL.ImgLib GetInstanceOfDAL ()
		{
			return new TopSpaceMAUI.DAL.ImgLib (Category);
		}


		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityFileDepot");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			if (Category.Equals (Config.URL_API_MODULO_IMG_LIB)) {
				return String.Format (Config.URL_API_REQUEST_IMG_LIB_GET_CHANGES, Category, XNSUserDefaults.GetStringForKey (Config.KEY_IMG_LIB_DATE_SYNC));
			} else if (Category.Equals (Config.URL_API_MODULO_POS_MAT)) {
				return String.Format (Config.URL_API_REQUEST_IMG_LIB_GET_CHANGES, Category, XNSUserDefaults.GetStringForKey (Config.KEY_POS_MAT_DATE_SYNC));
			}

			return String.Format (Config.URL_API_REQUEST_IMG_LIB_GET_CHANGES, Category, XNSUserDefaults.GetStringForKey (Config.KEY_IMG_LIB_DATE_SYNC));
		}
	}
}

