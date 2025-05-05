using SQLite;
using System;

namespace TopSpaceMAUI.Model
{
    [Table("Tag")]
    public class Tag : IDisposable
    {
        [PrimaryKey, Column("TagID")]
        public int TagID { get; set; }

        [Column("Name"), MaxLength(50)]
        public string Name { get; set; }

        [Column("TagTypeID")]
        public int TagTypeID { get; set; }

        public void Dispose()
        {
            Name = null;
        }
    }

    [Table("TagTemp")]
    public class TagTemp : Tag
    {
    }
}
