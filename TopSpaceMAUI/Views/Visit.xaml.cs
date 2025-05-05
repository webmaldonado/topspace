using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TopSpaceMAUI.Util;
using TopSpaceMAUI.ViewModel;
using Color = Microsoft.Maui.Graphics.Color;

namespace TopSpaceMAUI;

public partial class Visit : ContentPage, IQueryAttributable
{
    Color COLOR_DEFAULT = Color.FromHex("#003366");
    Color COLOR_SELECTED = Color.FromHex("#325b84");

    public static bool ALLOW_REGISTER_LOG = Preferences.Get("VisitLogDebug", false);
    public static string POSCode { get; set; }
    public static Model.POS POS { get; set; }
    public static int TagID;
    private int CurrentBrandID;
    private string CurrentBrandName;
    public static List<Model.Brand> Brands { get; set; } = new();

    //public static List<Model.LPScoreSKU> LP_SCORE_SKUS;
    //public static List<Model.ObjectiveBrand> OBJECTIVE_BRANDS;
    //public static List<Model.LPScoreBrand> LP_SCORE_BRANDS;
    //public static List<Model.SKU> SKUs;
    //public static List<Model.TagPresenca> TAG_PRESENCAs;
    //public static List<Model.Metric> METRICS;
    //public static List<Model.TagMerchandisingAcao> TAG_MERCHANDISING_ACAOs;
    //public static List<Model.LPGrade> LP_GRADE;

    //public static VisitViewModel myVisitViewModel { get; set; } = new();
    //public static List<VisitDataShelfViewModel> VisitDataShelfSaved { get; set; } = new();
    //public static List<VisitDataSKUViewModel> VisitDataSKUSaved { get; set; } = new();
    //public static List<VisitDataMerchandisingViewModel> VisitDataMerchandisingSaved { get; set; } = new();
    //public static List<VisitDataActionViewModel> VisitDataActionSaved { get; set; } = new();
    //public static List<VisitQuizViewModel> VisitQuizSaved { get; set; } = new();

    private int METRIC_SHELF_ID;
    private int METRIC_STOCK_ID;
    public static decimal WEIGHT_SHELF;
    public static decimal WEIGHT_STOCK;
    public static decimal WEIGHT_DISPLAY;
    public static decimal WEIGHT_ACTION;

    public static string SCORE_LEGEND_NAME;
    public static Color? SCORE_LEGEND_COLOR;

    public Visit()
    {
        InitializeComponent();
    }

    public Visit(string pos_code)
    {
        InitializeComponent();
        InitVisit(pos_code);
    }

    public void InitVisit(string pos_code)
    {
        ClearStaticValues();

        POSCode = pos_code;

        RegisterLog("InitVisit", true);

        VisitState.myVisitViewModel = new();

        LoadPOSInfo();
        LoadLists();
        LoadMenuBrands();

        Slider_ValueChanged(sldScore, new ValueChangedEventArgs(0, sldScore.Value));

        BindingContext = VisitState.myVisitViewModel;
    }

    private void LoadLists()
    {
        RegisterLog("LoadLists");

        VisitState.METRICS = new DAL.Metric().GetAll();
        Model.Metric metricsShelf = VisitState.METRICS.Where(m => m.MetricType == Config.METRIC_TYPE_SHELF_NAME).FirstOrDefault();
        if (metricsShelf != null)
        {
            METRIC_SHELF_ID = metricsShelf.MetricID;
        }

        Model.Metric metricsStock = VisitState.METRICS.Where(m => m.MetricType == Config.METRIC_TYPE_STOCK_NAME).FirstOrDefault();
        if (metricsStock != null)
        {
            METRIC_STOCK_ID = metricsStock.MetricID;
        }

        LoadWeights();

        DAL.ObjectiveBrand objectiveBrandController = new();
        VisitState.OBJECTIVE_BRANDS = objectiveBrandController.GetAll().Where(o => o.POSCode == POSCode).ToList();

        //var x = objectiveBrandController.GetAll().ToList();

        DAL.LPScoreBrand LPScoreBrandController = new();
        VisitState.LP_SCORE_BRANDS = LPScoreBrandController.GetAll().Where(w => w.TagID == TagID).ToList();

        DAL.LPScoreSKU LPScoreSKUController = new();
        VisitState.LP_SCORE_SKUS = LPScoreSKUController.GetAll().Where(w => w.TagID == TagID && w.MetricID == METRIC_STOCK_ID).ToList();

        VisitState.SKUs = new DAL.SKU().GetAll();

        VisitState.TAG_PRESENCAs = new DAL.TagPresenca().GetAll().Where(w => w.TagID == TagID).ToList();

        VisitState.TAG_MERCHANDISING_ACAOs = new DAL.TagMerchandisingAcao().GetAll().Where(w => w.TagID == TagID).ToList();

        DAL.LPGrade LPGradeController = new();
        VisitState.LP_GRADE = LPGradeController.GetAll().Where(w => w.TagID == TagID).OrderBy(o => o.ScoreMin).ToList();
    }

    public static void ClearStaticValues()
    {
        VisitState.Clear();

        POSCode = string.Empty;
        POS = new();
        TagID = 0;
        VisitState.LP_SCORE_SKUS = new();
        VisitState.OBJECTIVE_BRANDS = new();
        VisitState.LP_SCORE_BRANDS = new();
        VisitState.myVisitViewModel = new();
        VisitState.VisitDataShelfSaved = new();
        VisitState.VisitDataSKUSaved = new();
        VisitState.VisitDataMerchandisingSaved = new();
        VisitState.VisitDataActionSaved = new();
        VisitState.VisitQuizSaved = new();
    }

