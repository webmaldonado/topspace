using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class LPMetricType : Syncable<Model.LPMetricType, Model.LPMetricTypeTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPMetricType");
		}

		protected override TopSpaceMAUI.Model.LPMetricType ConvertTempToEntity (TopSpaceMAUI.Model.LPMetricTypeTemp temp)
		{
			return new Model.LPMetricType () {
				MetricTypeCode = temp.MetricTypeCode,
				Weight = temp.Weight,
				StartPeriod = temp.StartPeriod,
				EndPeriod = temp.EndPeriod,
                TagID = temp.TagID
            };
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.LPMetricType local, TopSpaceMAUI.Model.LPMetricType remote)
		{
			return local.MetricTypeCode == remote.MetricTypeCode &&
			local.StartPeriod == remote.StartPeriod &&
            local.TagID == remote.TagID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.LPMetricType local, TopSpaceMAUI.Model.LPMetricType remote)
		{
			return local.Weight != remote.Weight ||
			local.EndPeriod != remote.EndPeriod;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.LPMetricType> OrderBy (IEnumerable<TopSpaceMAUI.Model.LPMetricType> source)
		{
			return source.OrderBy (o => o.StartPeriod).ThenBy (o => o.Weight);
		}

		protected override void CreateTableTemp (SQLiteConnection db)
		{
			db.Execute ("CREATE TABLE IF NOT EXISTS LPMetricTypeTemp (MetricTypeCode varchar(50), Weight float, StartPeriod text, EndPeriod text, TagID integer, PRIMARY KEY (MetricTypeCode, StartPeriod, TagID))");
		}

		protected override void DropTableTemp (SQLiteConnection db)
		{
			db.Execute ("DROP TABLE IF EXISTS LPMetricTypeTemp");
		}

		public override List<TopSpaceMAUI.Model.LPMetricType> GetAll (SQLiteConnection db)
		{
			string query = String.Format ("SELECT * FROM LPMetricType WHERE Datetime(StartPeriod) <= '{0}' AND Datetime(EndPeriod) >= '{0}'", DateTime.Now.ToPeriod().ToString ("yyyy-MM-dd HH:mm:ss"));
			return db.Query<Model.LPMetricType> (query);
		}

		public override void Update (TopSpaceMAUI.Model.LPMetricType e, SQLiteConnection db)
		{
			string query = String.Format ("UPDATE LPMetricType SET Weight = {0}, EndPeriod = '{1}' WHERE MetricTypeCode = '{2}' AND StartPeriod = '{3}' AND TagID = {4}", e.Weight, e.EndPeriod, e.MetricTypeCode, e.StartPeriod, e.TagID);
			db.Execute (query);
		}

		public override void Delete (TopSpaceMAUI.Model.LPMetricType e, SQLiteConnection db)
		{
			string query = String.Format ("DELETE FROM LPMetricType WHERE MetricTypeCode = '{0}' AND StartPeriod = '{1}' AND TagID = {2}", e.MetricTypeCode, e.StartPeriod, e.TagID);
			db.Execute (query);
		}

        public decimal GetGradeWeight(string metricTypeCode)
        {
            decimal weight = 0;

            SQLiteConnection db = Database.GetNewConnection();
            Model.LPMetricType LPMetricType = db.Table<Model.LPMetricType>().Where(m => m.MetricTypeCode == metricTypeCode).FirstOrDefault();
            if (LPMetricType != null)
            {
                weight = LPMetricType.Weight;
            }
            Database.Close(db);
            db = null;

            return weight;
        }

        public decimal GetGradeWeight (string metricTypeCode, int tagID)
		{
			decimal weight = 0;

			SQLiteConnection db = Database.GetNewConnection ();
			Model.LPMetricType LPMetricType = db.Table<Model.LPMetricType> ().Where (m => m.MetricTypeCode == metricTypeCode && m.TagID == tagID).FirstOrDefault ();
			if (LPMetricType != null) {
				weight = LPMetricType.Weight;
			}
			Database.Close (db);
			db = null;

			return weight;
		}
	}
}
