using RestSharp;
using SQLite;
using TopSpaceMAUI.Util;
using TopSpaceMAUI.DAL;
using Newtonsoft.Json;
using Config = TopSpaceMAUI.Config;
#if IOS
using UIKit;
#endif

namespace TopSpaceMAUI.Service
{
    public class Sync : IDisposable
    {
        private string OPERATION = "Sync";

        private string WhenUtc { get; set; }

        private CancellationTokenSource CTS { get; set; }

        private DAL.Sync SyncDAL { get; set; }

        public Sync(CancellationTokenSource cts)
        {
            CTS = cts;
            SyncDAL = new DAL.Sync();
        }

        public void Dispose()
        {
            WhenUtc = null;
            CTS = null;
            SyncDAL = null;
        }

        public SyncStatusCode DoSync()
        {
            //			Console.WriteLine ("DoSync");

            Model.POSObjectiveCount.ResetCounts();

            SyncStatusCode status;
            DAL.SyncHistory syncHistoryDAL = new DAL.SyncHistory();
            List<ISyncable> syncsService = new List<ISyncable>();
            List<DAL.ISyncable> syncsDAL = new List<DAL.ISyncable>();
            DAL.VisitCount visitCountDAL = new DAL.VisitCount();

            bool downloadBaseData = false;

            Model.Sync.Stage = Localization.TryTranslateText("SyncStarted");
            if ((status = Start()) == SyncStatusCode.RequestOK)
            {
                XTask.ThrowIfCancellationRequested(CTS);

                Model.Sync.Stage = Localization.TryTranslateText("DownloadTimeUTC");
                if ((status = GetWhenUtc()) == SyncStatusCode.RequestOK)
                {
                    XTask.ThrowIfCancellationRequested(CTS);

                    Model.SyncHistory syncHistory = syncHistoryDAL.Get(Config.KEY_STAGE_DOWNLOAD_BASE_DATA);

                    //DateTime parsedDate;
                    //string[] formats = { "M/d/yyyy h:mm:ss tt", "yyyy-MM-ddTHH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
                    //DateTime.TryParseExact(syncHistory.Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

                    //if (syncHistory == null || (syncHistory != null && DateTime.Now.Subtract(parsedDate).TotalMinutes > Config.RESYNC_TOLERANCE_MINUTES))
                    if (syncHistory == null || syncHistory != null)
                    {
                        downloadBaseData = true;
                        syncsService.Add(new Service.POS(WhenUtc));
                        syncsService.Add(new Service.Brand(WhenUtc));
                        syncsService.Add(new Service.SKU(WhenUtc));
                        syncsService.Add(new Service.SKUCompetitor(WhenUtc));
                        syncsService.Add(new Service.MetricType(WhenUtc));
                        syncsService.Add(new Service.Metric(WhenUtc));
                        syncsService.Add(new Service.ObjectiveBrand(WhenUtc));
                        syncsService.Add(new Service.LPGrade(WhenUtc));
                        syncsService.Add(new Service.LPMetricType(WhenUtc));
                        syncsService.Add(new Service.LPScoreBrand(WhenUtc));
                        syncsService.Add(new Service.LPScoreSKU(WhenUtc));
                        syncsService.Add(new Service.TagType(WhenUtc));
                        syncsService.Add(new Service.Tag(WhenUtc));
                        syncsService.Add(new Service.TagBrandPontoNatural(WhenUtc));
                        syncsService.Add(new Service.TagMerchandisingAcao(WhenUtc));
                        syncsService.Add(new Service.TagPresenca(WhenUtc));
                        syncsService.Add(new Service.Quiz(WhenUtc));
                        syncsService.Add(new Service.QuizType(WhenUtc));
                        syncsService.Add(new Service.QuizOption(WhenUtc));
                        //syncsService.Add(new Service.QuizAnswer(WhenUtc));
                        //syncsService.Add(new Service.QuizPOS(WhenUtc));
                        //syncsService.Add(new Service.QualityCheck(WhenUtc));
                        syncsService.Add(new Service.Promotion(WhenUtc));
                        //syncsService.Add(new Service.PromotionPOS(WhenUtc));
                        syncsService.Add(new Service.POSGps(WhenUtc));
                        syncsService.Add(new Service.ImgLib(WhenUtc));


                        if ((status = GetData(syncsService)) == SyncStatusCode.RequestOK)
                        {
                            XTask.ThrowIfCancellationRequested(CTS);
                            syncsService.Clear();

                            syncsDAL.Add(new DAL.POS());
                            syncsDAL.Add(new DAL.Brand());
                            syncsDAL.Add(new DAL.SKU());
                            syncsDAL.Add(new DAL.SKUCompetitor());
                            syncsDAL.Add(new DAL.MetricType());
                            syncsDAL.Add(new DAL.Metric());
                            syncsDAL.Add(new DAL.ObjectiveBrand());
                            syncsDAL.Add(new DAL.LPGrade());
                            syncsDAL.Add(new DAL.LPMetricType());
                            syncsDAL.Add(new DAL.LPScoreBrand());
                            syncsDAL.Add(new DAL.LPScoreSKU());
                            syncsDAL.Add(new DAL.TagType());
                            syncsDAL.Add(new DAL.Tag());
                            syncsDAL.Add(new DAL.TagBrandPontoNatural());
                            syncsDAL.Add(new DAL.TagMerchandisingAcao());
                            syncsDAL.Add(new DAL.TagPresenca());
                            syncsDAL.Add(new DAL.Quiz());
                            syncsDAL.Add(new DAL.QuizType());
                            syncsDAL.Add(new DAL.QuizOption());
                            //syncsDAL.Add(new DAL.QuizAnswer());
                            //syncsDAL.Add(new DAL.QuizPOS());
                            syncsDAL.Add(new DAL.Promotion());
                            //syncsDAL.Add(new DAL.PromotionPOS());
                            syncsDAL.Add(new DAL.POSGps());
                            syncsDAL.Add(new DAL.ImgLib());

                            new DAL.ImgLib().DeleteAll();

                            Model.Sync.Stage = Localization.TryTranslateText("MoveDataTempToProdStage");
                            if ((status = SyncDAL.MoveTempToProd(syncsDAL, CTS, true)) == SyncStatusCode.SaveOK)
                            {
                                Model.Sync.Stage = Localization.TryTranslateText("POSSetObjectivesCountStart");
                                int totalPOS = new DAL.POS().SetTotalObjetivos();
                                Model.Sync.Stage = Localization.TryTranslateText(String.Format("POSSetObjectivesCountEnd", totalPOS));

                                XNSUserDefaults.SetStringValueForKey(Config.KEY_DATABASE_VERSION, WhenUtc);
                                XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
                                // Data/hora da última sincronização de POS, Brand, SKU, SKUCompetitor, MetricType, Metric e ObjectiveBrand
                                syncHistoryDAL.InsertOrReplace(new Model.SyncHistory { Type = Config.KEY_STAGE_DOWNLOAD_BASE_DATA, Date = DateTime.Now.ToString() });
                                syncsDAL.Clear();
                            }
                        }
                    }

                    if (!downloadBaseData || (downloadBaseData && status == SyncStatusCode.SaveOK))
                    {
                        Model.Sync.Stage = Localization.TryTranslateText("UploadVisitData");
                        if ((status = SendVisit()) == SyncStatusCode.RequestOK)
                        {
                            Model.Sync.Progress += Config.PROGRESS_PERCENT;

                            Model.Sync.Stage = Localization.TryTranslateText("UploadLogData");
                            //if ((status = SendLog()) == SyncStatusCode.RequestOK)
                            SendLog();
                            if (true)
                            {

                                syncsService.Add(new Service.LastVisit(WhenUtc));
                                syncsService.Add(new Service.VisitCount(WhenUtc));
                                syncsService.Add(new Service.LastVisitDataTrackPrice(WhenUtc));

                                Model.Sync.Stage = Localization.TryTranslateText("UpdateLastVisitData");
                                if ((status = GetData(syncsService)) == SyncStatusCode.RequestOK)
                                {
                                    XTask.ThrowIfCancellationRequested(CTS);
                                    syncsService.Clear();

                                    syncsDAL.Add(new DAL.LastVisit());
                                    visitCountDAL.DeleteAll();
                                    syncsDAL.Add(new DAL.VisitCount());
                                    syncsDAL.Add(new DAL.LastVisitDataTrackPrice());

                                    Model.Sync.Stage = Localization.TryTranslateText("MoveDataTempToProdStage");
                                    if ((status = SyncDAL.MoveTempToProd(syncsDAL, CTS)) == SyncStatusCode.SaveOK)
                                    {
                                        XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
                                        syncsDAL.Clear();

                                        Model.Sync.Stage = Localization.TryTranslateText("MoveDataToUploadQueue");

                                        if ((status = MoveDataToUploadQueue()) == SyncStatusCode.SaveOK)
                                        {
                                            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
                                            Model.Sync.Progress += Config.PROGRESS_PERCENT;

                                            Model.Sync.Stage = Localization.TryTranslateText("UploadVisitPhoto");
                                            if ((status = SendPhoto(CTS)) == SyncStatusCode.RequestOK)
                                            {
                                                //Send Photo Quality Check
                                                Model.Sync.Stage = Localization.TryTranslateText("UploadVisitPhotoQualityCheck");
                                                SendPhotoQualityCheck(CTS);

                                                Model.Sync.Progress += Config.PROGRESS_PERCENT;
                                                Model.Sync.Stage = Localization.TryTranslateText("SyncFinished");

                                                if ((status = Finish()) == SyncStatusCode.RequestOK)
                                                {

                                                    XTask.ThrowIfCancellationRequested(CTS);
                                                    XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, String.Empty);

                                                    Model.Sync.Progress += Config.PROGRESS_PERCENT;
                                                    return SyncStatusCode.RequestOK;
                                                }

                                            }

                                        }

                                    }

                                }

                            }

                        }

                    }

                }

            }

