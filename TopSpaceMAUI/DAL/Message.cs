using System;
using System.Linq;
using TopSpaceMAUI.Util;
using SQLite;
using System.Collections.Generic;

namespace TopSpaceMAUI.DAL
{
	public class Message
	{
		public List<Model.Message> GetAll()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<Model.Message> message = db.Table<Model.Message> ().OrderBy (m => m.CreationDate).ToList ();
			Database.Close (db);
			db = null;
			return message;
		}

		public void InsertAll(List<Model.Message> message)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertAll(message);
			Database.Close (db);
			db = null;
		}

		public void Insert(Model.Message message)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			db.InsertOrReplace(message);
			Database.Close (db);
			db = null;
		}

		public void UpdateSent(string messageID)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format("UPDATE Message SET Status = '{0}' WHERE MessageID = '{1}' AND Type = '{2}'", Config.MESSAGE_WHATS_GOING_ON_MESSAGE_SENT, messageID, Config.MESSAGE_WHATS_GOING_ON_MESSAGE_LOCAL);
			db.Execute (query);
			Database.Close (db);
			db = null;
		} 

		public void CleanSentData(string channel)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			string query = String.Format("DELETE FROM Message WHERE ChannelCode = '{0}' AND Status = '{1}' AND Type = '{2}'", channel, Config.MESSAGE_WHATS_GOING_ON_MESSAGE_SENT, Config.MESSAGE_WHATS_GOING_ON_MESSAGE_LOCAL);
			db.Execute (query);
			Database.Close (db);
			db = null;
		}
	}
}

