using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI.Util;
using System.IO;


namespace TopSpaceMAUI.DAL
{
	public class Visit
	{
		private VisitDataBrand visitDataBrandController;
		private VisitDataSKU visitDataSKUController;
		private VisitDataTrackPrice visitDataTrackPriceController;
		private VisitPhotoQueue visitPhotoQueueController;
        private VisitPhotoQueueQualityCheck visitPhotoQueueQualityCheckController;
        private VisitDataQuiz visitDataQuizController;

        public Visit ()
		{
			visitDataBrandController = new VisitDataBrand ();
			visitDataSKUController = new VisitDataSKU ();
			visitDataTrackPriceController = new VisitDataTrackPrice ();
			visitPhotoQueueController = new VisitPhotoQueue ();
            visitPhotoQueueQualityCheckController = new VisitPhotoQueueQualityCheck();
            visitDataQuizController = new VisitDataQuiz();
        }

		public List<Model.Visit> GetVisit ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.Visit> visit = db.Table<Model.Visit> ().ToList ();
			Database.Close (db);
			db = null;
			return visit;
		}

		public List<Model.Visit> GetVisitData ()
		{
			Model.Visit visit = GetVisit ().Where (v => v.Status == Config.STATUS_VISIT_COMPLETED).OrderBy (v => v.VisitDate).FirstOrDefault ();
			List<Model.VisitDataBrand> lstVisitsDataBrand = null;
			List<Model.VisitDataSKU> lstVisitsDataSKU = null;
			List<Model.VisitDataTrackPrice> lstVisitsDataTrackPrice = null;
            List<Model.VisitDataQuiz> lstVisitsDataQuiz = null;

            lstVisitsDataBrand = visitDataBrandController.GetVisitDataBrand ().Where (v => v.POSCode == visit.POSCode && v.VisitDate == visit.VisitDate).ToList ();
			lstVisitsDataSKU = visitDataSKUController.GetVisitDataSKU ().Where (v => v.POSCode == visit.POSCode && v.VisitDate == visit.VisitDate).ToList ();
			lstVisitsDataTrackPrice = visitDataTrackPriceController.GetVisitDataTrackPrice ().Where (v => v.POSCode == visit.POSCode && v.VisitDate == visit.VisitDate).ToList ();
            lstVisitsDataQuiz = visitDataQuizController.GetVisitDataQuiz().Where(v => v.POSCode == visit.POSCode && v.VisitDate == visit.VisitDate).ToList();

            List<Model.Visit> visitData = new List<Model.Visit> ();
			visitData.Add(new Model.Visit {
				POSCode = visit.POSCode,
				VisitDate = visit.VisitDate,
				Latitude = visit.Latitude,
				Longitude = visit.Longitude,
				Precision = visit.Precision,
				Score = visit.Score,
				DatabaseVersion = visit.DatabaseVersion,
				PhotoTaken = visit.PhotoTaken,
				arrVisitBrand = lstVisitsDataBrand.ToArray (),
				arrVisitSKU = lstVisitsDataSKU.ToArray (),
				arrVisitTrackPrice = lstVisitsDataTrackPrice.ToArray (),
				arrVisitQuiz = lstVisitsDataQuiz.ToArray()
			});

			return visitData;
		}

