using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class QuizOption : Syncable<Model.QuizOptionTemp, DAL.QuizOption>
	{
		public QuizOption(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizOption");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_QUIZ_OPTION, DAL.Token.Current.Username);
		}
	}
}