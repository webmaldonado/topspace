using System;

using SQLite;
using TopSpaceMAUI.Util;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

#if IOS
using UIKit;
#endif

namespace TopSpaceMAUI.DAL
{
	public class Sync
	{
		public Sync ()
		{
		}

		public SyncStatusCode MoveTempToProd (List<ISyncable> syncs, CancellationTokenSource cts, bool actionAfter = false)
		{
			SQLiteConnection db = null;

			try {
				db = Database.GetNewConnection ();
				db.BeginTransaction ();

				foreach (var sync in syncs) {
					XTask.ThrowIfCancellationRequested(cts); // Não corre risco de ficar com o db inconsistente por causa da transação

                    Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE)));

                    //#if IOS
                    //					Model.Sync.LogInfo (String.Format (Localization.TryTranslateText ("DateAndTime"), DateTime.Now) + " / " + String.Format (Localization.TryTranslateText ("MATR"), UIApplication.SharedApplication.BackgroundTimeRemaining) + " / " + String.Format (Localization.TryTranslateText ("ApplicationState"), XNSUserDefaults.GetStringForKey (Config.KEY_APPLICATION_STATE)));
                    //					if (UIApplication.SharedApplication.BackgroundTimeRemaining < 5.0)
                    //						XTask.ThrowOperationCanceled (cts, String.Format(Localization.TryTranslateText("InsufficientTimeSaveData"), UIApplication.SharedApplication.BackgroundTimeRemaining));
                    //#endif

                    if (sync.MoveTempToProd (db))
						Model.Sync.Progress += Config.PROGRESS_PERCENT;
					else
						throw new SyncCrash (String.Format (Localization.TryTranslateText("FailSaveData"), sync.GetEntityName().ToLower()));
				}


				if (actionAfter)
					AfterMoveTempToProd(db);

				db.Commit ();

				return SyncStatusCode.SaveOK;
			}
			catch (OperationCanceledException ex) {
				throw ex;
			}
			catch (Exception ex) {
				Model.Sync.LogError (ex.Message);
				return SyncStatusCode.SaveError;
			}
			finally {
				if (syncs != null) {
					syncs.Clear ();
					syncs = null;
				}

				Database.Close (db);
				db = null;
			}
		}



		protected void AfterMoveTempToProd (SQLiteConnection db)
		{
			Model.Sync.LogInfo (Localization.TryTranslateText("SaveObjectiveCount"));
			var x = Model.POSObjectiveCount.Counts.Values;
			DAL.POSObjectiveCount.Save (Model.POSObjectiveCount.Counts.Values.ToList (), db);
		}



		protected class SyncCrash : Exception
		{
			public SyncCrash (string message) : base (message)
			{
			}
		}
	}
}