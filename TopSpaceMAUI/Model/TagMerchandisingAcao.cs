using System;
using System.Collections;
using SQLite;
namespace TopSpaceMAUI.Model
{
    [Table("TagMerchandisingAcao")]
    public class TagMerchandisingAcao : IDisposable
    {
        [PrimaryKey, Column("TagMerchandisingAcaoID")]
        public int TagMerchandisingAcaoID { get; set; }        

        [Column("TagID")]
        public int TagID { get; set; }

        [Column("BrandID")]
        public int BrandID { get; set; }

        [Column("MetricID")]
        public int MetricID { get; set; }

        [Column("MetricTypeCode"), MaxLength(50)]
        public string MetricTypeCode { get; set; }

        public void Dispose()
        {
            MetricTypeCode = null;
        }
    }

    [Table("TagMerchandisingAcaoTemp")]
    public class TagMerchandisingAcaoTemp : TagMerchandisingAcao
    {
    }
}
