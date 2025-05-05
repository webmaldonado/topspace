using System;
namespace TopSpaceMAUI.ViewModel
{
	public class ListVisitViewModel
	{
		public string Name { get; set; }
        public string POSCode { get; set; }
        public string TagBaseName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int VisitCount { get; set; }
        public int Category { get; set; }
        public string CurrentScoreAVG { get; set; }
        public string PreviousScoreAVG { get; set; }
        public List<ListVisitHistoryViewModel> VisitHistory { get; set; }
    }
}

