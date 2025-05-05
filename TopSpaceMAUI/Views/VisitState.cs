using System;
using System.Collections.Generic;
using TopSpaceMAUI.ViewModel;

namespace TopSpaceMAUI
{
    public static class VisitState
    {
        public static List<Model.LPScoreSKU> LP_SCORE_SKUS;
        public static List<Model.ObjectiveBrand> OBJECTIVE_BRANDS;
        public static List<Model.LPScoreBrand> LP_SCORE_BRANDS;
        public static List<Model.SKU> SKUs;
        public static List<Model.TagPresenca> TAG_PRESENCAs;
        public static List<Model.Metric> METRICS;
        public static List<Model.TagMerchandisingAcao> TAG_MERCHANDISING_ACAOs;
        public static List<Model.LPGrade> LP_GRADE;

        public static VisitViewModel myVisitViewModel { get; set; } = new();
        //private static WeakReference<VisitViewModel>? _myVisitViewModel;
        //public static VisitViewModel? myVisitViewModel
        //{
        //    get
        //    {
        //        if (_myVisitViewModel != null && _myVisitViewModel.TryGetTarget(out var vm))
        //            return vm;

        //        return new();
        //    }
        //    set
        //    {
        //        if (value != null)
        //            _myVisitViewModel = new WeakReference<VisitViewModel>(value);
        //        else
        //            _myVisitViewModel = new(null);
        //    }
        //}

        public static List<VisitDataShelfViewModel> VisitDataShelfSaved { get; set; } = new();
        public static List<VisitDataSKUViewModel> VisitDataSKUSaved { get; set; } = new();
        public static List<VisitDataMerchandisingViewModel> VisitDataMerchandisingSaved { get; set; } = new();
        public static List<VisitDataActionViewModel> VisitDataActionSaved { get; set; } = new();
        public static List<VisitQuizViewModel> VisitQuizSaved { get; set; } = new();

        public static VisitResumeViewModel myVisitResumeViewModel { get; set; } = new();

        public static void Clear()
        {
            LP_SCORE_SKUS = null;
            OBJECTIVE_BRANDS = null;
            LP_SCORE_BRANDS= null;
            SKUs = null;
            TAG_PRESENCAs = null;
            METRICS = null;
            TAG_MERCHANDISING_ACAOs= null;
            LP_GRADE = null;

            myVisitViewModel = null;
            VisitDataShelfSaved = null;
            VisitDataSKUSaved = null;
            VisitDataMerchandisingSaved = null;
            VisitDataActionSaved = null;
            VisitQuizSaved = null;

            myVisitResumeViewModel = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}