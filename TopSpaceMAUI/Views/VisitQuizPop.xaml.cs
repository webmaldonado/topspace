using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class VisitQuizPop : ContentPage
{
    public ObservableCollection<VisitQuizViewModel> myVisitQuizViewModel { get; set; } = new();

    public VisitQuizPop()
	{
        InitializeComponent();
        if (VisitState.VisitQuizSaved.Count > 0)
        {
            myVisitQuizViewModel = new ObservableCollection<VisitQuizViewModel>(VisitState.VisitQuizSaved);
            myQuizsList.ItemsSource = myVisitQuizViewModel;
            return;
        };
        LoadQuizs();

    }

    private void LoadQuizs()
    {
        DAL.Quiz QuizDAL = new();
        DAL.QuizOption QuizOptionDAL = new();

        var quizs = QuizDAL.GetAll();

        foreach (var item in quizs)
        {
            var quiz = new VisitQuizViewModel
            {
                QuizID = item.QuizID,
                QuizTypeID = item.QuizTypeID,
                Question = item.Question,
                IsFreeTextQuestion = item.QuizTypeID.Equals(1),
                IsMultipleChoiseQuestion = item.QuizTypeID.Equals(2)
            };

            if (quiz.IsMultipleChoiseQuestion)
            {
                var quiz_options = from o in QuizOptionDAL.GetAll()
                                   where o.QuizID == item.QuizID
                                   select new VisitQuizOptionViewModel()
                                   {
                                       QuizID = o.QuizID,
                                       OptionID = o.QuizOptionID,
                                       OptionDescription = o.Option
                                   };

                quiz.Options = new ObservableCollection<VisitQuizOptionViewModel>(quiz_options);
            }

            myVisitQuizViewModel.Add(quiz);
        }

        myQuizsList.ItemsSource = myVisitQuizViewModel;
    }

    async void btnClose_Clicked(System.Object sender, System.EventArgs e)
    {
        VisitState.VisitQuizSaved = myVisitQuizViewModel.ToList();

        await Shell.Current.GoToAsync("//VisitList/Visit");

    }
}