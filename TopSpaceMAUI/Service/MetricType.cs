using System;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.Service
{
	public class MetricType : Syncable<Model.MetricTypeTemp, DAL.MetricType>
	{
		public MetricType (string whenUtc) : base (whenUtc)
		{
		}



		public override string GetEntityName ()
		{
			return Localization.TryTranslateText("EntityMetricType");
		}



		protected override string GetRestClientBaseUrl ()
		{
			return Config.URL_API_BASE;
		}



		protected override string GetRequestResource ()
		{
			return String.Format (Config.URL_API_REQUEST_METRIC_TYPES, DAL.Token.Current.Username);
		}
	}
}