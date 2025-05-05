using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public class Tag : Syncable<Model.Tag, Model.TagTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityTag");
		}

		protected override TopSpaceMAUI.Model.Tag ConvertTempToEntity (TopSpaceMAUI.Model.TagTemp temp)
		{
			return new Model.Tag() {
                TagID = temp.TagID,
                Name = temp.Name,
                TagTypeID = temp.TagTypeID
			};
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.Tag local, TopSpaceMAUI.Model.Tag remote)
		{
			return local.TagID == remote.TagID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.Tag local, TopSpaceMAUI.Model.Tag remote)
		{
			return local.Name != remote.Name ||
			local.TagTypeID != remote.TagTypeID;
		}

		protected override IOrderedEnumerable<TopSpaceMAUI.Model.Tag> OrderBy (IEnumerable<TopSpaceMAUI.Model.Tag> source)
		{
			return source.OrderBy (o => o.Name);
		}
	}
}