    private void LoadPOSInfo()
    {
        RegisterLog("LoadPOSInfo");

        var PosDAL = new DAL.POS();
        POS = PosDAL.GetPOS(POSCode);

        TagID = new DAL.Tag().GetAll().Where(t => t.Name.ToUpper() == POS.TagBaseName.ToUpper()).First().TagID;

        VisitState.myVisitViewModel.CNPJ  = Convert.ToUInt64(POS.POSCode).ToString(@"00\.000\.000\/0000\-00");

        var name_split = POS.Name.Split('_');
        if (name_split.Length > 1)
        {
            VisitState.myVisitViewModel.Name = name_split[0];
        }
        else
        {
            VisitState.myVisitViewModel.Name = POS.Name;
        }
        VisitState.myVisitViewModel.Address = POS.Address;
        VisitState.myVisitViewModel.Cluster = POS.TagBaseName;

        var CurrentVisitHistory = LoadVisitHistory(POSCode);
        var PreviousVisitHistory = LoadVisitHistory(POSCode, -1);
        var PreviousScoreAVG = $"{CalculateAVG(PreviousVisitHistory)}%";
        var CurrentScoreAVG = $"{CalculateAVG(CurrentVisitHistory)}%";

        VisitState.myVisitViewModel.CurrentScoreAVG = CurrentScoreAVG;
        VisitState.myVisitViewModel.PreviousScoreAVG = PreviousScoreAVG;
        VisitState.myVisitViewModel.VisitHistory = CurrentVisitHistory;


        VisitState.myVisitViewModel.VisitLegendViewModels = LoadLegends(POS.UnitVariation);

        VisitState.myVisitViewModel.LegendIsVisible = VisitState.myVisitViewModel.VisitLegendViewModels.Count > 0;
    }

    private ObservableCollection<VisitLegendViewModel> LoadLegends(string legend)
    {
        RegisterLog("LoadLegends");

        var ret = new ObservableCollection<VisitLegendViewModel>();

        //legend = "B-21|-1/D-42|43/F-23|19/G49|56/R88|-1";

        if (legend == null) return ret;

        var products = legend.Split('/');

        if (products.Length > 0)
        {
            foreach (var product in products)
            {

                if (product.Length == 0) continue;

                var first_letter = product.Substring(0, 1);

                var product_values = product.Substring(1, product.Length -1);

                var values = product_values.Split('|');

                if (values.Length > 0)
                {

                    ret.Add(new VisitLegendViewModel()
                    {
                        Description = GetLegendName(first_letter),
                        MonthPrev = values[0],
                        YearPrev = values[1],
                        MonthImage = GetLegendImage(values[0]),
                        YearImage = GetLegendImage(values[1]),
                        MonthColor = GetLegendColor(values[0]),
                        YearColor = GetLegendColor(values[1])
                    });

                }
            }
        }
        
        return ret;
    }

    private string GetLegendImage(string p_value)
    {
        var ret = string.Empty;

        var num = float.Parse(p_value);

        ret = (num > 0) ? "up" : "down";

        return ret;
    }

    private string GetLegendName(string first_letter)
    {
        string ret = string.Empty;

        switch (first_letter)
        {
            case "B":
                ret = "Bepantol";
                break;
            case "D":
                ret = "Derma";
                break;
            case "F":
                ret = "Flanax";
                break;
            case "G":
                ret = "Gino";
                break;
            case "R":
                ret = "Redoxon";
                break;
        }
        return ret;
    }

    private Color GetLegendColor(string p_value)
    {
        var ret = Colors.Black;

        float num = float.Parse(p_value);

        ret = (num > 0) ? Colors.Green : Colors.Red;

        return ret;
    }

    private float CalculateAVG(List<ListVisitHistoryViewModel> myList)
    {
        float myAVG = 0f;
        if (myList != null && myList.Count > 0)
            myAVG = (myList.Sum(f => f.Score) / myList.Count);
        return myAVG;
    }

    private List<ListVisitHistoryViewModel> LoadVisitHistory(string pos_code, int month_ref = 0)
    {
        RegisterLog("LoadVisitHistory");

        var list_visit_history = new List<ListVisitHistoryViewModel>();
        var dalVisits = new TopSpaceMAUI.DAL.Visit();
        var dalLastVisit = new TopSpaceMAUI.DAL.LastVisit();

        var current_month = DateTime.Now.AddMonths(month_ref).Month;
        var current_year = DateTime.Now.AddMonths(month_ref).Year;

        var visits = dalVisits.GetVisit()
            .FindAll(w => w.POSCode == pos_code
            && w.Status == Config.STATUS_VISIT_COMPLETED
            && DateTime.Parse(w.VisitDate).Month == current_month
            && DateTime.Parse(w.VisitDate).Year == current_year);

        var lastVisits = dalLastVisit.GetAll().
            FindAll(w => w.POSCode == pos_code
            && DateTime.Parse(w.VisitDate).Month == current_month
            && DateTime.Parse(w.VisitDate).Year == current_year);

        if (visits != null && visits.Count > 0)
        {
            foreach (var item in visits)
            {
                lastVisits.Add(new Model.LastVisit()
                {
                    POSCode = item.POSCode,
                    Score = item.Score,
                    //Score = new Random().Next(100),
                    VisitDate = item.VisitDate
                    //VisitDate = RandomDay().ToString()
                });
            }
        }

        lastVisits.Sort(new DateStringComparer());

        foreach (var item in lastVisits)
        {
            list_visit_history.Add(new ListVisitHistoryViewModel()
            {
                VisitDate = DateTime.Parse(item.VisitDate).ToString("dd/MM/yyyy"),
                Score = item.Score
            });
        }

        return list_visit_history;
    }

    private void LoadMenuBrands()
    {
        RegisterLog("LoadMenuBrands");

        Button firstButton = null;

        Brands = (from b in new DAL.Brand().GetAll()
                            join tb in new DAL.TagBrandPontoNatural().GetAll() on b.BrandID equals tb.BrandID
                            where tb.TagID == TagID && tb.MetricTypeCode == null
                            select b).ToList();

        foreach (var item in Brands)
        {
            var button = new Button
            {
                Text = item.Name,
                CommandParameter = item.BrandID,
                FontFamily = "Verdana",
                TextColor = Colors.White,
                FontSize = 15,
                HeightRequest = 50,
                MinimumWidthRequest = 150,
                CornerRadius = 0
            };

            button.Clicked += OnButtonClicked;

            if (menuBrands.Count == 0)
            {
                button.BackgroundColor = COLOR_SELECTED;
                firstButton = button;
            }

            menuBrands.Add(button);
        }

        if (firstButton != null)
        {
            firstButton.SendClicked();
            btnMetricNaturalPoint.SendClicked();
        }
    }

