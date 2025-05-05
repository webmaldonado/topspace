using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class QuizType : Syncable<Model.QuizTypeTemp, DAL.QuizType>
	{
		public QuizType(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizType");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_QUIZ_TYPE, DAL.Token.Current.Username);
		}
	}
}