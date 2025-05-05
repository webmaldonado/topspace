using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("POSGps")]
	public class POSGps : IDisposable
	{
		[PrimaryKey, Column("ID")]
		public int ID { get; set; }

        [Column("POSCode")]
        public string POSCode { get; set; }

        [Column("Latitude")]
        public decimal? Latitude { get; set; }

        [Column("Longitude")]
        public decimal? Longitude { get; set; }

        [Column("Precision")]
        public decimal? Precision { get; set; }

        public void Dispose ()
		{
			
		}
	}

	[Table("POSGpsTemp")]
	public class POSGpsTemp : POSGps
    {
	}
}