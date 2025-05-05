using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("Promotion")]
	public class Promotion : IDisposable
	{
		[PrimaryKey, Column("PromotionID")]
		public int PromotionID { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Description")]
		public string Description { get; set; }

        [Column("BrandID")]
        public int BrandID { get; set; }

        [Column("SKUID")]
        public int SKUID { get; set; }

        public void Dispose ()
		{
			
		}
	}

	[Table("PromotionTemp")]
	public class PromotionTemp : Promotion
    {
	}
}