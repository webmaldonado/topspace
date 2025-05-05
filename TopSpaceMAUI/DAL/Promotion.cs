using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class Promotion : Syncable<Model.Promotion, Model.PromotionTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPromotion");
		}



		protected override TopSpaceMAUI.Model.Promotion ConvertTempToEntity (TopSpaceMAUI.Model.PromotionTemp temp)
		{
			return new Model.Promotion () {
				PromotionID = temp.PromotionID,
				Title = temp.Title,
				Description = temp.Description,
				BrandID = temp.BrandID,
				SKUID = temp.SKUID
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.Promotion local, TopSpaceMAUI.Model.Promotion remote)
		{
			return local.PromotionID == remote.PromotionID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.Promotion local, TopSpaceMAUI.Model.Promotion remote)
		{
			return local.PromotionID != remote.PromotionID ||
                local.Title != remote.Title ||
            local.Description != remote.Description;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.Promotion> OrderBy (IEnumerable<TopSpaceMAUI.Model.Promotion> source)
		{
			return source.OrderBy (b => b.PromotionID);
		}
	}
}