    private void LoadWeights()
    {
        RegisterLog("LoadWeights");

        DAL.LPMetricType LPMetricTypeController = new();

        WEIGHT_SHELF = LPMetricTypeController.GetGradeWeight(Config.METRIC_TYPE_SHELF_NAME, TagID);
        WEIGHT_STOCK = LPMetricTypeController.GetGradeWeight(Config.METRIC_TYPE_STOCK_NAME, TagID);
        WEIGHT_DISPLAY = LPMetricTypeController.GetGradeWeight(Config.METRIC_TYPE_DISPLAY_NAME, TagID);
        WEIGHT_ACTION = LPMetricTypeController.GetGradeWeight(Config.METRIC_TYPE_ACTION_NAME, TagID);
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        foreach (var control in menuBrands.Children)
        {
            (control as Button).BackgroundColor = COLOR_DEFAULT;
        }

        var button = sender as Button;
        if (button != null)
        {
            RegisterLog($"OnButtonClicked (Brand_ID: {button.CommandParameter} - {button.Text})");

            button.BackgroundColor = COLOR_SELECTED;
            CurrentBrandID = (int) button.CommandParameter;
            CurrentBrandName = button.Text;

            //VisitDataShelfSaveInMemory();
            //VisitDataSKUSaveInMemory();
            //VisitDataActionSaveInMemory();
            //VisitDataMerchandisingSaveInMemory();

            LoadShelf();
            LoadSKUs();
            LoadMerchandising();
            LoadActions();
            RefreshScore();
        }
    }

    //private void LoadShelf()
    //{
    //    // Objetivo
    //    Model.ObjectiveBrand objectiveBrand = OBJECTIVE_BRANDS.Where(o => o.MetricID == METRIC_SHELF_ID && o.BrandID == CurrentBrandID).FirstOrDefault();
    //    var objectivePercent = objectiveBrand != null ? objectiveBrand.Objective : 0;


    //    // Pontuação
    //    int? grade = null;
    //    decimal? gradeWeight = null;

    //    Model.LPScoreBrand scoreBrand = LP_SCORE_BRANDS.Where(m => m.MetricID == METRIC_SHELF_ID && m.BrandID == CurrentBrandID && m.TagID == TagID).FirstOrDefault();
    //    if (scoreBrand != null)
    //    {
    //        grade = scoreBrand.Score;
    //        gradeWeight = scoreBrand.Score * WEIGHT_SHELF;
    //    }

    //    var item_finded = VisitDataShelfSaved.Where(w => w.BrandID == CurrentBrandID).FirstOrDefault();

    //    myVisitViewModel.VisitDataShelfViewModel = new();
    //    myVisitViewModel.VisitDataShelfViewModel.MetricID = METRIC_SHELF_ID;
    //    myVisitViewModel.VisitDataShelfViewModel.BrandID = CurrentBrandID;
    //    myVisitViewModel.VisitDataShelfViewModel.BrandName = CurrentBrandName;
    //    myVisitViewModel.VisitDataShelfViewModel.Objective = objectivePercent;
    //    myVisitViewModel.VisitDataShelfViewModel.Score = item_finded == null ? 0 : item_finded.Score;
    //    myVisitViewModel.VisitDataShelfViewModel.Grade = grade;
    //    myVisitViewModel.VisitDataShelfViewModel.GradeWeight = gradeWeight;
    //    myVisitViewModel.VisitDataShelfViewModel.ShowScore = gradeWeight > 0 ? true : false;
    //    myVisitViewModel.VisitDataShelfViewModel.Photo = item_finded != null ? item_finded.Photo : null;
    //}
    private void LoadShelf()
    {
        RegisterLog("LoadShelf");

        var objectiveBrand = VisitState.OBJECTIVE_BRANDS.FirstOrDefault(o => o.MetricID == METRIC_SHELF_ID && o.BrandID == CurrentBrandID);
        var objectivePercent = objectiveBrand?.Objective ?? 0;

        var scoreBrand = VisitState.LP_SCORE_BRANDS.FirstOrDefault(m => m.MetricID == METRIC_SHELF_ID && m.BrandID == CurrentBrandID && m.TagID == TagID);
        var grade = scoreBrand?.Score;
        var gradeWeight = grade.HasValue ? grade.Value * WEIGHT_SHELF : (decimal?)null;

        var itemFound = VisitState.VisitDataShelfSaved.FirstOrDefault(w => w.BrandID == CurrentBrandID);

        VisitState.myVisitViewModel.VisitDataShelfViewModel = new()
        {
            MetricID = METRIC_SHELF_ID,
            BrandID = CurrentBrandID,
            BrandName = CurrentBrandName,
            Objective = objectivePercent,
            Score = itemFound?.Score ?? 0,
            Grade = grade,
            GradeWeight = gradeWeight,
            ShowScore = gradeWeight > 0,
            Photo = itemFound?.Photo
        };

        objectiveBrand?.Dispose();
        scoreBrand?.Dispose();
        itemFound = null;
    }


    //private void LoadSKUs()
    //{
    //    int grade = 0;
    //    decimal gradeWeight = 0;

    //    List<Model.SKU> lstSKUBrand = (from s in SKUs
    //                                   join tp in TAG_PRESENCAs on s.SKUID equals tp.SKUID
    //                                   where tp.TagID == TagID && tp.BrandID == CurrentBrandID
    //                                   orderby s.Order
    //                                   select s).ToList();

    //    myVisitViewModel.VisitDataSKUViewModels.Clear();

    //    var indice = 0;

