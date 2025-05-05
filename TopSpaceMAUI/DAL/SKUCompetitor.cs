using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class SKUCompetitor : Syncable<Model.SKUCompetitor, Model.SKUCompetitorTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntitySKUCompetitor");
		}



		protected override TopSpaceMAUI.Model.SKUCompetitor ConvertTempToEntity (TopSpaceMAUI.Model.SKUCompetitorTemp temp)
		{
			return new Model.SKUCompetitor () {
				SKUCompetitorID = temp.SKUCompetitorID,
				Name = temp.Name
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.SKUCompetitor local, TopSpaceMAUI.Model.SKUCompetitor remote)
		{
			return local.SKUCompetitorID == remote.SKUCompetitorID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.SKUCompetitor local, TopSpaceMAUI.Model.SKUCompetitor remote)
		{
			return local.Name != remote.Name;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.SKUCompetitor> OrderBy (IEnumerable<TopSpaceMAUI.Model.SKUCompetitor> source)
		{
			return source.OrderBy (s => s.Name);
		}
	}
}