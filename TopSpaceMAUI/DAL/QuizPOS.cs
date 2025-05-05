using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class QuizPOS : Syncable<Model.QuizPOS, Model.QuizPOSTemp>
	{
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityQuizPOS");
		}



		protected override TopSpaceMAUI.Model.QuizPOS ConvertTempToEntity (TopSpaceMAUI.Model.QuizPOSTemp temp)
		{
			return new Model.QuizPOS() {
                QuizPOSID = temp.QuizPOSID,
                QuizID = temp.QuizID,
                Sector = temp.Sector,
				POSCode = temp.POSCode
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.QuizPOS local, TopSpaceMAUI.Model.QuizPOS remote)
		{
			return local.QuizPOSID == remote.QuizPOSID;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.QuizPOS local, TopSpaceMAUI.Model.QuizPOS remote)
		{
			return local.QuizID != remote.QuizID ||
			local.Sector != remote.Sector ||
			local.POSCode != remote.POSCode;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.QuizPOS> OrderBy (IEnumerable<TopSpaceMAUI.Model.QuizPOS> source)
		{
			return source.OrderBy (b => b.QuizPOSID);
		}
	}
}