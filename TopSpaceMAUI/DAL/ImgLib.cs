using System;
using System.Linq;
using TopSpaceMAUI.Util;
using System.Collections.Generic;
using SQLite;
using System.Globalization;

namespace TopSpaceMAUI.DAL
{
	public class ImgLib : Syncable<Model.ImgLib, Model.ImgLibTemp>
	{
		public const string ACTION_CODE_MODIFIED = "MODIFIED";
		public const string ACTION_CODE_DELETED = "DELETED";

		public ImgLib () : base ()
		{
			DeleteRecordCeasedToExists = false;

			//DeleteAll();

        }

		public ImgLib (string category) : base ()
		{
			DeleteRecordCeasedToExists = false;
			Category = category;
		}

		public string Category { get; set; }

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityFileDepot");
		}

		protected override TopSpaceMAUI.Model.ImgLib ConvertTempToEntity (TopSpaceMAUI.Model.ImgLibTemp temp)
		{
			if (!temp.Tags.Equals("BANNERAPP"))
			{
				return null;
			}

			var x = new Model.ImgLib()
            {
                ItemID = temp.ItemID,
                Title = temp.Title,
                Tags = temp.Tags,
                Brand = temp.Brand,
                FileID = temp.FileID,
                URLDownload = temp.URLDownload,
                URLThumb = temp.URLThumb,
                ActionCode = temp.ActionCode,
                CreationDate = temp.CreationDate
            };

			return x;
		}


		protected override bool KeyMatch (TopSpaceMAUI.Model.ImgLib local, TopSpaceMAUI.Model.ImgLib remote)
		{
			return local.ItemID == remote.ItemID;
		}

		public List<TopSpaceMAUI.Model.ImgLib> GetAllModified ()
		{
			SQLiteConnection db = Database.GetNewConnection ();

			string LastDateSync = string.Empty;
			if (Category.Equals (Config.URL_API_MODULO_IMG_LIB)) {
				LastDateSync = XNSUserDefaults.GetStringForKey (Config.KEY_IMG_LIB_DATE_SYNC);
			} else if (Category.Equals (Config.URL_API_MODULO_POS_MAT)) {
				LastDateSync = XNSUserDefaults.GetStringForKey (Config.KEY_POS_MAT_DATE_SYNC);
			}

			string query = string.Empty;
			if (!string.IsNullOrEmpty (LastDateSync)) {
				query = String.Format ("SELECT * FROM ImgLib WHERE Datetime(CreationDate) > strftime('%Y-%m-%d %H:%M:%S', '{0}') AND LibCode = '{1}'", LastDateSync, Category);
			} else {
				query = String.Format ("SELECT * FROM ImgLib WHERE LibCode = '{0}'", Category);
			}

			List<Model.ImgLib> lstImgLibModified = db.Query<Model.ImgLib>(query);
			Database.Close (db);
			db = null;
			return lstImgLibModified;
		}

		public List<TopSpaceMAUI.Model.ImgLib> GetAllRemoved (SQLiteConnection db)
		{
			List<Model.ImgLib> lstImgLibRemoved = db.Table<Model.ImgLib> ().Where (f => ACTION_CODE_DELETED.Equals (f.ActionCode)).ToList ();
			return lstImgLibRemoved;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.ImgLib local, TopSpaceMAUI.Model.ImgLib remote)
		{
			return local.Title != remote.Title ||
				local.Tags != remote.Tags ||
				local.Brand != remote.Brand ||
				local.FileID != remote.FileID ||
				local.URLDownload != remote.URLDownload ||
				local.URLThumb != remote.URLThumb ||
				local.ActionCode != remote.ActionCode;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.ImgLib> OrderBy (IEnumerable<TopSpaceMAUI.Model.ImgLib> source)
		{
			return source.OrderBy (f => f.Title);
		}

		public override void Delete (TopSpaceMAUI.Model.ImgLib e, SQLiteConnection db)
		{
			DeleteFile (e);

			string query = String.Format("DELETE FROM ImgLib WHERE ItemID = '{0}'", e.ItemID);
			db.Execute (query);

			// Apagar dados de POSMaterial
			string queryPOSMaterial = String.Format("DELETE FROM POSMaterial WHERE ItemID = '{0}'", e.ItemID);
			db.Execute (queryPOSMaterial);
		}

		public void DeleteFile (Model.ImgLib imgLib)
		{
			Util.FileSystem.DeleteFile (System.IO.Path.Combine(Category, imgLib.FileID + Config.EXTENSION_IMAGES_IMG_LIB));
		}

		public void DeleteAll(string LibCode = "") 
		{
			SQLiteConnection db = DAL.Database.GetNewConnection ();
			if (!string.IsNullOrEmpty(LibCode)) {
				string query = String.Format("DELETE FROM ImgLib WHERE LibCode = '{0}'", LibCode);
				db.Execute (query);
			} else {
				db.DeleteAll<ImgLib> ();
			}
			Database.Close (db);
			db = null;
		}
	}
}

