using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
	public class VisitDataQuiz
	{
		public List<Model.VisitDataQuiz> GetVisitDataQuiz()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.VisitDataQuiz> visitDataQuiz = db.Table<Model.VisitDataQuiz> ().ToList();
			Database.Close (db);
			db = null;
			return visitDataQuiz;
		}

		public void InsertAll (List<Model.VisitDataQuiz> lstVisitDataQuiz)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertAll (lstVisitDataQuiz);
			Database.Close (db);
			db = null;
		}

		public void DeleteAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.VisitDataQuiz>();
			Database.Close (db);
			db = null;
		}
	}
}

