using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class SKU : Syncable<Model.SKU, Model.SKUTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntitySKU");
		}



		protected override TopSpaceMAUI.Model.SKU ConvertTempToEntity (TopSpaceMAUI.Model.SKUTemp temp)
		{
			return new Model.SKU () {
				SKUID = temp.SKUID,
				BrandID = temp.BrandID,
				Name = temp.Name,
				Order = temp.Order,
				TrackPrice = temp.TrackPrice,
				QtdMin = temp.QtdMin
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.SKU local, TopSpaceMAUI.Model.SKU remote)
		{
			return local.SKUID == remote.SKUID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.SKU local, TopSpaceMAUI.Model.SKU remote)
		{
			return local.BrandID != remote.BrandID ||
			local.Name != remote.Name ||
			local.Order != remote.Order ||
			local.TrackPrice != remote.TrackPrice ||
			local.QtdMin != remote.QtdMin;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.SKU> OrderBy (IEnumerable<TopSpaceMAUI.Model.SKU> source)
		{
			return source.OrderBy (s => s.Order);
		}
	}
}