using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("QuizType")]
	public class QuizType : IDisposable
	{
		[PrimaryKey, Column("QuizTypeID")]
		public int QuizTypeID { get; set; }

        [Column("Description"), MaxLength(500)]
		public string Description { get; set; }

		public void Dispose ()
		{
            Description = null;
		}
	}

	[Table("QuizTypeTemp")]
	public class QuizTypeTemp : QuizType
    {
	}
}