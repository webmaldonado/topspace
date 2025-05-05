using System;
using System.Collections;
using SQLite;
namespace TopSpaceMAUI.Model
{
    [Table("TagPresenca")]
    public class TagPresenca : IDisposable
    {
        [PrimaryKey, Column("TagPresencaID")]
        public int TagPresencaID { get; set; }        

        [Column("TagID")]
        public int TagID { get; set; }

        [Column("BrandID")]
        public int BrandID { get; set; }

        [Column("SKUID")]
        public int SKUID { get; set; }

        [Column("MetricTypeCode"), MaxLength(50)]
        public string MetricTypeCode { get; set; }

        public void Dispose()
        {
            MetricTypeCode = null;
        }

    }

    [Table("TagPresencaTemp")]
    public class TagPresencaTemp : TagPresenca
    {
    }
}
