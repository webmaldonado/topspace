using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class FileDepot : Syncable<Model.FileDepotTemp, DAL.FileDepot>
	{
		public string Category { get; set; }

		public FileDepot (string category, string whenUtc) : base (whenUtc)
		{
			Category = category;
		}

		public FileDepot (string whenUtc) : base (whenUtc)
		{
		}

		public override TopSpaceMAUI.DAL.FileDepot GetInstanceOfDAL ()
		{
			return new TopSpaceMAUI.DAL.FileDepot (Category);
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
			return String.Format (Config.URL_API_REQUEST_FILE_DEPOT_GET_CHANGES, Category, XNSUserDefaults.GetStringForKey (Config.KEY_NEWS_DATE_SYNC));
		}
	}
}