    //    foreach (Model.SKU item in lstSKUBrand)
    //    {
    //        // Pontuação
    //        Model.LPScoreSKU scoreSKU = LP_SCORE_SKUS.Where(m => m.SKUID == item.SKUID).FirstOrDefault();
    //        if (scoreSKU != null)
    //        {
    //            grade = scoreSKU.Score;
    //            gradeWeight = scoreSKU.Score * WEIGHT_STOCK;
    //        }

    //        var item_finded = VisitDataSKUSaved.Where(w => w.SKUID == item.SKUID && w.BrandID == item.BrandID).FirstOrDefault();

    //        myVisitViewModel.VisitDataSKUViewModels.Add(new VisitDataSKUViewModel
    //        {
    //            SKUID = item.SKUID,
    //            MetricID = METRIC_STOCK_ID,
    //            Name = item.Name,
    //            BrandName = CurrentBrandName,
    //            BrandID = item.BrandID,
    //            Value = item_finded == null ? 0 : item_finded.Value,
    //            Score = grade,
    //            Grade = grade,
    //            GradeWeight = gradeWeight,
    //            ShowScore = gradeWeight > 0 ? true : false,
    //            LineColor = GetLineColor(indice)
    //        });

    //        indice++;
    //    }
    //}
    private void LoadSKUs()
    {
        RegisterLog("LoadSKUs");

        VisitState.myVisitViewModel.VisitDataSKUViewModels.Clear();

        var lstSKUBrand = (from s in VisitState.SKUs
                           join tp in VisitState.TAG_PRESENCAs on s.SKUID equals tp.SKUID
                           where tp.TagID == TagID && tp.BrandID == CurrentBrandID
                           orderby s.Order
                           select s).ToList();

        int indice = 0;

        foreach (var item in lstSKUBrand)
        {
            var scoreSKU = VisitState.LP_SCORE_SKUS.FirstOrDefault(m => m.SKUID == item.SKUID);
            var grade = scoreSKU?.Score ?? 0;
            var gradeWeight = grade * WEIGHT_STOCK;

            var itemFound = VisitState.VisitDataSKUSaved.FirstOrDefault(w => w.SKUID == item.SKUID && w.BrandID == item.BrandID);

            VisitState.myVisitViewModel.VisitDataSKUViewModels.Add(new VisitDataSKUViewModel
            {
                SKUID = item.SKUID,
                MetricID = METRIC_STOCK_ID,
                Name = item.Name,
                BrandName = CurrentBrandName,
                BrandID = item.BrandID,
                Value = itemFound?.Value ?? 0,
                Score = grade,
                Grade = grade,
                GradeWeight = gradeWeight,
                ShowScore = gradeWeight > 0,
                LineColor = GetLineColor(indice)
            });

            indice++;

            //scoreSKU.Dispose();
            itemFound = null;
        }

        lstSKUBrand = null;

    }


    private Color GetLineColor(int indice)
    {
        var alternate_line_color = AppInfo.RequestedTheme == AppTheme.Dark ? Color.FromHex("#444444") : Color.FromHex("#eeeeee");

        return (indice % 2 == 0) ? alternate_line_color : Colors.Transparent;
    }

    //private void LoadMerchandising()
    //{
    //    var MetricType = Config.METRIC_TYPE_DISPLAY_NAME;
    //    DAL.ObjectiveBrand objectiveBrandController = new();
    //    DAL.LPScoreBrand LPScoreBrandController = new();

    //    List<Model.Metric> metrics = (from m in METRICS
    //                                  join ma in TAG_MERCHANDISING_ACAOs on m.MetricID equals ma.MetricID
    //                                  where ma.BrandID == CurrentBrandID && ma.MetricTypeCode == MetricType
    //                                  select m).ToList();

    //    myVisitViewModel.VisitDataMerchandisingViewModels = new();

    //    foreach (Model.Metric metric in metrics)
    //    {

    //        // Objetivo
    //        Model.ObjectiveBrand objectiveBrand = OBJECTIVE_BRANDS.Where(o => o.MetricID == metric.MetricID && o.BrandID == CurrentBrandID).FirstOrDefault();
    //        decimal? objective = objectiveBrand != null ? objectiveBrand.Objective : (decimal?)null;

    //        // Pontuação
    //        int grade = 0;
    //        decimal gradeWeight = 0;
    //        Model.LPScoreBrand scoreBrand = null;

    //        // Pontuação do tipo Ações não possuem marca
    //        scoreBrand = LP_SCORE_BRANDS.Where(m => m.MetricID == metric.MetricID && m.BrandID == CurrentBrandID && m.TagID == TagID).FirstOrDefault();
    //        if (scoreBrand != null)
    //        {
    //            grade = scoreBrand.Score;
    //            gradeWeight = scoreBrand.Score * WEIGHT_DISPLAY;
    //        }

    //        var item_finded = VisitDataMerchandisingSaved.Where(w => w.BrandID == CurrentBrandID && w.MetricID == metric.MetricID).FirstOrDefault();

