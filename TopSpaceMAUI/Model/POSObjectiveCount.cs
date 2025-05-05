using System;
using TopSpaceMAUI.Util;
using SQLite;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	[Table ("POSObjectiveCount")]
	public class POSObjectiveCount : IDisposable
	{
		protected static Dictionary<string, POSObjectiveCount> _counts = null;


		[Ignore]
		public static Dictionary<string, POSObjectiveCount> Counts {
			get {
				if (_counts == null)
					Counts = new Dictionary<string, POSObjectiveCount> ();
				return _counts;
			}
			protected set {
				if (_counts != null) {
					foreach (POSObjectiveCount poc in _counts.Values)
						poc.Dispose ();
					_counts.Clear ();
				}
				_counts = value;
			}
		}



		public static void ResetCounts ()
		{
			Counts = null;
		}



		public static void IncrementCount (string posCode)
		{
			POSObjectiveCount poc = null;

			if (!Counts.TryGetValue (posCode, out poc))
				Counts.Add (posCode, (poc = new POSObjectiveCount (posCode)));

			poc.ObjectiveCount++;
		}



		public POSObjectiveCount () : base ()
		{
			POSCode = null;
			ObjectiveCount = 0;
		}



		public POSObjectiveCount (string posCode) : base ()
		{
			POSCode = posCode;
		}



		[PrimaryKey, Column ("POSCode"), MaxLength (20)]
		public string POSCode { get; set; }



		[Column ("ObjectiveCount")]
		public int ObjectiveCount { get; set; }



		public void Dispose ()
		{
			POSCode = null;
		}
	}
}