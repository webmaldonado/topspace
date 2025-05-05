using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("LastVisitDataTrackPrice")]
	public class LastVisitDataTrackPrice : IDisposable
	{
		[Column("POSCode")] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("SKUID")] //PrimaryKey
		public int SKUID { get; set; }

		[Column("Value")]
		public decimal Value { get; set; }

		public void Dispose ()
		{
			POSCode =  VisitDate = null;
		}
	}

	[Table("LastVisitDataTrackPriceTemp")]
	public class LastVisitDataTrackPriceTemp : LastVisitDataTrackPrice
	{
	}
}