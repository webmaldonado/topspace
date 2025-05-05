using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpaceMAUI.Model;

namespace TopSpaceMAUI.Util
{
    public class UserResizableRuler : GraphicsView, IDrawable
    {
        public double UsedOffset { get; set; } = 0;
        public double UsedWidth { get; set; } = 0;
        public double Score { get; set; } = 0;
        public bool IsActive { get; set;} = false;

        public const float RulerBorder = 4.0f;
        public const float RulerPadding = 4.0f;

        private bool IsMoving { get; set; } = false;
        private bool IsResizing { get; set; } = false;
        private bool IsMovingUsed { get; set; } = false;
        private bool IsResizingUsed { get; set; } = false;

        public event EventHandler<UserResizableRuler> Activated;
        public event EventHandler<double> ScoreUpdated;

        private float MovingOffset = 0;

        public UserResizableRuler()
        {
            Drawable = this;
            StartInteraction += UserResizableRuler_StartInteraction;
            DragInteraction += UserResizableRuler_DragInteraction;
            EndInteraction += UserResizableRuler_EndInteraction;
        }

        private void UserResizableRuler_StartInteraction(object? sender, TouchEventArgs e)
        {
            PointF point = e.Touches[0];
            IsResizingUsed = (point.X > (float)(UsedOffset + UsedWidth) - 4*RulerPadding) && (point.X < (float)(UsedOffset + UsedWidth) + 2*RulerPadding);
            IsMovingUsed = !IsResizingUsed && point.X >= (float)UsedOffset && point.X <= (float)(UsedOffset + UsedWidth);
            IsResizing = !IsResizingUsed && (point.X >= this.Width - 4*RulerPadding);
            Activated?.Invoke(this, this);
        }

        private void UserResizableRuler_EndInteraction(object? sender, TouchEventArgs e)
        {
            if (IsResizing || IsResizingUsed)
            {
                foreach (var item in e.Touches)
                {
                    if (IsResizingUsed)
                    {
                        double increase = (item.X - this.UsedWidth - this.UsedOffset - 2*RulerPadding);
                        if ((UsedOffset + UsedWidth + increase) > (this.Width - RulerPadding))
                        {
                            increase = this.Width - (UsedOffset + UsedWidth + 2*RulerPadding);
                        }
                        this.UsedWidth += (double)increase;
                        this.Invalidate();
                    }
                    else
                    {
                        float increase = (item.X - (float)this.Width);
                        ((AbsoluteLayout)this.Parent).SetLayoutBounds(this, new RectF((float)this.X, (float)this.Y, (float)this.Width + increase, (float)this.Height));
                    }
                    IsResizing = false;
                    IsResizingUsed = false;
                }

            }
            MovingOffset = 0;

            Score = (UsedWidth / (this.Width - RulerPadding)) * 100;
            Score = Double.Round(Score * 100) / 100;
            ScoreUpdated?.Invoke(this, Score);
        }


        private void UserResizableRuler_DragInteraction(object? sender, TouchEventArgs e)
        {
            if (!(IsResizing || IsResizingUsed))
            {
                foreach (var item in e.Touches)
                {
                    if (MovingOffset == 0)
                    {
                        MovingOffset = item.X;
                    }
 
                    if (IsMovingUsed)
                    {
                        UsedOffset = item.X > RulerPadding ? (double)item.X : RulerPadding;
                        if (UsedOffset + UsedWidth > this.Width)
                        {
                            UsedOffset = this.Width - UsedWidth - RulerPadding;
                        }
                        if (UsedOffset < RulerPadding)
                        {
                            UsedOffset = RulerPadding;
                        }
                        this.Invalidate();
                    }
                    else
                    {
                        ((AbsoluteLayout)this.Parent).SetLayoutBounds(this, new RectF((float)this.X + item.X - MovingOffset, (float)this.Y + item.Y, (float)this.Width, (float)this.Height));
                        if (item.X % 11 == 0)
                        {
                            this.Invalidate();
                        }
                    }
                }
            } else
            {
                foreach (var item in e.Touches)
                {
                    if (IsResizingUsed)
                    {
                        double increase = (item.X - this.UsedWidth - this.UsedOffset - 2 * RulerPadding);
                        if ((UsedOffset + UsedWidth + increase) > (this.Width - RulerPadding))
                        {
                            increase = this.Width - (UsedOffset + UsedWidth + 2 * RulerPadding);
                        }
                        this.UsedWidth += (double)increase;
                        this.Invalidate();
                    }
                    else
                    {
                        float increase = (item.X - (float)this.Width);
                        ((AbsoluteLayout)this.Parent).SetLayoutBounds(this, new RectF((float)this.X, (float)this.Y, (float)this.Width + increase, (float)this.Height));
                    }
                }
            }
        }


        public void ToggleActive(bool active)
        {
            this.IsActive = active;
            this.Invalidate();
        }


        #region GripViewBorderDrawable
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // (1) Draw the bounding box.
            if (IsActive)
            {
                canvas.StrokeSize = (float)1.5 * RulerBorder;
                canvas.StrokeColor = Color.FromRgb(16, 247, 199);
            }
            else
            {
                canvas.StrokeSize = RulerBorder;
                canvas.StrokeColor = Color.FromRgb(148, 246, 90);
            }
            canvas.FillColor = Colors.Transparent;
            canvas.DrawRectangle(new RectF(0, 0, (float)this.Width, (float)this.Height));

            if (IsActive)
            {
                canvas.FillColor = Color.FromRgba(16, 247, 199, 40);
                canvas.FillRoundedRectangle(new Rect(this.Width - 4 * RulerPadding, 5, 4 * RulerPadding, this.Height - 10), 20.0);
            }

            if (UsedWidth > 0)
            {
                // (1) Draw the bounding box.
                canvas.StrokeSize = 2;
                canvas.StrokeColor = Color.FromRgba(235, 60, 0, 125);
                canvas.FillColor = Color.FromRgba(235, 60, 0, 90);
                canvas.DrawRectangle(new RectF((float)UsedOffset + RulerPadding, RulerPadding, (float)UsedWidth, (float)this.Height - (2 * RulerBorder)));
                canvas.FillRectangle(new RectF((float)UsedOffset + RulerPadding, RulerPadding, (float)UsedWidth, (float)this.Height - (2 * RulerBorder)));

                if (IsActive)
                {
                    canvas.FillColor = Color.FromRgba(215, 50, 0, 80);
                    canvas.FillRoundedRectangle(new Rect((float)UsedOffset + this.UsedWidth - 4 * RulerPadding, 8, 4 * RulerPadding, this.Height - 16), 20.0);
                }
            }
        }
        #endregion
    }
}
