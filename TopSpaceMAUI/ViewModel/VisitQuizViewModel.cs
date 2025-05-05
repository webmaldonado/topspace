using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TopSpaceMAUI.ViewModel;

public partial class VisitQuizViewModel: ObservableObject
{
    [ObservableProperty]
    private int _QuizID;

    [ObservableProperty]
    private int _QuizTypeID;

    [ObservableProperty]
    private string _Question;

    [ObservableProperty]
    private ObservableCollection<VisitQuizOptionViewModel> _Options = new();

    [ObservableProperty]
    private string _AnswerText;

    [ObservableProperty]
    private bool _IsFreeTextQuestion = false;

    [ObservableProperty]
    private bool _IsMultipleChoiseQuestion = false;
}


