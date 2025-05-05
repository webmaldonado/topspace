using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class Quiz : Syncable<Model.Quiz, Model.QuizTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuiz");
		}



		protected override TopSpaceMAUI.Model.Quiz ConvertTempToEntity (TopSpaceMAUI.Model.QuizTemp temp)
		{
			return new Model.Quiz () {
				QuizID = temp.QuizID,
				QuizTypeID = temp.QuizTypeID,
				Question = temp.Question
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.Quiz local, TopSpaceMAUI.Model.Quiz remote)
		{
			return local.QuizID == remote.QuizID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.Quiz local, TopSpaceMAUI.Model.Quiz remote)
		{
			return local.QuizTypeID != remote.QuizTypeID ||
			local.Question != remote.Question;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.Quiz> OrderBy (IEnumerable<TopSpaceMAUI.Model.Quiz> source)
		{
			return source.OrderBy (b => b.QuizID);
		}
	}
}