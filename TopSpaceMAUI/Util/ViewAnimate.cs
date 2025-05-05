using Microsoft.Maui.Graphics;
using System;
using MainDisplayInfo = Microsoft.Maui.Devices.DeviceDisplay;

namespace TopSpaceMAUI.Util
{
	public static class ViewAnimate
	{
		const float KEYBOARD_ANIMATION_DURATION = 0.3f;
		const float MINIMUM_SCROLL_FRACTION = 0.2f;
		const float MAXIMUM_SCROLL_FRACTION = 0.8f;
		public const float LANDSCAPE_KEYBOARD_HEIGHT = 352f;
		static float animatedDistance;

		public static Rect CurrentScreenBounds()
		{
            Rect screenBounds = new Rect
            {
                Width = MainDisplayInfo.MainDisplayInfo.Width,
                Height = MainDisplayInfo.MainDisplayInfo.Height
            };
           
			if (XNSUserDefaults.GetIntValueForKey(Config.KEY_OS_VERSION) < 8)
				return screenBounds;


            double width = screenBounds.Width;
            double height = screenBounds.Height;

			DisplayOrientation interfaceOrientation = MainDisplayInfo.Current.MainDisplayInfo.Orientation;
			if (interfaceOrientation == DisplayOrientation.Landscape) {
				screenBounds.Size = new Size(width, height);
			} else {
				screenBounds.Size = new Size(height, width);
			}

			return screenBounds;
		}

		public static void ViewUp (IView view, bool transform = false)
		{
			animatedDistance = (float)Math.Floor (LANDSCAPE_KEYBOARD_HEIGHT);
			if (!transform) {
				animatedDistance *= 0.5f;
			}

			Rect viewFrame = view.Frame;

			if (!transform) {
				viewFrame.Y -= animatedDistance;
			} else {
                DisplayOrientation interfaceOrientation = MainDisplayInfo.Current.MainDisplayInfo.Orientation;
                if (interfaceOrientation == DisplayOrientation.Landscape) {
					if (XNSUserDefaults.GetIntValueForKey (Config.KEY_OS_VERSION) >= 8) {
						viewFrame.Y -= animatedDistance;
					} else {
						viewFrame.X -= animatedDistance;
					}
				} else {
					if (XNSUserDefaults.GetIntValueForKey (Config.KEY_OS_VERSION) >= 8) {
						viewFrame.Y -= animatedDistance;
					} else {
						viewFrame.X += animatedDistance;
					}
				}
			}

			//UIView.BeginAnimations (null);
			//UIView.SetAnimationBeginsFromCurrentState (true);
			//UIView.SetAnimationDuration (KEYBOARD_ANIMATION_DURATION);

			view.Frame = viewFrame;
			//UIView.CommitAnimations ();
		}
			
		public static void ViewDown (IView view, bool transform = false)
		{
			Rect viewFrame = view.Frame;

			if (!transform) {
				viewFrame.Y += animatedDistance;
			} else {
                DisplayOrientation interfaceOrientation = MainDisplayInfo.Current.MainDisplayInfo.Orientation;
                if (interfaceOrientation == DisplayOrientation.Landscape) {
					if (XNSUserDefaults.GetIntValueForKey (Config.KEY_OS_VERSION) >= 8) {
						viewFrame.Y += animatedDistance;
					} else {
						viewFrame.X += animatedDistance;
					}
				} else {
					if (XNSUserDefaults.GetIntValueForKey (Config.KEY_OS_VERSION) >= 8) {
						viewFrame.Y += animatedDistance;
					} else {
						viewFrame.X -= animatedDistance;
					}
				}
			}

			//UIView.BeginAnimations (null);
			//UIView.SetAnimationBeginsFromCurrentState (true);
			//UIView.SetAnimationDuration (KEYBOARD_ANIMATION_DURATION);

			view.Frame = viewFrame;
			//UIView.CommitAnimations ();
		}

		public static IView RootView (IView actualView)
		{
			IView view = actualView;
			//while (view.Superview != null) {
			//	view = view.Superview;
			//}

			return view;
		}
	}
}
