using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class LPScoreBrand : Syncable<Model.LPScoreBrand, Model.LPScoreBrandTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPScoreBrand");
		}

		protected override TopSpaceMAUI.Model.LPScoreBrand ConvertTempToEntity (TopSpaceMAUI.Model.LPScoreBrandTemp temp)
		{
			return new Model.LPScoreBrand () {
				LPScoreBrandID = temp.LPScoreBrandID,
				BrandID = temp.BrandID,
				MetricID = temp.MetricID,
				StartPeriod = temp.StartPeriod,
				EndPeriod = temp.EndPeriod,
				Score = temp.Score,
                TagID = temp.TagID
            };
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.LPScoreBrand local, TopSpaceMAUI.Model.LPScoreBrand remote)
		{
            return local.LPScoreBrandID == remote.LPScoreBrandID &&
            local.TagID == remote.TagID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.LPScoreBrand local, TopSpaceMAUI.Model.LPScoreBrand remote)
		{
			return local.BrandID != remote.BrandID ||
			local.MetricID != remote.MetricID ||
			local.StartPeriod != remote.StartPeriod ||
			local.EndPeriod != remote.EndPeriod || 
			local.Score != remote.Score;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.LPScoreBrand> OrderBy (IEnumerable<TopSpaceMAUI.Model.LPScoreBrand> source)
		{
			return source.OrderBy (b => b.StartPeriod);
		}

  //      protected override void CreateTableTemp(SQLite.SQLiteConnection db)
  //      {
  //          db.Execute("CREATE TABLE IF NOT EXISTS LPScoreBrandTemp (LPScoreBrandID integer, BrandID integer, MetricID integer, StartPeriod text, EndPeriod text, Score integer, TagID integer, PRIMARY KEY (LPScoreBrandID, TagID))");
  //      }

  //      protected override void DropTableTemp(SQLite.SQLiteConnection db)
  //      {
  //          db.Execute("DROP TABLE IF EXISTS LPScoreBrandTemp");
  //      }

  //      public override List<Model.LPScoreBrand> GetAll (SQLite.SQLiteConnection db)
		//{
		//	string query = String.Format ("SELECT * FROM LPScoreBrand WHERE Datetime(StartPeriod) <= '{0}' AND Datetime(EndPeriod) >= '{0}'", DateTime.Now.ToPeriod().ToString ("yyyy-MM-dd HH:mm:ss"));            
  //          return db.Query<Model.LPScoreBrand> (query);
		//}
	}
}