    //        myVisitViewModel.VisitDataMerchandisingViewModels.Add(new VisitDataMerchandisingViewModel
    //        {
    //            MetricID = metric.MetricID,
    //            MetricType = metric.MetricType,
    //            Name = metric.Name,
    //            BrandID = CurrentBrandID,
    //            BrandName = CurrentBrandName,
    //            Objective = objective,
    //            ImageEditOpacity = objectiveBrand != null ? 1 : 0.1,
    //            ImageEditIsVisible = objectiveBrand != null ? true : false,
    //            Value = item_finded == null ? 0 : item_finded.Value,
    //            Score = grade,
    //            Grade = grade,
    //            GradeWeight = gradeWeight,
    //            ShowScore = gradeWeight > 0 ? true : false,
    //            Photo = item_finded != null ? item_finded.Photo : null,
    //            IsItemSelected = item_finded != null ? item_finded.IsItemSelected : false,
    //            ExecutionOptions = objectiveBrand != null ? BindExecutionContext() : null
    //        });
    //    }
    //}
    private void LoadMerchandising()
    {
        RegisterLog("LoadMerchandising");

        var MetricType = Config.METRIC_TYPE_DISPLAY_NAME;

        var metrics = (from m in VisitState.METRICS
                       join ma in VisitState.TAG_MERCHANDISING_ACAOs on m.MetricID equals ma.MetricID
                       where ma.BrandID == CurrentBrandID && ma.MetricTypeCode == MetricType
                       select m).ToList();

        VisitState.myVisitViewModel.VisitDataMerchandisingViewModels = new();

        foreach (var metric in metrics)
        {
            // Objetivo
            var objectiveBrand = VisitState.OBJECTIVE_BRANDS.FirstOrDefault(o => o.MetricID == metric.MetricID && o.BrandID == CurrentBrandID);
            var objective = objectiveBrand?.Objective;
            var hasObjective = objectiveBrand != null;

            // Pontuação
            var scoreBrand = VisitState.LP_SCORE_BRANDS.FirstOrDefault(m => m.MetricID == metric.MetricID && m.BrandID == CurrentBrandID && m.TagID == TagID);
            var grade = scoreBrand?.Score ?? 0;
            var gradeWeight = grade * WEIGHT_DISPLAY;

            var itemFound = VisitState.VisitDataMerchandisingSaved.FirstOrDefault(w => w.BrandID == CurrentBrandID && w.MetricID == metric.MetricID);

            VisitState.myVisitViewModel.VisitDataMerchandisingViewModels.Add(new VisitDataMerchandisingViewModel
            {
                MetricID = metric.MetricID,
                MetricType = metric.MetricType,
                Name = metric.Name,
                BrandID = CurrentBrandID,
                BrandName = CurrentBrandName,
                Objective = objective,
                ImageEditOpacity = hasObjective ? 1 : 0.1,
                ImageEditIsVisible = hasObjective,
                Value = itemFound?.Value ?? 0,
                Score = grade,
                Grade = grade,
                GradeWeight = gradeWeight,
                ShowScore = gradeWeight > 0,
                Photo = itemFound?.Photo,
                IsItemSelected = itemFound?.IsItemSelected ?? false,
                ExecutionOptions = hasObjective ? BindExecutionContext() : null
            });

            objectiveBrand?.Dispose();
            scoreBrand?.Dispose();
            itemFound = null;
        }

        metrics = null;
    }

    private ObservableCollection<Model.ExecutionContext> BindExecutionContext()
    {
        var executionContextDAL = new DAL.ExecutionContext();
        var executionContextList = executionContextDAL.GetAll();

        var ret = new ObservableCollection<Model.ExecutionContext>(executionContextList);

        return ret;
    }

    //private void LoadActions()
    //{
    //    var MetricType = Config.METRIC_TYPE_ACTION_NAME;
    //    DAL.ObjectiveBrand objectiveBrandController = new();
    //    DAL.LPScoreBrand LPScoreBrandController = new();

    //    List<Model.Metric> metrics = (from m in METRICS
    //                                  join ma in TAG_MERCHANDISING_ACAOs on m.MetricID equals ma.MetricID
    //                                  where ma.BrandID == CurrentBrandID && ma.MetricTypeCode == MetricType
    //                                  select m).ToList();

    //    myVisitViewModel.VisitDataActionViewModels = new();

    //    foreach (Model.Metric metric in metrics)
    //    {

    //        // Objetivo
    //        Model.ObjectiveBrand objectiveBrand = OBJECTIVE_BRANDS.Where(o => o.MetricID == metric.MetricID && o.BrandID == CurrentBrandID).FirstOrDefault();
    //        decimal? objective = objectiveBrand != null ? objectiveBrand.Objective : (decimal?)null;

    //        // Pontuação
    //        int grade = 0;
    //        decimal gradeWeight = 0;
    //        Model.LPScoreBrand scoreBrand = null;

    //        // Pontuação do tipo Ações não possuem marca
    //        if (MetricType == Config.METRIC_TYPE_ACTION_NAME)
    //        {
    //            scoreBrand = LP_SCORE_BRANDS.Where(m => m.MetricID == metric.MetricID && m.TagID == TagID).FirstOrDefault();
    //            if (scoreBrand != null)
    //            {
    //                grade = scoreBrand.Score;
    //                gradeWeight = scoreBrand.Score * WEIGHT_ACTION;
    //            }
    //        }
    //        else
    //        {
    //            scoreBrand = LP_SCORE_BRANDS.Where(m => m.MetricID == metric.MetricID && m.BrandID == CurrentBrandID && m.TagID == TagID).FirstOrDefault();
    //            if (scoreBrand != null)
    //            {
    //                grade = scoreBrand.Score;
    //                gradeWeight = scoreBrand.Score * WEIGHT_DISPLAY;
    //            }
    //        }

    //        var item_finded = VisitDataActionSaved.Where(w => w.BrandID == CurrentBrandID && w.MetricID == metric.MetricID).FirstOrDefault();

