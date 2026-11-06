﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using client.Classes;
using System.Diagnostics;
using System.IO;
using IWshRuntimeLibrary;

namespace client.User_controls
{
    public partial class ucShortcut : UserControl
    {
        public ProgramShortcut Psc { get; set; }
        public frmMain MotherForm { get; set; }
        public Category ThisCategory { get; set; }
        // DPI-safe tile: 88x82 at 100% -- 28px icon on top, name (up to two lines) underneath
        public static readonly int TileW = Tron.S(88), TileH = Tron.S(82);
        private readonly Label lblName;

        public ucShortcut()
        {
            InitializeComponent();
            this.Size = new Size(TileW, TileH);
            picIcon.Size = Tron.S(28, 28);
            picIcon.Location = new Point((this.Width - picIcon.Width) / 2, Tron.S(10));
            lblName = new Label
            {
                AutoSize = false,
                AutoEllipsis = false,                        // wrap onto a second line instead of "…"
                TextAlign = ContentAlignment.TopCenter,
                Font = Tron.F(8.5f),
                BackColor = Color.Transparent,
                Bounds = new Rectangle(Tron.S(2), Tron.S(42), TileW - Tron.S(4), Tron.S(36))
            };
            lblName.Click += ucShortcut_Click;
            lblName.MouseEnter += ucShortcut_MouseEnter;
            lblName.MouseLeave += ucShortcut_MouseLeave;
            this.Controls.Add(lblName);
        }

        private void ucShortcut_Load(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            this.BackColor = MotherForm.BackColor;
            picIcon.BackgroundImage = ThisCategory.loadImageCache(Psc); // Use the local icon cache for the file specified as the icon image

            lblName.Text = DisplayName(Psc);
            Color bg = MotherForm.BackColor;
            bool lightBg = bg.R * 0.2126 + bg.G * 0.7152 + bg.B * 0.0722 > 140;
            lblName.ForeColor = lightBg ? Color.FromArgb(30, 30, 30) : (Tron.IsTron(ThisCategory) ? Tron.Text : Color.FromArgb(235, 235, 235));
            new ToolTip().SetToolTip(lblName, lblName.Text);
        }

        // Name under the icon: the name set in the editor, else the file / folder / app name
        private static string DisplayName(ProgramShortcut p)
        {
            if (!string.IsNullOrWhiteSpace(p.name)) return p.name;
            try
            {
                if (p.isWindowsApp) return handleWindowsApp.findWindowsAppsName(p.FilePath);
                string path = p.FilePath.TrimEnd('\\');
                return Directory.Exists(path) ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
            }
            catch { return Path.GetFileNameWithoutExtension(p.FilePath); }
        }

        public void ucShortcut_Click(object sender, EventArgs e)
        {
            if (Psc.isWindowsApp)
            {
                Process p = new Process() {StartInfo = new ProcessStartInfo() { UseShellExecute = true, FileName = $@"shell:appsFolder\{Psc.FilePath}" }};
                p.Start();
            } else
            {
                if(Path.GetExtension(Psc.FilePath).ToLower() == ".lnk" && Psc.FilePath == MainPath.exeString)

                {
                    MotherForm.OpenFile(Psc.Arguments, Psc.FilePath, MainPath.path);
                } else
                {
                    MotherForm.OpenFile(Psc.Arguments, Psc.FilePath, Psc.WorkingDirectory);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // TRON: neon frame only on the hovered tile
            if (MotherForm != null && Tron.IsTron(MotherForm.ThisCategory) && this.BackColor == MotherForm.HoverColor)
            {
                using (Pen p = new Pen(Tron.Cyan, 1f))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }

        public void ucShortcut_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = MotherForm.HoverColor;
        }

        public void ucShortcut_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }
    }
}
