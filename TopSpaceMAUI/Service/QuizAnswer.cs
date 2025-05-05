using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class QuizAnswer : Syncable<Model.QuizAnswerTemp, DAL.QuizAnswer>
	{
		public QuizAnswer(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizAnswer");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_QUIZ_ANSWER, DAL.Token.Current.Username);
		}
	}
}