using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("LastVisit")]
	public class LastVisit : IDisposable
	{
		[Column("POSCode")] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("Score")]
		public int Score { get; set; }

		public void Dispose ()
		{
			POSCode =  VisitDate = null;
		}
	}

	[Table("LastVisitTemp")]
	public class LastVisitTemp : LastVisit
	{
	}
}

