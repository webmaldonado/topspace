using TopSpaceMAUI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Model;

namespace TopSpaceMAUI.DAL
{
    public class TagType : Syncable<Model.TagType, Model.TagTypeTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityTagType");
		}

		protected override TopSpaceMAUI.Model.TagType ConvertTempToEntity (TopSpaceMAUI.Model.TagTypeTemp temp)
		{
			return new Model.TagType() {
                TagTypeID = temp.TagTypeID,
				Name = temp.Name
            };
		}

		protected override bool KeyMatch (TopSpaceMAUI.Model.TagType local, TopSpaceMAUI.Model.TagType remote)
		{
			return local.TagTypeID == remote.TagTypeID;
		}

		protected override bool HasChanged (TopSpaceMAUI.Model.TagType local, TopSpaceMAUI.Model.TagType remote)
		{
			return local.Name != remote.Name;
		}

        protected override IOrderedEnumerable<Model.TagType> OrderBy(IEnumerable<Model.TagType> source)
        {
            return source.OrderBy(m => m.Name);
        }
    }
}