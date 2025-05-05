using SQLite;
using System;

namespace TopSpaceMAUI.Model
{
    [Table("TagBrandPontoNatural")]
    public class TagBrandPontoNatural : IDisposable
    {
        [PrimaryKey, Column("TagBrandPontoNaturalID")]
        public int TagBrandPontoNaturalID { get; set; }        

        [Column("TagID")]
        public int TagID { get; set; }

        [Column("BrandID")]
        public int BrandID { get; set; }

        [Column("MetricTypeCode"), MaxLength(50)]
        public string MetricTypeCode { get; set; }

        public void Dispose()
        {
            MetricTypeCode = null;
        }
    }

    [Table("TagBrandPontoNaturalTemp")]
    public class TagBrandPontoNaturalTemp : TagBrandPontoNatural
    {
    }
}
