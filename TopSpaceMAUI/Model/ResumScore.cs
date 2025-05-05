using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	public class ResumScore
	{
		public string MetricTypeName { get; set; }

		List<ResumScoreDetail> items = new List<ResumScoreDetail> ();
		public List<ResumScoreDetail> Items {
			get { return items; }
			set { items = value; }
		}
	}

	public class ResumScoreDetail
	{
		public string MetricType { get; set; }

		public int MetricID { get; set; }

		public int? TargetID { get; set; } // BrandID or SKUID

		public string TargetName { get; set; }

        public decimal Score { get; set; }

		public bool HasScore { get; set; }

	}
}
