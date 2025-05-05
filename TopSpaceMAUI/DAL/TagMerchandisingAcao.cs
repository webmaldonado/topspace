using TopSpaceMAUI.Util;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
    public class TagMerchandisingAcao : Syncable<Model.TagMerchandisingAcao, Model.TagMerchandisingAcaoTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagMerchandisingAcao");
		}

		protected override TopSpaceMAUI.Model.TagMerchandisingAcao ConvertTempToEntity (TopSpaceMAUI.Model.TagMerchandisingAcaoTemp temp)
		{
			return new Model.TagMerchandisingAcao() {
                TagMerchandisingAcaoID = temp.TagMerchandisingAcaoID,
                TagID = temp.TagID,
                BrandID = temp.BrandID,
                MetricID = temp.MetricID,
                MetricTypeCode = temp.MetricTypeCode
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.TagMerchandisingAcao local, TopSpaceMAUI.Model.TagMerchandisingAcao remote)
		{
			return local.TagMerchandisingAcaoID == remote.TagMerchandisingAcaoID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.TagMerchandisingAcao local, TopSpaceMAUI.Model.TagMerchandisingAcao remote)
		{
            return local.TagID != remote.TagID ||
                local.BrandID != remote.BrandID ||
                local.MetricID != remote.MetricID ||
                local.MetricTypeCode != remote.MetricTypeCode;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.TagMerchandisingAcao> OrderBy (IEnumerable<TopSpaceMAUI.Model.TagMerchandisingAcao> source)
		{
			return source.OrderBy (o => o.TagMerchandisingAcaoID);
		}
	}
}
