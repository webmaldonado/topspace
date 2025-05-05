using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class TagPresenca : Syncable<Model.TagPresenca, Model.TagPresencaTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagPresenca");
		}

		protected override TopSpaceMAUI.Model.TagPresenca ConvertTempToEntity (TopSpaceMAUI.Model.TagPresencaTemp temp)
		{
			return new Model.TagPresenca() {
                TagPresencaID = temp.TagPresencaID,
                TagID = temp.TagID,
                BrandID = temp.BrandID,
                SKUID = temp.SKUID,
                MetricTypeCode = temp.MetricTypeCode
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.TagPresenca local, TopSpaceMAUI.Model.TagPresenca remote)
		{
			return local.TagPresencaID == remote.TagPresencaID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.TagPresenca local, TopSpaceMAUI.Model.TagPresenca remote)
		{
			return local.TagID != remote.TagID ||
                local.BrandID != remote.BrandID ||
                local.SKUID != remote.SKUID ||
                local.MetricTypeCode != remote.MetricTypeCode;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.TagPresenca> OrderBy (IEnumerable<TopSpaceMAUI.Model.TagPresenca> source)
		{
			return source.OrderBy (o => o.TagPresencaID);
		}
	}
}
