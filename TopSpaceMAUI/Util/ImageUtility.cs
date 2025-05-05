using Microsoft.Maui.Graphics;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace TopSpaceMAUI.Util
{
	public class ImageUtility
	{
        //public static IImage ResizeImage (IImage originalImage, float maxWidth, float maxHeight)
        //{
        //	double width = originalImage.Width, height = originalImage.Height;

        //	double maxAspect = (double)maxWidth / (double)maxHeight;
        //	double aspect = (double)originalImage.Width / (double)originalImage.Height;

        //	if (maxAspect > aspect && originalImage.Width > maxWidth) {
        //		//Width is the bigger dimension relative to max bounds
        //		width = maxWidth;
        //		height = maxWidth / aspect;
        //	} else if (maxAspect <= aspect && originalImage.Height > maxHeight) {
        //		//Height is the bigger dimension
        //		height = maxHeight;
        //		width = maxHeight * aspect;
        //	}

        //	return originalImage.Resize((float)width, (float)height, ResizeMode.Fit); //.AsJPEG (Config.PHOTO_COMPRESSION);
        //}

        public static IImage ResizeImage(IImage originalImage, float maxWidth, float maxHeight)
        {
            double width = originalImage.Width, height = originalImage.Height;

            double maxAspect = (double)maxWidth / (double)maxHeight;
            double aspect = (double)originalImage.Width / (double)originalImage.Height;

            if (maxAspect > aspect && originalImage.Width > maxWidth)
            {
                //Width is the bigger dimension relative to max bounds
                width = maxWidth;
                height = maxWidth / aspect;
            }
            else if (maxAspect <= aspect && originalImage.Height > maxHeight)
            {
                //Height is the bigger dimension
                height = maxHeight;
                width = maxHeight * aspect;
            }

            return originalImage.Resize((float)width, (float)height, ResizeMode.Fit); //.AsJPEG (Config.PHOTO_COMPRESSION);
        }
    }
}
