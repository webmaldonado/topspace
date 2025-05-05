using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class QuizType : Syncable<Model.QuizType, Model.QuizTypeTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizType");
		}



		protected override TopSpaceMAUI.Model.QuizType ConvertTempToEntity (TopSpaceMAUI.Model.QuizTypeTemp temp)
		{
			return new Model.QuizType() {
                QuizTypeID = temp.QuizTypeID,
				Description = temp.Description
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.QuizType local, TopSpaceMAUI.Model.QuizType remote)
		{
			return local.QuizTypeID == remote.QuizTypeID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.QuizType local, TopSpaceMAUI.Model.QuizType remote)
		{
			return local.QuizTypeID != remote.QuizTypeID ||
			local.Description != remote.Description;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.QuizType> OrderBy (IEnumerable<TopSpaceMAUI.Model.QuizType> source)
		{
			return source.OrderBy (b => b.QuizTypeID);
		}
	}
}