using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.ViewModel
{
    public partial class PictureShelfViewModel: ObservableObject
    {
        [Bindable(true)]
        public ImageSource Source { get; set; }
        public string POSCode { get; set; }
        public DateTime VisitDate { get; set; }
        public int BrandID { get; set; }
        public int MetricID { get; set; }
        [Bindable(true)]
        public string Name { get; set; }
        [Bindable(true)]
        public decimal Objective { get; set; } = 0;
        [Bindable(true)]
        public decimal Score { get; set; } = 0;
        public byte[] PhotoStream { get; set; }
        [Bindable(true)]
        public bool IsOk { get
            {
                return Score > 0 && Score >= Objective;
            } 
        }
        public string PhotoPath { get; set; }

        public List<UserResizableRuler> Rulers { get; set; }

        public PictureShelfViewModel()
        {
            this.Rulers = new List<UserResizableRuler>();
        }
    }
}
