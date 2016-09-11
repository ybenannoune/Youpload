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
using System.Windows.Forms;

namespace ShareX.ScreenCaptureLib
{
    public abstract class BaseShape : IDisposable
    {
        protected const int MinimumSize = 3;

        public abstract ShapeCategory ShapeCategory { get; }

        public abstract ShapeType ShapeType { get; }

        public Rectangle Rectangle { get; set; }

        private Point startPosition;

        public Point StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;

                Rectangle = CaptureHelpers.CreateRectangle(StartPosition, EndPosition);
            }
        }

        private Point endPosition;

        public Point EndPosition
        {
            get
            {
                return endPosition;
            }
            set
            {
                endPosition = value;

                Rectangle = CaptureHelpers.CreateRectangle(StartPosition, EndPosition);
            }
        }

        public virtual bool IsValidShape => !Rectangle.IsEmpty && Rectangle.Width >= MinimumSize && Rectangle.Height >= MinimumSize;

        internal ShapeManager Manager { get; set; }

        protected RegionCaptureOptions Options => Manager.Config;

        protected AnnotationOptions AnnotationOptions => Manager.Config.AnnotationOptions;

        private Point tempNodePos, tempStartPos, tempEndPos;

        public virtual bool Intersects(Point position)
        {
            return Rectangle.Contains(position);
        }

        public virtual void ShowNodes()
        {
            Manager.NodesVisible = true;
        }

        public void Remove()
        {
            Manager.DeleteShape(this);
        }

        public void AddShapePath(GraphicsPath gp, int sizeOffset = 0)
        {
            Rectangle rect = Rectangle;

            if (sizeOffset != 0)
            {
                rect = rect.SizeOffset(sizeOffset);
            }

            OnShapePathRequested(gp, rect);
        }

        public virtual void Move(int x, int y)
        {
            Rectangle = Rectangle.LocationOffset(x, y);
        }

        public void Move(Point point)
        {
            Move(point.X, point.Y);
        }

        public virtual void Resize(int x, int y, bool fromBottomRight)
        {
            if (fromBottomRight)
            {
                Rectangle = Rectangle.SizeOffset(x, y);
            }
            else
            {
                Rectangle = Rectangle.LocationOffset(x, y).SizeOffset(-x, -y);
            }
        }

        public virtual void OnCreating()
        {
            Point pos = InputManager.MousePosition0Based;

            if (Options.IsFixedSize && ShapeCategory == ShapeCategory.Region)
            {
                Manager.IsMoving = true;
                Rectangle = new Rectangle(new Point(pos.X - Options.FixedSize.Width / 2, pos.Y - Options.FixedSize.Height / 2), Options.FixedSize);
            }
            else
            {
                Manager.IsCreating = true;
                StartPosition = EndPosition = pos;
            }
        }

        public virtual void OnCreated()
        {
        }

        public virtual void OnUpdate()
        {
            if (Manager.IsCreating)
            {
                Point pos = InputManager.MousePosition0Based;

                if (Manager.IsCornerMoving)
                {
                    StartPosition = StartPosition.Add(InputManager.MouseVelocity);
                }
                else if (Manager.IsProportionalResizing)
                {
                    float degree, startDegree;

                    if (ShapeType == ShapeType.DrawingLine || ShapeType == ShapeType.DrawingArrow)
                    {
                        degree = 45;
                        startDegree = 0;
                    }
                    else
                    {
                        degree = 90;
                        startDegree = 45;
                    }

                    pos = CaptureHelpers.SnapPositionToDegree(StartPosition, pos, degree, startDegree);
                }
                else if (Manager.IsSnapResizing)
                {
                    pos = Manager.SnapPosition(StartPosition, pos);
                }

                EndPosition = pos;
            }
            else if (Manager.IsMoving)
            {
                Move(InputManager.MouseVelocity);
            }
        }

        public virtual void OnShapePathRequested(GraphicsPath gp, Rectangle rect)
        {
            gp.AddRectangle(rect);
        }

        public virtual void OnConfigLoad()
        {
        }

        public virtual void OnConfigSave()
        {
        }

        public virtual void OnDoubleClicked()
        {
        }

        public virtual void OnNodeVisible()
        {
            for (int i = 0; i < 8; i++)
            {
                ResizeNode node = Manager.ResizeNodes[i];
                node.Shape = NodeShape.Square;
                node.Visible = true;
            }
        }

        public virtual void OnNodeUpdate()
        {
            for (int i = 0; i < 8; i++)
            {
                ResizeNode node = Manager.ResizeNodes[i];

                if (node.IsDragging)
                {
                    Manager.IsResizing = true;

                    if (!InputManager.IsBeforeMouseDown(MouseButtons.Left))
                    {
                        tempNodePos = node.Position;
                        tempStartPos = Rectangle.Location;
                        tempEndPos = new Point(Rectangle.X + Rectangle.Width - 1, Rectangle.Y + Rectangle.Height - 1);
                    }

                    Point pos = InputManager.MousePosition0Based;
                    Point startPos = tempStartPos;
                    Point endPos = tempEndPos;

                    NodePosition nodePosition = (NodePosition)i;

                    int x = pos.X - tempNodePos.X;

                    switch (nodePosition)
                    {
                        case NodePosition.TopLeft:
                        case NodePosition.Left:
                        case NodePosition.BottomLeft:
                            startPos.X += x;
                            break;
                        case NodePosition.TopRight:
                        case NodePosition.Right:
                        case NodePosition.BottomRight:
                            endPos.X += x;
                            break;
                    }

                    int y = pos.Y - tempNodePos.Y;

                    switch (nodePosition)
                    {
                        case NodePosition.TopLeft:
                        case NodePosition.Top:
                        case NodePosition.TopRight:
                            startPos.Y += y;
                            break;
                        case NodePosition.BottomLeft:
                        case NodePosition.Bottom:
                        case NodePosition.BottomRight:
                            endPos.Y += y;
                            break;
                    }

                    Rectangle = CaptureHelpers.CreateRectangle(startPos, endPos);
                }
            }
        }

        public virtual void OnNodePositionUpdate()
        {
            int xStart = Rectangle.X;
            int xMid = Rectangle.X + Rectangle.Width / 2;
            int xEnd = Rectangle.X + Rectangle.Width - 1;

            int yStart = Rectangle.Y;
            int yMid = Rectangle.Y + Rectangle.Height / 2;
            int yEnd = Rectangle.Y + Rectangle.Height - 1;

            Manager.ResizeNodes[(int)NodePosition.TopLeft].Position = new Point(xStart, yStart);
            Manager.ResizeNodes[(int)NodePosition.Top].Position = new Point(xMid, yStart);
            Manager.ResizeNodes[(int)NodePosition.TopRight].Position = new Point(xEnd, yStart);
            Manager.ResizeNodes[(int)NodePosition.Right].Position = new Point(xEnd, yMid);
            Manager.ResizeNodes[(int)NodePosition.BottomRight].Position = new Point(xEnd, yEnd);
            Manager.ResizeNodes[(int)NodePosition.Bottom].Position = new Point(xMid, yEnd);
            Manager.ResizeNodes[(int)NodePosition.BottomLeft].Position = new Point(xStart, yEnd);
            Manager.ResizeNodes[(int)NodePosition.Left].Position = new Point(xStart, yMid);
        }

        public virtual void Dispose()
        {
        }
    }
}