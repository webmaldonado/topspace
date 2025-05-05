using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class QuizPOS : Syncable<Model.QuizPOSTemp, DAL.QuizPOS>
	{
		public QuizPOS(string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizPOS");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_QUIZ_POS, DAL.Token.Current.Username);
		}
	}
}