using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class MetricType : Syncable<Model.MetricType, Model.MetricTypeTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityMetricType");
		}



		protected override TopSpaceMAUI.Model.MetricType ConvertTempToEntity (TopSpaceMAUI.Model.MetricTypeTemp temp)
		{
			return new Model.MetricType () {
				MetricTypeCode = temp.MetricTypeCode,
				Name = temp.Name,
				Target = temp.Target,
				Order = temp.Order
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.MetricType local, TopSpaceMAUI.Model.MetricType remote)
		{
			return local.MetricTypeCode == remote.MetricTypeCode;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.MetricType local, TopSpaceMAUI.Model.MetricType remote)
		{
			return local.Name != remote.Name ||
			local.Target != remote.Target ||
			local.Order != remote.Order;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.MetricType> OrderBy (IEnumerable<TopSpaceMAUI.Model.MetricType> source)
		{
			return source.OrderBy (m => m.Order);
		}
	}
}