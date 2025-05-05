using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class POSMaterial : Syncable<Model.POSMaterialTemp, DAL.POSMaterial>
	{
		private DAL.POSMaterial POSMaterialDAL;
		private int totalPOSMaterial = 0;

		public POSMaterial ()
		{
			POSMaterialDAL = new DAL.POSMaterial ();
		}
			
		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityPOSMaterial");
		}

		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}

		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_POS_MATERIAL, DAL.Token.Current.Username);
		}

		public string SendData ()
		{
			return PrepareData ();
		}
			
		private string PrepareData ()
		{
			string json = String.Empty;

			bool sync = Util.POSMaterial.CheckSyncData (DateTime.Now);

			Model.Material material = null;

			List<Model.POSMaterial> lstPOSMaterial = null;

			if (sync) {
				lstPOSMaterial = POSMaterialDAL.GetAll ().ToList();
				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("POSMaterialPrepareNeededDataUpload"));
			} else {
				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("POSMaterialNoActionUpload"));
			}

			if (lstPOSMaterial != null && lstPOSMaterial.Count > 0) {
				material = new Model.Material {
					material = lstPOSMaterial.ToArray ()
				};
				totalPOSMaterial = lstPOSMaterial.Count;
			}

			if (material != null) {
				json = JsonConvert.SerializeObject (material);
			}

			return json;
		}

		public void CleanUpData(DateTime now)
		{
			POSMaterialDAL.CleanUp (now);
		}

		public int GetTotalPOSMaterial()
		{
			return totalPOSMaterial;
		}
	}
}

