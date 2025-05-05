using System;
using SQLite;
using System.Collections.Generic;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Model
{
	[Table("ImgLib")]
	public class ImgLib : IDisposable
	{
		[PrimaryKey, Column("ItemID")]
		public string ItemID { get; set; }

		[Column("Title"), MaxLength(500)]
		public string Title { get; set; }

		[Ignore] 
		public string TitleWithoutAccent { 
			get { 
				return Title.RemoveAccents ().Replace ("-", " ").ToUpper ();
			}
			protected set { } 
		}

		[Column("Tags"), MaxLength(500)]
		public string Tags { get; set; }

		[Ignore] 
		public string TagsWithoutAccent { 
			get { 
				return Tags.RemoveAccents ().Replace ("-", " ").ToUpper ();
			}
			protected set { } 
		}
	
		[Column("Brand"), MaxLength(500)]
		public string Brand { get; set; }

		[Ignore] 
		public string BrandWithoutAccent { 
			get { 
				return Brand.RemoveAccents ().Replace ("-", " ").ToUpper ();
			}
			protected set { } 
		}

		[Ignore]
		public List<string> Brands { get; set; }

		[Column("FileID")]
		public string FileID { get; set; }

		[Column("URLDownload")]
		public string URLDownload { get; set; }

		[Column("URLThumb")]
		public string URLThumb { get; set; }

		[Column("ActionCode"), MaxLength(100)]
		public string ActionCode { get; set; } 

		[Column("CreationDate")]
		public string CreationDate { get; set; }

		[Column("LibCode")]
		public string LibCode { get; set; }

		public void Dispose ()
		{
			ItemID = Title = Tags = Brand = FileID = URLDownload = URLThumb = ActionCode = CreationDate = LibCode = null;
		}
	}

	[Table("ImgLibTemp")]
	public class ImgLibTemp : ImgLib
	{
	}
}

