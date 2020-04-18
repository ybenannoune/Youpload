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
using System;
using System.Windows.Forms;

namespace ShareX.ScreenCaptureLib
{
    public partial class CanvasSizeForm : Form
    {
        public Padding Canvas { get; private set; }

        public CanvasSizeForm()
        {
            InitializeComponent();
            ShareXResources.ApplyTheme(this);
        }

        public CanvasSizeForm(Padding canvas) : this()
        {
            Canvas = canvas;
            nudLeft.SetValue(canvas.Left);
            nudTop.SetValue(canvas.Top);
            nudRight.SetValue(canvas.Right);
            nudBottom.SetValue(canvas.Bottom);
        }

        private void CanvasSizeForm_Shown(object sender, EventArgs e)
        {
            this.ForceActivate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Canvas = new Padding((int)nudLeft.Value, (int)nudTop.Value, (int)nudRight.Value, (int)nudBottom.Value);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}