using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
	public class ExecutionContext
	{
		public List<Model.ExecutionContext> GetAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.ExecutionContext> executionContexts= db.Table<Model.ExecutionContext> ().ToList();
			Database.Close (db);
			db = null;
			return executionContexts;
		}
	}
}

