using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("Quiz")]
	public class Quiz : IDisposable
	{
		[PrimaryKey, Column("QuizID")]
		public int QuizID { get; set; }

        [Column("QuizTypeID")]
        public int QuizTypeID { get; set; }

        [Column("Question"), MaxLength(500)]
		public string Question { get; set; }

		public void Dispose ()
		{
			Question = null;
		}
	}

	[Table("QuizTemp")]
	public class QuizTemp : Quiz
	{
	}
}