    //        myVisitViewModel.VisitDataActionViewModels.Add(new VisitDataActionViewModel
    //        {
    //            MetricID = metric.MetricID,
    //            MetricType = metric.MetricType,
    //            Name = metric.Name,
    //            BrandID = CurrentBrandID,
    //            BrandName = CurrentBrandName,
    //            Objective = objective,
    //            Value = item_finded == null ? 0 : item_finded.Value,
    //            Score = grade,
    //            Grade = grade,
    //            GradeWeight = gradeWeight,
    //            ShowScore = gradeWeight > 0 ? true : false,
    //            Photo = item_finded != null ? item_finded.Photo : null
    //        });
    //    }
    //}
    private void LoadActions()
    {
        RegisterLog("LoadActions");

        var MetricType = Config.METRIC_TYPE_ACTION_NAME;

        var metrics = (from m in VisitState.METRICS
                       join ma in VisitState.TAG_MERCHANDISING_ACAOs on m.MetricID equals ma.MetricID
                       where ma.BrandID == CurrentBrandID && ma.MetricTypeCode == MetricType
                       select m).ToList();

        VisitState.myVisitViewModel.VisitDataActionViewModels = new();

        foreach (var metric in metrics)
        {
            // Objetivo
            var objectiveBrand = VisitState.OBJECTIVE_BRANDS.FirstOrDefault(o => o.MetricID == metric.MetricID && o.BrandID == CurrentBrandID);
            var objective = objectiveBrand?.Objective;

            // Pontuação
            var isActionMetric = MetricType == Config.METRIC_TYPE_ACTION_NAME;
            var scoreBrand = VisitState.LP_SCORE_BRANDS.FirstOrDefault(m => m.MetricID == metric.MetricID && (isActionMetric || m.BrandID == CurrentBrandID) && m.TagID == TagID);
            var grade = scoreBrand?.Score ?? 0;
            var gradeWeight = grade * (isActionMetric ? WEIGHT_ACTION : WEIGHT_DISPLAY);

            var itemFound = VisitState.VisitDataActionSaved.FirstOrDefault(w => w.BrandID == CurrentBrandID && w.MetricID == metric.MetricID);

            VisitState.myVisitViewModel.VisitDataActionViewModels.Add(new VisitDataActionViewModel
            {
                MetricID = metric.MetricID,
                MetricType = metric.MetricType,
                Name = metric.Name,
                BrandID = CurrentBrandID,
                BrandName = CurrentBrandName,
                Objective = objective,
                Value = itemFound?.Value ?? 0,
                Score = grade,
                Grade = grade,
                GradeWeight = gradeWeight,
                ShowScore = gradeWeight > 0,
                Photo = itemFound?.Photo
            });
        }
    }


    public static void RefreshScore()
    {
        var scoreShelf = (int)VisitState.VisitDataShelfSaved.Sum(i => i.Grade);
        var scoreStock = (int)VisitState.VisitDataSKUSaved.Where(w => w.Value > 0).Sum(i => i.Grade);
        var scoreMerchandising = (int)VisitState.VisitDataMerchandisingSaved.Sum(i => i.Grade);
        var scoreAction = (int)VisitState.VisitDataActionSaved.Sum(i => i.Grade);

        int grade = CalculateScore(scoreStock, scoreMerchandising, scoreShelf, scoreAction);
        VisitState.myVisitViewModel.LabelScore = grade.ToString();
        VisitState.myVisitViewModel.SliderValue = grade;
    }

    private static int CalculateScore(int scoreStock, int scoreDisplay, int scoreShelf, int scoreAction)
    {
        RegisterLog("CalculateScore");

        //DAL.LPGrade LPGradeController = new();

        decimal stock = scoreStock * WEIGHT_STOCK;
        decimal display = scoreDisplay * WEIGHT_DISPLAY;
        decimal shelf = scoreShelf * WEIGHT_SHELF;
        decimal action = scoreAction * WEIGHT_ACTION;

        int total = (int)Math.Round(stock + display + shelf + action, 0, MidpointRounding.AwayFromZero);
        Model.Score.ScoreTotal = total;
        //Model.Score.ScoreName = LPGradeController.GradeName(total);
        Model.Score.ScoreName = GetGradeName(total);

        return total;
    }

    public static void VisitDataSKUSaveInMemory()
    {
        RegisterLog("VisitDataSKUSaveInMemory");

        foreach (var item in VisitState.myVisitViewModel.VisitDataSKUViewModels)
        {
            //if (item.Value > 0)
            //{
            //    var item_finded = VisitDataSKUSaved.Where(w => w.SKUID == item.SKUID && w.BrandID == item.BrandID).FirstOrDefault();

            //    if (item_finded == null)
            //        VisitDataSKUSaved.Add(item);
            //    else
            //        item_finded.Value = item.Value;
            //}
            //else
            //{
            //    VisitDataSKUSaved.RemoveAll(w => w.SKUID == item.SKUID && w.BrandID == item.BrandID);
            //}

            var item_finded = VisitState.VisitDataSKUSaved.Where(w => w.SKUID == item.SKUID && w.BrandID == item.BrandID).FirstOrDefault();

            if (item_finded == null)
                VisitState.VisitDataSKUSaved.Add(item);
            else
                item_finded.Value = item.Value;
        }
    }

    public static void VisitDataActionSaveInMemory()
    {
        RegisterLog("VisitDataActionSaveInMemory");

        foreach (var item in VisitState.myVisitViewModel.VisitDataActionViewModels)
        {
            //if (item.Value > 0 || item.Photo != null)
            if (item.Value > 0 && item.Photo != null)
            {
                var item_finded = VisitState.VisitDataActionSaved.Where(w => w.BrandID == item.BrandID && w.MetricID == item.MetricID).FirstOrDefault();

                if (item_finded == null)
                    VisitState.VisitDataActionSaved.Add(item);
                else
                {
                    item_finded.Value = item.Value;
                    item_finded.Photo = item.Photo;
                }

            }
            else
            {
                VisitState.VisitDataActionSaved.RemoveAll(w => w.MetricID == item.MetricID && w.BrandID == item.BrandID);
            }
        }
    }

    public static void VisitDataMerchandisingSaveInMemory()
    {
        foreach (var item in VisitState.myVisitViewModel.VisitDataMerchandisingViewModels)
        {
            var mm = item;

            //if (item.Value > 0 || item.IsItemSelected || item.Photo != null)
            if (item.Value > 0 && item.Photo != null)
            {
                var item_finded = VisitState.VisitDataMerchandisingSaved.Where(w => w.BrandID == item.BrandID && w.MetricID == item.MetricID).FirstOrDefault();

                if (item_finded == null)
                    VisitState.VisitDataMerchandisingSaved.Add(item);
                else
                {
                    item_finded.Value = item.Value;
                    item_finded.Photo = item.Photo;
                    item_finded.IsItemSelected = item.IsItemSelected;
                }

            }
            else
            {
                VisitState.VisitDataMerchandisingSaved.RemoveAll(w => w.MetricID == item.MetricID && w.BrandID == item.BrandID);
            }
        }
    }
    

