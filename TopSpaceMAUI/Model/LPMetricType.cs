using System;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table ("LPMetricType")]
	public class LPMetricType : IDisposable
	{
		[Column ("MetricTypeCode"), MaxLength (50)] //PrimaryKey
		public string MetricTypeCode { get; set; }

		[Column ("Weight")]
		public decimal Weight { get; set; }

		[Column ("StartPeriod")] //PrimaryKey
		public string StartPeriod { get; set; }

		[Column ("EndPeriod")]
		public string EndPeriod { get; set; }

        [Column("TagID")]  //PrimaryKey
        public int TagID { get; set; }

        public void Dispose ()
		{
			MetricTypeCode = StartPeriod = EndPeriod = null;
		}
	}

	[Table ("LPMetricTypeTemp")]
	public class LPMetricTypeTemp : LPMetricType
	{
	}

	public class LPMetricTypeComparer : IEqualityComparer<LPMetricType>
	{
		public bool Equals (LPMetricType x, LPMetricType y)
		{
			return x.MetricTypeCode == y.MetricTypeCode && x.StartPeriod == y.StartPeriod && x.TagID == y.TagID;
		}

		public int GetHashCode (LPMetricType obj)
		{
            int hCode = obj.MetricTypeCode.GetHashCode() ^ obj.StartPeriod.GetHashCode() ^ obj.TagID.GetHashCode();
			return hCode.GetHashCode ();
		}
	}
}
