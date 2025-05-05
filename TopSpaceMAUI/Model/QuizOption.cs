using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("QuizOption")]
	public class QuizOption : IDisposable
	{
		[PrimaryKey, Column("QuizOptionID")]
		public int QuizOptionID { get; set; }

        [Column("QuizID")]
        public int QuizID { get; set; }

        [Column("Option"), MaxLength(500)]
		public string Option { get; set; }

		public void Dispose ()
		{
            Option = null;
		}
	}

	[Table("QuizOptionTemp")]
	public class QuizOptionTemp : QuizOption
    {
	}
}