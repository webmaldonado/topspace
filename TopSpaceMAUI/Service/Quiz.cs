using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class Quiz : Syncable<Model.QuizTemp, DAL.Quiz>
	{
		public Quiz (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuiz");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_QUIZ, DAL.Token.Current.Username);
		}
	}
}