    public static void VisitDataShelfSaveInMemory()
    {
        RegisterLog("VisitDataShelfSaveInMemory");

        if (VisitState.myVisitViewModel.VisitDataShelfViewModel == null) return;

        var item = VisitState.myVisitViewModel.VisitDataShelfViewModel;

        if (item.Score > 0 || item.Photo != null)
        {
            var item_finded = VisitState.VisitDataShelfSaved.Where(w => w.BrandID == item.BrandID).FirstOrDefault();

            if (item_finded == null)
                VisitState.VisitDataShelfSaved.Add(item);
            else
            {
                item_finded.Score = item.Score;
                item_finded.Photo = item.Photo;
            }
        }
        else
        {
            VisitState.VisitDataShelfSaved.RemoveAll(w => w.BrandID == item.BrandID);
        }
    }

    //async void btnFinalizar_Clicked(System.Object sender, System.EventArgs e)
    //{
    //    //var x = VisitDataShelfSaved;
    //    //await Shell.Current.GoToAsync("VisitResume");

    //    //await Shell.Current.GoToAsync("VisitResume");

    //    Navigation.PushAsync(new VisitResume());
    //}

    async void btnFinalizar_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnFinalizar_Clicked");

        var button = sender as Button;
        if (button != null)
        {
            button.Dispatcher.Dispatch(() =>
            {
                button.Text = "Aguarde...";
                button.IsEnabled = false;
            });
        }

        await Task.Delay(300);

        //await Navigation.PushAsync(new VisitResume());
        await Navigation.PushModalAsync(new VisitResume());

