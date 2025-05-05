using System;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class LastVisitDataTrackPrice : Syncable<Model.LastVisitDataTrackPrice, Model.LastVisitDataTrackPriceTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityLastVisitDataTrackPrice");
		}

		protected override TopSpaceMAUI.Model.LastVisitDataTrackPrice ConvertTempToEntity (TopSpaceMAUI.Model.LastVisitDataTrackPriceTemp temp)
		{
			return new Model.LastVisitDataTrackPrice () {
				POSCode = temp.POSCode,
				VisitDate = temp.VisitDate,
				SKUID = temp.SKUID,
				Value = temp.Value
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.LastVisitDataTrackPrice local, TopSpaceMAUI.Model.LastVisitDataTrackPrice remote)
		{
			return local.POSCode == remote.POSCode &&
			local.VisitDate == remote.VisitDate &&
			local.SKUID == remote.SKUID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.LastVisitDataTrackPrice local, TopSpaceMAUI.Model.LastVisitDataTrackPrice remote)
		{
			return local.Value != remote.Value;
		}

		protected override System.Linq.IOrderedEnumerable<TopSpaceMAUI.Model.LastVisitDataTrackPrice> OrderBy (System.Collections.Generic.IEnumerable<TopSpaceMAUI.Model.LastVisitDataTrackPrice> source)
		{
			return source.OrderBy (o => o.VisitDate);
		}

		protected override void CreateTableTemp (SQLite.SQLiteConnection db)
		{
			db.Execute ("CREATE TABLE IF NOT EXISTS LastVisitDataTrackPriceTemp (POSCode varchar(20), VisitDate text, SKUID integer, Value float, PRIMARY KEY (POSCode, VisitDate, SKUID))");
		}

		protected override void DropTableTemp (SQLite.SQLiteConnection db)
		{
			db.Execute ("DROP TABLE IF EXISTS LastVisitDataTrackPriceTemp");
		}

		public override void Update (TopSpaceMAUI.Model.LastVisitDataTrackPrice e, SQLite.SQLiteConnection db)
		{
			string query = String.Format ("UPDATE LastVisitDataTrackPrice SET Value = {0} WHERE POSCode = '{1}' AND VisitDate = '{2}' AND SKUID = {3}", e.Value, e.POSCode, e.VisitDate, e.SKUID);
			db.Execute (query);
		}

		public override void Delete (TopSpaceMAUI.Model.LastVisitDataTrackPrice e, SQLite.SQLiteConnection db)
		{
			string query = String.Format ("DELETE FROM LastVisitDataTrackPrice WHERE POSCode = '{0}' AND VisitDate = '{1}' AND SKUID = {2}", e.POSCode, e.VisitDate, e.SKUID);	
			db.Execute (query);
		}
	}
}

