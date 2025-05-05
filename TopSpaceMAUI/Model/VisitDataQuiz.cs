using System;
using SQLite;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	[Table("VisitDataQuiz")]
	public class VisitDataQuiz
	{
        [Column("POSCode"), MaxLength(20)]
        public string POSCode { get; set; }

        [Column("VisitDate")]
        public string VisitDate { get; set; }


        [Column("QuizID")]
		public int QuizID { get; set; }

        [Column("Type")]
        public int QuizTypeID { get; set; }

        [Column("Question")]
		public string Question { get; set; }

        [Column("AnswerValue")]
        public int AnswerValue { get; set; }

        [Column("Answer")]
        public string Answer { get; set; }

        [Column("REP")]
        public string REP { get; set; }


        [Ignore]
        [JsonIgnore]
        public List<QuizOption> QuizOptions { get; set; }

        [Ignore]
        [JsonIgnore]
        public List<QuizPOS> QuizPOS { get; set; }
    }
}