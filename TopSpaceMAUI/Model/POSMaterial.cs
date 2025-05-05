using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("POSMaterial")]
	public class POSMaterial : IDisposable
	{
		[Column("ItemID")] //PrimaryKey
		public string ItemID { get; set; }

		[Ignore][JsonIgnore]
		public string Title { get; set; }

		[Ignore][JsonIgnore]
		public string FileID { get; set; }

		[Column("Month")] //PrimaryKey
		public string Month { get; set; }

		[Column("Quantity")]
		public int Quantity { get; set; }

		public void Dispose ()
		{
			ItemID = Title = FileID = Month = null;
		}
	}

	[Table("POSMaterialTemp")]
	public class POSMaterialTemp : POSMaterial
	{
	}
}

