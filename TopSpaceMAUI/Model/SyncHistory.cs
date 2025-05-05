using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("SyncHistory")]
	public class SyncHistory
	{
		[PrimaryKey, Column ("Type")]
		public string Type { get; set; }

		[Column("Date")]
		public string Date { get; set; }
	}
}