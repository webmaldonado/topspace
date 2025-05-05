using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("VisitDataSKU")]
	public class VisitDataSKU
	{
		[Column("POSCode"), MaxLength(20)] //PrimaryKey
		public string POSCode { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }

		[Column("MetricID")] //PrimaryKey
		public int MetricID { get; set; }

		[Column("SKUID")] //PrimaryKey
		public int SKUID { get; set; }

		[Ignore][JsonIgnore]
		public string BrandName { get; set; }

		[Ignore][JsonIgnore]
		public string Name { get; set; }

		[Column("Value")]
		public decimal? Value { get; set; }

		[Column("Score")]
		public int Score { get; set; }

		[Ignore][JsonIgnore]
		public int? Grade { get; set; }

		[Ignore][JsonIgnore]
		public decimal? GradeWeight { get; set; }

        [Ignore]
        [JsonIgnore]
        public int QtdMinima { get; set; }
    }
}
