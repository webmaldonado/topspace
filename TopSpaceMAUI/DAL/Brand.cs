using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class Brand : Syncable<Model.Brand, Model.BrandTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityBrand");
		}



		protected override TopSpaceMAUI.Model.Brand ConvertTempToEntity (TopSpaceMAUI.Model.BrandTemp temp)
		{
			return new Model.Brand () {
				BrandID = temp.BrandID,
				Name = temp.Name,
				Style = temp.Style,
				CompetitorName = temp.CompetitorName,
				Order = temp.Order
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.Brand local, TopSpaceMAUI.Model.Brand remote)
		{
			return local.BrandID == remote.BrandID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.Brand local, TopSpaceMAUI.Model.Brand remote)
		{
			return local.Name != remote.Name ||
			local.Style != remote.Style ||
			local.CompetitorName != remote.CompetitorName ||
			local.Order != remote.Order;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.Brand> OrderBy (IEnumerable<TopSpaceMAUI.Model.Brand> source)
		{
			return source.OrderBy (b => b.Order);
		}
	}
}