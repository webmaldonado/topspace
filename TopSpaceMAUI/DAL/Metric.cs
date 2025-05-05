using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class Metric : Syncable<Model.Metric, Model.MetricTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityMetric");
		}



		protected override TopSpaceMAUI.Model.Metric ConvertTempToEntity (TopSpaceMAUI.Model.MetricTemp temp)
		{
			return new Model.Metric () {
				MetricID = temp.MetricID,
				MetricType = temp.MetricType,
				Name = temp.Name,
				Order = temp.Order
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.Metric local, TopSpaceMAUI.Model.Metric remote)
		{
			return local.MetricID == remote.MetricID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.Metric local, TopSpaceMAUI.Model.Metric remote)
		{
			return local.MetricType != remote.MetricType ||
			local.Name != remote.Name ||
			local.Order != remote.Order;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.Metric> OrderBy (IEnumerable<TopSpaceMAUI.Model.Metric> source)
		{
			return source.OrderBy (m => m.Order);
		}

        public string GetName(int metricID)
        {
            using (SQLiteConnection db = Database.GetNewConnection())
            {
                return db.Table<Model.Metric>().Where(m => m.MetricID == metricID).FirstOrDefault().Name;
            }
        }
    }
}