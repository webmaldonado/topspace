using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class LPScoreSKU : Syncable<Model.LPScoreSKU, Model.LPScoreSKUTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPScoreSKU");
		}

		protected override TopSpaceMAUI.Model.LPScoreSKU ConvertTempToEntity (TopSpaceMAUI.Model.LPScoreSKUTemp temp)
		{
			return new Model.LPScoreSKU () {
				LPScoreSKUID = temp.LPScoreSKUID,
				SKUID = temp.SKUID,
				MetricID = temp.MetricID,
				StartPeriod = temp.StartPeriod,
				EndPeriod = temp.EndPeriod,
				Score = temp.Score,
                TagID = temp.TagID
            };
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.LPScoreSKU local, TopSpaceMAUI.Model.LPScoreSKU remote)
		{
			return local.LPScoreSKUID == remote.LPScoreSKUID &&
            local.TagID == remote.TagID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.LPScoreSKU local, TopSpaceMAUI.Model.LPScoreSKU remote)
		{
			return local.SKUID != remote.SKUID ||
			local.MetricID != remote.MetricID ||
			local.StartPeriod != remote.StartPeriod ||
			local.EndPeriod != remote.EndPeriod ||
			local.Score != remote.Score;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.LPScoreSKU> OrderBy (IEnumerable<TopSpaceMAUI.Model.LPScoreSKU> source)
		{
			return source.OrderBy (b => b.StartPeriod);
		}

        protected override void CreateTableTemp(SQLite.SQLiteConnection db)
        {
            db.Execute("CREATE TABLE IF NOT EXISTS LPScoreSKUTemp (LPScoreSKUID integer, SKUID integer, MetricID integer, StartPeriod text, EndPeriod text, Score integer, TagID integer, PRIMARY KEY (LPScoreSKUID, TagID))");
        }

        protected override void DropTableTemp(SQLite.SQLiteConnection db)
        {
            db.Execute("DROP TABLE IF EXISTS LPScoreSKUTemp");
        }

        public override List<Model.LPScoreSKU> GetAll (SQLite.SQLiteConnection db)
		{
			string query = String.Format ("SELECT * FROM LPScoreSKU WHERE Datetime(StartPeriod) <= '{0}' AND Datetime(EndPeriod) >= '{0}'", DateTime.Now.ToPeriod().ToString ("yyyy-MM-dd HH:mm:ss"));
			return db.Query<Model.LPScoreSKU> (query);
		}
	}
}
