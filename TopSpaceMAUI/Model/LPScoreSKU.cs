using System;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table ("LPScoreSKU")]
	public class LPScoreSKU : IDisposable
	{
		[PrimaryKey, Column("LPScoreSKUID")] //PrimaryKey
        public string LPScoreSKUID { get; set; }

		[Column ("SKUID")]
		public int SKUID { get; set; }

		[Column ("MetricID")]
		public int MetricID { get; set; }

		[Column ("StartPeriod")]
        public string StartPeriod { get; set; }

		[Column ("EndPeriod")]
		public string EndPeriod { get; set; }

		[Column ("Score")]
		public int Score { get; set; }

        [Column("TagID")] //PrimaryKey
        public int TagID { get; set; }

        public void Dispose ()
		{
			StartPeriod = EndPeriod = null;
		}
	}

	[Table ("LPScoreSKUTemp")]
	public class LPScoreSKUTemp : LPScoreSKU
	{
	}

    public class LPScoreSKUComparer : IEqualityComparer<LPScoreSKU>
    {
        public bool Equals(LPScoreSKU x, LPScoreSKU y)
        {
            return x.LPScoreSKUID == y.LPScoreSKUID && x.TagID == y.TagID;
        }

        public int GetHashCode(LPScoreSKU obj)
        {
            int hCode = obj.LPScoreSKUID.GetHashCode() ^ obj.TagID.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
