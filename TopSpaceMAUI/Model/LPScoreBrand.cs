using System;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table ("LPScoreBrand")]
	public class LPScoreBrand : IDisposable
	{
		[PrimaryKey, Column ("LPScoreBrandID")]
		public string LPScoreBrandID { get; set; }

		[Column ("BrandID")]
		public int? BrandID { get; set; }

		[Column ("MetricID")]
		public int MetricID { get; set; }

		[Column ("StartPeriod")]
		public string StartPeriod { get; set; }

		[Column ("EndPeriod")]
		public string EndPeriod { get; set; }

		[Column ("Score")]
		public int Score { get; set; }

        [Column("TagID")]
        public int TagID { get; set; }

        public void Dispose ()
		{
			StartPeriod = EndPeriod = null;
		}
	}

	[Table ("LPScoreBrandTemp")]
	public class LPScoreBrandTemp : LPScoreBrand
	{
	}

    public class LPScoreBrandComparer : IEqualityComparer<LPScoreBrand>
    {
        public bool Equals(LPScoreBrand x, LPScoreBrand y)
        {
            return x.LPScoreBrandID == y.LPScoreBrandID && x.TagID == y.TagID;
        }

        public int GetHashCode(LPScoreBrand obj)
        {
            int hCode = obj.LPScoreBrandID.GetHashCode() ^ obj.TagID.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
