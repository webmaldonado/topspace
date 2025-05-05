using System;
using SQLite;
using Newtonsoft.Json;

namespace TopSpaceMAUI.Model
{
	[Table("Message")]
	public class Message : IDisposable
	{
		[PrimaryKey, Column("MessageID")]
		public string MessageID { get; set; }

		[Column("Body")]
		public string Body { get; set; }

		[Column("From")][JsonIgnore]
		public string From { get; set; }

		[Column("To")]
		public string To { get; set; }

		private string _status;
		[Column("Status")][JsonIgnore]
		public string Status {
			get { 
				if (_status == null) { 
					_status = Config.MESSAGE_WHATS_GOING_ON_MESSAGE_SENT; 
				} 
				return _status; 
			} 
			set { _status = value; } 
		}

		[Column("CreationDate")][JsonIgnore]
		public string CreationDate { get; set; }

		[Column("ChannelCode")][JsonIgnore]
		public string ChannelCode { get; set; }

		private string _type;
		[Column("Type")][JsonIgnore]
		public string Type {
			get { 
				if (_type == null) { 
					_type = Config.MESSAGE_WHATS_GOING_ON_MESSAGE_SERVER; 
				} 
				return _type; 
			} 
			set { _type = value; } 
		}
			
		public void Dispose ()
		{
			MessageID = Body = From = To = Status = CreationDate = ChannelCode = Type = null;
		}
	}
}

