using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Model
{
	public class SyncLog : IDisposable
	{
		public SyncLog (string message, string detail, bool isError) : base ()
		{
			Message = message;
			Detail = detail;
            IsError = isError;
		}

		public string Message { get; protected set; }



		public string Detail { get; protected set; }



		public bool IsError { get; protected set; }

        public string IsErrorIcon
		{
			get
			{
				return IsError ? "red_icon.png" : "green_icon.png";
			}
		}

        public void Dispose ()
		{
			Message = Detail = null;
		}
	}
}