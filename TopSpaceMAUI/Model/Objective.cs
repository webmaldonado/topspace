using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	public class Objective
	{
		public int BrandID { get; set; }

		public string BrandName { get; set; }

		List<ObjectiveDetail> items = new List<ObjectiveDetail> ();
		public List<ObjectiveDetail> Items
		{
			get { return items; }
			set { items = value; }
		}

		public static int? TotalObjectives = null;
		public static Dictionary<string, int?> TotalObjectivesBrand = new Dictionary<string, int?> ();
		public static Dictionary<string, int?> TotalObjectivesMetricTypeShelf = new Dictionary<string, int?> ();
		public static Dictionary<string, int?> TotalObjectivesMetricTypeDisplay = new Dictionary<string, int?> ();

		public static Dictionary<string, bool> RefreshedMetricTypeShelf = new Dictionary<string, bool> ();
		public static Dictionary<Tuple<string, string>, bool> RefreshedMetricTypeOthers = new Dictionary<Tuple<string, string>, bool> ();

		public static Dictionary<string, int?> TotalExecutionContextBrand = new Dictionary<string, int?> ();
	}

	public class ObjectiveDetail 
	{
		public decimal Objective { get; set; }

		public int MetricID { get; set; }

		public string MetricName { get; set; }

		public string MetricType { get; set; }

		public int Score { get; set; }
	}
}