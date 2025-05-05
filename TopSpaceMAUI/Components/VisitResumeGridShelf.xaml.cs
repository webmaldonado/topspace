using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Components;

public partial class VisitResumeGridShelf : ContentView
{
    private List<Model.LPScoreBrand> LP_SCORE_BRANDs = VisitState.LP_SCORE_BRANDS;
    private List<Model.Metric> METRICs = VisitState.METRICS;
    private List<Model.Brand> BRANDs = new DAL.Brand().GetAll().ToList();
    
    public VisitResumeViewModel myVisitResumeViewModel { get; set; } = VisitState.myVisitResumeViewModel;


    public VisitResumeGridShelf()
	{
		InitializeComponent();

		BindingContext = myVisitResumeViewModel;

        _ = Task.Run( () => BindMetricShelf());

    }

    private async Task BindMetricShelf()
    {
        var MetricName = "Shelf";
        var ScoreShelfVM = (from sb in LP_SCORE_BRANDs
                            join m in METRICs.Where(w => w.MetricType == MetricName) on sb.MetricID equals m.MetricID
                            join b in BRANDs on sb.BrandID equals b.BrandID
                            select new VisitResumeScoreViewModel()
                            {
                                BrandID = b.BrandID,
                                BrandName = b.Name,
                                Score = sb.Score * Visit.WEIGHT_SHELF
                            }).ToList();

        foreach (var item in ScoreShelfVM)
        {
            var isOk = VisitState.VisitDataShelfSaved.Any(x => x.BrandID == item.BrandID);
            item.ImageOpacity = isOk ? 1 : 0.2;
            item.LineColor = isOk ? Colors.Green : Colors.DarkGray;
        }

        Device.BeginInvokeOnMainThread(() => {
            myVisitResumeViewModel.VisitResumeShelfScoreViewModels = ScoreShelfVM;
            myVisitResumeViewModel.IsLoadingMetricShelf = false;
        });

    }
}
