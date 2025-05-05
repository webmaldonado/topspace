using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
	public class VisitDataTrackPrice
	{
		public List<Model.VisitDataTrackPrice> GetVisitDataTrackPrice()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.VisitDataTrackPrice> visitDataTrackPrice = db.Table<Model.VisitDataTrackPrice> ().ToList();
			Database.Close (db);
			db = null;
			return visitDataTrackPrice;
		}

		public void InsertAll(List<Model.VisitDataTrackPrice> lstVisitDataTrackPrice)
		{
            SQLiteConnection db = Database.GetNewConnection();
            DeleteAll();
            db.InsertAll(lstVisitDataTrackPrice);
            Database.Close(db);
            db = null;
        }

        public void DeleteAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.VisitDataTrackPrice>();
			Database.Close (db);
			db = null;
		}
	}
}

