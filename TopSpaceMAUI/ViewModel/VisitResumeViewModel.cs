using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitResumeViewModel: ObservableObject
{
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
    private int _ScoreMaxMetricAction;
    [ObservableProperty]
    private int _MyScoreMetricAction;

    [ObservableProperty]
    private int _ScoreMaxMetricDisplay;
    [ObservableProperty]
    private int _MyScoreMetricDisplay;

    [ObservableProperty]
    private int _ScoreMaxMetricShelf;
    [ObservableProperty]
    private int _MyScoreMetricShelf;

    [ObservableProperty]
    private int _ScoreMaxMetricStock;
    [ObservableProperty]
    private int _MyScoreMetricStock;

    [ObservableProperty]
    private List<VisitResumeScoreViewModel> _VisitResumeShelfScoreViewModels = new();

    [ObservableProperty]
    private List<VisitResumeScoreViewModel> _VisitResumeStockScoreViewModels = new();

    [ObservableProperty]
    private List<VisitResumeScoreViewModel> _VisitResumeDisplayScoreViewModels = new();

    [ObservableProperty]
    private List<VisitResumeScoreViewModel> _VisitResumeActionScoreViewModels = new();

    [ObservableProperty]
    private List<VisitResumeObjectivesViewModel> _VisitResumeObjectivesViewModels = new();

    [ObservableProperty]
    private  VisitQualityCheckViewModel _QualityCheck = new();

    [ObservableProperty]
    private Boolean _IsQualityCheckEnable = false;


    [ObservableProperty]
    private Boolean _IsLoadingMetricShelf = false;
    [ObservableProperty]
    private Boolean _IsLoadingMetricStock = false;
    [ObservableProperty]
    private Boolean _IsLoadingMetricDisplay = false;
    [ObservableProperty]
    private Boolean _IsLoadingMetricAction = false;
}

