using Newtonsoft.Json;
using TopSpaceMAUI.Util;
using SQLite;

namespace TopSpaceMAUI.DAL
{
	public static class LogApp
	{
		public static List<Model.LogApp> GetAll ()
		{
			MoveTempToProd ();
			SQLiteConnection db = Database.GetNewConnection ();
			return db.Table<Model.LogApp> ().OrderBy (l => l.AppID).ToList ();
		}

		private static void Insert (List<Model.LogApp> log)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertAll (log);
			Database.Close (db);
			db = null;
		}

		public static void DeleteAll ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format ("DELETE FROM LogApp");
			db.Execute (query);
			Database.Close (db);
			db = null;
			XNSUserDefaults.RemoveObject (Config.KEY_LOCAL_LOG);
		}

		private static List<Model.LogApp> GetAllUserDefaults ()
		{
			string json = XNSUserDefaults.GetStringForKey (Config.KEY_LOCAL_LOG);
			List<Model.LogApp> lstLog = new List<Model.LogApp> ();
			if (!string.IsNullOrEmpty (json)) {
				lstLog = JsonConvert.DeserializeObject<List<Model.LogApp>> (XNSUserDefaults.GetStringForKey (Config.KEY_LOCAL_LOG));
			}
			return lstLog;
		}

		private static void InsertUserDefaults (Model.LogApp log)
		{
			List<Model.LogApp> lstLog = GetAllUserDefaults();
			lstLog.Add (log);
			XNSUserDefaults.SetStringValueForKey (Config.KEY_LOCAL_LOG, JsonConvert.SerializeObject (lstLog));
		}

		private static void MoveTempToProd ()
		{
			List<Model.LogApp> temp = GetAllUserDefaults ();
			if (temp != null && temp.Count > 0) {
				Insert (temp);
			}
			XNSUserDefaults.RemoveObject (Config.KEY_LOCAL_LOG);
		}

		public static bool Write (
			Config.LogType logType,
			string operation,
			string description,
			string entityType = null,
			object entityID = null,
			string url = null,
			object comments = null,
			DateTime? now = null)
		{
			Model.LogApp log = new Model.LogApp ();
			try {
				log.DeviceDate = now.HasValue ? now.Value.ToLongDateString () : DateTime.Now.ToLongDateString ();
				log.DeviceTimezone = TimeZone.CurrentTimeZone.StandardName;
				log.InstallID = XNSUserDefaults.GetStringForKey (Config.KEY_INSTALL_ID);
				log.AppVersion = AppInfo.Current.VersionString;
				log.LogType = logType.ToString ();
				log.Operation = (operation ?? "").Clean (false, 50);
				log.EntityType = (entityType ?? "").Clean (true, 50);
				if (entityID != null)
					log.EntityID = entityID.ToString ().Clean (true, 50);
				else
					log.EntityID = null;

				log.Description = (description ?? "").Clean (false, 300);

				log.URL = (url ?? "").Clean (true, 200);

				if (comments != null) {
					try {
						log.Comments = JsonConvert.SerializeObject (comments);
					} catch {
						log.Comments = comments.ToString ();
					}
				} else
					log.Comments = null;

				MoveTempToProd ();

				Insert (new List<Model.LogApp> () { log });

				return true;
			} catch (Exception ex) {
				try {
					InsertUserDefaults (log);
				} catch { }
				return false;
			}
		}

}
}