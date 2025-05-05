using System;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table("POS")]
	public class POS : IDisposable
	{
		[PrimaryKey, Column("POSCode"), MaxLength(20)]
		public string POSCode { get; set; }

		[Column("ChainID"), MaxLength(20)]
		public int? ChainID { get; set; }

        [Column("ChainType"), MaxLength(1)]
        public string ChainType { get; set; }

		[Column("Name"), MaxLength(100)]
		public string Name { get; set; }

		[Ignore]
		public string NameSplit { get; set; }

		[Ignore]
		public string CNPJSplit { get; set; }

		[Ignore]
		public string NameWithoutAccent {
			get {
				return Name.RemoveAccents().Replace("-", " ").ToUpper();
			}
			protected set { }
		}

		public int TotalObjetivos { get; set; }

		[Column("Address"), MaxLength(150)]
		public string Address { get; set; }

		[Ignore]
		public string AddressWithoutAccent {
			get {
				return Address.RemoveAccents().Replace("-", " ").ToUpper();
			}
			protected set { }
		}

		[Column("District"), MaxLength(70)]
		public string District { get; set; }

		[Column("City"), MaxLength(70)]
		public string City { get; set; }

		[Ignore]
		public string CityWithoutAccent {
			get {
				return City.RemoveAccents().Replace("-", " ").ToUpper();
			}
			protected set { }
		}

		[Column("State"), MaxLength(2)]
		public string State { get; set; }

		[Column("Zipcode"), MaxLength(10)]
		public string Zipcode { get; set; }

		[Column("Category")]
		public int Category { get; set; }

		[Column("Latitude")]
		public Nullable<decimal> Latitude { get; set; }

		[Column("Longitude")]
		public Nullable<decimal> Longitude { get; set; }

		[Column("Precision")]
		public Nullable<decimal> Precision { get; set; }

		[Column("TagBaseName"), MaxLength(50)]
		public string TagBaseName { get; set; }

		[Ignore]
		public string LastVisitDate { get; set; }

		[Ignore]
		public decimal Objective { get; set; }

		[Ignore]
		public Nullable<decimal> Distance { get; set; }

		[Ignore]
		public int VisitCount { get; set; }

		[Column("UnitVariation"), MaxLength(250)]
		public string UnitVariation { get; set; }


		public void Dispose()
		{
			POSCode = ChainType = Name = Address = District = City = State = Zipcode = UnitVariation = null;
		}
	}

	


    [Table("POSTemp")]
	public class POSTemp : POS
	{


	}
}