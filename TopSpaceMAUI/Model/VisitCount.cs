using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("VisitCount")]
	public class VisitCount : IDisposable
	{
		[PrimaryKey, Column("POSCode")]
		public string POSCode { get; set; }

		private string _period;
		[Column ("Period")]
		public string Period {
			get {
				if (_period == null) {
					_period = new DateTime (DateTime.Now.Year, DateTime.Now.Month, 1).ToString("s");
				}
				return _period;
			}
			set { _period = value; }
		}

		[Column("Count")]
		public int Count { get; set; }

		public void Dispose ()
		{
			POSCode = null;
		}
	}

	[Table("VisitCountTemp")]
	public class VisitCountTemp : VisitCount
	{
	}
}

