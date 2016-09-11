﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2016 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShareX.ScreenCaptureLib
{
    public class SpeechBalloonDrawingShape : TextDrawingShape
    {
        public override ShapeType ShapeType { get; } = ShapeType.DrawingSpeechBalloon;

        private Point tailPosition;

        public Point TailPosition
        {
            get
            {
                return tailPosition;
            }
            private set
            {
                tailPosition = value;
                TailNode.Position = tailPosition;
            }
        }

        public bool TailVisible => !Rectangle.Contains(TailPosition);

        internal ResizeNode TailNode => Manager.ResizeNodes[(int)NodePosition.Extra];

        // If rectangle average size is 100px then tail width will be 30px
        protected const float TailWidthMultiplier = 0.3f;

        public override void OnCreated()
        {
            base.OnCreated();

            TailPosition = Rectangle.Location.Add(0, Rectangle.Height + 30);
        }

        public override void OnNodeVisible()
        {
            base.OnNodeVisible();

            TailNode.Position = TailPosition;
            TailNode.Shape = NodeShape.Circle;
            TailNode.Visible = true;
        }

        public override void OnNodeUpdate()
        {
            base.OnNodeUpdate();

            if (TailNode.IsDragging)
            {
                TailPosition = InputManager.MousePosition0Based;
            }
        }

        public override void Move(int x, int y)
        {
            base.Move(x, y);

            TailPosition = TailPosition.Add(x, y);
        }

        public override void OnDraw(Graphics g)
        {
            if (Rectangle.Width > 10 && Rectangle.Height > 10)
            {
                GraphicsPath gpTail = null;

                if (TailVisible)
                {
                    gpTail = CreateTailPath();
                }

                if (FillColor.A > 0)
                {
                    using (Brush brush = new SolidBrush(FillColor))
                    {
                        g.FillRectangle(brush, Rectangle);
                    }
                }

                if (gpTail != null)
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if (FillColor.A > 0)
                    {
                        g.ExcludeClip(Rectangle);

                        using (Brush brush = new SolidBrush(FillColor))
                        {
                            g.FillPath(brush, gpTail);
                        }

                        g.ResetClip();
                    }

                    if (BorderSize > 0 && BorderColor.A > 0)
                    {
                        g.ExcludeClip(Rectangle.Offset(-1));

                        using (Pen pen = new Pen(BorderColor, BorderSize))
                        {
                            g.DrawPath(pen, gpTail);
                        }

                        g.ResetClip();
                    }

                    g.SmoothingMode = SmoothingMode.None;
                }

                if (BorderSize > 0 && BorderColor.A > 0)
                {
                    if (gpTail != null)
                    {
                        using (Region region = new Region(gpTail))
                        {
                            g.ExcludeClip(region);
                        }
                    }

                    Rectangle rect = Rectangle.Offset(BorderSize - 1);

                    using (Pen pen = new Pen(BorderColor, BorderSize) { Alignment = PenAlignment.Inset })
                    {
                        g.DrawRectangleProper(pen, rect);
                    }

                    g.ResetClip();
                }

                if (gpTail != null)
                {
                    gpTail.Dispose();
                }

                DrawText(g);
            }
        }

        protected GraphicsPath CreateTailPath()
        {
            GraphicsPath gpTail = new GraphicsPath();
            Point center = Rectangle.Center();
            int rectAverageSize = (Rectangle.Width + Rectangle.Height) / 2;
            int tailWidth = (int)(TailWidthMultiplier * rectAverageSize);
            tailWidth = Math.Min(Math.Min(tailWidth, Rectangle.Width), Rectangle.Height);
            int tailOrigin = tailWidth / 2;
            int tailLength = (int)MathHelpers.Distance(center, TailPosition);
            gpTail.AddLine(0, -tailOrigin, 0, tailOrigin);
            gpTail.AddLine(0, tailOrigin, tailLength, 0);
            gpTail.CloseFigure();
            using (Matrix matrix = new Matrix())
            {
                matrix.Translate(center.X, center.Y);
                float tailDegree = MathHelpers.LookAtDegree(center, TailPosition);
                matrix.Rotate(tailDegree);
                gpTail.Transform(matrix);
            }
            return gpTail;
        }
    }
}