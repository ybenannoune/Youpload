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
using System.Drawing;

namespace ShareX.ScreenCaptureLib
{
    public class PixelateEffectShape : BaseEffectShape
    {
        public override ShapeType ShapeType { get; } = ShapeType.EffectPixelate;

        public override string OverlayText => $"Pixelate [{PixelSize}]";

        public int PixelSize { get; set; }

        public override void OnConfigLoad()
        {
            PixelSize = AnnotationOptions.PixelateSize;
        }

        public override void OnConfigSave()
        {
            AnnotationOptions.PixelateSize = PixelSize;
        }

        public override void ApplyEffect(Bitmap bmp)
        {
            ImageHelpers.Pixelate(bmp, PixelSize);
        }

        public override void OnDrawFinal(Graphics g, Bitmap bmp)
        {
            if (PixelSize > 1)
            {
                base.OnDrawFinal(g, bmp);
            }
        }
    }
}