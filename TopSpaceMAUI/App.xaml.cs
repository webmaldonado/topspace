namespace TopSpaceMAUI;

public partial class App : Application
{
    protected AppVersion appVersion = new AppVersion();

    public App()
    {
        InitializeComponent();

        if (!File.Exists(TopSpaceMAUI.Util.LogUnhandledException.logFilePath))
        {
            File.AppendAllText(TopSpaceMAUI.Util.LogUnhandledException.logFilePath, $"{DateTime.Now}: Log file created - logFilePath\n\n\n");
        }

        // Captura exceções não tratadas
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        TopSpaceMAUI.Util.LogUnhandledException.LogError(e.ExceptionObject as Exception);

        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            TopSpaceMAUI.Util.LogUnhandledException.LogError(e.Exception);
            e.SetObserved();
        };

        MainPage = new AppShell();
    }
}
