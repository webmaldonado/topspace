using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class QuizOption : Syncable<Model.QuizOption, Model.QuizOptionTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizOption");
		}



		protected override TopSpaceMAUI.Model.QuizOption ConvertTempToEntity (TopSpaceMAUI.Model.QuizOptionTemp temp)
		{
			return new Model.QuizOption() {
                QuizOptionID = temp.QuizOptionID,
                QuizID = temp.QuizID,
                Option = temp.Option
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.QuizOption local, TopSpaceMAUI.Model.QuizOption remote)
		{
			return local.QuizOptionID == remote.QuizOptionID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.QuizOption local, TopSpaceMAUI.Model.QuizOption remote)
		{
			return local.QuizID != remote.QuizID ||
			local.Option != remote.Option;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.QuizOption> OrderBy (IEnumerable<TopSpaceMAUI.Model.QuizOption> source)
		{
			return source.OrderBy (b => b.QuizOptionID);
		}
	}
}