using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("VisitDataBrand")]
	public class VisitDataBrand
	{
		[Column("POSCode"), MaxLength(20)] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("MetricID")] //PrimaryKey
		public int MetricID { get; set; }

		[Ignore][JsonIgnore]
		public string MetricType { get; set; }

		[Ignore][JsonIgnore]
		public string Name { get; set; }

		[Column("BrandID")] //PrimaryKey
		public int BrandID { get; set; }

		[Ignore][JsonIgnore]
		public string BrandName { get; set; }

		[Ignore][JsonIgnore]
		public decimal? Objective { get; set; }

		[Column("Value")]
		public decimal Value { get; set; }

		[Column("CompetitorValue")]
		public decimal? CompetitorValue { get; set; }

		[Column ("ExecutionContextID")]
		public int? ExecutionContextID { get; set; }

		[Ignore][JsonIgnore]
		public int? Grade { get; set; }

		[Ignore][JsonIgnore]
        public decimal? GradeWeight { get; set; }

		[Column("Score")]
		public int Score { get; set; }

		[Ignore][JsonIgnore]
		public string DocumentsDirectory { get; set; }
	}
}