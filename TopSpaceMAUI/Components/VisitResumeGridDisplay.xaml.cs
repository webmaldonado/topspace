using System.Collections.ObjectModel;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Components;

public partial class VisitResumeGridDisplay : ContentView
{
    //private List<Model.LPScoreBrand> LP_SCORE_BRANDs = VisitState.LP_SCORE_BRANDS;
    //private List<Model.Metric> METRICs = VisitState.METRICS;
    private List<Model.Brand> BRANDs = new DAL.Brand().GetAll().ToList();
    //public VisitResumeViewModel myVisitResumeViewModel { get; set; } = VisitState.myVisitResumeViewModel;

    public VisitResumeGridDisplay()
	{
		InitializeComponent();
        CreateListView();
    }


    private void CreateListView()
    {
        ListViewGridDisplay.ItemTemplate = new DataTemplate(() =>
        {
            var metricNameLabel = new Label
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(20)
            };
            metricNameLabel.SetBinding(Label.TextProperty, "MetricName");
            metricNameLabel.SetBinding(Label.TextColorProperty, "LineColor");

            var brandNameLabel = new Label
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(20)
            };
            brandNameLabel.SetBinding(Label.TextProperty, "BrandName");
            brandNameLabel.SetBinding(Label.TextColorProperty, "LineColor");

            var scoreLabel = new Label
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(20)
            };
            scoreLabel.SetBinding(Label.TextProperty, "Score");
            scoreLabel.SetBinding(Label.TextColorProperty, "LineColor");

            var image = new Image
            {
                Source = "finish.png",
                HeightRequest = 20,
                Margin = new Thickness(0, 0, 10, 0)
            };
            image.SetBinding(Image.OpacityProperty, "ImageOpacity");

            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { metricNameLabel, brandNameLabel, scoreLabel, image }
            };

            return new ViewCell { View = stackLayout };
        });

        BindMetricDisplayAsync();
    }


    private void BindMetricDisplayAsync()
    {
        var MetricName = "Display";
        var ScoreDisplayVM = (from sb in VisitState.LP_SCORE_BRANDS
                              join m in VisitState.METRICS.Where(w => w.MetricType == MetricName) on sb.MetricID equals m.MetricID
                              join b in BRANDs on sb.BrandID equals b.BrandID
                              select new VisitResumeScoreViewModel()
                              {
                                  BrandID = b.BrandID,
                                  BrandName = b.Name,
                                  MetricID = m.MetricID,
                                  MetricName = m.Name,
                                  Score = sb.Score * Visit.WEIGHT_DISPLAY
                              }).OrderBy(o => o.MetricName).ToList();

        foreach (var item in ScoreDisplayVM)
        {
            var isOk = VisitState.VisitDataMerchandisingSaved.Any(x => x.BrandID == item.BrandID && x.MetricID == item.MetricID);
            item.ImageOpacity = isOk ? 1 : 0.2;
            item.LineColor = isOk ? Colors.Green : Colors.DarkGray;
        }

        ListViewGridDisplay.ItemsSource = ScoreDisplayVM;
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (Parent == null)
        {
            // A ContentView foi REMOVIDA da tela → hora de limpar
            if (BindingContext is IDisposable disposable)
            {
                disposable.Dispose();

                BRANDs = null;
            }
        }
    }

}
