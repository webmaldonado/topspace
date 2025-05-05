using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("ExecutionContext")]
	public class ExecutionContext
	{
		[PrimaryKey, Column ("ExecutionContextID")]
		public int ExecutionContextID { get; set; }

		[Column ("Description")]
		public string Description { get; set; }
	}
}
