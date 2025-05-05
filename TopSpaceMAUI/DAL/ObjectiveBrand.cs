using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class ObjectiveBrand : Syncable<Model.ObjectiveBrand, Model.ObjectiveBrandTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityObjectiveBrand");
		}



		protected override void AfterSaveTempInsertAll (List<TopSpaceMAUI.Model.ObjectiveBrandTemp> temp)
		{
			Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("NoteObjectiveCount"));
			DateTime today = DateTime.Today;
			foreach (var t in temp) {
				if (t.DueDate == null || Convert.ToDateTime(t.DueDate) >= today)
					Model.POSObjectiveCount.IncrementCount (t.POSCode);
			}
		}



		protected override TopSpaceMAUI.Model.ObjectiveBrand ConvertTempToEntity (TopSpaceMAUI.Model.ObjectiveBrandTemp temp)
		{
			return new Model.ObjectiveBrand () {
				POSCode = temp.POSCode,
				MetricID = temp.MetricID,
				BrandID = temp.BrandID,
				Objective = temp.Objective,
				DueDate = temp.DueDate
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.ObjectiveBrand local, TopSpaceMAUI.Model.ObjectiveBrand remote)
		{
			return local.POSCode == remote.POSCode &&
			local.MetricID == remote.MetricID &&
			local.BrandID == remote.BrandID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.ObjectiveBrand local, TopSpaceMAUI.Model.ObjectiveBrand remote)
		{
			return local.Objective != remote.Objective ||
			local.DueDate != remote.DueDate;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.ObjectiveBrand> OrderBy (IEnumerable<TopSpaceMAUI.Model.ObjectiveBrand> source)
		{
			return source.OrderBy (o => o.POSCode).ThenBy (o => o.MetricID).ThenBy (o => o.BrandID);
		}



		protected override void CreateTableTemp (SQLiteConnection db)
		{
			db.Execute ("CREATE TABLE ObjectiveBrandTemp (POSCode varchar(20), MetricID integer, BrandID integer, Objective float, DueDate text, PRIMARY KEY (POSCode, MetricID, BrandID))");
		}



		protected override void DropTableTemp (SQLiteConnection db)
		{
			db.Execute ("DROP TABLE IF EXISTS ObjectiveBrandTemp");
		}



		public override List<TopSpaceMAUI.Model.ObjectiveBrand> GetAll (SQLiteConnection db)
		{
			string query = String.Format("SELECT * FROM ObjectiveBrand WHERE DueDate IS NULL OR DueDate = '' OR Datetime(DueDate) >= '{0}'", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"));
			return db.Query<Model.ObjectiveBrand>(query);
		}



		public override void Update (TopSpaceMAUI.Model.ObjectiveBrand e, SQLiteConnection db)
		{
			string query = String.Format("UPDATE ObjectiveBrand SET Objective = {0}, DueDate = '{1}' WHERE POSCode = '{2}' AND MetricID = {3} AND BrandID = {4}", e.Objective, e.DueDate, e.POSCode, e.MetricID, e.BrandID);
			db.Execute (query);
		}



		public override void Delete (TopSpaceMAUI.Model.ObjectiveBrand e, SQLiteConnection db)
		{
			string query = String.Format("DELETE FROM ObjectiveBrand WHERE POSCode = '{0}' AND MetricID = {1} AND BrandID = {2}", e.POSCode, e.MetricID, e.BrandID);
			db.Execute (query);
		}
	}
}