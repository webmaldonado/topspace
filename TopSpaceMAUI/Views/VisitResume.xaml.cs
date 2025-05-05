using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TopSpaceMAUI.Service;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI;

public partial class VisitResume : ContentPage
{
    private static bool ALLOW_REGISTER_LOG = Visit.ALLOW_REGISTER_LOG;
    public static int TagID;
    public static string POSCode;
    public static Model.POS POS;
    private int Score = 0;

    private List<Model.LPScoreBrand> LP_SCORE_BRANDs = VisitState.LP_SCORE_BRANDS;
    private List<Model.Metric> METRICs = VisitState.METRICS;
    private List<Model.Brand> BRANDs = new DAL.Brand().GetAll().ToList();
    private List<Model.TagMerchandisingAcao> TAG_MERCHANDISING_ACAOs = VisitState.TAG_MERCHANDISING_ACAOs;
    private List<Model.LPScoreSKU> LP_SCORE_SKUS = VisitState.LP_SCORE_SKUS;
    private List<Model.SKU> SKUs = VisitState.SKUs;
    private Decimal WEIGHT_STOCK = Visit.WEIGHT_STOCK;

    //public static VisitResumeViewModel myVisitResumeViewModel { get; set; } = new();

    public ObservableCollection<VisitResumeObjectivesViewModel> myListObjectives { get; set; }

    public VisitResume()
    {
        TagID = Visit.TagID;
        POSCode = Visit.POSCode;
        POS = Visit.POS;

        RegisterLog("VisitResume");

        VisitState.myVisitResumeViewModel = new();

        InitializeComponent();

        VisitState.myVisitResumeViewModel.VisitResumeShelfScoreViewModels = new();
        VisitState.myVisitResumeViewModel.VisitResumeStockScoreViewModels = new();
        VisitState.myVisitResumeViewModel.VisitResumeDisplayScoreViewModels = new();
        VisitState.myVisitResumeViewModel.VisitResumeActionScoreViewModels = new();

        BindingContext = VisitState.myVisitResumeViewModel;

        NavigationPage.SetHasBackButton(this, false);
        LoadScore();
        LoadResumeScoreMetrics();

        VisitState.myVisitResumeViewModel.IsLoadingMetricShelf = true;
        VisitState.myVisitResumeViewModel.IsLoadingMetricStock = true;
        VisitState.myVisitResumeViewModel.IsLoadingMetricDisplay = true;
        VisitState.myVisitResumeViewModel.IsLoadingMetricAction = true;

        //_ = Task.Run(async () => await BindMetricShelf());
        //_ = Task.Run(async () => await BindMetricStock());
        //_ = Task.Run(async () => await BindMetricDisplay());
        //_ = Task.Run(async () => await BindMetricAction());

        LoadResumeObjectives();
        //Slider_ValueChanged(sldScore, new ValueChangedEventArgs(0, sldScore.Value));
        SetQualityCheck();
    }

    private void SetQualityCheck()
    {
        RegisterLog("SetQualityCheck");

        VisitState.myVisitResumeViewModel.QualityCheck = new();
        VisitState.myVisitResumeViewModel.IsQualityCheckEnable = false;

        if (VisitState.VisitDataSKUSaved.Count > 0)
        {
            var lstVisitDataSKURandom = VisitState.VisitDataSKUSaved.Where(s => s.Value >= 1).ToList();
            if (lstVisitDataSKURandom.Count > 0)
            {
                int indice = new Random().Next(0, lstVisitDataSKURandom.Count);
                var sku = lstVisitDataSKURandom[indice];
                var QualityCheck = new VisitQualityCheckViewModel()
                {
                    BrandID = sku.BrandID,
                    BrandName = sku.BrandName,
                    SKUID = sku.SKUID,
                    Name = sku.Name,
                    IsPhotoOK = false,
                    Photo = null,
                    BackColor = Color.FromHex("#ffe5e5")
                };

                VisitState.myVisitResumeViewModel.QualityCheck = QualityCheck;
                VisitState.myVisitResumeViewModel.IsQualityCheckEnable = true;
            }
        }
    }

