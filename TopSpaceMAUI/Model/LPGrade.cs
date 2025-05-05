using System;
using System.Collections.Generic;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table ("LPGrade")]
	public class LPGrade : IDisposable
	{
		[Column ("Name"), MaxLength (20)] //PrimaryKey
		public string Name { get; set; }

		[Column ("ScoreMin")]
		public int ScoreMin { get; set; }

		[Column ("ScoreMax")]
		public int ScoreMax { get; set; }

		[Column ("StartPeriod")] //PrimaryKey
		public string StartPeriod { get; set; }

		[Column ("EndPeriod")]
		public string EndPeriod { get; set; }

        [Column("TagID")] //PrimaryKey
        public int TagID { get; set; }

		public void Dispose ()
		{
			Name = StartPeriod = EndPeriod = null;
		}
	}

	[Table ("LPGradeTemp")]
	public class LPGradeTemp : LPGrade
	{
	}

	public class LPGradeComparer : IEqualityComparer<LPGrade>
	{
		public bool Equals (LPGrade x, LPGrade y)
		{
			return x.Name == y.Name && x.StartPeriod == y.StartPeriod && x.TagID == y.TagID;
		}

		public int GetHashCode (LPGrade obj)
		{
            int hCode = obj.Name.GetHashCode() ^ obj.StartPeriod.GetHashCode() ^ obj.TagID.GetHashCode();
			return hCode.GetHashCode ();
		}
	}
}