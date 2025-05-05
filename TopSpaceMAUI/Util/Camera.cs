using System;
#if ANDROID
using Android.App;
#elif IOS
using UIKit;
using Foundation;
#endif

namespace TopSpaceMAUI.Util
{
	public static class Camera
	{
        static IMediaPicker _picker;

		static void Init ()
		{
			//if (_picker != null)
			//	return;

			_picker = MediaPicker.Default;
        }

        public async static void TakePicture (Func<FileResult?, bool> callback)
		{
			try {
				Init ();
				FileResult? result = await _picker.CapturePhotoAsync();
				callback.Invoke(result);
			} catch (Exception ex) {
				Util.LogUnhandledException.LogError (ex);
			}
		}

        public static async Task<FileResult?> TakePictureAsync()
        {
            try
            {
                Init();
                return await _picker.CapturePhotoAsync();
            }
            catch (Exception ex)
            {
                Util.LogUnhandledException.LogError(ex);
                return null;
            }
        }


	}
}
