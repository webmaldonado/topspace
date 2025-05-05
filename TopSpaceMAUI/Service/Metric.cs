using System;
using TopSpaceMAUI.Util;
#if IOS
	using Foundation;
#elif ANDROID
	using Android.Content;
	using Android.Content.PM;
	using Android.App;
#endif


namespace TopSpaceMAUI.Service
{
	public class Metric : Syncable<Model.MetricTemp, DAL.Metric>
	{
		public Metric (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityMetric");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_METRICS, DAL.Token.Current.Username, GetAppVersion());
		}

        public static string GetAppVersion()
        {
			#if IOS
				return NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
			#elif ANDROID
				Context context = Android.App.Application.Context;
				PackageInfo packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
				return packageInfo.VersionName;
			#else
				throw new NotImplementedException("Plataforma não suportada");
			#endif
        }
    }
}