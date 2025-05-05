using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class LPGrade : Syncable<Model.LPGradeTemp, DAL.LPGrade>
	{
		public LPGrade (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText ("EntityLPGrade");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_LPGRADES, DAL.Token.Current.Username);
		}
	}
}
