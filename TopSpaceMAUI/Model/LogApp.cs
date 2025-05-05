using System;
using SQLite;

namespace TopSpaceMAUI.Model
{
	[Table ("LogApp")]
	public class LogApp
	{
		[PrimaryKey, AutoIncrement, Column ("AppID")]
		public int AppID { get; set; }

		[Column ("DeviceDate")]
		public string DeviceDate { get; set; }

		[Column ("DeviceTimezone")]
		public string DeviceTimezone { get; set; }

		[Column ("InstallID")]
		public string InstallID { get; set; }

		[Column ("AppVersion")]
		public string AppVersion { get; set; }

		[Column ("LogType")]
		public string LogType { get; set; }

		[Column ("Operation")]
		public string Operation { get; set; }

		[Column ("EntityType")]
		public string EntityType { get; set; }

		[Column ("EntityID")]
		public string EntityID { get; set; }

		[Column ("Description")]
		public string Description { get; set; }

		[Column ("URL")]
		public string URL { get; set; }

		[Column ("Comments")]
		public string Comments { get; set; }	
	}
}
