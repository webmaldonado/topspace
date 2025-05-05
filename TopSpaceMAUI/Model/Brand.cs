using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("Brand")]
	public class Brand : IDisposable
	{
		[PrimaryKey, Column("BrandID")]
		public int BrandID { get; set; }

		[Column("Name"), MaxLength(50)]
		public string Name { get; set; }

		[Column("Style"), MaxLength(100)]
		public string Style { get; set; } 

		[Column("CompetitorName"), MaxLength(50)]
		public string CompetitorName { get; set; } 

		[Column("Order")]
		public int Order { get; set; } 

		public void Dispose ()
		{
			Name = Style = CompetitorName = null;
		}
	}

	[Table("BrandTemp")]
	public class BrandTemp : Brand
	{
	}
}