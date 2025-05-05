namespace TopSpaceMAUI;

using System.Collections.ObjectModel;
using TopSpaceMAUI.DAL;
using TopSpaceMAUI.Model;
using TopSpaceMAUI.Util;

public partial class Sync : ContentPage
{
    public bool showLog { get; private set; } = true;
    protected List<SyncLog> logs = new List<SyncLog>();
    protected AppVersion appVersion = new AppVersion();

    private int lastLogRowShown = 0;
    private System.Timers.Timer timer = null;
    private System.Timers.Timer animationTimer = null;
    private Task<SyncStatusCode> syncTask = null;
    private CancellationTokenSource syncTaskCTS = null;
    public string old_title = string.Empty;

    public Sync()
	{
        InitializeComponent();
        lblEnvInfo.Text = Util.AppCurrentVersion.Full_Description();
        logList.ItemsSource = logs;

        MessagingCenter.Subscribe<AppShell, bool>(this, "LoginStatusChanged", (sender, isLoggedIn) =>
        {
            btnSync.IsEnabled = isLoggedIn;
        });
    }

    private async void BtnSync_Clicked(object? sender, EventArgs e)
    {
        old_title = btnSync.Text;
        btnSync.IsEnabled = false;
        btnSync.IsVisible = false;
        imgAnimation.IsVisible = true;
        btnSync.Text = "Aguarde...";
        logList.IsVisible = true;
        animationTimer = new System.Timers.Timer(1000);
        animationTimer.AutoReset = true;
        animationTimer.Elapsed += AnimationTimer_Elapsed;
        animationTimer.Start();

        //await AnimateProgressBar(100);
        //var rotationTask = StartButtonRotation();

        await DoSync();
        //await Task.Run(async () => await DoSync());
        btnSync.Text = old_title;
        btnSync.IsEnabled = true;
        btnSync.IsVisible = true;
        imgAnimation.IsVisible = false;
        animationTimer.Stop();
        animationTimer.Dispose();
    }


