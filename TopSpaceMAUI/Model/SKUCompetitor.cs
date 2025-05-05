using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("SKUCompetitor")]
	public class SKUCompetitor : IDisposable
	{
		[PrimaryKey, Column("SKUCompetitorID")]
		public int SKUCompetitorID { get; set; }

		[Column("Name"), MaxLength(50)]
		public string Name { get; set; }

		public void Dispose ()
		{
			Name = null;
		}
	}

	[Table("SKUCompetitorTemp")]
	public class SKUCompetitorTemp : SKUCompetitor
	{
	}
}

