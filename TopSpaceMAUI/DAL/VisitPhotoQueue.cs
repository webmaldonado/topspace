using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class VisitPhotoQueue
	{
        public List<Model.VisitPhotoQueue> GetVisitPhotoQueue(SQLiteConnection db)
        {
            List<Model.VisitPhotoQueue> visitPhotoQueue = db.Table<Model.VisitPhotoQueue>().ToList();
            return visitPhotoQueue;
        }
        //public List<Model.Visit> GetVisitPhotoQueueQualityCheck(SQLiteConnection db)
        //{
        //    List<Model.Visit> visitPhotoQueueQualityCheck = db.Table<Model.Visit>().ToList();
        //    return visitPhotoQueueQualityCheck;
        //}

        //public string GetMetricName(int metricID)
        //{
        //    SQLiteConnection db = Database.GetNewConnection();
        //    return db.ExecuteScalar<string>(String.Format("SELECT Name FROM Metric WHERE MetricID = {0}", metricID));
        //}

        //public Model.VisitPhotoQueue GetRandomPhotoQueue(string POSCode)
        //{
        //    var lista = GetVisitPhotoQueue();
        //    var indice = new Random().Next(1, lista.Count);
        //    return lista[indice];
        //}

        //public static int GetRandomPhotoQueue(string POSCode)
        //{
        //    string sql_random = String.Format("SELECT M.MetricID FROM Metric as M INNER JOIN VisitPhotoQueue as V ON V.MetricID=M.MetricID WHERE V.POSCode='{0}' ORDER BY RANDOM()", POSCode);
        //    SQLiteConnection db = Database.GetNewConnection();
        //    int pos_objetive = db.ExecuteScalar<int>(sql_random);

        //    return pos_objetive;
        //}

        public List<Model.VisitPhotoQueue> GetVisitPhotoQueue ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.VisitPhotoQueue> visitPhotoQueue = GetVisitPhotoQueue(db);
			Database.Close (db);
			db = null;
			return visitPhotoQueue;
		}

		public void Insert (Model.VisitPhotoQueue visitPhoto)
		{
			//OBS: Como a chave primária visitPhoto.SKUID é sempre null, a inserção de valores duplicados com a mesma chave primária deve ser impedida manualmente
			if (GetVisitPhotoQueue ().Where (v => v.POSCode == visitPhoto.POSCode && v.VisitDate == visitPhoto.VisitDate && v.MetricID == visitPhoto.MetricID && v.BrandID == visitPhoto.BrandID && v.SKUID == visitPhoto.SKUID && v.PhotoID == visitPhoto.PhotoID).Count() == 0) {
                SQLiteConnection db = Database.GetNewConnection();
                db.Insert (visitPhoto);
				Database.Close (db);
				db = null;
			}
		}
			
		public void Delete (SQLiteConnection db, Model.VisitPhotoQueue visitPhoto, bool deleteFile = false)
		{
			//OBS: visitPhoto.SKUID é sempre null porque não há fotos de métricas de SKU
			string query = String.Format ("DELETE FROM VisitPhotoQueue WHERE POSCode = '{0}' AND VisitDate = '{1}' AND MetricID = '{2}' AND BrandID = '{3}' AND SKUID {4} AND PhotoID = '{5}'", visitPhoto.POSCode, visitPhoto.VisitDate, visitPhoto.MetricID, visitPhoto.BrandID, "IS NULL", visitPhoto.PhotoID);
			db.Execute (query);

			if (deleteFile) {
				DeletePhoto (visitPhoto);
			}
		}

        //public int GetRandom(string posCode)
        //{
        //    using (SQLiteConnection db = Database.GetNewConnection())
        //    {
        //        return db.ExecuteScalar<int>(String.Format("select MetricID FROM VisitPhotoQueue WHERE POSCode = '{0}' order by random() limit 1", posCode));
        //    }
        //}

        public void GiveUpPhoto (SQLiteConnection db)
		{
			try {
				List<Model.VisitPhotoQueue> lstVisitPhotoQueue = GetVisitPhotoQueue (db).ToList ();
				foreach (Model.VisitPhotoQueue visitPhotoQueue in lstVisitPhotoQueue) {
					List<Model.VisitPhotoQueue> tempListVisit = lstVisitPhotoQueue.Where (v => v.POSCode == visitPhotoQueue.POSCode && v.VisitDate == visitPhotoQueue.VisitDate).ToList ();
					int sampleVisit = tempListVisit.Select (v => v.SampleVisit).Sum ();
					if (((DateTime.Now - Convert.ToDateTime (visitPhotoQueue.VisitDate)).Days > 7) && (sampleVisit > visitPhotoQueue.SampleCategory)) {
						Delete (db, visitPhotoQueue, true);
					}
				}
			} catch (Exception ex) {
				Model.Sync.LogInfo (Localization.TryTranslateText ("StageVisitPhoto") + String.Format (Localization.TryTranslateText ("GiveUpPhotoFail"), ex.Message));
			}
		}

		/// <summary>
		/// Selects the photo. Critérios:
		/// 1º Amostra mínima
		/// 2º Categoria e data de visita mais antiga - escolha aleatória
		/// (envia todas que fazem parte da amostra obrigatória)
		/// 3º Foto extra 
		/// 4º Categoria e data de visita mais antiga - escolha aleatória  
		/// </summary>
		/// <returns>The photo.</returns>
		public Model.VisitPhotoQueue SelectPhoto (SQLiteConnection db)
		{	
//			Console.WriteLine ("SelectPhoto");
			Model.VisitPhotoQueue visit = null;

			try {
				List<Model.VisitPhotoQueue> lstVisitPhotoQueue = GetVisitPhotoQueue (db).ToList ();
				List<Model.VisitPhotoQueue> lstVisit = new List<Model.VisitPhotoQueue> ();
				int sampleVisit = 0;

				// Percorrer a lista de visitas e verificar quais a amostra mínima não foi enviada
				foreach (Model.VisitPhotoQueue tempVisit in lstVisitPhotoQueue) {
					List<Model.VisitPhotoQueue> tempLstVisit = lstVisitPhotoQueue.Where (v => v.POSCode == tempVisit.POSCode && v.VisitDate == tempVisit.VisitDate).ToList ();
					sampleVisit = tempLstVisit.Select (v => v.SampleVisit).Sum ();
					// Amostra mínima
					if (sampleVisit <= tempVisit.SampleCategory) {
						lstVisit.Add (tempVisit);
					}
				}

				// Extra
				if (lstVisit.Count == 0) {
					lstVisit = lstVisitPhotoQueue;
				}

				// Selecionar uma foto da lista de fotos ordenadas por categoria e data da visita
				lstVisit.OrderBy (v => v.Category).ThenBy (v => Convert.ToDateTime (v.VisitDate)).ToList ();
				var rand = new Random ();
				visit = lstVisit.ElementAt (rand.Next (lstVisit.Count ()));
				return visit;
			} catch (Exception ex) {
				Model.Sync.LogInfo (Localization.TryTranslateText ("StageVisitPhoto") + String.Format (Localization.TranslateText ("SelectPhotoFail"), ex.Message));
				return visit;
			}
		}

		public void DeletePhoto (Model.VisitPhotoQueue visitPhotoQueue)
		{
			Util.FileSystem.DeleteFile (visitPhotoQueue.Photo, visitPhotoQueue.PhotoDirectory);
		}
	}
}