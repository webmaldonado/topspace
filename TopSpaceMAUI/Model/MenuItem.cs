using System;

namespace TopSpaceMAUI.Model
{
	public class MenuItem
	{
		public string Title { get; set; }
		public string Value { get; set; }
		public string Image { get; set; }
		public string Notification { get; set; }
		public event EventHandler Selected;

		public void OnSelected (object sender, EventArgs e)
		{
			var h = Selected;
			if (h != null)
				h (null, EventArgs.Empty);
		}
	}
}

