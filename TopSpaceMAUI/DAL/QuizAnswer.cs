using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class QuizAnswer : Syncable<Model.QuizAnswer, Model.QuizAnswerTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizAnswer");
		}



		protected override TopSpaceMAUI.Model.QuizAnswer ConvertTempToEntity (TopSpaceMAUI.Model.QuizAnswerTemp temp)
		{
			return new Model.QuizAnswer() {
                QuizAnswerID = temp.QuizAnswerID,
                QuizID = temp.QuizID,
                Answer = temp.Answer,
				REPUsername = temp.REPUsername,
				CreationDate = temp.CreationDate
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.QuizAnswer local, TopSpaceMAUI.Model.QuizAnswer remote)
		{
			return local.QuizAnswerID == remote.QuizAnswerID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.QuizAnswer local, TopSpaceMAUI.Model.QuizAnswer remote)
		{
			return local.QuizID != remote.QuizID ||
			local.Answer != remote.Answer ||
			local.REPUsername != remote.REPUsername ||
			local.CreationDate != remote.CreationDate;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.QuizAnswer> OrderBy (IEnumerable<TopSpaceMAUI.Model.QuizAnswer> source)
		{
			return source.OrderBy (b => b.QuizAnswerID);
		}
	}
}