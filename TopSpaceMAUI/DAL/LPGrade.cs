using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class LPGrade : Syncable<Model.LPGrade, Model.LPGradeTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPGrade");
		}

		protected override TopSpaceMAUI.Model.LPGrade ConvertTempToEntity (TopSpaceMAUI.Model.LPGradeTemp temp)
		{
			return new Model.LPGrade () {
				Name = temp.Name,
				ScoreMin = temp.ScoreMin,
				ScoreMax = temp.ScoreMax,
				StartPeriod = temp.StartPeriod,
				EndPeriod = temp.EndPeriod,
                TagID = temp.TagID
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.LPGrade local, TopSpaceMAUI.Model.LPGrade remote)
		{
			return local.Name == remote.Name &&
			local.StartPeriod == remote.StartPeriod &&
            local.TagID == remote.TagID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.LPGrade local, TopSpaceMAUI.Model.LPGrade remote)
		{
			return local.ScoreMin != remote.ScoreMin ||
			local.ScoreMax != remote.ScoreMax || 
			local.EndPeriod != remote.EndPeriod;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.LPGrade> OrderBy (IEnumerable<TopSpaceMAUI.Model.LPGrade> source)
		{
			return source.OrderBy (o => o.StartPeriod).ThenBy (o => o.ScoreMin);
		}

		protected override void CreateTableTemp (SQLiteConnection db)
		{
			db.Execute ("CREATE TABLE IF NOT EXISTS LPGradeTemp (Name varchar(20), ScoreMin integer, ScoreMax integer, StartPeriod text, EndPeriod text, TagID integer, PRIMARY KEY (Name, StartPeriod, TagID))");
		}

		protected override void DropTableTemp (SQLiteConnection db)
		{
			db.Execute ("DROP TABLE IF EXISTS LPGradeTemp");
		}

		public override List<TopSpaceMAUI.Model.LPGrade> GetAll (SQLiteConnection db)
		{
			string query = String.Format ("SELECT * FROM LPGrade WHERE Datetime(StartPeriod) <= '{0}' AND Datetime(EndPeriod) >= '{0}'", DateTime.Now.ToPeriod().ToString ("yyyy-MM-dd HH:mm:ss"));
			return db.Query<Model.LPGrade> (query);
		}

		public override void Update (TopSpaceMAUI.Model.LPGrade e, SQLiteConnection db)
		{
			string query = String.Format ("UPDATE LPGrade SET ScoreMin = {0}, ScoreMax = {1}, EndPeriod = '{2}' WHERE Name = '{3}' AND StartPeriod = '{4}' AND TagID = {5}", e.ScoreMin, e.ScoreMax, e.EndPeriod, e.Name, e.StartPeriod, e.TagID);
			db.Execute (query);
		}

		public override void Delete (TopSpaceMAUI.Model.LPGrade e, SQLiteConnection db)
		{
			string query = String.Format ("DELETE FROM LPGrade WHERE Name = '{0}' AND StartPeriod = '{1}' AND TagID = {2}", e.Name, e.StartPeriod, e.TagID);
			db.Execute (query);
		}

		public string GradeName (int grade)
		{
			string name = string.Empty;

			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.LPGrade> lstLPGrades = GetAll ();
			if (lstLPGrades != null) {
				var grades = lstLPGrades.Where (g => grade >= g.ScoreMin && grade <= g.ScoreMax).FirstOrDefault ();
				if (grades != null) {
					name = grades.Name;
				}
			}
			Database.Close (db);
			db = null;

			return name;
		}
	}
}
