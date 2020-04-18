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
using ShareX.ImageEffectsLib.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ShareX.ImageEffectsLib
{
    public partial class ImageEffectsForm : Form
    {
        public event Action<Bitmap> ImageProcessRequested;

        public bool AutoGeneratePreviewImage { get; set; }
        public Bitmap PreviewImage { get; private set; }
        public List<ImageEffectPreset> Presets { get; private set; }
        public int SelectedPresetIndex { get; private set; }
        public string FilePath { get; private set; }

        private bool ignorePresetsSelectedIndexChanged = false;
        private bool pauseUpdate = false;

        public ImageEffectsForm(Bitmap bmp, List<ImageEffectPreset> presets, int selectedPresetIndex)
        {
            InitializeComponent();
            ShareXResources.ApplyTheme(this);

            PreviewImage = bmp;
            if (PreviewImage == null)
            {
                AutoGeneratePreviewImage = true;
            }

            Presets = presets;
            if (Presets.Count == 0)
            {
                Presets.Add(new ImageEffectPreset());
            }

            SelectedPresetIndex = selectedPresetIndex;
            eiImageEffects.ObjectType = typeof(ImageEffectPreset);
            AddAllEffectsToContextMenu();
        }

        public void EnableToolMode(Action<Bitmap> imageProcessRequested, string filePath = null)
        {
            FilePath = filePath;
            ImageProcessRequested += imageProcessRequested;
            pbResult.AllowDrop = true;
            mbLoadImage.Visible = true;
            btnSaveImage.Visible = true;
            btnUploadImage.Visible = true;
        }

        public void EditorMode()
        {
            btnOK.Visible = true;
            btnClose.Text = Resources.ImageEffectsForm_EditorMode_Cancel;
        }

        protected void OnImageProcessRequested(Bitmap bmp)
        {
            if (ImageProcessRequested != null)
            {
                ImageProcessRequested(bmp);
            }
        }

        private void AddAllEffectsToContextMenu()
        {
            AddEffectToContextMenu(Resources.ImageEffectsForm_AddAllEffectsToTreeView_Drawings,
                typeof(DrawBackground),
                typeof(DrawBorder),
                typeof(DrawCheckerboard),
                typeof(DrawImage),
                typeof(DrawText),
                typeof(DrawParticles));

            AddEffectToContextMenu(Resources.ImageEffectsForm_AddAllEffectsToTreeView_Manipulations,
                typeof(AutoCrop),
                typeof(Canvas),
                typeof(Crop),
                typeof(Flip),
                typeof(Resize),
                typeof(Rotate),
                typeof(RoundedCorners),
                typeof(Scale),
                typeof(Skew));

            AddEffectToContextMenu(Resources.ImageEffectsForm_AddAllEffectsToTreeView_Adjustments,
                typeof(Alpha),
                typeof(BlackWhite),
                typeof(Brightness),
                typeof(Colorize),
                typeof(Contrast),
                typeof(Gamma),
                typeof(Grayscale),
                typeof(Hue),
                typeof(Inverse),
                typeof(MatrixColor),
                typeof(Polaroid),
                typeof(Saturation),
                typeof(SelectiveColor),
                typeof(Sepia));

            AddEffectToContextMenu(Resources.ImageEffectsForm_AddAllEffectsToTreeView_Filters,
                typeof(Blur),
                typeof(EdgeDetect),
                typeof(Emboss),
                typeof(GaussianBlur),
                typeof(MatrixConvolution),
                typeof(MeanRemoval),
                typeof(Outline),
                typeof(Pixelate),
                typeof(Reflection),
                typeof(Shadow),
                typeof(Sharpen),
                typeof(Slice),
                typeof(Smooth),
                typeof(TornEdge));
        }

        private void AddEffectToContextMenu(string groupName, params Type[] imageEffects)
        {
            ToolStripMenuItem tsmiParent = new ToolStripMenuItem(groupName);
            tsmiParent.HideImageMargin();

            cmsEffects.Items.Add(tsmiParent);

            foreach (Type imageEffect in imageEffects)
            {
                ToolStripItem tsmiChild = tsmiParent.DropDownItems.Add(imageEffect.GetDescription().Replace("&", "&&"));
                tsmiChild.Tag = imageEffect;
                tsmiChild.Click += tsmiEffectClick;
            }
        }

        private void LoadSettings()
        {
            pauseUpdate = true;

            foreach (ImageEffectPreset preset in Presets)
            {
                cbPresets.Items.Add(preset);
            }

            if (SelectedPresetIndex > -1 && SelectedPresetIndex < cbPresets.Items.Count)
            {
                cbPresets.SelectedIndex = SelectedPresetIndex;
            }

            UpdateControlStates();

            pauseUpdate = false;
        }

        private ImageEffectPreset GetSelectedPreset()
        {
            int index = cbPresets.SelectedIndex;

            if (Presets.IsValidIndex(index))
            {
                return Presets[index];
            }

            return null;
        }

        private void AddPreset()
        {
            AddPreset(new ImageEffectPreset());
        }

        private void AddPreset(ImageEffectPreset preset)
        {
            if (preset != null)
            {
                Presets.Add(preset);
                cbPresets.Items.Add(preset);
                cbPresets.SelectedIndex = cbPresets.Items.Count - 1;
                LoadPreset(preset);
                txtPresetName.Focus();
            }
        }

        private void UpdatePreview()
        {
            if (!pauseUpdate)
            {
                ImageEffectPreset preset = GetSelectedPreset();

                if (preset != null)
                {
                    Cursor = Cursors.WaitCursor;

                    try
                    {
                        if (AutoGeneratePreviewImage)
                        {
                            GeneratePreviewImage(25);
                        }

                        if (PreviewImage != null)
                        {
                            Stopwatch timer = Stopwatch.StartNew();

                            using (Image preview = ApplyEffects())
                            {
                                if (preview != null)
                                {
                                    pbResult.LoadImage(preview);
                                    Text = string.Format("ShareX - " + Resources.ImageEffectsForm_UpdatePreview_Image_effects___Width___0___Height___1___Render_time___2__ms,
                                        preview.Width, preview.Height, timer.ElapsedMilliseconds);
                                }
                                else
                                {
                                    pbResult.Reset();
                                    Text = string.Format("ShareX - " + Resources.ImageEffectsForm_UpdatePreview_Image_effects___Width___0___Height___1___Render_time___2__ms,
                                        0, 0, timer.ElapsedMilliseconds);
                                }
                            }
                        }
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }

                UpdateControlStates();
            }
        }

        private void UpdateControlStates()
        {
            btnRemovePreset.Enabled = cbPresets.Enabled = txtPresetName.Enabled = btnAdd.Enabled = cbPresets.SelectedIndex > -1;
            btnRemove.Enabled = btnDuplicate.Enabled = lvEffects.SelectedItems.Count > 0;
            btnClear.Enabled = lvEffects.Items.Count > 0;
        }

        private void GeneratePreviewImage(int padding)
        {
            if (pbResult.ClientSize.Width > 0 && pbResult.ClientSize.Height > 0)
            {
                int horizontalPadding = padding, verticalPadding = padding;
                int minSizePadding = 300;

                if (pbResult.ClientSize.Width < (horizontalPadding * 2) + minSizePadding)
                {
                    horizontalPadding = 0;
                }

                if (pbResult.ClientSize.Height < (verticalPadding * 2) + minSizePadding)
                {
                    verticalPadding = 0;
                }

                if (PreviewImage != null) PreviewImage.Dispose();
                PreviewImage = new Bitmap(pbResult.ClientSize.Width - (horizontalPadding * 2), pbResult.ClientSize.Height - (verticalPadding * 2));

                Color backgroundColor;

                if (ShareXResources.UseCustomTheme)
                {
                    backgroundColor = ShareXResources.Theme.BackgroundColor;
                }
                else
                {
                    backgroundColor = Color.DarkGray;
                }

                using (Graphics g = Graphics.FromImage(PreviewImage))
                {
                    g.Clear(backgroundColor);

                    if (PreviewImage.Width > 260 && PreviewImage.Height > 260)
                    {
                        using (Bitmap logo = ShareXResources.Logo)
                        {
                            g.DrawImage(logo, (PreviewImage.Width / 2) - (logo.Width / 2), (PreviewImage.Height / 2) - (logo.Height / 2));
                        }
                    }
                }
            }
        }

        private Bitmap ApplyEffects()
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                return preset.ApplyEffects(PreviewImage);
            }

            return null;
        }

        private void tsmiEffectClick(object sender, EventArgs e)
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

                if (tsmi != null && tsmi.Tag is Type)
                {
                    Type type = (Type)tsmi.Tag;
                    ImageEffect imageEffect = (ImageEffect)Activator.CreateInstance(type);
                    AddEffect(imageEffect, preset);
                    UpdatePreview();
                }
            }
        }

        private void RemoveSelectedEffects()
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                int index = lvEffects.SelectedIndex;

                if (index > -1)
                {
                    preset.Effects.RemoveAt(index);
                    lvEffects.Items.RemoveAt(index);

                    UpdatePreview();
                }
            }
        }

        private void ClearFields()
        {
            txtPresetName.Text = "";
            lvEffects.Items.Clear();
            UpdatePreview();
        }

        private void AddEffect(ImageEffect imageEffect, ImageEffectPreset preset = null)
        {
            pauseUpdate = true;

            ListViewItem lvi = new ListViewItem(imageEffect.GetType().GetDescription());
            lvi.Checked = imageEffect.Enabled;
            lvi.Tag = imageEffect;

            if (lvEffects.SelectedIndices.Count > 0)
            {
                int index = lvEffects.SelectedIndices[lvEffects.SelectedIndices.Count - 1] + 1;
                lvEffects.Items.Insert(index, lvi);

                if (preset != null)
                {
                    preset.Effects.Insert(index, imageEffect);
                }
            }
            else
            {
                lvEffects.Items.Add(lvi);

                if (preset != null)
                {
                    preset.Effects.Add(imageEffect);
                }
            }

            lvi.EnsureVisible();
            lvi.Selected = true;

            pauseUpdate = false;
        }

        private void LoadPreset(ImageEffectPreset preset)
        {
            pauseUpdate = true;

            txtPresetName.Text = preset.Name;
            lvEffects.Items.Clear();
            pgSettings.SelectedObject = null;

            foreach (ImageEffect imageEffect in preset.Effects)
            {
                AddEffect(imageEffect);
            }

            pauseUpdate = false;

            UpdatePreview();
        }

        #region Form events

        private void ImageEffectsForm_Shown(object sender, EventArgs e)
        {
            LoadSettings();
            this.ForceActivate();
        }

        private void btnAddPreset_Click(object sender, EventArgs e)
        {
            AddPreset();
        }

        private void btnRemovePreset_Click(object sender, EventArgs e)
        {
            int selected = cbPresets.SelectedIndex;

            if (selected > -1)
            {
                cbPresets.Items.RemoveAt(selected);
                Presets.RemoveAt(selected);

                if (cbPresets.Items.Count > 0)
                {
                    cbPresets.SelectedIndex = selected == cbPresets.Items.Count ? cbPresets.Items.Count - 1 : selected;
                }
                else
                {
                    ClearFields();
                    btnAddPreset.Focus();
                }
            }
        }

        private void cbPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignorePresetsSelectedIndexChanged)
            {
                SelectedPresetIndex = cbPresets.SelectedIndex;

                ImageEffectPreset preset = GetSelectedPreset();
                if (preset != null)
                {
                    LoadPreset(preset);
                }
            }
        }

        private void txtPresetName_TextChanged(object sender, EventArgs e)
        {
            ImageEffectPreset preset = GetSelectedPreset();
            if (preset != null)
            {
                preset.Name = txtPresetName.Text;
                ignorePresetsSelectedIndexChanged = true;
                cbPresets.RefreshItems();
                ignorePresetsSelectedIndexChanged = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmsEffects.Show(btnAdd, 0, btnAdd.Height + 1);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedEffects();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                if (lvEffects.SelectedItems.Count > 0)
                {
                    ListViewItem lvi = lvEffects.SelectedItems[0];

                    if (lvi.Tag is ImageEffect)
                    {
                        ImageEffect imageEffect = (ImageEffect)lvi.Tag;
                        ImageEffect imageEffectClone = imageEffect.Copy();
                        AddEffect(imageEffectClone, preset);
                        UpdatePreview();
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                lvEffects.Items.Clear();
                preset.Effects.Clear();
                pgSettings.SelectedObject = null;
                UpdatePreview();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void lvEffects_ItemMoved(object sender, int oldIndex, int newIndex)
        {
            ImageEffectPreset preset = GetSelectedPreset();

            if (preset != null)
            {
                preset.Effects.Move(oldIndex, newIndex);
                UpdatePreview();
            }
        }

        private void lvEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgSettings.SelectedObject = null;

            if (lvEffects.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvEffects.SelectedItems[0];

                if (lvi.Tag is ImageEffect)
                {
                    pgSettings.SelectedObject = lvi.Tag;
                }
            }

            UpdateControlStates();
        }

        private void lvEffects_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item != null && e.Item.Tag is ImageEffect)
            {
                ImageEffect imageEffect = (ImageEffect)e.Item.Tag;
                imageEffect.Enabled = e.Item.Checked;
                UpdatePreview();
            }
        }

        private void lvEffects_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    RemoveSelectedEffects();
                    e.SuppressKeyPress = true;
                    break;
                case Keys.F5:
                    UpdatePreview();
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void pgSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdatePreview();
        }

        private object eiImageEffects_ExportRequested()
        {
            return GetSelectedPreset();
        }

        private void eiImageEffects_ImportRequested(object obj)
        {
            ImageEffectPreset preset = obj as ImageEffectPreset;

            if (preset != null && preset.Effects.Count > 0)
            {
                AddPreset(preset);
            }
        }

        private void tsmiLoadImageFromFile_Click(object sender, EventArgs e)
        {
            string filePath = ImageHelpers.OpenImageFileDialog();

            if (!string.IsNullOrEmpty(filePath))
            {
                if (PreviewImage != null) PreviewImage.Dispose();
                PreviewImage = ImageHelpers.LoadImage(filePath);
                FilePath = filePath;
                UpdatePreview();
            }
        }

        private void tsmiLoadImageFromClipboard_Click(object sender, EventArgs e)
        {
            Bitmap bmp = ClipboardHelpers.GetImage();

            if (bmp != null)
            {
                if (PreviewImage != null) PreviewImage.Dispose();
                PreviewImage = bmp;
                FilePath = null;
                UpdatePreview();
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (PreviewImage != null)
            {
                using (Image img = ApplyEffects())
                {
                    if (img != null)
                    {
                        string filePath = ImageHelpers.SaveImageFileDialog(img, FilePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            FilePath = filePath;
                        }
                    }
                }
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            if (PreviewImage != null)
            {
                Bitmap bmp = ApplyEffects();

                if (bmp != null)
                {
                    OnImageProcessRequested(bmp);
                }
            }
        }

        private void pbResult_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) || e.Data.GetDataPresent(DataFormats.Bitmap, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pbResult_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop, false) as string[];

                if (files != null && files.Length > 0)
                {
                    if (Helpers.IsImageFile(files[0]))
                    {
                        if (PreviewImage != null) PreviewImage.Dispose();
                        PreviewImage = ImageHelpers.LoadImage(files[0]);
                        UpdatePreview();
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap, false))
            {
                Bitmap bmp = e.Data.GetData(DataFormats.Bitmap, false) as Bitmap;

                if (bmp != null)
                {
                    if (PreviewImage != null) PreviewImage.Dispose();
                    PreviewImage = bmp;
                    UpdatePreview();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Form events
    }
}