    private async void AnimationTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        imgAnimation.CancelAnimations();
        await imgAnimation.RelRotateTo(360, 1000);
    }

    private async Task StartButtonRotation()
    {
        while (!btnSync.IsEnabled)
        {
            await btnSync.RotateTo(360, 1000);
            btnSync.Rotation = 0;
        }
    }

    private async Task AnimateProgressBar(double progressValue)
    {
        uint duration = 500; // Duração da animação em milissegundos
        await pgvSync.ProgressTo(progressValue, duration, Easing.Linear);
    }

    private void BtnSendLog_Clicked(object? sender, EventArgs e)
    {
        SendLog();
    }

    private void ResetThingsToStartSync()
    {
        Model.Sync.Stage = string.Empty;
        Model.Sync.ResetLog();
        logList.ItemsSource = Model.Sync.Log;
        ReloadLogAndScrollBottom(true);
        pgvSync.Progress = Model.Sync.Progress = 0;
        pgvSync.IsVisible = true;
        StartTimer();

        // Configura a ação de rolagem para o final
        Model.Sync.ScrollToEnd = () =>
        {
            if (Model.Sync.Log.Count > 0)
            {
                ReloadLogAndScrollBottom();
                //logList.ScrollTo(Model.Sync.LastLog, ScrollToPosition.End, true);
                //logList.ScrollTo(Model.Sync.Log[Model.Sync.Log.Count - 1], ScrollToPosition.End, true);
            }
        };
    }

    private async Task<bool> DoSync()
    {
        var tokenService = new Service.Token();
        var cwid = await SecureStorage.GetAsync("CWID");

        // TODO: Simulate User
        //var myToken = tokenService.CreateTokenID("EFYUZ", "Bayer155Bayer155");
        var myToken = tokenService.CreateTokenID(cwid, string.Empty);

        // Bloquear sincroniza��es simult�neas verificando a diferen�a entre a momento atual e o �ltimo momento salvo durante a sincroniza��o
        DateTime? lastSync = String.IsNullOrEmpty(XNSUserDefaults.GetStringForKey(Config.KEY_LAST_DATE_SYNC)) ? (DateTime?)null : Convert.ToDateTime(XNSUserDefaults.GetStringForKey(Config.KEY_LAST_DATE_SYNC));
        TimeSpan toleranceTime = TimeSpan.FromSeconds(10);

        if ((lastSync == null) || ((DateTime.Now - lastSync) > toleranceTime))
        {

            SyncStatusCode syncResult = SyncStatusCode.SaveError;
            try
            {
                CancelSyncTask();
            }
            catch
            {
            }

            try
            {
                ResetThingsToStartSync();
                syncTaskCTS = new CancellationTokenSource();
                Model.Sync.LogInfo(Localization.TryTranslateText("EmailLogStartedAt"), DateTime.Now.ToString(Localization.TryTranslateText("EmailLogDateFormat")));


                //imvLoading.Frame = new CGRect(new Point(btnSyncVisit.Frame.X - 1.25f, btnSyncVisit.Frame.Y - 2.5f), imvLoading.Frame.Size);
                Service.Sync syncServiceVisit = new Service.Sync(syncTaskCTS);
                syncTask = Task<SyncStatusCode>.Run((Func<SyncStatusCode>)syncServiceVisit.DoSync, syncTaskCTS.Token);
                syncResult = await syncTask;
                if (syncResult != SyncStatusCode.RequestOK)
                {
                    btnSync.Text = old_title;
                    btnSync.IsEnabled = true;
                    DisplayAlert("Top Space", "Erro no processo de sincronizacao.!!", "OK");
                    return false;
                }
                if (syncResult == SyncStatusCode.RequestOK)
                {
                    //POSViewController.Refresh = true;
                    //Model.Sync.LogInfo(Localization.TryTranslateText("SyncSucessMessage"));
                    //XUIAlertView.ShowTranslated("SyncSucessTitle", "SyncSucessMessage", null, "SyncSucessPositiveButton");
                    await DisplayAlert("Top Space", "Sincronizacao Realizada com Sucesso!!", "OK");
                }
                else if (syncResult == SyncStatusCode.RequestUnauthorized)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("SyncFailMessage"));
                    //LoginViewControllerModal loginViewControllerModal = (LoginViewControllerModal)Storyboard.InstantiateViewController("LoginViewControllerModal");
                    //loginViewControllerModal.UserAuthorizedSucess += () => { DoSync(syncOthers, syncVisit); };
                    //XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, String.Empty);
                    //this.PresentViewController(loginViewControllerModal, true, null);
                    XUIAlertView.ShowTranslated("SyncUnexpectedErrorTitle", "SyncFailMessage", null, "SyncUnexpectedErrorNegativeButton");
                }
                else if (syncResult == SyncStatusCode.RequestConnectionError)
                {
                    Model.Sync.LogInfo(Localization.TryTranslateText("SyncFailMessage"));
                    XUIAlertView.ShowTranslated("SyncUnexpectedErrorTitle", "SyncConnectionError", null, "SyncUnexpectedErrorNegativeButton", new string[] { "SyncUnexpectedErrorPositiveButton" });
                    XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, String.Empty);
                    //if (e.ButtonIndex == 1)
                    //{
                    //    DoSync(syncOthers, syncVisit); // Quando for eventualmente executado, StopBackgroundTask j� ter� executado antes.
                    //}
                }
                else
                {
                    //Model.Sync.LogInfo(Localization.TryTranslateText("SyncFailMessage"));
                    //XUIAlertView.ShowTranslated("SyncUnexpectedErrorTitle", "SyncUnexpectedErrorMessage", null, "SyncUnexpectedErrorNegativeButton", new string[] { "SyncUnexpectedErrorPositiveButton" });
                    //XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, String.Empty);
                    //if (e.ButtonIndex == 1)
                    //{
                    //    DoSync(syncOthers, syncVisit); // Quando for eventualmente executado, StopBackgroundTask j� ter� executado antes.
                    //}
                }
            }
            catch (OperationCanceledException ex)
            {
                //				Console.WriteLine ("DVC.DoSync.OperationCanceledException");
                //Model.Sync.LogInfo(Localization.TryTranslateText("LogInfoOperationCanceled"), ex);
            }
            catch (Exception ex)
            {
                //				Console.WriteLine ("DVC.DoSync.Exception {0}", ex.Message);
                Model.Sync.LogError(Localization.TryTranslateText("LogInfoError"), ex.Message);
            }
            finally
            {
                //Model.Sync.LogInfo(String.Format(Localization.TryTranslateText("DateAndTime"), DateTime.Now) + " / " + String.Format(Localization.TryTranslateText("MATR")) + " / " + String.Format(Localization.TryTranslateText("ApplicationState"), XNSUserDefaults.GetStringForKey(Config.KEY_APPLICATION_STATE)));
                //Model.Sync.LogInfo(Localization.TryTranslateText("EmailLogFinishedAt"), DateTime.Now.ToString(Localization.TryTranslateText("EmailLogDateFormat")));
                //ResetThingsToStartSync();
                StopTimer();
            }
        }

        return true;
    }

    private void CancelSyncTask()
    {
        //			Console.WriteLine ("CancelSyncTask");

        try
        {
            if (syncTask != null &&
                syncTask.Status != TaskStatus.Canceled && syncTask.Status != TaskStatus.Faulted && syncTask.Status != TaskStatus.RanToCompletion &&
                syncTaskCTS != null)
            {
                //					Console.WriteLine ("CANCELADO!");
                syncTaskCTS.Cancel();
                XNSUserDefaults.SetStringValueForKey(Config.KEY_LAST_DATE_SYNC, String.Empty);
            }
        }
        catch
        {
        }
        finally
        {
            syncTask = null;
            syncTaskCTS = null;
        }
    }

    private void SendLog()
    {
        DAL.LogApp.Write(Config.LogType.Operation, "Sync.SendLog", "SendLog");

        EmaiUtility.SendEmailLog(this.Content, Localization.TryTranslateText("EmailLogSyncSubject"), Localization.TryTranslateText("EmailLogSyncTitle"), EmaiUtility.PrepareEmailSync(Model.Sync.Log));
    }


    private void StartTimer()
    {
        if (timer == null)
        {
            timer = new System.Timers.Timer(3000); // in miliseconds = 3 segundos
            timer.Elapsed += OnTimedEvent;
        }
        else if (timer.Enabled)
            timer.Stop();
        timer.Start();
    }



    private void StopTimer()
    {
        if (timer != null)
        {
            timer.Stop();
            timer.Dispose();
        }
        timer = null;
    }



    private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (timer != null)
        {
            //lblDescriptionSync.Text = Model.Sync.Stage;
            //if ((int)Model.Sync.Progress <= Config.PROGRESS_TOTAL) {	
            bool? syncStatus = XNSUserDefaults.GetBoolValueForKey(Config.KEY_STATUS_SYNC);
            //if (syncStatus.HasValue && syncStatus == false)
            //{
                ReloadLogAndScrollBottom();
                if (pgvSync != null)
                    pgvSync.Progress = Model.Sync.Progress / Config.PROGRESS_TOTAL;

                //lblFotosSincronizadas.Text = string.Format("Download {0}%", Model.Sync.VisitSyncs.ToString());


                if ((int)Model.Sync.Progress == Config.PROGRESS_TOTAL)
                {
                    Model.Sync.Progress++;
                }
            //}
            //else
            //{
            //    StopTimer();
            //}
        }
    }


    private void ReloadLogAndScrollBottom(bool forceReload = true)
    {
        if (logList.ItemsSource != null)
        {
            int row = Model.Sync.Log.Count - 1;
            if (row != lastLogRowShown || forceReload)
            {
                lastLogRowShown = row;
            }
            if (row > 10)
            {
                logList.ScrollTo(row, animate: true, position: ScrollToPosition.MakeVisible);
            }
        }
    }
}