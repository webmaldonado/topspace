using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
	public class VisitDataSKU
	{
		public List<Model.VisitDataSKU> GetVisitDataSKU()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.VisitDataSKU> visitDataSKU = db.Table<Model.VisitDataSKU> ().ToList();
			Database.Close (db);
			db = null;
			return visitDataSKU;
		}

		public void InsertAll (List<Model.VisitDataSKU> lstVisitDataSKU)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertAll (lstVisitDataSKU);
			Database.Close (db);
			db = null;
		}

		public void DeleteAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.VisitDataSKU>();
			Database.Close (db);
			db = null;
		}
	}
}

