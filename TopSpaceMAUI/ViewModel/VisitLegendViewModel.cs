using System;
using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitLegendViewModel: ObservableObject
{
	public VisitLegendViewModel()
	{
	}

    [ObservableProperty]
    private string _Description;

    [ObservableProperty]
    private string _MonthPrev;

    [ObservableProperty]
    private string _YearPrev;

    [ObservableProperty]
    private Microsoft.Maui.Graphics.Color _MonthColor;

    [ObservableProperty]
    private Microsoft.Maui.Graphics.Color _YearColor;

    [ObservableProperty]
    private string _MonthImage;

    [ObservableProperty]
    private string _YearImage;
}