            return status;
        }

        public SyncStatusCode Start()
        {
            //			Console.WriteLine ("Start");
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

            RestClient client = null;
            RestRequest request = null;
            RestSharp.RestResponseBase response = null;

            try
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("StageSync") + Localization.TryTranslateText("StartSync"));

                client = new RestClient(Config.URL_API_BASE + Config.URL_API_MODULO_SYNC);
                var myUser = DAL.Token.Current.Username;
                request = GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_START, myUser), true);
                request.AddParameter("App", Application.Current.Id);
                response = client.Execute(request);

                if (response != null)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("StartSyncSucess"), response.ResponseStatus, response.StatusCode));
                }

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return SyncStatusCode.RequestOK;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return SyncStatusCode.RequestUnauthorized;
                    }
                }
                else
                {
                    if (response.StatusCode == 0)
                    {
                        Model.Sync.LogError(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("StartSyncFail"), response.ErrorMessage));
                        return SyncStatusCode.RequestConnectionError;
                    }
                }
                throw new Exception(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("StartSyncFail"), ex.Message));
            }
            finally
            {
                client = null;
                request = null;
                response = null;
            }

            return SyncStatusCode.RequestError;
        }

        public SyncStatusCode GetWhenUtc()
        {
            //			Console.WriteLine ("GetWhenUtc");
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

            RestClient client = null;
            RestRequest request = null;
            RestResponse<Syncs_Request> response = null;

            try
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("StageDownloadTimeUTC") + Localization.TryTranslateText("DownloadTimeUTCStart"));

                client = new RestClient(Config.URL_API_BASE + Config.URL_API_MODULO_SYNC);
                request = GetRequestWithHeader(Config.URL_API_REQUEST_NOW, true);
                request.AddParameter("UtcLocal", DateTime.UtcNow.ToString("o"));
                response = client.Execute<Syncs_Request>(request);

                if (response != null)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageDownloadTimeUTC") + String.Format(Localization.TryTranslateText("DownloadTimeFinished"), response.ResponseStatus, response.StatusCode));
                }

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (!string.IsNullOrWhiteSpace(response.Content))
                        {
                            WhenUtc = response.Data.Now;

                            DateTime serverDate = DateTime.Parse(WhenUtc).ToUniversalTime();
                            DateTime localDate = DateTime.UtcNow;

                            double minutesDiference = serverDate.Subtract(localDate).TotalMinutes;

                            if (minutesDiference >= 90 || minutesDiference <= -90)
                            {
                                Model.Sync.LogError(String.Format(Localization.TryTranslateText("SyncErrorDateTimeInaccurateLocalDate"), DateTime.Now.ToString(Localization.TryTranslateText("EmailLogDateFormat"))));
                                Model.Sync.LogError(String.Format(Localization.TryTranslateText("SyncErrorDateTimeInaccurateLocalDateUTC"), DateTime.UtcNow.ToString(Localization.TryTranslateText("EmailLogDateFormat"))));
                                Model.Sync.LogError(String.Format(Localization.TryTranslateText("SyncErrorDateTimeInaccurateLastServerDate"), serverDate.ToString(Localization.TryTranslateText("EmailLogDateFormat"))));
                                throw new OperationCanceledException(Localization.TryTranslateText("SyncErrorDateTimeInaccurate"));
                            }

                            return SyncStatusCode.RequestOK;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return SyncStatusCode.RequestUnauthorized;
                    }
                }
                else
                {
                    if (response.StatusCode == 0)
                    {
                        Model.Sync.LogError(Localization.TryTranslateText("StageDownloadTimeUTC") + String.Format(Localization.TryTranslateText("DownloadTimeFail"), response.ErrorMessage));
                        return SyncStatusCode.RequestConnectionError;
                    }
                }

                throw new Exception(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("StageDownloadTimeUTC") + String.Format(Localization.TryTranslateText("DownloadTimeFail"), ex.Message));
            }
            finally
            {
                client = null;
                request = null;
                response = null;
            }

            return SyncStatusCode.RequestError;
        }

        public SyncStatusCode GetData(List<ISyncable> syncs)
        {
            //			Console.WriteLine ("GetData");

            try
            {
                SyncStatusCode status;

                Model.Sync.VisitSyncs = syncs.Count;
                foreach (ISyncable sync in syncs)
                {
                    XTask.ThrowIfCancellationRequested(CTS);
                    XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

                    status = sync.GetData(CTS);

                    if (status == SyncStatusCode.RequestOK)
                    {
                        Model.Sync.VisitSyncs--;
                        Model.Sync.Progress += Config.PROGRESS_PERCENT;
                    }
                    else
                        return status;
                }

                return SyncStatusCode.RequestOK;
            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(ex.Message);
                return SyncStatusCode.RequestError;
            }
            finally
            {
                syncs.Clear();
                syncs = null;
            }
        }

        public SyncStatusCode SendVisit()
        {
            //			Console.WriteLine ("SendVisit");

            XTask.ThrowIfCancellationRequested(CTS);
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

#if IOS
            //Data/hora: 27/10/2023 13:20:07 / MATR: 1.79769313486232E+308s / Estado: OnActivated
            Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
            //Visitas: Iniciando upload dos dados das visitas
            Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + Localization.TryTranslateText("VisitDataStart"));
