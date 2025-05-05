using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("Visit")][JsonObject(Title = "arrVisit")]
	public class Visit
	{
		[Column("POSCode")] //PrimaryKey
		public string POSCode { get; set; }

		[Column("Category")][JsonIgnore]
		public int? Category { get; set; }

		[Column("VisitDate")] //PrimaryKey
		public string VisitDate { get; set; }
	
		[Column("Latitude")]
		public Nullable<decimal> Latitude { get; set; }

		[Column("Longitude")]
		public Nullable<decimal> Longitude { get; set; }

		[Column("Precision")]
		public Nullable<decimal> Precision { get; set; }

		[Column("Score")]
		public int Score { get; set; }

		[Column("PhotosDirectory")][JsonIgnore]
		public string PhotosDirectory { get; set; }

		[Column("DatabaseVersion")]
		public string DatabaseVersion { get; set; }

		[Column("PhotoTaken")]
		public int PhotoTaken { get; set; }

		[Column("Status")] [JsonIgnore]
		public string Status { get; set; }
        /* S = started, C = completed */

        //[Column("QualityCheck")]
        //public string QualityCheck { get; set; }

        [Ignore]
		public Model.VisitDataBrand[] arrVisitBrand { get; set; }

		[Ignore]
		public Model.VisitDataSKU[] arrVisitSKU { get; set; }

		[Ignore]
		public Model.VisitDataTrackPrice[] arrVisitTrackPrice { get; set; }

        [Ignore]
        public Model.VisitDataQuiz[] arrVisitQuiz{ get; set; }
    }
}
