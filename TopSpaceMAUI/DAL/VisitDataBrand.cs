using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
	public class VisitDataBrand
	{
		public List<Model.VisitDataBrand> GetVisitDataBrand()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.VisitDataBrand> visitDataBrand = db.Table<Model.VisitDataBrand> ().ToList();
			Database.Close (db);
			db = null;
			return visitDataBrand;
		}

		public void InsertAll (List<Model.VisitDataBrand> lstVisitDataBrand)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertAll (lstVisitDataBrand);
			Database.Close (db);
			db = null;
		}

		public void DeleteAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.VisitDataBrand>();
			Database.Close (db);
			db = null;
		}
	}
}