        if (button != null)
        {
            button.Dispatcher.Dispatch(() =>
            {
                button.Text = "Finalizar";
                button.IsEnabled = true;
            });
        }
    }




    private void ShowContentMetric(Button button_metric)
    {
        this.ContentMetricNaturalPoint.IsVisible = (button_metric == btnMetricNaturalPoint);
        this.ContentMetricStock.IsVisible = (button_metric == btnMetricStock);
        this.ContentMetricMerchandising.IsVisible = (button_metric == btnMetricMerchandising);
        this.ContentMetricAction.IsVisible = (button_metric == btnMetricAction);
    }

    private void ChangeColorToSelected(Button button_metric)
    {
        this.btnMetricNaturalPoint.BackgroundColor = (button_metric == btnMetricNaturalPoint ? COLOR_SELECTED : COLOR_DEFAULT);
        this.btnMetricStock.BackgroundColor = (button_metric == btnMetricStock ? COLOR_SELECTED : COLOR_DEFAULT);
        this.btnMetricMerchandising.BackgroundColor = (button_metric == btnMetricMerchandising ? COLOR_SELECTED : COLOR_DEFAULT);
        this.btnMetricAction.BackgroundColor = (button_metric == btnMetricAction ? COLOR_SELECTED : COLOR_DEFAULT);
    }

    void btnMetricNaturalPoint_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMetricNaturalPoint_Clicked");

        ShowContentMetric((Button)sender);
        ChangeColorToSelected((Button)sender);
    }

    void btnMetricStock_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMetricStock_Clicked");

        ShowContentMetric((Button)sender);
        ChangeColorToSelected((Button)sender);
    }

    void btnMetricMerchandising_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMetricMerchandising_Clicked");

        ShowContentMetric((Button)sender);
        ChangeColorToSelected((Button)sender);
    }

    void btnMetricAction_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMetricAction_Clicked");

        ShowContentMetric((Button)sender);
        ChangeColorToSelected((Button)sender);
    }

    void btnMetricQuiz_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMetricQuiz_Clicked");

        ShowContentMetric((Button)sender);
        ChangeColorToSelected((Button)sender);
    }

    async void btnObjectives_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnObjectives_Clicked");

        var popup = new VisitObjectivesPop(POSCode);
        //popup.CanBeDismissedByTappingOutsideOfPopup = true;
        //this.ShowPopup(popup);
        await Navigation.PushModalAsync(popup);
    }

    async void btnPromotions_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnPromotions_Clicked");

        var popup = new VisitPromotionsPop(POSCode);
        //popup.CanBeDismissedByTappingOutsideOfPopup = true;
        //this.ShowPopup(popup);
        await Navigation.PushModalAsync(popup);
    }

    async void btnPesquisa_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnPesquisa_Clicked");

        var popup = new VisitQuizPop();
        //popup.CanBeDismissedByTappingOutsideOfPopup = true;
        //this.ShowPopup(popup);
        await Navigation.PushModalAsync(popup);

    }

    //async void btnMerchandisingTakePhotos_Clicked(System.Object sender, System.EventArgs e)
    //{
    //    await MainThread.InvokeOnMainThreadAsync(async () =>
    //    {
    //        await Task.Delay(500); // Aguarda 500ms antes de abrir a câmera

    //        ImageSource myPhoto = null;
    //        var result = await MediaPicker.Default.CapturePhotoAsync();

    //        if (result != null)
    //        {
    //            using (var stream = await result.OpenReadAsync())
    //            {
    //                byte[] imageBytes;
    //                using (var memoryStream = new MemoryStream())
    //                {
    //                    await stream.CopyToAsync(memoryStream);
    //                    imageBytes = memoryStream.ToArray();
    //                }

    //                foreach (var item in myVisitViewModel.VisitDataMerchandisingViewModels)
    //                {
    //                    if (item.IsItemSelected)
    //                    {
    //                        myPhoto = ImageSource.FromStream(() => new MemoryStream(imageBytes));
    //                        item.Photo = myPhoto;
    //                        //item._photoBytes = imageBytes;
    //                    }
    //                }
    //            }
    //        }

    //        //var imagePath = "test_photo.jpg";
    //        //var myPhoto = ImageSource.FromFile(imagePath);

    //        Visit.VisitDataMerchandisingSaveInMemory();

    //        // Libera memória manualmente
    //        GC.Collect();
    //        GC.WaitForPendingFinalizers();

    //    });
    //}

    async void btnMerchandisingTakePhotos_Clicked(System.Object sender, System.EventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                await Task.Delay(500); // Pequeno atraso para estabilidade

                var result = await MediaPicker.Default.CapturePhotoAsync();

                if (result == null)
                    return;

                using var stream = await result.OpenReadAsync();

                // Converte a imagem para byte array para possível uso posterior
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                foreach (var item in VisitState.myVisitViewModel.VisitDataMerchandisingViewModels.Where(i => i.IsItemSelected))
                {
                    // Caminho temporário para salvar a imagem
                    var tempFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.CacheDirectory, $"{Guid.NewGuid()}.jpg");

                    // Salvar a imagem no arquivo
                    await File.WriteAllBytesAsync(tempFilePath, imageBytes);

                    // Armazena apenas o caminho da imagem
                    item.PhotoPath = tempFilePath;
                    item.Photo = ImageSource.FromFile(tempFilePath);
                    item.Value = 1;
                }

                Visit.VisitDataMerchandisingSaveInMemory();
                Visit.RefreshScore();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        });
    }


    void btnMerchandisingSelectAll_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnMerchandisingSelectAll_Clicked");

        foreach (var item in VisitState.myVisitViewModel.VisitDataMerchandisingViewModels)
        {
            item.IsItemSelected = !item.IsItemSelected;
        }


        Visit.VisitDataMerchandisingSaveInMemory();
    }

    void Slider_ValueChanged(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
    {
        RegisterLog("Slider_ValueChanged");

        var slider = sender as Slider;

        var COLOR_TERRIBLE = Colors.Red;
        var COLOR_BAD = Colors.Orange;
        var COLOR_GOOD = Microsoft.Maui.Graphics.Color.FromHex("#ffd966");
        var COLOR_EXCELLENT = Colors.Green;

        if (slider != null)
        {
            var value = slider.Value;
            var value_integer = int.Parse(value.ToString());

            DAL.LPGrade LPGradeController = new();
            SCORE_LEGEND_NAME = GetGradeName(value_integer);
            SCORE_LEGEND_COLOR = GetGradeColor(value_integer);

            slider.ThumbColor = SCORE_LEGEND_COLOR;
            slider.MinimumTrackColor = SCORE_LEGEND_COLOR;
            VisitState.myVisitViewModel.LabelScoreColor = SCORE_LEGEND_COLOR;
            VisitState.myVisitViewModel.LabelScoreName = SCORE_LEGEND_NAME;

            //if (value <= 25)
            //{
            //    slider.ThumbColor = COLOR_TERRIBLE;
            //    slider.MinimumTrackColor = COLOR_TERRIBLE;
            //    myVisitViewModel.LabelScoreColor = COLOR_TERRIBLE;
            //    myVisitViewModel.LabelScoreName = grade_name;
            //}
            //else if (value > 25 && value <= 50)
            //{
            //    slider.ThumbColor = COLOR_BAD;
            //    slider.MinimumTrackColor = COLOR_BAD;
            //    myVisitViewModel.LabelScoreColor = COLOR_BAD;
            //    myVisitViewModel.LabelScoreName = grade_name;
            //}
            //else if (value > 50 && value <= 75)
            //{
            //    slider.ThumbColor = COLOR_GOOD;
            //    slider.MinimumTrackColor = COLOR_GOOD;
            //    myVisitViewModel.LabelScoreColor = COLOR_GOOD;
            //    myVisitViewModel.LabelScoreName = grade_name;
            //}
            //else if (value > 75 && value <= 100)
            //{
            //    slider.ThumbColor = COLOR_EXCELLENT;
            //    slider.MinimumTrackColor = COLOR_EXCELLENT;
            //    myVisitViewModel.LabelScoreColor = COLOR_EXCELLENT;
            //    myVisitViewModel.LabelScoreName = grade_name;
            //}
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("pos_code"))
        {
            POSCode = Uri.UnescapeDataString(query["pos_code"].ToString());
            InitVisit(POSCode);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Shell.SetNavBarIsVisible(this, false);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsVisible = false
        });
    }

    private static string GetGradeName(int score)
    {
        string ret = string.Empty;

        var grade = VisitState.LP_GRADE.Where(g => score >= g.ScoreMin && score <= g.ScoreMax).FirstOrDefault();
        if (grade != null)
        {
            ret = grade.Name;
        }

        return ret;
    }

    private static Color? GetGradeColor(int score)
    {
        var COLOR_TERRIBLE = Colors.Red;
        var COLOR_BAD = Colors.Orange;
        var COLOR_GOOD = Microsoft.Maui.Graphics.Color.FromHex("#ffd966");
        var COLOR_EXCELLENT = Colors.Green;

        var index = VisitState.LP_GRADE.FindIndex(0, g => score >= g.ScoreMin && score <= g.ScoreMax);

        if (index > -1)
        {
            switch (index)
            {
                case 0:
                    return COLOR_TERRIBLE;
                case 1:
                    return COLOR_BAD;
                case 2:
                    return COLOR_GOOD;
                case 3:
                    return COLOR_EXCELLENT;
                default:
                    return COLOR_EXCELLENT;
            }
        }

        return COLOR_EXCELLENT;
    }

    private void ShowFullImage(object sender, TappedEventArgs e)
    {
        if ((sender as Image)?.Source != null)
        {
            var popup = new ImagePopup((sender as Image).Source);
            this.ShowPopup(popup);
        }
    }

    private static void RegisterLog(string method_name, bool break_line = false)
    {
        if (ALLOW_REGISTER_LOG)
        {
            var char_break = break_line ? "\n" : "";
            Util.LogUnhandledException.LogError($"{char_break}{DateTime.Now} | {POSCode} | {Util.MemoryRAM.CurrentValue} | {method_name}");
        }
    }

}
