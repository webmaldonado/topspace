using System;
using System.Linq;
using SQLite;
using System.Collections.Generic;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class LastVisit : Syncable<Model.LastVisit, Model.LastVisitTemp>
	{

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityLastVisit");
		}



		protected override TopSpaceMAUI.Model.LastVisit ConvertTempToEntity (TopSpaceMAUI.Model.LastVisitTemp temp)
		{
			return new Model.LastVisit () {
				POSCode = temp.POSCode,
				VisitDate = temp.VisitDate,
				Score = temp.Score
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.LastVisit local, TopSpaceMAUI.Model.LastVisit remote)
		{
			return local.POSCode == remote.POSCode &&
			local.VisitDate == remote.VisitDate;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.LastVisit local, TopSpaceMAUI.Model.LastVisit remote)
		{
			return local.Score != remote.Score;
		}



		protected override System.Linq.IOrderedEnumerable<TopSpaceMAUI.Model.LastVisit> OrderBy (System.Collections.Generic.IEnumerable<TopSpaceMAUI.Model.LastVisit> source)
		{
			return source.OrderBy (o => o.VisitDate);
		}



		protected override void CreateTableTemp (SQLiteConnection db)
		{
			db.Execute ("CREATE TABLE IF NOT EXISTS LastVisitTemp (POSCode varchar(20), VisitDate text, Score integer, PRIMARY KEY (POSCode, VisitDate))");
		}



		protected override void DropTableTemp (SQLiteConnection db)
		{
			db.Execute ("DROP TABLE IF EXISTS LastVisitTemp");
		}
			


		public override void Update (TopSpaceMAUI.Model.LastVisit e, SQLiteConnection db)
		{
			string query = String.Format("UPDATE LastVisit SET Score = {0} WHERE POSCode = '{1}' AND VisitDate = '{2}'", e.Score, e.POSCode, e.VisitDate);
			db.Execute (query);
		}



		public override void Delete (TopSpaceMAUI.Model.LastVisit e, SQLiteConnection db)
		{
			string query = String.Format("DELETE FROM LastVisit WHERE POSCode = '{0}' AND VisitDate = '{1}'", e.POSCode, e.VisitDate);
			db.Execute (query);
		}
	}
}