#endif

            RestClient client = null;
            RestRequest request = null;
            RestResponse response = null;
            int totalVisit = 0;

            try
            {
                DAL.Visit visitController = new DAL.Visit();
                int totalVisitQueue = visitController.GetVisit().Where(v => v.Status == Config.STATUS_VISIT_COMPLETED).Count();

                if (totalVisitQueue > 0)
                {

                    Model.Sync.VisitSyncs = totalVisitQueue;

                    Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitCount"), totalVisitQueue));

                    LogApp.Write(Config.LogType.Debug, OPERATION + ".SendVisit", "SendVisit", comments: new { VisitCount = totalVisitQueue });

                    bool responseOK = false;
                    string responseStatus = null;
                    string statusCode = null;
                    List<POS_Visit> lstPOSVisit = new List<POS_Visit>();

                    client = new RestClient(Config.URL_API_BASE);

                    for (int i = 0; i < totalVisitQueue; i++)
                    {

                        List<Model.Visit> visit = visitController.GetVisitData();

                        if (visit != null)
                        {

                            Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
                            Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataUpload"), i + 1));

                            request = Service.Sync.GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_VISIT_DATA, DAL.Token.Current.Username), true, false, null, true);
                            request.RequestFormat = DataFormat.Json;
                            request.Timeout = TimeSpan.FromMilliseconds(300000);
                            string visit_json = JsonConvert.SerializeObject(visit);
                            request.AddParameter("application/json", visit_json, ParameterType.RequestBody);
                            //request.AddHeader("Content-Encoding", "gzip"); //TODO: Gzip
                            //request.AddParameter ("application/json", GzipUtil.Compress (JsonConvert.SerializeObject (visit)), ParameterType.RequestBody); //TODO: Gzip
                            response = client.Execute(request);

                            responseOK = false;

                            if (response.ResponseStatus == ResponseStatus.Completed)
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    totalVisit++;
                                    responseOK = true;
                                    visitController.UpdateSent(visit[0].POSCode, visit[0].VisitDate);
                                    responseStatus = response.ResponseStatus.ToString();
                                    statusCode = response.StatusCode.ToString();
                                    Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataUploadSucess"), totalVisit, responseStatus, statusCode));
                                    lstPOSVisit.Add(new POS_Visit { POSCode = visit[0].POSCode, VisitDate = visit[0].VisitDate });


                                    Model.Sync.VisitSyncs--;

                                }
                                else if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                                {
                                    Model.Sync.LogError(Localization.TryTranslateText("StageVisitData") + Localization.TryTranslateText("VisitDataFailNotAcceptable"));
                                    return SyncStatusCode.RequestError;
                                }
                                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    Model.Sync.LogError(Localization.TryTranslateText("StageVisitData") + Localization.TryTranslateText("VisitDataFailUnauthorized"));
                                    return SyncStatusCode.RequestUnauthorized;
                                }
                                else
                                {
                                    Model.Sync.LogError(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataFail"), response.StatusCode));
                                    return SyncStatusCode.RequestError;
                                }
                            }
                            else
                            {
                                if (response.StatusCode == 0)
                                {
                                    Model.Sync.LogError(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataFail"), response.ErrorMessage));
                                    return SyncStatusCode.RequestConnectionError;
                                }
                            }

                        }

                    }

                    if (responseOK)
                    {
                        string dataCount = totalVisit.ToString();
                        Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataUploadFinished"), responseStatus, statusCode, dataCount));

                        LogApp.Write(Config.LogType.Operation, OPERATION + ".SendVisit", "SendVisit", comments: new { POSVisit = lstPOSVisit });

                        client = null;
                        request = null;
                        response = null;
                        return SyncStatusCode.RequestOK;
                    }

                    throw new Exception(response.ErrorMessage);
                }
                else
                {
                    //Visitas: Sem dados para realizar upload
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitData") + Localization.TryTranslateText("VisitDataNoData"));
                    return SyncStatusCode.RequestOK;
                }
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("StageVisitData") + String.Format(Localization.TryTranslateText("VisitDataFail"), ex.Message));
                return SyncStatusCode.RequestError;
            }
            finally
            {
                client = null;
                request = null;
                response = null;
            }
        }

        public SyncStatusCode MoveDataToUploadQueue()
        {
            //			Console.WriteLine ("MoveDataToUploadQueue");
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
            DAL.Visit visitController = new DAL.Visit();
            return visitController.MoveDataToUploadQueue();
        }

        public SyncStatusCode SendLog()
        {
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

            Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));

            //Log iniciado
            Model.Sync.LogInfo(Localization.TryTranslateText("EntityLog") + Localization.TryTranslateText("Start"));

            RestClient client = null;
            RestRequest request = null;
            RestResponse response = null;

            try
            {
                List<Model.LogApp> logs = LogApp.GetAll().ToList();

                if (logs != null && logs.Count > 0)
                {
                    client = new RestClient(Config.URL_API_BASE);
                    request = Service.Sync.GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_LOG_DATA, DAL.Token.Current.Username), true, false, null, true);
                    request.RequestFormat = DataFormat.Json;
                    request.Timeout = TimeSpan.FromMilliseconds(300000);
                    request.AddParameter("application/json", JsonConvert.SerializeObject(logs), ParameterType.RequestBody);
                    response = client.Execute(request);

                    if (response != null)
                    {
                        Model.Sync.LogInfo(Localization.TryTranslateText("EntityLog") + String.Format(Localization.TryTranslateText("VisitDataUploadFinished"), response.ResponseStatus, response.StatusCode, logs.Count()));
                    }

                    if (response.ResponseStatus == ResponseStatus.Completed)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            LogApp.DeleteAll();
                            return SyncStatusCode.RequestOK;
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        {
                            Model.Sync.LogError(Localization.TryTranslateText("EntityLog") + Localization.TryTranslateText("VisitDataFailNotAcceptable"));
                            return SyncStatusCode.RequestError;
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            Model.Sync.LogError(Localization.TryTranslateText("EntityLog") + Localization.TryTranslateText("VisitDataFailUnauthorized"));
                            return SyncStatusCode.RequestUnauthorized;
                        }
                        else
                        {
                            Model.Sync.LogError(Localization.TryTranslateText("EntityLog") + String.Format(Localization.TryTranslateText("VisitDataFail"), response.StatusCode));
                            return SyncStatusCode.RequestError;
                        }
                    }
                    else
                    {
                        if (response.StatusCode == 0)
                        {
                            Model.Sync.LogError(Localization.TryTranslateText("EntityLog") + String.Format(Localization.TryTranslateText("VisitDataFail"), response.ErrorMessage));
                            return SyncStatusCode.RequestConnectionError;
                        }
                    }

                    throw new Exception(response.ErrorMessage);
                }
                else
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("EntityLog") + Localization.TryTranslateText("VisitDataNoData"));
                    return SyncStatusCode.RequestOK;
                }
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("EntityLog") + String.Format(Localization.TryTranslateText("VisitDataFail"), ex.Message));
                return SyncStatusCode.RequestError;
            }
            finally
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("EntityLog") + Localization.TryTranslateText("Finish"));
                client = null;
                request = null;
                response = null;
            }
        }

        public SyncStatusCode SendPhoto(CancellationTokenSource cts)
        {
            //			Console.WriteLine ("SendPhoto");
            Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
            Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoStart"));

            RestClient client = null;
            RestRequest request = null;
            RestResponse response = null;
            int totalVisitPhoto = 0;
            SQLiteConnection db = null;

            try
            {
                db = DAL.Database.GetNewConnection();
                // Visit Photo
                DAL.VisitPhotoQueue visitPhotoController = new DAL.VisitPhotoQueue();
                visitPhotoController.GiveUpPhoto(db);
                int totalVisitPhotoQueue = visitPhotoController.GetVisitPhotoQueue(db).Count;

                if (totalVisitPhotoQueue > 0)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoQueueCount"), totalVisitPhotoQueue));

                    bool responseOK = false;
                    string responseStatus = null;
                    string statusCode = null;

                    client = new RestClient(Config.URL_API_BASE);



                    //Model.Sync.VisitSyncs = totalVisitPhotoQueue;



                    for (int i = 0; i < totalVisitPhotoQueue; i++)
                    {

                        XTask.ThrowIfCancellationRequested(CTS);
                        XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
                        Model.VisitPhotoQueue visit = visitPhotoController.SelectPhoto(db);

                        if (visit != null)
                        {

                            string photoName = String.Format(Config.PHOTO_NAME, visit.BrandID, visit.MetricID);
                            string photoFile = visit.PhotoDirectory + Path.DirectorySeparatorChar + photoName;

                            if (Util.FileSystem.CheckFile(photoFile))
                            {
                                Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
                                Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoUpload"), i + 1));

                                request = Sync.GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_VISIT_PHOTO, DAL.Token.Current.Username), true, false, null, true);
                                request.Timeout = TimeSpan.FromMilliseconds(300000);
                                request.AddParameter(new GetOrPostParameter("POSCode", visit.POSCode));
                                request.AddParameter(new GetOrPostParameter("VisitDate", visit.VisitDate));
                                request.AddParameter(new GetOrPostParameter("MetricID", visit.MetricID.ToString()));
                                request.AddParameter(new GetOrPostParameter("BrandID", visit.BrandID.ToString()));
                                if (visit.SKUID != null && visit.SKUID.Value > 0)
                                {
                                    request.AddParameter(new GetOrPostParameter("SKUID", visit.SKUID.ToString()));
                                }
                                request.AddParameter(new GetOrPostParameter("PhotoID", visit.PhotoID.ToString()));

                                if (visit.MetricID == (int)XNSUserDefaults.GetIntValueForKey(Config.METRIC_TYPE_SHELF_NAME))
                                {
                                    request.AddFile("Photo", Util.FileSystem.ResizePhoto(visit.Photo), photoName, "image/jpeg");
                                }
                                else
                                {
                                    request.AddFile("Photo", Util.FileSystem.ResizePhoto(visit.Photo), photoName, "image/jpeg");
                                }

                                response = client.Execute(request);
                                responseOK = false;

                                if (response.ResponseStatus == ResponseStatus.Completed)
                                {
                                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        totalVisitPhoto++;
                                        responseOK = true;
                                        visitPhotoController.Delete(db, visit, true);
                                        responseStatus = response.ResponseStatus.ToString();
                                        statusCode = response.StatusCode.ToString();

                                        //Model.Sync.VisitSyncs--;

                                        Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoUploadSucess"), totalVisitPhoto, responseStatus, statusCode));
                                    }
                                    else if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                                    {
                                        Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoFailNotAcceptable"));
                                        return SyncStatusCode.RequestError;
                                    }
                                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                    {
                                        Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoFailUnauthorized"));
                                        return SyncStatusCode.RequestUnauthorized;
                                    }
                                    else
                                    {
                                        Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), response.StatusCode));
                                        return SyncStatusCode.RequestError;
                                    }
                                }
                                else
                                {
                                    if (response.StatusCode == 0)
                                    {
                                        Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), response.ErrorMessage));
                                        return SyncStatusCode.RequestConnectionError;
                                    }
                                }
                            }
                            else
                            {
                                //							// Caso a foto não exista, apagar da fila de upload
                                // visitPhotoController.Delete(db, visit, false);
                            }

                        }
                    }

                    if (responseOK)
                    {
                        string dataCount = totalVisitPhoto.ToString();
                        Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoUploadFinished"), responseStatus, statusCode, dataCount));

                        client = null;
                        request = null;
                        response = null;
                        return SyncStatusCode.RequestOK;
                    }

                    throw new Exception(response.ErrorMessage);
                }
                else
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoNoData"));
                    return SyncStatusCode.RequestOK;
                }
            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), ex.Message + " (StackTrace: " + ex.StackTrace + ")"));
                return SyncStatusCode.RequestError;
            }
            finally
            {
                Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
                Database.Close(db);
                db = null;
                client = null;
                request = null;
                response = null;
            }
        }

        public SyncStatusCode SendPhotoQualityCheck(CancellationTokenSource cts)
        {
            Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"),
                String.Format(Localization.TryTranslateText("ApplicationState"),
                XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))));
            Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhotoQualityCheck") + Localization.TryTranslateText("VisitPhotoQualityCheckStart"));

            RestClient client = null;
            RestRequest request = null;
            RestResponse response = null;
            int totalVisitPhoto = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                using (SQLiteConnection db = DAL.Database.GetNewConnection())
                {
                    // Visit Photo Quality Check
                    DAL.VisitPhotoQueueQualityCheck visitPhotoQualityCheckController = new DAL.VisitPhotoQueueQualityCheck();
                    visitPhotoQualityCheckController.GiveUpPhoto(db);
                    int totalVisitPhotoQualityCheckQueue = visitPhotoQualityCheckController.GetVisitPhotoQualityCheckQueue(db).Count;

                    if (totalVisitPhotoQualityCheckQueue > 0)
                    {
                        Model.Sync.LogInfo(
                            sb.Remove(0, sb.Length)
                            .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                            .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckQueueCount"), totalVisitPhotoQualityCheckQueue)
                            .ToString()
                            /*Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                            String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckQueueCount"),
                            totalVisitPhotoQualityCheckQueue)*/);

                        bool responseOK = false;
                        string responseStatus = null;
                        string statusCode = null;
                        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                        client = new RestClient(Config.URL_API_BASE);

                        for (int i = 0; i < totalVisitPhotoQualityCheckQueue; i++)
                        {
                            XTask.ThrowIfCancellationRequested(CTS);
                            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
                            Model.VisitPhotoQualityCheckQueue visitQualityCheck = visitPhotoQualityCheckController.SelectPhoto(db);

                            if (visitQualityCheck != null)
                            {
                                
                                string photoName = visitQualityCheck.Photo;
                                //string photoFile = visitQualityCheck.PhotoDirectory + Path.DirectorySeparatorChar + photoName;
                                //string photoFile = "/Users/admin/foto.jpg";//inserção manual
                                string photoFile = Path.Combine(folderPath, photoName);

                                if (Util.FileSystem.CheckFileQualityCheck(photoFile))
                                {
                                    sb.Remove(0, sb.Length);
                                    Model.Sync.LogInfo(
                                        sb.Remove(0, sb.Length)
                                        .AppendFormat(Localization.TryTranslateText("DateAndTime"), DateTime.Now)
                                        .Append(" / ")
                                        //.AppendFormat(Localization.TryTranslateText("MATR"), UIApplication.SharedApplication.BackgroundTimeRemaining)
                                        //.Append(" / ")
                                        .AppendFormat(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))
                                        .ToString()
                                        //String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + 
                                        //String.Format(Localization.TryTranslateText("MATR"), 
                                        //String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))
                                        );
#if IOS
                                    if (UIApplication.SharedApplication.BackgroundTimeRemaining < 10.0)
                                        XTask.ThrowOperationCanceled(cts,
                                            sb.Remove(0, sb.Length)
                                            .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckInsufficientTime"),
                                            UIApplication.SharedApplication.BackgroundTimeRemaining)
                                            .ToString()
                                            //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckInsufficientTime"), 
                                            //UIApplication.SharedApplication.BackgroundTimeRemaining)
                                            );
#endif

                                    Model.Sync.LogInfo(
                                        sb.Remove(0, sb.Length)
                                        .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                        .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckUpload"), i + 1)
                                        .ToString()
                                        //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                        //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckUpload"), i + 1)
                                        );

                                    request = Service.Sync.GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_VISIT_PHOTO_QUALITY_CHECK, DAL.Token.Current.Username), true,
                                        false, null, true);
                                    request.Timeout = TimeSpan.FromMilliseconds(300000);
                                    request.AddParameter(new GetOrPostParameter("POSCode", visitQualityCheck.POSCode));
                                    request.AddParameter(new GetOrPostParameter("VisitDate", visitQualityCheck.VisitDate));
                                    request.AddParameter(new GetOrPostParameter("MetricID", visitQualityCheck.MetricID.ToString()));
                                    request.AddParameter(new GetOrPostParameter("BrandID", visitQualityCheck.BrandID.ToString()));
                                    request.AddParameter(new GetOrPostParameter("SKUID", visitQualityCheck.SKUID.ToString()));
                                    request.AddParameter(new GetOrPostParameter("PhotoQualityCheckID", visitQualityCheck.PhotoID.ToString()));

                                    var photoBin = Util.FileSystem.ResizePhoto(photoName);
                                    //var photoBin = File.ReadAllBytes(visitQualityCheck.Photo);

                                    request.AddFile("PhotoQualityCheck", photoBin, photoName, "image/jpeg");
                                                                    
                                    response = client.Execute(request);
                                    responseOK = false;

                                    if (response.ResponseStatus == ResponseStatus.Completed)
                                    {
                                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            totalVisitPhoto++;
                                            responseOK = true;
                                            visitPhotoQualityCheckController.Delete(db, visitQualityCheck, true);
                                            responseStatus = response.ResponseStatus.ToString();
                                            statusCode = response.StatusCode.ToString();
                                            Model.Sync.LogInfo(
                                                sb.Remove(0, sb.Length)
                                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                                .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckUploadSucess"), totalVisitPhoto, responseStatus, statusCode)
                                                .ToString()
                                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                                //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckUploadSucess"), totalVisitPhoto, responseStatus, statusCode)
                                                );
                                        }
                                        else if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                                        {
                                            Model.Sync.LogError(
                                                sb.Remove(0, sb.Length)
                                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                                .Append(Localization.TryTranslateText("VisitPhotoQualityCheckFailNotAcceptable"))
                                                .ToString()
                                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                                //Localization.TryTranslateText("VisitPhotoQualityCheckFailNotAcceptable")
                                                );
                                            return SyncStatusCode.RequestError;
                                        }
                                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                                        {
                                            Model.Sync.LogError(
                                                sb.Remove(0, sb.Length)
                                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                                .Append(Localization.TryTranslateText("VisitPhotoQualityCheckFailNotAcceptable"))
                                                .ToString()
                                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                                //Localization.TryTranslateText("VisitPhotoQualityCheckFailUnauthorized")
                                                );
                                            return SyncStatusCode.RequestUnauthorized;
                                        }
                                        else
                                        {
                                            Model.Sync.LogError(
                                                sb.Remove(0, sb.Length)
                                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                                .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckFail"), response.StatusCode)
                                                .ToString()
                                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                                //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckFail"), response.StatusCode)
                                                );
                                            return SyncStatusCode.RequestError;
                                        }
                                    }
                                    else
                                    {
                                        if (response.StatusCode == 0)
                                        {
                                            Model.Sync.LogError(
                                                sb.Remove(0, sb.Length)
                                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                                .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckFail"), response.ErrorMessage)
                                                .ToString()
                                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                                //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckFail"), response.ErrorMessage)
                                                );
                                            return SyncStatusCode.RequestConnectionError;
                                        }
                                    }
                                }
                                else
                                {
                                    // Caso a foto não exista, apagar da fila de upload
                                    visitPhotoQualityCheckController.Delete(db, visitQualityCheck, false);
                                    responseOK = true;
                                }
                            }
                        }

                        if (responseOK)
                        {
                            string dataCount = totalVisitPhoto.ToString();
                            Model.Sync.LogInfo(
                                sb.Remove(0, sb.Length)
                                .Append(Localization.TryTranslateText("StageVisitPhotoQualityCheck"))
                                .AppendFormat(Localization.TryTranslateText("VisitPhotoQualityCheckUploadFinished"), responseStatus, statusCode, dataCount)
                                .ToString()
                                //Localization.TryTranslateText("StageVisitPhotoQualityCheck") + 
                                //String.Format(Localization.TryTranslateText("VisitPhotoQualityCheckUploadFinished"), responseStatus, statusCode, dataCount)
                                );

                            client = null;
                            request = null;
                            response = null;
                            return SyncStatusCode.RequestOK;
                        }

                        throw new Exception(response.ErrorMessage);
                    }
                    else
                    {                       
                        return SyncStatusCode.RequestOK;
                    }
                }

            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(
                    sb.Remove(0, sb.Length)
                    .Append(Localization.TryTranslateText("StageVisitPhoto"))
                    .AppendFormat(Localization.TryTranslateText("VisitPhotoFail"), ex.Message)
                    .AppendFormat(" (StackTrace: {0})", ex.StackTrace)
                    .ToString()
                //Localization.TryTranslateText("StageVisitPhoto") + 
                //    String.Format(Localization.TryTranslateText("VisitPhotoFail"), ex.Message + 
                //    " (StackTrace: " + ex.StackTrace + ")")
                );
                return SyncStatusCode.RequestError;
            }
            finally
            {
                Model.Sync.LogInfo(
                    sb.Remove(0, sb.Length)
                    .AppendFormat(Localization.TryTranslateText("DateAndTime"), DateTime.Now)
                    .Append(" / ")
                    //.AppendFormat(Localization.TryTranslateText("MATR"), UIApplication.SharedApplication.BackgroundTimeRemaining)
                    //.Append(" / ")
                    .AppendFormat(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))
                    .ToString()
                    //String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + 
                    //" / " + 
                    //String.Format(Localization.TryTranslateText("MATR"), UIApplication.SharedApplication.BackgroundTimeRemaining) + 
                    //" / " + 
                    //String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE))
                    );
                //Database.Close(db);
                //db = null;
                client = null;
                request = null;
                response = null;
            }
        }


        /**
         * 
         * Envia as fotos do Quality Check
         * 
         * */
        //public SyncStatusCode SendPhotoQualityCheck(CancellationTokenSource cts)
        //{
        //    //			Console.WriteLine ("SendPhoto");
        //    Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE)));
        //    Model.Sync.LogInfo("Sincronizando Fotos do Quality Check");

        //    RestClient client = null;
        //    RestRequest request = null;
        //    RestResponse response = null;
        //    int totalVisitPhoto = 0;
        //    SQLiteConnection db = null;

        //    try
        //    {
        //        db = DAL.Database.GetNewConnection();
        //        DAL.VisitPhotoQueueQualityCheck visitPhotoQCController = new DAL.VisitPhotoQueueQualityCheck();
        //        visitPhotoQCController.GiveUpPhoto(db);
        //        int totalVisitPhotoQueue = visitPhotoQCController.GetVisitPhotoQualityCheckQueue(db).Count;

        //        Model.Sync.LogInfo("Fotos do Quality Check: " + totalVisitPhotoQueue);

        //        if (totalVisitPhotoQueue > 0)
        //        {
        //            bool responseOK = false;
        //            string responseStatus = null;
        //            string statusCode = null;

        //            client = new RestClient(Config.URL_API_BASE);

        //            for (int i = 0; i < totalVisitPhotoQueue; i++)
        //            {
        //                XTask.ThrowIfCancellationRequested(CTS);
        //                XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());
        //                Model.VisitPhotoQualityCheckQueue visit = visitPhotoQCController.SelectPhoto(db);

        //                Model.Sync.LogInfo("Diretorio Foto Quality Check: " + visit.PhotoDirectory);

        //                if (visit != null)
        //                {
        //                    string photoName = String.Format(Config.PHOTO_NAME_QUALITY_CHECK, visit.BrandID, visit.MetricID);
        //                    string photoFile = visit.PhotoDirectory + Path.DirectorySeparatorChar + photoName;

        //                    Model.Sync.LogInfo("Nome da foto Quality Check: " + photoFile);

        //                    if (!Util.FileSystem.CheckFile(photoFile))
        //                    {
        //                        Model.Sync.LogInfo("Fazendo Upload Quality Check");

        //                        Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE)));

        //                        if (UIApplication.SharedApplication.BackgroundTimeRemaining < 10.0)
        //                            XTask.ThrowOperationCanceled(cts, String.Format(Localization.TryTranslateText("VisitPhotoInsufficientTime"), UIApplication.SharedApplication.BackgroundTimeRemaining));

        //                        Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("Visit Photo Upload QualityCheck"), i + 1));

        //                        Model.Sync.LogInfo("POSCode " + visit.POSCode);
        //                        Model.Sync.LogInfo("VisitDate " + visit.VisitDate);
        //                        Model.Sync.LogInfo("MetricID " + visit.MetricID);
        //                        Model.Sync.LogInfo("BrandID" + visit.BrandID);
        //                        Model.Sync.LogInfo("SKUID " + visit.SKUID);
        //                        Model.Sync.LogInfo("PhotoQualityCheck " + visit.Photo);

        //                        request = Service.Sync.GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_VISIT_PHOTO_QUALITY_CHECK, DAL.Token.Current.Username), true, false, null, true);
        //                        request.Timeout = 300000;
        //                        request.AddParameter("POSCode", visit.POSCode);                                
        //                        request.AddParameter("VisitDate", visit.VisitDate);                                
        //                        request.AddParameter("MetricID", visit.MetricID);                                
        //                        request.AddParameter("BrandID", visit.BrandID);                                
        //                        request.AddParameter("SKUID", visit.SKUID);                                
        //                        request.AddParameter("PhotoQualityCheckID", 1);

        //                        request.AddFile("PhotoQualityCheck", Util.FileSystem.SelectPhoto(visit.Photo), photoName, "image/jpeg");
        //                        //request.AddFile("PhotoQualityCheck", new byte[0101], photoName, "image/jpeg");

        //                        response = client.Execute(request);
        //                        responseOK = false;

        //                        if (response.ResponseStatus == ResponseStatus.Completed)
        //                        {
        //                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                            {
        //                                totalVisitPhoto++;
        //                                responseOK = true;
        //                                //visitPhotoController.Delete(db, visit, true);
        //                                responseStatus = response.ResponseStatus.ToString();
        //                                statusCode = response.StatusCode.ToString();
        //                                Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoUploadSucess"), totalVisitPhoto, responseStatus, statusCode));
        //                            }
        //                            else if (response.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
        //                            {
        //                                Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoFailNotAcceptable"));
        //                                return SyncStatusCode.RequestError;
        //                            }
        //                            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //                            {
        //                                Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoFailUnauthorized"));
        //                                return SyncStatusCode.RequestUnauthorized;
        //                            }
        //                            else
        //                            {
        //                                Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), response.StatusCode));
        //                                return SyncStatusCode.RequestError;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (response.StatusCode == 0)
        //                            {
        //                                Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), response.ErrorMessage));
        //                                return SyncStatusCode.RequestConnectionError;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //							// Caso a foto não exista, apagar da fila de upload
        //                        visitPhotoQCController.Delete(db, visit, false);
        //                    }

        //                }
        //            }

        //            if (responseOK)
        //            {
        //                string dataCount = totalVisitPhoto.ToString();
        //                Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoUploadFinished"), responseStatus, statusCode, dataCount));

        //                client = null;
        //                request = null;
        //                response = null;
        //                return SyncStatusCode.RequestOK;
        //            }

        //            throw new Exception(response.ErrorMessage);
        //        }
        //        else
        //        {
        //            Model.Sync.LogInfo(Localization.TryTranslateText("StageVisitPhoto") + Localization.TryTranslateText("VisitPhotoNoData"));
        //            return SyncStatusCode.RequestOK;
        //        }
        //    }
        //    catch (OperationCanceledException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Model.Sync.LogError(Localization.TryTranslateText("StageVisitPhoto") + String.Format(Localization.TryTranslateText("VisitPhotoFail"), ex.Message + " (StackTrace: " + ex.StackTrace + ")"));
        //        return SyncStatusCode.RequestError;
        //    }
        //    finally
        //    {
        //        Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR"), String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE)));
        //        Database.Close(db);
        //        db = null;
        //        client = null;
        //        request = null;
        //        response = null;
        //    }
        //}



        public SyncStatusCode Finish()
        {
            //			Console.WriteLine ("Finish");

            XTask.ThrowIfCancellationRequested(CTS);
            XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, DateTime.Now.ToString());

            RestClient client = null;
            RestRequest request = null;
            RestResponse response = null;

            try
            {
                Model.Sync.LogInfo(Localization.TryTranslateText("StageSync") + Localization.TryTranslateText("FinishSync"));

                client = new RestClient(Config.URL_API_BASE + Config.URL_API_MODULO_SYNC);
                request = GetRequestWithHeader(String.Format(Config.URL_API_REQUEST_FINISH, DAL.Token.Current.Username), true);
                response = client.Execute(request);

                if (response != null)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("FinishSyncSucess"), response.ResponseStatus, response.StatusCode));
                }

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return SyncStatusCode.RequestOK;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return SyncStatusCode.RequestUnauthorized;
                    }
                }
                else
                {
                    if (response.StatusCode == 0)
                    {
                        Model.Sync.LogError(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("FinishSyncFail"), response.ErrorMessage));
                        return SyncStatusCode.RequestConnectionError;
                    }
                }
                throw new Exception(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                Model.Sync.LogError(Localization.TryTranslateText("StageSync") + String.Format(Localization.TryTranslateText("FinishSyncFail"), ex.Message));
            }
            finally
            {
                client = null;
                request = null;
                response = null;
            }

            return SyncStatusCode.RequestError;
        }

        public static RestRequest GetRequestWithHeader(string resource, bool addToken = false, bool addWhenUtc = false, string whenUtc = null, bool methodPost = false)
        {
            RestRequest request = null;

            if (!methodPost)
                request = new RestRequest(resource);
            else
                request = new RestRequest(resource, Method.Post);

            if (addToken && !string.IsNullOrEmpty(DAL.Token.Current.TokenID))
                request.AddHeader(Config.API_HEADER_TOKEN, DAL.Token.Current.TokenID);

            if (addWhenUtc && !string.IsNullOrEmpty(whenUtc))
                request.AddParameter(Config.URL_API_PARAMETER_UTC, whenUtc);

            return request;
        }

        protected class Syncs_Request
        {
            public string Now { get; set; }
        }

        protected class POS_Visit
        {
            public string POSCode { get; set; }
            public string VisitDate { get; set; }
        }
    }
}