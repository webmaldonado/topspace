using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class POSGps : Syncable<Model.POSGps, Model.POSGpsTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOSGps");
		}



		protected override TopSpaceMAUI.Model.POSGps ConvertTempToEntity (TopSpaceMAUI.Model.POSGpsTemp temp)
		{
			return new Model.POSGps()
			{
				ID = temp.ID,
				POSCode = temp.POSCode,
				Latitude = temp.Latitude,
				Longitude = temp.Longitude,
				Precision = temp.Precision
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.POSGps local, TopSpaceMAUI.Model.POSGps remote)
		{
			return local.ID == remote.ID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.POSGps local, TopSpaceMAUI.Model.POSGps remote)
		{
			return	local.ID != remote.ID ||
					local.POSCode != remote.POSCode ||
                    local.Latitude != remote.Latitude ||
					local.Longitude != remote.Longitude;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.POSGps> OrderBy (IEnumerable<TopSpaceMAUI.Model.POSGps> source)
		{
			return source.OrderBy (b => b.ID);
		}
	}
}