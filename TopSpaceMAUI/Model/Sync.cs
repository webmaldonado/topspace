using System;
using System.Collections.Generic;
namespace TopSpaceMAUI.Model
{
	public class Sync
	{
		public static float Progress { get; set; }

		public static string Stage { get; set; }

		public static int VisitSyncs { get; set; }

		static Sync ()
		{
			LogHasErrors = false;
		}



		#region Log



		protected static System.Collections.ObjectModel.ObservableCollection<Model.SyncLog> _log = null;
        //protected static List<Model.SyncLog> _log = null;
        protected static SyncLog _lastLog = null;
		public static bool LogHasErrors { get; protected set; }



        //public static List<Model.SyncLog> Log {
        //	get {
        //		if (_log == null)
        //			_log = new List<SyncLog> ();
        //		return _log;
        //	}
        //	protected set {
        //		_log = value;
        //	}
        //}
        public static System.Collections.ObjectModel.ObservableCollection<Model.SyncLog> Log
        {
            get
            {
                if (_log == null)
                    _log = new System.Collections.ObjectModel.ObservableCollection<SyncLog>();
                return _log;
            }
            protected set
            {
                _log = value;
            }
        }



        public static SyncLog LastLog {
			get {
				return _lastLog;
			}
			protected set {
				_lastLog = value;
			}
		}



		public static void ResetLog ()
		{
			LogHasErrors = false;
			//Log.ForEach (l => l.Dispose ());
			Log.ToList().ForEach(l => l.Dispose());
			Log.Clear ();
			Log = null;
			LastLog = null;
		}



		protected static void AddLog (string message, string detail, bool isError)
		{
			SyncLog l = new SyncLog (message, detail, isError);
			Log.Add (l);
            //ScrollToEnd?.Invoke();

            LastLog = l;

			if (isError && !LogHasErrors)
				LogHasErrors = true;
		}

        public static Action ScrollToEnd { get; set; }


        public static void LogThis (string message, object[] formatValue, string detail, bool isError)
		{
			if (formatValue != null && formatValue.Length > 0 && formatValue[0] is Exception && !string.IsNullOrEmpty ((formatValue[0] as Exception).Message))
				formatValue[0] = (formatValue[0] as Exception).Message;

			if (string.IsNullOrEmpty (message)) {
				if (formatValue != null && formatValue.Length > 0 && !string.IsNullOrEmpty (formatValue [0].ToString ()))
					message = formatValue [0].ToString ();
				else if (!string.IsNullOrEmpty (detail))
					message = detail;
				else
					return; // não tem nem message, nem o... não tem o que logar...
			}
			else if (formatValue != null && formatValue.Length > 0) {
				for (int i = 0; i < formatValue.Length; i++) {
					if (formatValue [i] == null)
						formatValue [i] = "";
				}

				message = string.Format (message, formatValue);
			}

			AddLog (message, detail, isError);
		}



		public static void LogThis (string message, object formatValue = null, bool isError = false)
		{
			if (formatValue == null)
				LogThis (message, null, null, isError);
			else
				LogThis (message, new object[] { formatValue }, null, isError);
		}



		public static void LogInfo (string message, object[] formatValue, string detail)
		{
			LogThis (message, formatValue, detail, false);
		}



		public static void LogInfo (string message, object formatValue = null)
		{
			if (formatValue == null)
				LogInfo (message, null, null);
			else
				LogInfo (message, new object[] { formatValue }, null);
		}



		public static void LogError (string message, object[] formatValue, string detail)
		{
			LogThis (message, formatValue, detail, true);
		}



		public static void LogError (string message, object formatValue = null)
		{
			if (formatValue == null)
				LogError (message, null, null);
			else
				LogError (message, new object[] { formatValue }, null);
		}



		#endregion Log
	}
}