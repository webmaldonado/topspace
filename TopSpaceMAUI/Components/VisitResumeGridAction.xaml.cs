using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Components;

public partial class VisitResumeGridAction : ContentView
{
    //private List<Model.TagMerchandisingAcao> TAG_MERCHANDISING_ACAOs = VisitState.TAG_MERCHANDISING_ACAOs;
    //private List<Model.LPScoreBrand> LP_SCORE_BRANDs = VisitState.LP_SCORE_BRANDS;
    //private List<Model.Metric> METRICs = VisitState.METRICS;
    private List<Model.Brand> BRANDs = new DAL.Brand().GetAll().ToList();
    public VisitResumeViewModel myVisitResumeViewModel { get; set; } = VisitState.myVisitResumeViewModel;

    public VisitResumeGridAction()
	{
		InitializeComponent();

        _ = Task.Run( () => BindMetricAction());
    }

    private async Task BindMetricAction()
    {
        var MetricName = "Action";
        var ScoreActionVM = (from t in VisitState.TAG_MERCHANDISING_ACAOs.Where(w => w.MetricTypeCode == MetricName)
                             join sb in VisitState.LP_SCORE_BRANDS on t.MetricID equals sb.MetricID
                             join m in VisitState.METRICS.Where(w => w.MetricType == MetricName) on sb.MetricID equals m.MetricID
                             join b in BRANDs on t.BrandID equals b.BrandID
                             select new VisitResumeScoreViewModel()
                             {
                                 BrandID = b.BrandID,
                                 BrandName = b.Name,
                                 MetricID = m.MetricID,
                                 MetricName = m.Name,
                                 Score = sb.Score * Visit.WEIGHT_ACTION
                             }).OrderBy(o => o.MetricName).ToList();

        foreach (var item in ScoreActionVM)
        {
            var isOk = VisitState.VisitDataActionSaved.Any(x => x.MetricID == item.MetricID);
            item.ImageOpacity = isOk ? 1 : 0.2;
            item.LineColor = isOk ? Colors.Green : Colors.DarkGray;
        }

        Device.BeginInvokeOnMainThread(() => {
            myVisitResumeViewModel.VisitResumeActionScoreViewModels = ScoreActionVM;
            myVisitResumeViewModel.IsLoadingMetricAction = false;
        });
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

                myVisitResumeViewModel = null;
                BRANDs = null;
            }
        }
    }
}
