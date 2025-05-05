using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public class VisitPhotoQueueQualityCheck
    {
        public List<Model.VisitPhotoQualityCheckQueue> GetVisitPhotoQualityCheckQueue(SQLiteConnection db)
        {
            return db.Table<Model.VisitPhotoQualityCheckQueue>().ToList();
        }

        //public static string GetRandomPhotoQueue(string POSCode)
        //{
        //    string sql_random = String.Format("SELECT m.Name FROM Metric m INNER JOIN VisitPhotoQualityCheckQueue v ON v.MetricID = m.MetricID WHERE v.POSCode='{0}' ORDER BY RANDOM()", POSCode);
        //    SQLiteConnection db = Database.GetNewConnection();     
        //    string pos_objetive = db.ExecuteScalar<string>(sql_random);
        //    pos_objetive = "Display de Balcão";
        //    return pos_objetive;
        //}

		public List<Model.VisitPhotoQualityCheckQueue> GetVisitPhotoQualityCheckQueue ()
		{
            using (SQLiteConnection db = Database.GetNewConnection())
            {
                return GetVisitPhotoQualityCheckQueue(db);
            }
		}

		public void Insert (Model.VisitPhotoQualityCheckQueue visitPhoto)
		{
            //OBS: Como a chave primária visitPhoto.SKUID é sempre null, a inserção de valores duplicados com a mesma chave primária deve ser impedida manualmente
            if (GetVisitPhotoQualityCheckQueue().Where(v => v.POSCode == visitPhoto.POSCode && v.VisitDate == visitPhoto.VisitDate && v.MetricID == visitPhoto.MetricID &&
                v.BrandID == visitPhoto.BrandID && v.SKUID == visitPhoto.SKUID && v.PhotoID == visitPhoto.PhotoID).Count() == 0)
            {
                using (SQLiteConnection db = Database.GetNewConnection())
                {
                    db.Insert(visitPhoto);
                }
            }
		}
			
		public void Delete (SQLiteConnection db, Model.VisitPhotoQualityCheckQueue visitPhoto, bool deleteFile = false)
		{
			string query = String.Format ("DELETE FROM VisitPhotoQualityCheckQueue WHERE POSCode = '{0}' AND VisitDate = '{1}' AND MetricID = '{2}' AND BrandID = '{3}' AND SKUID = {4} AND PhotoID = '{5}'", visitPhoto.POSCode, visitPhoto.VisitDate, visitPhoto.MetricID, visitPhoto.BrandID, visitPhoto.SKUID, visitPhoto.PhotoID);
			db.Execute (query);

			if (deleteFile) {
				DeletePhoto (visitPhoto);
			}
		}

        public void GiveUpPhoto(SQLiteConnection db)
        {
            try
            {
                List<Model.VisitPhotoQualityCheckQueue> lstVisitPhotoQueue = GetVisitPhotoQualityCheckQueue(db).ToList();
                foreach (Model.VisitPhotoQualityCheckQueue visitPhotoQueue in lstVisitPhotoQueue)
                {
                    List<Model.VisitPhotoQualityCheckQueue> tempListVisit = lstVisitPhotoQueue.Where(v => v.POSCode == visitPhotoQueue.POSCode &&
                       v.VisitDate == visitPhotoQueue.VisitDate).ToList();
                    int sampleVisit = tempListVisit.Select(v => v.SampleVisit).Sum();
                    if (((DateTime.Now - Convert.ToDateTime(visitPhotoQueue.VisitDate)).Days > 7) && (sampleVisit > visitPhotoQueue.SampleCategory))
                    {
                        Delete(db, visitPhotoQueue, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhotoQualityCheck") + String.Format(Localization.TryTranslateText("GiveUpPhotoFail"), ex.Message));
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
		public Model.VisitPhotoQualityCheckQueue SelectPhoto(SQLiteConnection db)
        {
            Model.VisitPhotoQualityCheckQueue visit = null;
            try
            {
                List<Model.VisitPhotoQualityCheckQueue> lstVisitPhotoQualityCheckQueue = GetVisitPhotoQualityCheckQueue(db).ToList();
                List<Model.VisitPhotoQualityCheckQueue> lstVisitQualityCheck = new List<Model.VisitPhotoQualityCheckQueue>();
                int sampleVisitQualityCheck = 0;

                // Percorrer a lista de visitas e verificar quais a amostra mínima não foi enviada
                foreach (Model.VisitPhotoQualityCheckQueue tempVisitQualityCheck in lstVisitPhotoQualityCheckQueue)
                {
                    List<Model.VisitPhotoQualityCheckQueue> tempLstVisitQualityCheck = lstVisitPhotoQualityCheckQueue.Where(v => v.POSCode == tempVisitQualityCheck.POSCode &&
                        v.VisitDate == tempVisitQualityCheck.VisitDate).ToList();
                    sampleVisitQualityCheck = tempLstVisitQualityCheck.Select(v => v.SampleVisit).Sum();
                    // Amostra mínima
                    if (sampleVisitQualityCheck <= tempVisitQualityCheck.SampleCategory)
                    {
                        lstVisitQualityCheck.Add(tempVisitQualityCheck);
                    }
                }

                // Extra
                if (lstVisitQualityCheck.Count == 0)
                {
                    lstVisitQualityCheck = lstVisitPhotoQualityCheckQueue;
                }

                // Selecionar uma foto da lista de fotos ordenadas por categoria e data da visita
                lstVisitQualityCheck.OrderBy(v => v.Category).ThenBy(v => Convert.ToDateTime(v.VisitDate)).ToList();
                var rand = new Random();
                visit = lstVisitQualityCheck.ElementAt(rand.Next(lstVisitQualityCheck.Count()));
                return visit;
            }
            catch (Exception ex)
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhotoQualityCheck") + String.Format(Localization.TranslateText("SelectPhotoFail"), ex.Message));
                return visit;
            }
        }

        public void DeletePhoto(Model.VisitPhotoQualityCheckQueue visitPhotoQueue)
        {
            Util.FileSystem.DeleteFile(visitPhotoQueue.Photo, visitPhotoQueue.PhotoDirectory);
        }
	}
}