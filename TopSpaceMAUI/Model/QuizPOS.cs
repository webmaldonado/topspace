using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("QuizPOS")]
	public class QuizPOS : IDisposable
	{
		[PrimaryKey, Column("QuizPOSID")]
		public int QuizPOSID { get; set; }

        [Column("QuizID")]
        public int QuizID { get; set; }

        [Column("Sector"), MaxLength(20)]
		public string Sector { get; set; }

        [Column("POSCode"), MaxLength(40)]
        public string POSCode { get; set; }

        public void Dispose ()
		{
            Sector = POSCode = null;
		}
	}

	[Table("QuizPOSTemp")]
	public class QuizPOSTemp : QuizPOS
    {
	}
}