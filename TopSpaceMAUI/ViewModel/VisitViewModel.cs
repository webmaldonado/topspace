using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitViewModel: ObservableObject
{
    [ObservableProperty]
    private string _CNPJ;

    [ObservableProperty]
    private string _Name;

    [ObservableProperty]
    private string _Address;

    [ObservableProperty]
    private string _Cluster;

    [ObservableProperty]
    private string _CurrentScoreAVG;

    [ObservableProperty]
    private string _PreviousScoreAVG;

    [ObservableProperty]
    private List<ListVisitHistoryViewModel> _VisitHistory;

    [ObservableProperty]
    private VisitDataShelfViewModel _VisitDataShelfViewModel;

    [ObservableProperty]
    private ObservableCollection<VisitDataSKUViewModel> _VisitDataSKUViewModels = new();

    [ObservableProperty]
    private ObservableCollection<VisitDataActionViewModel> _VisitDataActionViewModels = new();

    [ObservableProperty]
    private ObservableCollection<VisitDataMerchandisingViewModel> _VisitDataMerchandisingViewModels = new();

    [ObservableProperty]
    private int _SliderValue;

    [ObservableProperty]
    private string _LabelScore;

    [ObservableProperty]
    private Color _LabelScoreColor;

    [ObservableProperty]
    private string _LabelScoreName;

    [ObservableProperty]
    private Color _LabelScoreNameColor;

    [ObservableProperty]
    private ObservableCollection<VisitLegendViewModel> _VisitLegendViewModels = new();

    [ObservableProperty]
    private Boolean _LegendIsVisible;

}

