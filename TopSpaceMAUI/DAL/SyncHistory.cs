using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;


namespace TopSpaceMAUI.DAL
{
	public class SyncHistory
	{
		public List<Model.SyncHistory> GetAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.SyncHistory> syncHistory = db.Table<Model.SyncHistory> ().ToList();
			Database.Close (db);
			db = null;
			return syncHistory;
		}

		public Model.SyncHistory Get (string type)
		{
			return GetAll ().Where (s => s.Type == type).FirstOrDefault ();
		}

		public void InsertOrReplace(Model.SyncHistory syncHistory)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertOrReplace(syncHistory);
			Database.Close (db);
			db = null;
		}
	}
}

