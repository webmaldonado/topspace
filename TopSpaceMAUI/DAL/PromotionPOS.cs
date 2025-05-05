using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class PromotionPOS : Syncable<Model.PromotionPOS, Model.PromotionPOSTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPromotionPOS");
		}



		protected override TopSpaceMAUI.Model.PromotionPOS ConvertTempToEntity (TopSpaceMAUI.Model.PromotionPOSTemp temp)
		{
			return new Model.PromotionPOS()
			{
				PromotionPOSID = temp.PromotionPOSID,
				PromotionID = temp.PromotionID,
				POSCode = temp.POSCode
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.PromotionPOS local, TopSpaceMAUI.Model.PromotionPOS remote)
		{
			return local.PromotionPOSID == remote.PromotionPOSID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.PromotionPOS local, TopSpaceMAUI.Model.PromotionPOS remote)
		{
			return	local.PromotionPOSID != remote.PromotionPOSID ||
					local.PromotionID != remote.PromotionID ||
					local.POSCode != remote.POSCode;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.PromotionPOS> OrderBy (IEnumerable<TopSpaceMAUI.Model.PromotionPOS> source)
		{
			return source.OrderBy (b => b.PromotionID);
		}
	}
}