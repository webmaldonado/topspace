using TopSpaceMAUI.DAL;


namespace TopSpaceMAUI.Util
{
	public class LogUnhandledException
	{
        public static readonly string logFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "error_log.txt");

		public static void LogException (Exception ex)
		{
			Exception currentException = ex;
			string currentScreen = string.Empty;
			while (currentException != null) {

                LogError(currentException);

                LogApp.Write(Config.LogType.Error, currentScreen, Localization.TryTranslateText("UnexpectedErrorTitle"), comments: currentException.ToString());
				currentException = currentException.InnerException;
			}

            XNSUserDefaults.SetBoolValueForKey(Config.KEY_LOG_ERROR, false);
		}

        public static void LogError(Exception ex)
        {
            if (ex != null)
            {
                string errorMessage = $"{DateTime.Now}: {ex.Message}\nStackTrace: {ex.StackTrace}\n\n\n";
                File.AppendAllText(logFilePath, errorMessage);
            }
            else
            {
                string errorMessage = $"{DateTime.Now}: Objeto Exception is NULL\n\n\n";
                File.AppendAllText(logFilePath, errorMessage);
            }
        }

        public static void LogError(string message)
        {
            string errorMessage = $"{message}\n";
            File.AppendAllText(logFilePath, errorMessage);
        }
    }
}