    private void LoadScore()
    {
        VisitState.myVisitResumeViewModel.SliderValue = VisitState.myVisitViewModel.SliderValue;
        VisitState.myVisitResumeViewModel.LabelScore = VisitState.myVisitViewModel.LabelScore;
        VisitState.myVisitResumeViewModel.LabelScoreColor = VisitState.myVisitViewModel.LabelScoreColor;
        VisitState.myVisitResumeViewModel.LabelScoreName = VisitState.myVisitViewModel.LabelScoreName;
        VisitState.myVisitResumeViewModel.LabelScoreNameColor = VisitState.myVisitViewModel.LabelScoreNameColor;

        Score = VisitState.myVisitResumeViewModel.SliderValue;
    }

    void Slider_ValueChanged(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
    {
        var slider = sender as Slider;

        slider.ThumbColor = Visit.SCORE_LEGEND_COLOR;
        slider.MinimumTrackColor = Visit.SCORE_LEGEND_COLOR;
        VisitState.myVisitResumeViewModel.LabelScoreColor = Visit.SCORE_LEGEND_COLOR;
        VisitState.myVisitResumeViewModel.LabelScoreName = Visit.SCORE_LEGEND_NAME;

        //var COLOR_TERRIBLE = Colors.Red;
        //var COLOR_BAD = Colors.Orange;
        //var COLOR_GOOD = Color.FromHex("#ffd966");
        //var COLOR_EXCELLENT = Colors.Green;

        //var TEXT_TERRIBLE = "Péssima";
        //var TEXT_BAD = "Ruim";
        //var TEXT_GOOD = "Boa";
        //var TEXT_EXCELLENT = "Ótima";


        //if (slider != null)
        //{
        //    var value = slider.Value;

        //    if (value <= 25)
        //    {
        //        slider.ThumbColor = COLOR_TERRIBLE;
        //        slider.MinimumTrackColor = COLOR_TERRIBLE;
        //        myVisitResumeViewModel.LabelScoreColor = COLOR_TERRIBLE;
        //        myVisitResumeViewModel.LabelScoreName = TEXT_TERRIBLE;
        //    }
        //    else if (value > 25 && value <= 50)
        //    {
        //        slider.ThumbColor = COLOR_BAD;
        //        slider.MinimumTrackColor = COLOR_BAD;
        //        myVisitResumeViewModel.LabelScoreColor = COLOR_BAD;
        //        myVisitResumeViewModel.LabelScoreName = TEXT_BAD;
        //    }
        //    else if (value > 50 && value <= 75)
        //    {
        //        slider.ThumbColor = COLOR_GOOD;
        //        slider.MinimumTrackColor = COLOR_GOOD;
        //        myVisitResumeViewModel.LabelScoreColor = COLOR_GOOD;
        //        myVisitResumeViewModel.LabelScoreName = TEXT_GOOD;
        //    }
        //    else if (value > 75 && value <= 100)
        //    {
        //        slider.ThumbColor = COLOR_EXCELLENT;
        //        slider.MinimumTrackColor = COLOR_EXCELLENT;
        //        myVisitResumeViewModel.LabelScoreColor = COLOR_EXCELLENT;
        //        myVisitResumeViewModel.LabelScoreName = TEXT_EXCELLENT;
        //    }
        //}
    }

    private void LoadResumeScoreMetrics()
    {
        RegisterLog("LoadResumeScoreMetrics");

        VisitState.myVisitResumeViewModel.ScoreMaxMetricShelf = (int)(Visit.WEIGHT_SHELF * 100);
        var myShelfScore = (int)Math.Round(VisitState.VisitDataShelfSaved?.Sum(x => x.GradeWeight ?? 0) ?? 0);
        VisitState.myVisitResumeViewModel.MyScoreMetricShelf = myShelfScore;

        VisitState.myVisitResumeViewModel.ScoreMaxMetricStock = (int)(Visit.WEIGHT_STOCK * 100);
        var myStockScore = (int)Math.Round(VisitState.VisitDataSKUSaved?.Sum(x => x.GradeWeight) ?? 0);
        VisitState.myVisitResumeViewModel.MyScoreMetricStock = myStockScore;

        VisitState.myVisitResumeViewModel.ScoreMaxMetricDisplay = (int)(Visit.WEIGHT_DISPLAY * 100);
        var myDisplayScore = (int)Math.Round(VisitState.VisitDataMerchandisingSaved?.Sum(x => x.GradeWeight) ?? 0);
        VisitState.myVisitResumeViewModel.MyScoreMetricDisplay = myDisplayScore;

        VisitState.myVisitResumeViewModel.ScoreMaxMetricAction = (int)(Visit.WEIGHT_ACTION * 100);
        var myActionScore = (int)Math.Round(VisitState.VisitDataActionSaved?.Sum(x => x.GradeWeight) ?? 0);
        VisitState.myVisitResumeViewModel.MyScoreMetricAction = myActionScore;
    }

    private async Task BindMetricShelf()
    {
        RegisterLog("BindMetricShelf");

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

        Device.BeginInvokeOnMainThread(() =>
        {
            VisitState.myVisitResumeViewModel.VisitResumeShelfScoreViewModels = ScoreShelfVM;
            VisitState.myVisitResumeViewModel.IsLoadingMetricShelf = false;
        });

    }

    private async Task BindMetricStock()
    {
        RegisterLog("BindMetricStock");

        var MetricName = "Stock";
        var ScoreStockVM = (from s in LP_SCORE_SKUS
                            join m in METRICs.Where(w => w.MetricType == MetricName) on s.MetricID equals m.MetricID
                            join sku in SKUs on s.SKUID equals sku.SKUID
                            join b in BRANDs on sku.BrandID equals b.BrandID
                            select new VisitResumeScoreViewModel()
                            {
                                BrandID = b.BrandID,
                                BrandName = b.Name,
                                SKUID = sku.SKUID,
                                SKUName = sku.Name,
                                MetricID = m.MetricID,
                                MetricName = m.Name,
                                Score = s.Score * WEIGHT_STOCK
                            }).OrderBy(o => o.BrandName).ToList();

        foreach (var item in ScoreStockVM)
        {
            var isOk = VisitState.VisitDataSKUSaved.Any(x => x.BrandID == item.BrandID && x.SKUID == item.SKUID);
            item.ImageOpacity = isOk ? 1 : 0.2;
            item.LineColor = isOk ? Colors.Green : Colors.DarkGray;
        }

        Device.BeginInvokeOnMainThread(() =>
        {
            VisitState.myVisitResumeViewModel.VisitResumeStockScoreViewModels = ScoreStockVM;
            VisitState.myVisitResumeViewModel.IsLoadingMetricStock = false;
        });

    }

    private async Task BindMetricDisplayAsync()
    {
        RegisterLog("BindMetricDisplayAsync");

        var MetricName = "Display";
        var ScoreDisplayVM = (from sb in LP_SCORE_BRANDs
                              join m in METRICs.Where(w => w.MetricType == MetricName) on sb.MetricID equals m.MetricID
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
    }

    private async Task BindMetricAction()
    {
        RegisterLog("BindMetricAction");

        var MetricName = "Action";
        var ScoreActionVM = (from t in TAG_MERCHANDISING_ACAOs.Where(w => w.TagID == TagID && w.MetricTypeCode == MetricName)
                             join sb in LP_SCORE_BRANDs.Where(w => w.TagID == TagID) on t.MetricID equals sb.MetricID
                             join m in METRICs.Where(w => w.MetricType == MetricName) on sb.MetricID equals m.MetricID
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

        Device.BeginInvokeOnMainThread(() =>
        {
            VisitState.myVisitResumeViewModel.VisitResumeActionScoreViewModels = ScoreActionVM;
            VisitState.myVisitResumeViewModel.IsLoadingMetricAction = false;
        });
    }


    private void LoadResumeObjectives()
    {
        RegisterLog("LoadResumeObjectives");

        VisitState.myVisitResumeViewModel.VisitResumeObjectivesViewModels = new();

        DAL.TagBrandPontoNatural TagBrandPontoNaturalDAL = new();
        DAL.Brand BrandDAL = new();

        var brands = from x in TagBrandPontoNaturalDAL.GetAll().Where(w => w.TagID == TagID && w.MetricTypeCode is null)
                     join b in BrandDAL.GetAll() on x.BrandID equals b.BrandID
                     select b;

        foreach (var brand in brands)
        {
            var item = new VisitResumeObjectivesViewModel
            {
                BrandID = brand.BrandID,
                BrandName = brand.Name
            };

            //LoadPendences(item);

            VisitState.myVisitResumeViewModel.VisitResumeObjectivesViewModels.Add(item);
        }
    }

    private void LoadPendences(VisitResumeObjectivesViewModel item)
    {
        RegisterLog("LoadPendences");

        DAL.ObjectiveBrand ObjectiveBrandDAL = new();
        DAL.Metric MetricDAL = new();

        var objectives = (from ob in ObjectiveBrandDAL.GetAll().Where(w => w.POSCode == POSCode)
                          join m in MetricDAL.GetAll() on ob.MetricID equals m.MetricID
                          where ob.BrandID == item.BrandID
                          select new
                          {
                              MetricType = m.MetricType,
                              MetricTypeName = m.Name
                          }).ToList();

        var objectivesClone = objectives.Select(o => new
        {
            MetricType = o.MetricType,
            MetricTypeName = o.MetricTypeName
        }).ToList();

        foreach (var objective in objectives)
        {
            if (objective.MetricType == "Shelf")
            {
                var ShelfSavedCount = VisitState.VisitDataShelfSaved.Where(w => w.BrandID == item.BrandID).Count();
                if (ShelfSavedCount > 0)
                {
                    objectivesClone.RemoveAll(w => w.MetricType == "Shelf");
                }
            }
            if (objective.MetricType == "Display")
            {
                var DisplaysSaved = VisitState.VisitDataMerchandisingSaved.Where(w => w.BrandID == item.BrandID).ToList();
                if (DisplaysSaved != null && DisplaysSaved.Count > 0)
                {
                    foreach (var item_saved in DisplaysSaved)
                    {
                        objectivesClone.RemoveAll(o => o.MetricTypeName == item_saved.Name);
                    }
                }
            }
            if (objective.MetricType == "Action")
            {
                var ActionsSaved = VisitState.VisitDataActionSaved.Where(w => w.BrandID == item.BrandID).ToList();
                if (ActionsSaved != null && ActionsSaved.Count > 0)
                {
                    foreach (var item_saved in ActionsSaved)
                    {
                        objectivesClone.RemoveAll(o => o.MetricTypeName == item_saved.Name);
                    }
                }
            }
        }


        item.QtdPendences = objectivesClone.Count();
        item.Warnning = item.QtdPendences > 0 ? $"{item.QtdPendences} pendência(s)" : string.Empty;
        item.AllowJustify = objectivesClone.Any(o => o.MetricType == "Display");
        item.ImageOpacity = item.AllowJustify ? 1 : 0.1;

    }

    async void btnVisitCancel_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnVisitCancel_Clicked");

        await Shell.Current.GoToAsync("//VisitList");
    }

    async void btnVisitContinue_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnVisitContinue_Clicked");

        await Shell.Current.GoToAsync("//VisitList/Visit");
    }

    async void btnVisitFinish_Clicked(System.Object sender, System.EventArgs e)
    {
        RegisterLog("btnVisitFinish_Clicked");

        if (VisitState.myVisitResumeViewModel.IsQualityCheckEnable)
        {
            if (VisitState.myVisitResumeViewModel.QualityCheck.IsPhotoOK == false)
            {
                await DisplayAlert("Quality Check", $"Registre a foto do produto selecionado para quality check!\n\n{VisitState.myVisitResumeViewModel.QualityCheck.BrandName} - {VisitState.myVisitResumeViewModel.QualityCheck.Name}", "Ok");
                return;
            }
        }

        DAL.Visit VisitDAL = new();
        DateTime visit_date = DateTime.Now;
        string visit_date_string = visit_date.ToString("yyyy-MM-dd HH:mm:ss");
        string photosDirectory = Guid.NewGuid().ToString();
        TopSpaceMAUI.Util.FileSystem.CreateDirectory(photosDirectory);

        // VISIT
        VisitDAL.InsertVisit(new Model.Visit
        {
            POSCode = POSCode,
            Category = POS.Category,
            VisitDate = visit_date_string,
            Score = Score,
            Latitude = 0,
            Longitude = 0,
            Precision = 0,
            PhotosDirectory = photosDirectory,
            DatabaseVersion = visit_date_string,
            Status = Config.STATUS_VISIT_STARTED
        });


        // SHELF + DISPLAY + ACTION
        DAL.VisitDataBrand VisitDataBrandDAL = new();
        List<Model.VisitDataBrand> lstDataBrand = new();

        // SKU
        DAL.VisitDataSKU VisitDataSKUDAL = new();
        List<Model.VisitDataSKU> lstDataSKU = new();

        // QUIZ
        DAL.VisitDataQuiz VisitDataQuizDAL = new();
        List<Model.VisitDataQuiz> lstDataQuiz = new();


        // SKU
        var sku_saveds = VisitState.VisitDataSKUSaved;
        foreach (var sku in sku_saveds)
        {
            var item_sku = new Model.VisitDataSKU()
            {
                BrandName = sku.BrandName,
                Grade = (int)Math.Round(sku.Grade),
                GradeWeight = sku.GradeWeight,
                MetricID = sku.MetricID,
                Name = string.Empty,
                POSCode = POSCode,
                Score = sku.Value,
                Value = sku.Value,
                VisitDate = visit_date_string,
                SKUID = sku.SKUID
            };

            lstDataSKU.Add(item_sku);
        }
        VisitDataSKUDAL.InsertAll(lstDataSKU);


        // SHELF
        var shelf_saveds = VisitState.VisitDataShelfSaved;
        foreach (var shelf in shelf_saveds)
        {
            shelf.SavePhotoAsync(photosDirectory);

            var item_shelf = new Model.VisitDataBrand()
            {
                BrandID = shelf.BrandID,
                BrandName = shelf.BrandName,
                CompetitorValue = 0,
                DocumentsDirectory = photosDirectory,
                ExecutionContextID = null,
                Grade = shelf.Grade,
                GradeWeight = shelf.GradeWeight,
                MetricID = shelf.MetricID,
                MetricType = string.Empty,
                Name = string.Empty,
                Objective = shelf.Objective,
                POSCode = POSCode,
                Score = shelf.Grade ?? 0,
                Value = shelf.Grade ?? 0,
                VisitDate = visit_date_string
            };

            lstDataBrand.Add(item_shelf);
        }


        // MERCHANDISING
        var merchandising_saveds = VisitState.VisitDataMerchandisingSaved;
        foreach (var merchandising in merchandising_saveds)
        {
            merchandising.SavePhotoAsync(photosDirectory);

            var item_shelf = new Model.VisitDataBrand()
            {
                BrandID = merchandising.BrandID,
                BrandName = merchandising.BrandName,
                CompetitorValue = 0,
                DocumentsDirectory = photosDirectory,
                ExecutionContextID = merchandising.ExecutionSelectedValue != null ? merchandising.ExecutionSelectedValue.ExecutionContextID : null,
                Grade = (int)Math.Round(merchandising.Grade),
                GradeWeight = merchandising.GradeWeight,
                MetricID = merchandising.MetricID,
                MetricType = string.Empty,
                Name = string.Empty,
                Objective = merchandising.Objective,
                POSCode = POSCode,
                Score = (int)Math.Round(merchandising.Grade),
                Value = merchandising.Value,
                VisitDate = visit_date_string
            };

            lstDataBrand.Add(item_shelf);
        }


        // ACTIONS
        var action_saveds = VisitState.VisitDataActionSaved;
        foreach (var action in action_saveds)
        {
            action.SavePhotoAsync(photosDirectory);

            var item_action = new Model.VisitDataBrand()
            {
                BrandID = action.BrandID,
                BrandName = action.BrandName,
                CompetitorValue = 0,
                DocumentsDirectory = photosDirectory,
                ExecutionContextID = null,
                Grade = (int)Math.Round(action.Grade),
                GradeWeight = action.GradeWeight,
                MetricID = action.MetricID,
                MetricType = string.Empty,
                Name = string.Empty,
                Objective = action.Objective,
                POSCode = POSCode,
                Score = (int)Math.Round(action.Grade),
                Value = action.Value,
                VisitDate = visit_date_string
            };

            lstDataBrand.Add(item_action);
        }
        VisitDataBrandDAL.InsertAll(lstDataBrand);


        // QUIZ
        var cwid = await SecureStorage.GetAsync("CWID");
        var quiz_saveds = VisitState.VisitQuizSaved;
        foreach (var quiz in quiz_saveds)
        {
            var item_quiz = new Model.VisitDataQuiz()
            {
                POSCode = POSCode,
                VisitDate = visit_date_string,
                QuizID = quiz.QuizID,
                QuizTypeID = quiz.QuizTypeID,
                Question = quiz.Question,
                AnswerValue = quiz.Options.Where(w => w.IsOptionSelected).FirstOrDefault().OptionID,
                Answer = quiz.Options.Where(w => w.IsOptionSelected).FirstOrDefault().OptionDescription,
                REP = cwid
            };

            lstDataQuiz.Add(item_quiz);
        }
        VisitDataQuizDAL.InsertAll(lstDataQuiz);


        // QUALITY CHECK
        if (VisitState.myVisitResumeViewModel.IsQualityCheckEnable)
        {
            var visit_date_qualitycheck = visit_date.ToString("dd-MM-yyyy_HH-mm-ss");
            var visit_quality_check = VisitState.myVisitResumeViewModel.QualityCheck;
            if (visit_quality_check != null)
            {
                var photo_path_saved = await visit_quality_check.SavePhotoAsync(POSCode, visit_date_qualitycheck);

                DAL.VisitPhotoQueueQualityCheck VisitPhotoQueueQualityCheckDAL = new();
                VisitPhotoQueueQualityCheckDAL.Insert(new Model.VisitPhotoQualityCheckQueue()
                {
                    POSCode = POSCode,
                    VisitDate = visit_date_string,
                    MetricID = 1,
                    BrandID = visit_quality_check.BrandID,
                    SKUID = visit_quality_check.SKUID,
                    PhotoID = 1,
                    PhotoDirectory = "/diretorio/",
                    Photo = photo_path_saved
                });
            }
        }


        // GET TOTAL PHOTOS TAKEN
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var current_path = Path.Combine(folderPath, photosDirectory);
        var total_photo_taken = Directory.GetFiles(current_path).Count();


        // VISIT UPDATE
        var visit_current = VisitDAL.GetVisit().Where(w => w.POSCode == POSCode && w.VisitDate == visit_date_string).FirstOrDefault();
        visit_current.Status = Config.STATUS_VISIT_COMPLETED;
        visit_current.PhotoTaken = total_photo_taken;
        VisitDAL.UpdateVisit(visit_current);

        ClearVisitObjects();
        ClearVisitResumeObjects();

        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        await Shell.Current.GoToAsync("///VisitList");
    }

    private static void RegisterLog(string method_name)
    {
        if (ALLOW_REGISTER_LOG)
        {
            Util.LogUnhandledException.LogError($"{DateTime.Now} | {POSCode} | {Util.MemoryRAM.CurrentValue} | {method_name}");
        }
    }

    private void ClearVisitObjects()
    {
        Visit.POS = null;
        Visit.Brands = null;

        //Visit.LP_SCORE_SKUS = null;
        //Visit.OBJECTIVE_BRANDS = null;
        //Visit.LP_SCORE_BRANDS = null;
        //Visit.SKUs = null;
        //Visit.TAG_PRESENCAs = null;
        //Visit.METRICS = null;
        //Visit.TAG_MERCHANDISING_ACAOs = null;
        //Visit.LP_GRADE = null;

        //Visit.myVisitViewModel = null;
        //Visit.VisitDataShelfSaved = null;
        //Visit.VisitDataSKUSaved = null;
        //Visit.VisitDataMerchandisingSaved = null;
        //Visit.VisitDataActionSaved = null;
        //Visit.VisitQuizSaved = null;

        VisitState.Clear();
    }

    private void ClearVisitResumeObjects()
    {
        LP_SCORE_BRANDs = null;
        METRICs = null;
        BRANDs = null;
        TAG_MERCHANDISING_ACAOs = null;
        LP_SCORE_SKUS = null;
        SKUs = null;
        //myVisitResumeViewModel = null;
        myListObjectives = null;
    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    ClearVisitObjects();
    //    ClearVisitResumeObjects();

    //    //GC.Collect();
    //    //GC.WaitForPendingFinalizers();
    //}
}