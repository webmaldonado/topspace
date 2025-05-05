using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("VisitDataTrackPrice")]
	public class VisitDataTrackPrice
	{
		[Column("POSCode"), MaxLength(20)] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("SKUID")] //PrimaryKey
		public int SKUID { get; set; }

		[Ignore][JsonIgnore]
		public string Name { get; set; }

		[Ignore][JsonIgnore]
		public decimal? LastVisitValue { get; set; }

		[Ignore][JsonIgnore]
		public string LastVisitDate { get; set; }

		[Column("Value")]
		public decimal? Value { get; set; }
	}
}
