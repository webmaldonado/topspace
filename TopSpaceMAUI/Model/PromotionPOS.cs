using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("PromotionPOS")]
	public class PromotionPOS : IDisposable
	{
		[PrimaryKey, Column("PromotionPOSID")]
		public int PromotionPOSID { get; set; }

        [Column("PromotionID")]
        public int PromotionID { get; set; }

        [Column("POSCode")]
        public string POSCode { get; set; }

        public void Dispose ()
		{
			
		}
	}

	[Table("PromotionPOSTemp")]
	public class PromotionPOSTemp : PromotionPOS
    {
	}
}