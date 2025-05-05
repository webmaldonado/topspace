using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("MetricType")]
	public class MetricType : IDisposable
	{
		[PrimaryKey, Column("MetricTypeCode"), MaxLength(50)]
		public string MetricTypeCode { get; set; }

		[Column("Name"), MaxLength(50)]
		public string Name { get; set; } 

		[Column("Target"), MaxLength(20)]
		public string Target { get; set; }

		[Column("Order")]
		public int Order { get; set; } 

		public void Dispose ()
		{
			MetricTypeCode = Name = Target = null;
		}
	}

	[Table("MetricTypeTemp")]
	public class MetricTypeTemp : MetricType
	{
	}
}