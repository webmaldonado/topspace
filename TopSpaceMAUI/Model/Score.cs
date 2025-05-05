using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	public class Score
	{
		public static int ScoreTotal = 0;
		public static string ScoreName = string.Empty;
		public static Dictionary<Tuple<int, int>, string> ScoreColors = new Dictionary<Tuple<int, int>, string> ();
		public static Dictionary<Tuple<int, int>, int> ScoreVisitDataSKU = new Dictionary<Tuple<int, int>, int> ();
		public static Dictionary<Tuple<int, int>, int> ScoreVisitDataBrandDisplay = new Dictionary<Tuple<int, int>, int> ();
		public static Dictionary<Tuple<int, int>, int> ScoreVisitDataBrandShelf = new Dictionary<Tuple<int, int>, int> ();
		public static Dictionary<Tuple<int, int>, int> ScoreVisitDataBrandAction = new Dictionary<Tuple<int, int>, int> ();
	}
}
