using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("VisitPhotoQueue")]
	public class VisitPhotoQueue
	{
		[Column("POSCode")] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("MetricID")] //PrimaryKey
		public int MetricID { get; set; }

		[Column("BrandID")] //PrimaryKey
		public int BrandID { get; set; }

		[Column("SKUID")] //PrimaryKey
		public int? SKUID { get; set; }

		[Column("PhotoID")] //PrimaryKey
		public int PhotoID { get; set; }

		[Column("PhotoDirectory")] [JsonIgnore]
		public string PhotoDirectory { get; set; }

		[Column("Photo")]
		public string Photo { get; set; }

		[Column("Category")] [JsonIgnore]
		public int Category { get; set; }

		[Column("SampleCategory")] [JsonIgnore]
		public int SampleCategory { get; set; }

		[Column("SampleVisit")] [JsonIgnore]
		public int SampleVisit { get; set; }
	}
}

