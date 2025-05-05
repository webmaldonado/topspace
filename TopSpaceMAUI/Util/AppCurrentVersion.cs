using System;
namespace TopSpaceMAUI.Util
{
	public static class AppCurrentVersion
	{
		public static string Version()
		{
            var version = string.Empty;

#if ANDROID
            version = global::Android.App.Application.Context.ApplicationContext.PackageManager
                .GetPackageInfo(global::Android.App.Application.Context.ApplicationContext.PackageName, 0)
                .VersionName;
#elif IOS
        version = Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
#endif

            return version;
        }

        public static string Enviorment()
        {
            string enviorment = string.Empty;
            switch (Config.URL_API_BASE)
            {
                case "https://topspacews.bayer.com.br/api/":
                    enviorment = "Produção";
                    break;
                case "http://10.27.114.8:8096/api/":
                    enviorment = "Qualidade";
                    break;
                case "http://BY0V0N:8096/api/":
                    enviorment = "Desenvolvimento";
                    break;
                default:
                    enviorment = "Ambiente nao encontrado";
                    break;
            }
            return enviorment;
        }

        public static string API_Address()
        {
            return Config.URL_API_BASE;
        }

        public static string Full_Description()
        {
            return $"App Version: {Version()} | Ambiente: {Enviorment()} | API: {API_Address()}";
        }
	}
}