		public void InsertVisit (Model.Visit visit)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.Insert (visit);
			Database.Close (db);
			db = null;
		}

		public void UpdateVisit (Model.Visit visit)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string latitude, longitude, precision;
			latitude = longitude = precision = "null";
			latitude = visit.Latitude != null ? visit.Latitude.ToString () : latitude;
			longitude = visit.Longitude != null ? visit.Longitude.ToString () : longitude;
			precision = visit.Precision != null ? visit.Precision.ToString () : precision;

			string query = String.Format ("UPDATE Visit SET Latitude = {0}, Longitude = {1}, Precision = {2}, Score = {3}, PhotoTaken = {4}, Status = '{5}' WHERE POSCode = '{6}' AND VisitDate = '{7}'", latitude, longitude, precision, visit.Score, visit.PhotoTaken, visit.Status, visit.POSCode, visit.VisitDate);
			db.Execute (query);

			Database.Close (db);
			db = null;
		}

        //public static void QualityCheck(string POSCode, string QualityCheck)
        //{
        //    SQLiteConnection db = Database.GetNewConnection();

        //    string query = String.Format("UPDATE Visit SET QualityCheck='{1}' WHERE POSCode = '{0}' ",POSCode, QualityCheck);
        //    db.Execute(query);

        //    Database.Close(db);
        //    db = null;
        //}

        public void DeleteAll ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.DeleteAll<Model.Visit> ();
			Database.Close (db);
			db = null;
		}

		public void DeleteVisit (string POSCode, string VisitDate)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format ("DELETE FROM Visit WHERE POSCode = '{0}' AND VisitDate = '{1}'", POSCode, VisitDate);
			db.Execute (query);
			Database.Close (db);
			db = null;
		}

		public SyncStatusCode MoveDataToUploadQueue ()
		{
			//Model.Sync.LogInfo (String.Format (Localization.TryTranslateText ("DateAndTime"), DateTime.Now) + " / " + String.Format (Localization.TryTranslateText ("MATR")) + " / " + String.Format (Localization.TryTranslateText ("ApplicationState"), XNSUserDefaults.GetStringForKey (Config.KEY_APPLICATION_STATE)));
			Model.Sync.LogInfo (Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("MoveDataToUploadQueueStart"));
			SQLiteConnection db = null;

			try {
				List<Model.Visit> lstVisits = GetVisit ().Where (v => v.Status == Config.STATUS_VISIT_SENT).ToList ();
				List<Model.VisitDataBrand> lstVisitsDataBrand = null;
				int countPhoto = 0;
                int countPhotoQualityCheck = 0;

                db = Database.GetNewConnection ();
				db.BeginTransaction();

				if (lstVisits.Count > 0) {

					foreach (Model.Visit visit in lstVisits) {
						if (visit.PhotoTaken > 0) {
						
							// Verificação adicionada para as versões da app que não possuem o campo Category na tabela Visit
							int category = 0; 
							try {
								if (visit.Category != null) {
									category = (int)visit.Category;
								} else {
									category = 4; // Categoria com maior prioridade e maior percentual de amostra de fotos
								}
							} catch {
								category = 4; // Categoria com maior prioridade e maior percentual de amostra de fotos
							}

							lstVisitsDataBrand = visitDataBrandController.GetVisitDataBrand ().Where (v => v.POSCode == visit.POSCode && v.VisitDate == visit.VisitDate).ToList ();
							foreach (Model.VisitDataBrand visitDataBrand in lstVisitsDataBrand) {
								// Montar o nome da foto
								string photoName = visit.PhotosDirectory + Path.DirectorySeparatorChar + String.Format (Config.PHOTO_NAME, visitDataBrand.BrandID, visitDataBrand.MetricID);
                                // Verificar se a foto existe
                                if (Util.FileSystem.CheckFile (photoName)) {
									Model.VisitPhotoQueue visitPhotoQueue = new Model.VisitPhotoQueue {
										POSCode = visit.POSCode,
										VisitDate = visit.VisitDate,
										MetricID = visitDataBrand.MetricID,
										BrandID = visitDataBrand.BrandID,
										SKUID = null,
										PhotoID = 1,
										PhotoDirectory = visit.PhotosDirectory,
										Photo = photoName,
										Category = category, // Categoria para saber a prioridade
										SampleCategory = Util.Category.CategorySample (category), // Preciso de n% dessa categoria
										SampleVisit = 100 / visit.PhotoTaken, // Represento n% dessa visita 
									};
									visitPhotoQueueController.Insert (visitPhotoQueue);
									countPhoto++;
								}
                            }

                            ////string photoNameQC = visit.PhotosDirectory + Path.DirectorySeparatorChar + String.Format(Config.PHOTO_NAME_QUALITY_CHECK, 1, 1);
                            //string photoNameQC = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + String.Format(Config.PHOTO_NAME_QUALITY_CHECK, visit.POSCode);
                            ////Montar o nome da foto do Quality Check
                            //if (Util.FileSystem.CheckFileQualityCheck(photoNameQC))
                            //{
                            //    Model.VisitPhotoQualityCheckQueue visitPhotoQualityCheckQueue = new Model.VisitPhotoQualityCheckQueue
                            //    {
                            //        POSCode = visit.POSCode,
                            //        VisitDate = visit.VisitDate,
                            //        MetricID = 1,
                            //        BrandID = 1,
                            //        SKUID = 1,
                            //        PhotoID = 1,
                            //        PhotoDirectory = "/diretorio/",
                            //        Photo = photoNameQC,
                            //        Category = category, // Categoria para saber a prioridade
                            //        SampleCategory = Util.Category.CategorySample(category), // Preciso de n% dessa categoria
                            //        SampleVisit = 100 / visit.PhotoTaken, // Represento n% dessa visita 
                            //    };
                            //    visitPhotoQueueQualityCheckController.Insert(visitPhotoQualityCheckQueue);
                            //    countPhotoQualityCheck++;
                            //}
                        }
					}

					DeleteAll ();
					visitDataBrandController.DeleteAll ();
					visitDataSKUController.DeleteAll ();
					visitDataTrackPriceController.DeleteAll ();
					visitDataQuizController.DeleteAll();

					db.Commit();
					Model.Sync.LogInfo (Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("MoveDataToUploadQueueFinished"), countPhoto));
					return SyncStatusCode.SaveOK;
				} else {
					db.Commit();
					Model.Sync.LogInfo (Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("MoveDataToUploadQueueNoData"));
					return SyncStatusCode.SaveOK;
				}
			} catch (Exception ex) {
				db.Rollback ();
				Model.Sync.LogInfo (Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("MoveDataToUploadQueueFail"), ex.Message));
				return SyncStatusCode.SaveError;
			} finally {
				Database.Close (db);
				db = null;
			}
		}

		public void UpdateSent (string POSCode, string VisitDate)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format ("UPDATE Visit SET Status = '{0}' WHERE POSCode = '{1}' AND VisitDate = '{2}'", Config.STATUS_VISIT_SENT, POSCode, VisitDate);
			db.Execute (query);
			Database.Close (db);
			db = null;
		}

        //public void UpdateQualityCheck(string POSCode, string img, string VisitDate)
        //{
        //    SQLiteConnection db = Database.GetNewConnection();
        //    string query = String.Format("UPDATE Visit SET QualityCheck = '{0}' WHERE POSCode = '{1}' AND VisitDate = '{2}'", img, POSCode, VisitDate);
        //    db.Execute(query);
        //    Database.Close(db);
        //    db = null;
        //}



	}
}