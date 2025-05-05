using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("SKU")]
	public class SKU : IDisposable
	{
		[PrimaryKey, Column("SKUID")]
		public int SKUID { get; set; }

		[Column("BrandID")]
		public int BrandID { get; set; }

		[Column("Name"), MaxLength(50)]
		public string Name { get; set; }

		[Column("Order")]
		public int Order { get; set; }

		[Column("TrackPrice")]
		public int TrackPrice { get; set; }

        [Column("QtdMin")]
        public int QtdMin { get; set; }

        public void Dispose ()
		{
			Name = null;
		}
	}

	[Table("SKUTemp")]
	public class SKUTemp : SKU
	{
	}
}

