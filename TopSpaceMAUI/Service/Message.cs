using System;
using TopSpaceMAUI.Util;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using TopSpaceMAUI.DAL;

namespace TopSpaceMAUI.Service
{
	public class Message
	{
		private DAL.Message messageDAL;

		public string Channel { get; set; }

		public Message (string channel)
		{
			messageDAL = new DAL.Message ();
			Channel = channel;
		}

		public List<Model.Message> GetData (string channel)
		{
			return messageDAL.GetAll ().Where(m => m.ChannelCode == channel && m.Status == Config.MESSAGE_WHATS_GOING_ON_MESSAGE_NOT_SEND && m.Type == Config.MESSAGE_WHATS_GOING_ON_MESSAGE_LOCAL).ToList();
		}

		public void InsertData(List<Model.Message> messages) 
		{
			messageDAL.InsertAll (messages);
		}

		public void UpdateSent(string messageID) 
		{
			messageDAL.UpdateSent (messageID);
		}

		public void CleanSentData(string channel) 
		{
			messageDAL.CleanSentData (channel);
		}
	}
}

