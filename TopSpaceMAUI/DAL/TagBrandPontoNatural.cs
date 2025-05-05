using TopSpaceMAUI.Util;
using System.Collections.Generic;
using System.Linq;

namespace TopSpaceMAUI.DAL
{
    public class TagBrandPontoNatural : Syncable<Model.TagBrandPontoNatural, Model.TagBrandPontoNaturalTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTagBrandPontoNatural");
		}

		protected override TopSpaceMAUI.Model.TagBrandPontoNatural ConvertTempToEntity (TopSpaceMAUI.Model.TagBrandPontoNaturalTemp temp)
		{
			return new Model.TagBrandPontoNatural() {
                TagBrandPontoNaturalID = temp.TagBrandPontoNaturalID,
                TagID = temp.TagID,
                BrandID = temp.BrandID,
                MetricTypeCode = temp.MetricTypeCode
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.TagBrandPontoNatural local, TopSpaceMAUI.Model.TagBrandPontoNatural remote)
		{
			return local.TagBrandPontoNaturalID == remote.TagBrandPontoNaturalID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.TagBrandPontoNatural local, TopSpaceMAUI.Model.TagBrandPontoNatural remote)
		{
            return local.TagID != remote.TagID ||
            local.BrandID != remote.BrandID ||
            local.MetricTypeCode != remote.MetricTypeCode;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.TagBrandPontoNatural> OrderBy (IEnumerable<TopSpaceMAUI.Model.TagBrandPontoNatural> source)
		{
			return source.OrderBy (o => o.TagBrandPontoNaturalID);
		}
	}
}
