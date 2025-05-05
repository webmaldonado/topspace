using System;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class POSObjectiveCount
	{
		public POSObjectiveCount ()
		{
		}



		public static void Save (List<Model.POSObjectiveCount> counts, SQLiteConnection db)
		{
			db.DropTable<Model.POSObjectiveCount> ();
			db.CreateTable<Model.POSObjectiveCount> ();
			db.InsertAll (counts);
		}



		public static Dictionary<string, int> GetAll (SQLiteConnection db)
		{
            Dictionary<string, int> counts = new Dictionary<string, int> ();

			try {
				foreach (var poc in db.Table<Model.POSObjectiveCount>()) {
					counts.Add (poc.POSCode, poc.ObjectiveCount);
					poc.Dispose ();
				}
				return counts;
			}
			catch {
			}
			finally {
				counts = null;
			}

			return null;
		}



		public static Dictionary<string, int> GetAll ()
		{
			SQLiteConnection db = null;

			try {
				db = Database.GetNewConnection ();
				return GetAll (db);
			}
			catch {
			}
			finally {
				Database.Close (db);
				db = null;
			}

			return null;
		}
	}
}