using System;
using RestSharp;
using System.Collections.Generic;
using SQLite;

using System.Threading;
using System.Threading.Tasks;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public interface ISyncable
	{
		string GetEntityName ();



		SyncStatusCode GetData (CancellationTokenSource cts);
	}



	public abstract class Syncable<TEntity, TDAL> : ISyncable
		where TEntity : class, IDisposable, new()
		where TDAL : class, DAL.ISyncable, new()
	{
		protected string WhenUtc { get; set; }



		public Syncable ()
		{
		}



		public Syncable (string whenUtc)
		{
			WhenUtc = whenUtc;
		}
			

		public SyncStatusCode GetData (CancellationTokenSource cts)
		{
			RestClient client = null;
			RestRequest request = null;
			RestResponse<List<TEntity>> response = null;
			//RestResponse myResponse = null;

			Model.Sync.Stage = String.Format(Localization.TryTranslateText("DownloadData"), GetEntityName ().ToLower());

            //Model.Sync.LogInfo (String.Format (Localization.TryTranslateText ("DateAndTime"), DateTime.Now) + " / " + String.Format (Localization.TryTranslateText ("MATR"), UIApplication.SharedApplication.BackgroundTimeRemaining) + " / " + String.Format (Localization.TryTranslateText ("ApplicationState"), XNSUserDefaults.GetStringForKey (Config.KEY_APPLICATION_STATE)));
            try
            {

                //var entity_name = GetEntityName();
                //if (entity_name == "EntityMetric")
                //{
                //    var x = "ok";
                //}

                Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("DownloadDataStart"));

				client = new RestClient (GetRestClientBaseUrl ());
				request = Service.Sync.GetRequestWithHeader (GetRequestResource (), true, true, WhenUtc);
//				request.AddHeader("Accept-Encoding", "gzip"); //TODO: Gzip
				request.RequestFormat = DataFormat.Json;
				request.Timeout = TimeSpan.FromMilliseconds(300000);

                //var myResponse = client.ExecuteGet(request);
				response = client.Execute<List<TEntity>>(request);


                if (response != null) {
					string dataCount = response.Data != null ? response.Data.Count.ToString () : "0";
					Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("DownloadDataFinished"), response.ResponseStatus, response.StatusCode, dataCount));
				}

				if (response.ResponseStatus == ResponseStatus.Completed) {
					if (response.StatusCode == System.Net.HttpStatusCode.OK) {
						if (!string.IsNullOrWhiteSpace (response.Content)) {
							TDAL dal = GetInstanceOfDAL();

							Model.Sync.LogInfo (String.Format (Localization.TryTranslateText ("DateAndTime"), DateTime.Now) + " / " + String.Format (Localization.TryTranslateText ("MATR"), String.Format (Localization.TryTranslateText ("ApplicationState"), XNSUserDefaults.GetStringForKey (Config.KEY_APPLICATION_STATE))));

							//if (UIApplication.SharedApplication.BackgroundTimeRemaining < 10.0)
							//	XTask.ThrowOperationCanceled (cts, String.Format(Localization.TryTranslateText("InsufficientTimeSaveData"), UIApplication.SharedApplication.BackgroundTimeRemaining));

							SyncStatusCode statusSaveTemp = dal.SaveTemp (response.Data);

							response.Data.ForEach (d => {
								d.Dispose ();
							});
							response.Data.Clear ();
							response.Data = null;

							if (statusSaveTemp == SyncStatusCode.SaveOK) {
								return SyncStatusCode.RequestOK;
							}
						}
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
						Model.Sync.LogError (GetEntityName () + Localization.TryTranslateText("DownloadDataFailUnauthorized"));
						return SyncStatusCode.RequestUnauthorized;
					}
				} else {
					if (response.StatusCode == 0) {
						Model.Sync.LogError (GetEntityName () + String.Format(Localization.TryTranslateText("DownloadDataFail"), response.ErrorMessage));
						return SyncStatusCode.RequestConnectionError;
					}
				}

				throw new Exception (response.ErrorMessage);
			}
			catch (OperationCanceledException ex) {
				throw ex;
			}
			catch (Exception ex) {
				var myResourceName = GetRequestResource();
				var myEntiryName = GetEntityName();
				var myErrorMessage = ex.Message;
				
                Model.Sync.LogError (myEntiryName + String.Format(Localization.TryTranslateText("DownloadDataFail"), ex.Message));
                Model.Sync.LogError(myResourceName + String.Format(Localization.TryTranslateText("DownloadDataFail"), ex.Message));

            }
            finally {
				client = null;
				request = null;
				response = null;
			}

			return SyncStatusCode.RequestError;
		}

		public virtual TDAL GetInstanceOfDAL()
		{
			return new TDAL ();
		}

		public abstract string GetEntityName ();



		protected abstract string GetRestClientBaseUrl ();



		protected abstract string GetRequestResource ();
	}
}