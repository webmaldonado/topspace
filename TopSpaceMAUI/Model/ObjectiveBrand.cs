using System;
using SQLite;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	[Table("ObjectiveBrand")]
	public class ObjectiveBrand : IDisposable
	{
		[Column("POSCode"), MaxLength(20)] //PrimaryKey
		public string POSCode { get; set; }

		[Column("MetricID")] //PrimaryKey
		public int MetricID { get; set; }

		[Column("BrandID")] //PrimaryKey
		public int BrandID { get; set; }

		[Column("Objective")]
		public decimal Objective { get; set; }

		[Column("DueDate")]
		public string DueDate { get; set; }

		public void Dispose ()
		{
			POSCode = DueDate = null;
		}
	}

	[Table("ObjectiveBrandTemp")]
	public class ObjectiveBrandTemp : ObjectiveBrand
	{
	}

	public class ObjectiveBrandComparer : IEqualityComparer<ObjectiveBrand>
	{
		public bool Equals(ObjectiveBrand x, ObjectiveBrand y)
		{
			return x.POSCode == y.POSCode && x.MetricID == y.MetricID && x.BrandID == y.BrandID;
		}

		public int GetHashCode(ObjectiveBrand obj)
		{
			int hCode = obj.POSCode.GetHashCode() ^ obj.MetricID.GetHashCode() ^ obj.BrandID.GetHashCode();
			return hCode.GetHashCode();
		}
	}
}