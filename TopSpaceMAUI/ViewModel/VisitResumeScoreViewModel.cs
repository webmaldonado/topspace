using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitResumeScoreViewModel: ObservableObject
{
    [ObservableProperty]
    private int _BrandID;
    [ObservableProperty]
    private string _BrandName;

    [ObservableProperty]
    private int _MetricID;
    [ObservableProperty]
    private string _MetricName;

    [ObservableProperty]
    private int _SKUID;
    [ObservableProperty]
    private string _SKUName;

    [ObservableProperty]
    private decimal _Score;

    [ObservableProperty]
    private double _ImageOpacity;

    [ObservableProperty]
    private Color _LineColor;
}

