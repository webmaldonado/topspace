using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel
{
	public partial class VisitResumeObjectivesViewModel: ObservableObject
	{
		[ObservableProperty]
		private int _BrandID;

        [ObservableProperty]
        private string _BrandName;

        [ObservableProperty]
        private int _QtdPendences;

        [ObservableProperty]
        private string _Warnning;

        [ObservableProperty]
        private Boolean _AllowJustify;

        [ObservableProperty]
        private double _ImageOpacity;
    }
}

