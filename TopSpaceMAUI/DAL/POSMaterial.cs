using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class POSMaterial : Syncable<Model.POSMaterial, Model.POSMaterialTemp>
	{

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOSMaterial");
		}
			
		protected override TopSpaceMAUI.Model.POSMaterial ConvertTempToEntity (TopSpaceMAUI.Model.POSMaterialTemp temp)
		{
			return new Model.POSMaterial () {
				ItemID = temp.ItemID,
				Month = temp.Month,
				Quantity = temp.Quantity
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.POSMaterial local, TopSpaceMAUI.Model.POSMaterial remote)
		{
			return local.ItemID == remote.ItemID &&
				local.Month == remote.Month;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.POSMaterial local, TopSpaceMAUI.Model.POSMaterial remote)
		{
			return local.Quantity != remote.Quantity;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.POSMaterial> OrderBy (IEnumerable<TopSpaceMAUI.Model.POSMaterial> source)
		{
			return source.OrderBy (o => o.Title);
		}

		public override List<Model.POSMaterial> GetAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.POSMaterial> material = db.Table<Model.POSMaterial> ().ToList();
			Database.Close (db);
			db = null;
			return material;
		}

		public override void Insert(Model.POSMaterial material)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertOrReplace(material);
			Database.Close (db);
			db = null;
		}

		public override void Delete (TopSpaceMAUI.Model.POSMaterial e, SQLiteConnection db)
		{
			string query = String.Format("DELETE FROM POSMaterial WHERE ItemID = '{0}' AND Month = '{1}'", e.ItemID, e.Month);
			db.Execute (query);
		}

		public bool CheckDownloadData()
		{
			SQLiteConnection db = Database.GetNewConnection ();

			List<Model.POSMaterial> material = db.Table<Model.POSMaterial> ().ToList();
			if (material.Count > 0) {
				return false;
			}

			Database.Close (db);
			db = null;

			return true;
		}

		public void CleanUp(DateTime now)
		{
			SQLiteConnection db = Database.GetNewConnection ();

			now = now.AddMonths (-1);
			string pastMonth = new DateTime (now.Year, now.Month, 1).ToString("s");

			string queryNeeded = String.Format("DELETE FROM POSMaterial WHERE Month = '{0}'", pastMonth);
			db.Execute (queryNeeded);

			Database.Close (db);
			db = null;
		}
	}
}

