using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI.Components;

public partial class VisitResumeGridStock : ContentView
{
    //private List<Model.Metric> METRICs = VisitState.METRICS;
    private List<Model.Brand> BRANDs = new DAL.Brand().GetAll().ToList();
    //private List<Model.LPScoreSKU> LP_SCORE_SKUS = VisitState.LP_SCORE_SKUS;
    //private List<Model.SKU> SKUs = VisitState.SKUs;
    //private Decimal WEIGHT_STOCK = Visit.WEIGHT_STOCK;
    public VisitResumeViewModel myVisitResumeViewModel { get; set; } = VisitState.myVisitResumeViewModel;

    public VisitResumeGridStock()
	{
		InitializeComponent();

        BindingContext = myVisitResumeViewModel;

        _ = Task.Run(() => BindMetricStock());
    }

    private async Task BindMetricStock()
    {
        var MetricName = "Stock";
        var ScoreStockVM = (from s in VisitState.LP_SCORE_SKUS
                            join m in VisitState.METRICS.Where(w => w.MetricType == MetricName) on s.MetricID equals m.MetricID
                            join sku in VisitState.SKUs on s.SKUID equals sku.SKUID
                            join b in BRANDs on sku.BrandID equals b.BrandID
                            select new VisitResumeScoreViewModel()
                            {
                                BrandID = b.BrandID,
                                BrandName = b.Name,
                                SKUID = sku.SKUID,
                                SKUName = sku.Name,
                                MetricID = m.MetricID,
                                MetricName = m.Name,
                                Score = s.Score * Visit.WEIGHT_STOCK
                            }).OrderBy(o => o.BrandName).ToList();

        foreach (var item in ScoreStockVM)
        {
            var isOk = VisitState.VisitDataSKUSaved.Any(x => x.BrandID == item.BrandID && x.SKUID == item.SKUID);
            item.ImageOpacity = isOk ? 1 : 0.2;
            item.LineColor = isOk ? Colors.Green : Colors.DarkGray;
        }

        Device.BeginInvokeOnMainThread(() => {
            myVisitResumeViewModel.VisitResumeStockScoreViewModels = ScoreStockVM;
            myVisitResumeViewModel.IsLoadingMetricStock = false;
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
