using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("Metric")]
	public class Metric : IDisposable
	{
		[PrimaryKey, Column("MetricID")]
		public int MetricID { get; set; }

		//MetricTypeCode
		[Column("MetricType"), MaxLength(50)]
		public string MetricType { get; set; }

		[Column("Name"), MaxLength(50)]
		public string Name { get; set; } 

		[Column("Order")]
		public int Order { get; set; } 

		public void Dispose ()
		{
			MetricType = Name = null;
		}
	}

	[Table("MetricTemp")]
	public class MetricTemp : Metric
	{
	}
}