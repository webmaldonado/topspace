using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("FileDepot")]
	public class FileDepot : IDisposable
	{
		[PrimaryKey, Column("FileID")]
		public string FileID { get; set; }

		[Column("FullName"), MaxLength(250)]
		public string FullName { get; set; }

		[Column("ActionCode"), MaxLength(100)]
		public string ActionCode { get; set; } 

		[Column("FileSize")]
		public int FileSize { get; set; } 

		[Column("FileMD5"), MaxLength(50)]
		public string FileMD5 { get; set; }

		[Column("DepotVersion"), MaxLength(50)]
		public string DepotVersion { get; set; }

		[Column("CategoryDepot"), MaxLength(50)]
		public string CategoryDepot { get; set; }

		public void Dispose ()
		{
			FullName = ActionCode = FileMD5 = DepotVersion = null;
		}
	}

	[Table("FileDepotTemp")]
	public class FileDepotTemp : FileDepot
	{
	}
}

