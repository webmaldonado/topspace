using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitQuizOptionViewModel: ObservableObject
{
    [ObservableProperty]
    private int _QuizID;

    [ObservableProperty]
    private int _OptionID;

    [ObservableProperty]
    private string _OptionDescription;

    [ObservableProperty]
    private bool _IsOptionSelected = false;
}

