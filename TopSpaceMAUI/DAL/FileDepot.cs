using System;
using System.Linq;
using TopSpaceMAUI.Util;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class FileDepot : Syncable<Model.FileDepot, Model.FileDepotTemp>
	{
		public const string ACTION_CODE_MODIFIED = "MODIFIED";
		public const string ACTION_CODE_DELETED = "DELETED";

		public FileDepot () : base ()
		{
			DeleteRecordCeasedToExists = false;
		}

		public FileDepot(string category) : base ()
		{
			DeleteRecordCeasedToExists = false;
			Category = category;
		}

		public string Category { get; set; }

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityFileDepot");
		}

		protected override void BeforeSaveTempInsertAll (List<TopSpaceMAUI.Model.FileDepotTemp> temp)
		{
			temp.ForEach (f => f.CategoryDepot = Category);
			base.BeforeSaveTempInsertAll (temp);
		}
			
		protected override TopSpaceMAUI.Model.FileDepot ConvertTempToEntity (TopSpaceMAUI.Model.FileDepotTemp temp)
		{
			return new Model.FileDepot () {
				FileID = temp.FileID,
				ActionCode = temp.ActionCode,
				CategoryDepot = temp.CategoryDepot,
				DepotVersion = temp.DepotVersion,
				FileMD5 = temp.FileMD5,
				FileSize = temp.FileSize,
				FullName = temp.FullName
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.FileDepot local, TopSpaceMAUI.Model.FileDepot remote)
		{
			return local.FullName == remote.FullName && local.CategoryDepot == remote.CategoryDepot;
		}

		public List<TopSpaceMAUI.Model.FileDepot> GetAllModified ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.FileDepot> lstFileDepotModified = db.Table<Model.FileDepot> ().Where(f => ACTION_CODE_MODIFIED.Equals(f.ActionCode)).ToList();
			Database.Close (db);
			db = null;
			return lstFileDepotModified;
		}

		public List<TopSpaceMAUI.Model.FileDepot> GetAllRemoved (SQLiteConnection db)
		{
			List<Model.FileDepot> lstFileDepotRemoved = db.Table<Model.FileDepot> ().Where(f => ACTION_CODE_DELETED.Equals(f.ActionCode)).ToList();
			return lstFileDepotRemoved;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.FileDepot local, TopSpaceMAUI.Model.FileDepot remote)
		{
			return 	local.FileMD5 != remote.FileMD5 ||
					local.ActionCode != remote.ActionCode ||
					local.DepotVersion != remote.DepotVersion ||
					local.FileSize != remote.FileSize;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.FileDepot> OrderBy (IEnumerable<TopSpaceMAUI.Model.FileDepot> source)
		{
			return source.OrderBy (f => f.CategoryDepot ).ThenBy(f =>f.DepotVersion).ThenBy(f =>f.FullName);
		}

		protected override void DropTableTemp (SQLiteConnection db)
		{
			string LastSync = db.Table<Model.FileDepot> ().Where(f => f.CategoryDepot.Equals(Category)).Max(f => f.DepotVersion);
			DateTime? LastDateSync = String.IsNullOrEmpty(LastSync) ? (DateTime?)null : Convert.ToDateTime(LastSync);

			if (!string.IsNullOrEmpty (LastSync)) {
				XNSUserDefaults.SetStringValueForKey (Config.KEY_NEWS_DATE_SYNC, LastDateSync.Value.ToString ("yyyy-MM-dd H:mm:ss.fff"));
			}

			base.DropTableTemp (db);
		}

		public override void Delete (TopSpaceMAUI.Model.FileDepot e, SQLiteConnection db)
		{
			string query = String.Format("DELETE FROM FileDepot WHERE FileID = '{0}'", e.FileID);
			db.Execute (query);
		}
	}
}

