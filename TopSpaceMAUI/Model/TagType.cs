using SQLite;
using System;
namespace TopSpaceMAUI.Model
{
    [Table("TagType")]
    public class TagType : IDisposable
    {
        [PrimaryKey, Column("TagTypeID")]
        public int TagTypeID { get; set; }

        [Column("Name"), MaxLength(50)]
        public string Name { get; set; }

        public void Dispose()
        {
            Name = null;
        }
    }

    [Table("TagTypeTemp")]
    public class TagTypeTemp : TagType
    {
    }
}
