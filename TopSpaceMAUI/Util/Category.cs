using System;

namespace TopSpaceMAUI.Util
{
	public static class Category
	{
		public static int CategorySample(int category)
		{
			if (category == 1) {
				return 20;
			} else if (category == 2) {
				return 30;
			} else if (category == 3) {
				return 60;
			} else if (category >= 4) {
				return 100;
			} else {
				return 20;
			}
		}
	}
}

