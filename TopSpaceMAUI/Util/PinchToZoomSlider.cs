using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSpaceMAUI.Util
{
    public class PinchToZoomSlider : Slider
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        public PinchToZoomSlider()
        {
            PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = this.Scale;
                this.AnchorX = 0;
                this.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = this.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (this.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = this.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (this.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * this.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * this.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                this.TranslationX = Math.Clamp(targetX, -this.Width * (currentScale - 1), 0);
                this.TranslationY = Math.Clamp(targetY, -this.Height * (currentScale - 1), 0);

                // Apply scale factor
                this.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = this.TranslationX;
                yOffset = this.TranslationY;
            }
        }
    }

}