﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2020 ShareX Team

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
using System.ComponentModel;
using System.Drawing;

namespace ShareX.ImageEffectsLib
{
    public class Resize : ImageEffect
    {
        [DefaultValue(250), Description("Use width as 0 to automatically adjust width to maintain aspect ratio.")]
        public int Width { get; set; }

        [DefaultValue(0), Description("Use height as 0 to automatically adjust height to maintain aspect ratio.")]
        public int Height { get; set; }

        [DefaultValue(ResizeMode.ResizeAll)]
        public ResizeMode Mode { get; set; }

        public Resize()
        {
            this.ApplyDefaultPropertyValues();
        }

        public Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override Bitmap Apply(Bitmap bmp)
        {
            if (Width <= 0 && Height <= 0)
            {
                return bmp;
            }

            int width = Width <= 0 ? (int)((float)Height / bmp.Height * bmp.Width) : Width;
            int height = Height <= 0 ? (int)((float)Width / bmp.Width * bmp.Height) : Height;

            if ((Mode == ResizeMode.ResizeIfBigger && bmp.Width <= width && bmp.Height <= height) ||
                (Mode == ResizeMode.ResizeIfSmaller && bmp.Width >= width && bmp.Height >= height))
            {
                return bmp;
            }

            return ImageHelpers.ResizeImage(bmp, width, height);
        }
    }
}