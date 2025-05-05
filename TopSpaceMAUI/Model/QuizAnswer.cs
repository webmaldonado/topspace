using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("QuizAnswer")]
	public class QuizAnswer : IDisposable
	{
		[PrimaryKey, Column("QuizAnswerID")]
		public int QuizAnswerID { get; set; }

        [Column("QuizID")]
        public int QuizID { get; set; }

        [Column("Answer"), MaxLength(500)]
		public string Answer { get; set; }

        [Column("REPUsername"), MaxLength(500)]
        public string REPUsername { get; set; }	

        [Column("CreationDate")]
        public string CreationDate { get; set; }

        public void Dispose ()
		{
            Answer = null;
		}
	}

	[Table("QuizAnswerTemp")]
	public class QuizAnswerTemp : QuizAnswer
    {
	}
}