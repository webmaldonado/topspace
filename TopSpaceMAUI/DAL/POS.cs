using SQLite;
using TopSpaceMAUI.Util;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace TopSpaceMAUI.DAL
{
	public class POS : Syncable<Model.POS, Model.POSTemp>
	{
        private List<Model.Tag> lstTags;
        private List<Model.TagBrandPontoNatural> lstTagBrand;
        private List<Model.TagBrandPontoNatural> lstPontoNatural;
        private List<Model.TagMerchandisingAcao> lstMerchandising;
        private List<Model.Promotion> lstPromotions;
        private int metricIDShelf = 0;
        
        private void LoadLists()
        {
            lstTags = new DAL.Tag().GetAll();
            lstTagBrand = new DAL.TagBrandPontoNatural().GetAll().Where(t => t.MetricTypeCode == null).ToList();
            lstPontoNatural = new DAL.TagBrandPontoNatural().GetAll().Where(t => t.MetricTypeCode == Config.METRIC_TYPE_SHELF_NAME).ToList();
            lstMerchandising = new DAL.TagMerchandisingAcao().GetAll().Where(t => t.MetricTypeCode == Config.METRIC_TYPE_DISPLAY_NAME).ToList();
        }

        private int GetObjetives(string posCode, int tagID, int metricIDShelf)
        {
            int totalObjetives = 0;

            // Listar todas as brands da tag
            var listaBrands = lstTagBrand.Where(t => t.TagID == tagID).ToList();
            var listaObjectiveBrands = new DAL.ObjectiveBrand().GetAll().Where(ob => ob.POSCode == posCode).ToList();

            // Percorrer as brands e verificar quais possuem ponto natural e merchandising
            foreach (var brand in listaBrands)
            {
                // Verificar se existe um ponto natural para a brand selecionada.
                var listaPontoNatural = lstPontoNatural.Where(t => t.TagID == tagID && t.BrandID == brand.BrandID).ToList();

                // Para cada ponto natural encontrado, verificar se existe na tabela de objetivos.
                foreach (var pontoNatural in listaPontoNatural)
                {
                    totalObjetives += listaObjectiveBrands.Where(ob => ob.BrandID == brand.BrandID && ob.MetricID == metricIDShelf).Count();
                }

                // Verificar se existe merchandising para a brand selecionada.
                var listaMerchandising = lstMerchandising.Where(t => t.TagID == tagID && t.BrandID == brand.BrandID).ToList();

                // Para cada merchandising encontrado, verificar se existe na tabela de objetivos.
                foreach (var merchandising in listaMerchandising)
                {
                    totalObjetives += listaObjectiveBrands.Where(ob => ob.BrandID == brand.BrandID && ob.MetricID == merchandising.MetricID).Count();
                }
            }
            return totalObjetives;
        }

        public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOS");
		}



		protected override TopSpaceMAUI.Model.POS ConvertTempToEntity (TopSpaceMAUI.Model.POSTemp temp)
		{
            return new Model.POS()
            {
                POSCode = temp.POSCode,
                ChainID = temp.ChainID,
                ChainType = temp.ChainType,
                Name = temp.Name,
                Address = temp.Address,
                District = temp.District,
                City = temp.City,
                State = temp.State,
                Zipcode = temp.Zipcode,
                Category = temp.Category,
                Latitude = temp.Latitude,
                Longitude = temp.Longitude,
                Precision = temp.Precision,
                TagBaseName = temp.TagBaseName,
				UnitVariation = temp.UnitVariation
			};
		}



		protected override bool KeyMatch (TopSpaceMAUI.Model.POS local, TopSpaceMAUI.Model.POS remote)
		{
			return local.POSCode == remote.POSCode;
		}



		protected override bool HasChanged (TopSpaceMAUI.Model.POS local, TopSpaceMAUI.Model.POS remote)
		{
			return local.ChainID != remote.ChainID ||
			local.ChainType != remote.ChainType ||
			local.Name != remote.Name ||
			local.Address != remote.Address ||
			local.District != remote.District ||
			local.City != remote.City ||
			local.State != remote.State ||
			local.Zipcode != remote.Zipcode ||
			local.Category != remote.Category ||
			local.Latitude != remote.Latitude ||
			local.Longitude != remote.Longitude ||
			local.Precision != remote.Precision ||
			local.TagBaseName != remote.TagBaseName ||
			local.UnitVariation != remote.UnitVariation;
		}



		protected override IOrderedEnumerable<TopSpaceMAUI.Model.POS> OrderBy (IEnumerable<TopSpaceMAUI.Model.POS> source)
		{
			return source.OrderBy (pos => pos.Name).ThenBy (pos => pos.Address);
		}

        public Model.POS GetPOS (string POSCode)
		{
			SQLiteConnection db = null;
			try {
				db = Database.GetNewConnection ();
				return db.Get<Model.POS> (POSCode);
			}
			catch {
				return null;
			}
			finally {
				Database.Close (db);
				db = null;
			}
		}

		public override void Update (TopSpaceMAUI.Model.POS e)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format("UPDATE POS SET Latitude = {0}, Longitude = {1}, Precision = {2} WHERE POSCode = '{3}'", e.Latitude, e.Longitude, e.Precision, e.POSCode);
			db.Execute (query);
			Database.Close (db);
			db = null;
		}

		public List<Model.POS> GetPOSs (bool? completed)
		{
			DAL.VisitCount visitCountController = new DAL.VisitCount ();
			DAL.Visit visitController = new DAL.Visit();
			DAL.POSGps posgpsController = new DAL.POSGps();


            // Visitas realizadas (servidor)
            List<Model.VisitCount> lstVisitCount = visitCountController.GetAll().Where(v => Convert.ToDateTime (v.Period).Year == DateTime.Now.Year && Convert.ToDateTime (v.Period).Month == DateTime.Now.Month).ToList ();

            // Visitas realizadas (local)
            List<Model.Visit> lstLastVisitsLocal = visitController.GetVisit().Where(v => v.Status != Config.STATUS_VISIT_STARTED && Convert.ToDateTime (v.VisitDate).Year == DateTime.Now.Year && Convert.ToDateTime (v.VisitDate).Month == DateTime.Now.Month).ToList ();

            // POS Gps
            List<Model.POSGps> lstPOSGps = posgpsController.GetAll();



            List<Model.POS> POSs = (from pos in GetAll()
                                    join visitCount in lstVisitCount
                                    on pos.POSCode equals visitCount.POSCode into POSVisit
                                    from visit in POSVisit.DefaultIfEmpty()
									//join geo in lstPOSGps
         //                           on pos.POSCode equals geo.POSCode 
                                    select new Model.POS {
                                        POSCode = pos.POSCode,
                                        Name = pos.Name,
										Address = pos.Address,
										City = pos.City,
										Category = pos.Category,
										Latitude = pos.Latitude,
										Longitude = pos.Longitude,
										Precision = pos.Precision,
										//Latitude = geo.Latitude,
										//Longitude = geo.Longitude,
										//Precision = geo.Precision,

										TagBaseName = pos.TagBaseName,
										UnitVariation = pos.UnitVariation,
										VisitCount = (visit == null ? lstLastVisitsLocal.Where (l => l.POSCode == pos.POSCode).Count () : visit.Count + lstLastVisitsLocal.Where (l => l.POSCode == pos.POSCode).Count ()),
                                        TotalObjetivos = pos.TotalObjetivos
									}).ToList ();
                      
            if (completed.HasValue) {
				if (!completed.Value) {
					POSs = POSs.Where (p => p.Category > p.VisitCount).ToList ();
				} else {
					POSs = POSs.Where (p => p.Category <= p.VisitCount).ToList ();
				}
			}

			return POSs;
		}

        public List<Model.POS> OrderNearestPOS(List<Model.POS> lstPOS, Microsoft.Maui.Devices.Sensors.Location location, bool orderByDate = false) 
		{
			int limit = 100; // Limite de distância em metros
			int tolerance = 500; // Tolerância da precisão em metros

			List<Model.POS> lstPOSNear = new List<Model.POS> ();
			List<Model.POS> lstPOSFar = new List<Model.POS> ();

			foreach (Model.POS POS in lstPOS)
			{
				Model.POSGps result = new Model.POSGps();
			}

            Location? currentPOS = Geolocation.Default.GetLastKnownLocationAsync().GetAwaiter().GetResult();
            foreach (Model.POS POS in lstPOS) {
				POS.Distance = null;
				if (POS.Latitude != null && POS.Longitude != null && POS.Precision != null) {
					decimal distance = (decimal)location.CalculateDistance(new Location ((double)POS.Latitude, (double)POS.Longitude), DistanceUnits.Kilometers);
					decimal precision = ((decimal)location.Accuracy + (decimal)POS.Precision) < tolerance ? ((decimal)location.Accuracy + (decimal)POS.Precision) : tolerance;
					POS.Distance = distance - precision;

					if (POS.Distance < limit) {
						lstPOSNear.Add (POS);
					} else {
						lstPOSFar.Add (POS);
					}
				} else {
					lstPOSFar.Add (POS);
				}
			}

			List<Model.POS> lstPOSOrdered = null;
			if (!orderByDate) {
				// POS dentro do limite de x metros de distância do ponto atual, os POS fora limite são colocados no final da lista ordenados de acordo com o nome e endereço
				lstPOSOrdered = lstPOSNear.OrderBy(p => p.Distance).Concat(lstPOSFar).ToList ();
			} else {
				// POS dentro do limite de x metros de distância do ponto atual, os POS fora limite são colocados no final da lista ordenados de acordo com a data de visita em ordem decrescente
				DAL.Visit visitController = new DAL.Visit ();
				foreach (Model.POS POS in lstPOS) {
					string lastVisitDate = visitController.GetVisit ().Where (v => v.POSCode == POS.POSCode && v.Status != Config.STATUS_VISIT_STARTED).OrderByDescending (o => o.VisitDate).Select (o => o.VisitDate).FirstOrDefault ();
					if (String.IsNullOrEmpty (lastVisitDate)) {
						DAL.LastVisit lastVisitController = new DAL.LastVisit ();
						lastVisitDate = lastVisitController.GetAll ().Where (o => o.POSCode == POS.POSCode).OrderByDescending (o => o.VisitDate).Select (o => o.VisitDate).FirstOrDefault ();
						if (!String.IsNullOrEmpty (lastVisitDate)) {
							lastVisitDate = Convert.ToDateTime (lastVisitDate).ToLongDateString ();
						}
					}
					POS.LastVisitDate = lastVisitDate;
				}
				lstPOSOrdered = lstPOSNear.OrderBy(p => p.Distance).Concat(lstPOSFar.OrderByDescending(p => Convert.ToDateTime(p.LastVisitDate))).ToList ();
			}

			return lstPOSOrdered;
		}

        public int SetTotalObjetivos()
        {
            int i = 0;
            var lista = GetPOSs(null);
            LoadLists();
            metricIDShelf = new DAL.Metric().GetAll().Where(m => m.MetricType == Config.METRIC_TYPE_SHELF_NAME).FirstOrDefault().MetricID;

            using (SQLiteConnection db = Database.GetNewConnection())
            {
                string query = "UPDATE POS SET TotalObjetivos = {0} WHERE POSCode = '{1}'";
                int total = 0;
				var lista_erro = new List<Model.POS>();
                foreach (var pos in lista)
                {
					try
					{
                        var tag_x = lstTags.Where(t => t.Name.ToUpper() == pos.TagBaseName.ToUpper()).FirstOrDefault();
                        total = GetObjetives(pos.POSCode, tag_x.TagID, metricIDShelf);
                        db.Execute(String.Format(query, total, pos.POSCode));
                        i++;
                    }
					catch (Exception ex)
					{
						lista_erro.Add(pos);
						continue;
                    }

                }
            }
            return i;
        }

		public List<Model.Promotion> GetPromotions(string POSCode)
		{
            try
            {
				var promotionsPOS = new DAL.PromotionPOS().GetAll().Where(w => w.POSCode == POSCode).ToList();
				var promotions = new List<Model.Promotion>();
				if (promotionsPOS != null)
				{
					foreach (var promoPOS in promotionsPOS)
					{
						var promotion = new DAL.Promotion().GetAll().Where(w => w.PromotionID == promoPOS.PromotionID).FirstOrDefault();
						if (promotion != null)
						{
                            promotions.Add(promotion);
                        }
                    }
				}

				return promotions;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
	}
}