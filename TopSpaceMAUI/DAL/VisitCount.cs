using System;
using System.Linq;
using SQLite;
using System.Collections.Generic;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class VisitCount : Syncable<Model.VisitCount, Model.VisitCountTemp>
	{

		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityVisitCount");
		}


		protected override Model.VisitCount ConvertTempToEntity (Model.VisitCountTemp temp)
		{
			return new Model.VisitCount () {
				POSCode = temp.POSCode,
				Count = temp.Count
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.VisitCount local, TopSpaceMAUI.Model.VisitCount remote)
		{
			return local.POSCode == remote.POSCode;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.VisitCount local, TopSpaceMAUI.Model.VisitCount remote)
		{
			return local.Count != remote.Count;
		}




		protected override IOrderedEnumerable<TopSpaceMAUI.Model.VisitCount> OrderBy (IEnumerable<TopSpaceMAUI.Model.VisitCount> source)
		{
			return source.OrderBy (b => b.POSCode);
		}

		public void DeleteAll ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.VisitCount>();
			Database.Close (db);
			db = null;
		}
	}
}

