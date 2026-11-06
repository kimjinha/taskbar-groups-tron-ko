﻿using System;
using System.Drawing;
using System.Windows.Forms;
using client.Classes;
using client.Forms;
using System.IO;
using System.Windows.Input;

namespace client.User_controls
{
    public partial class ucProgramShortcut : UserControl
    {
        public ProgramShortcut Shortcut { get; set; }
        public frmGroup MotherForm { get; set; }

        public bool IsSelected = false;
        public int Position { get; set; }

        public Bitmap logo;
        public ucProgramShortcut()
        {
            InitializeComponent();
        }

        private void ucProgramShortcut_Load(object sender, EventArgs e)
        {
            // Grab the file name without the extension to be used later as the naming scheme for the icon .jpg image

            if (Shortcut.isWindowsApp)
            {
                txtShortcutName.Text = handleWindowsApp.findWindowsAppsName(Shortcut.FilePath);
            } else if (Shortcut.name == "")
            {
                if (File.Exists(Shortcut.FilePath) && Path.GetExtension(Shortcut.FilePath).ToLower() == ".lnk")
                {
                    txtShortcutName.Text = frmGroup.handleExtName(Shortcut.FilePath);
                }
                else
                {
                    txtShortcutName.Text = Path.GetFileNameWithoutExtension(Shortcut.FilePath);
                }
            } else
            {
                txtShortcutName.Text = Shortcut.name;
            }

            LayoutRow();

            if (Shortcut.isWindowsApp)
            {
                picShortcut.BackgroundImage = logo = handleWindowsApp.getWindowsAppIcon(Shortcut.FilePath, true);
            }
            else if (File.Exists(Shortcut.FilePath)) // Checks if the shortcut actually exists; if not then display an error image
            {
                String imageExtension = Path.GetExtension(Shortcut.FilePath).ToLower();

                // Start checking if the extension is an lnk (shortcut) file
                // Depending on the extension, the icon can be directly extracted or it has to be gotten through other methods as to not get the shortcut arrow
                if (imageExtension == ".lnk")
                {
                    picShortcut.BackgroundImage = logo = frmGroup.handleLnkExt(Shortcut.FilePath);
                }
                else
                {
                    picShortcut.BackgroundImage = logo = Icon.ExtractAssociatedIcon(Shortcut.FilePath).ToBitmap();
                }

            } else if (Directory.Exists(Shortcut.FilePath))
            {
                try
                {
                    picShortcut.BackgroundImage = logo = handleFolder.GetFolderIcon(Shortcut.FilePath).ToBitmap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else
            {
                picShortcut.BackgroundImage = logo = global::client.Properties.Resources.Error;
            }

            this.DoubleClick += new EventHandler(RunTestProgram);
            txtShortcutName.DoubleClick += new EventHandler(RunTestProgram);
            picShortcut.DoubleClick += new EventHandler(RunTestProgram);

            if (Position == 0)
            {
                cmdNumUp.Enabled = false;
                cmdNumUp.ForeColor = Tron.Line;
            }
            if (Position == MotherForm.Category.ShortcutList.Count - 1)
            {
                cmdNumDown.Enabled = false;
                cmdNumDown.ForeColor = Tron.Line;
            }

        }

        private void ucProgramShortcut_MouseEnter(object sender, EventArgs e)
        {
            ucSelected();
        }

        private void ucProgramShortcut_MouseLeave(object sender, EventArgs e)
        {
            if (MotherForm.selectedShortcut != this)
            {
                ucDeselected();
            }
        }

        private void cmdNumUp_Click(object sender, EventArgs e)
        {
            MotherForm.Swap(MotherForm.Category.ShortcutList, Position, Position - 1);

        }

        private void cmdNumDown_Click(object sender, EventArgs e)
        {
            MotherForm.Swap(MotherForm.Category.ShortcutList, Position, Position + 1);

        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            MotherForm.DeleteShortcut(Shortcut);
        }

        // Handle what is selected/deselected when a shortcut is clicked on
        // If current item is already selected, then deselect everything
        private void ucProgramShortcut_Click(object sender, EventArgs e)
        {
            if (MotherForm.selectedShortcut == this)
            {
                MotherForm.resetSelection();
                //IsSelected = false;
            }
            else
            {
                if (MotherForm.selectedShortcut != null)
                {
                    MotherForm.resetSelection();
                    //IsSelected = false;
                }

                MotherForm.enableSelection(this);
                //IsSelected = true;
            }
        }

        public void ucDeselected()
        {
            txtShortcutName.DeselectAll();
            txtShortcutName.Enabled = false;
            txtShortcutName.Enabled = true;
            txtShortcutName.TabStop = false; // Deselecting textbox text

            SetRowColor(Tron.Bg);
        }

        public void ucSelected()
        {
            SetRowColor(Tron.Hover);
        }

        private void SetRowColor(Color c)
        {
            this.BackColor = c;
            txtShortcutName.BackColor = c;
            cmdNumUp.BackColor = c;
            cmdNumDown.BackColor = c;
            Invalidate();
        }

        // Arrows | icon | name ... delete. The name box ends before the delete button, so a long name can never cover it.
        private void LayoutRow()
        {
            int mid = this.Height / 2;
            cmdNumUp.Location = new Point(Tron.S(8), mid - cmdNumUp.Height);
            cmdNumDown.Location = new Point(Tron.S(8), mid);
            picShortcut.Location = new Point(Tron.S(40), mid - picShortcut.Height / 2);
            cmdDelete.Location = new Point(this.Width - Tron.S(10) - cmdDelete.Width, mid - cmdDelete.Height / 2);

            int left = Tron.S(80);
            int maxWidth = Math.Max(Tron.S(40), cmdDelete.Left - left - Tron.S(12));
            Size size = TextRenderer.MeasureText(txtShortcutName.Text + "  ", txtShortcutName.Font);
            txtShortcutName.Width = Math.Min(size.Width, maxWidth);
            txtShortcutName.Location = new Point(left, mid - txtShortcutName.Height / 2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen p = new Pen(Tron.Line, 1f))
                e.Graphics.DrawLine(p, 0, this.Height - 1, this.Width, this.Height - 1);
            if (IsSelected || this.BackColor == Tron.Hover)
                using (SolidBrush b = new SolidBrush(Tron.Cyan))
                    e.Graphics.FillRectangle(b, 0, 0, Tron.S(3), this.Height);
        }

        private void lbTextbox_TextChanged(object sender, EventArgs e)
        {
            LayoutRow();
            Shortcut.name = txtShortcutName.Text;
        }

        private void ucProgramShortcut_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                picShortcut.Focus();


                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void RunTestProgram(object sender, EventArgs e)
        {
            try
            {
                if (Shortcut.isWindowsApp)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { UseShellExecute = true, FileName = $@"shell:appsFolder\{Shortcut.FilePath}" });
                }
                else
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = Shortcut.FilePath,
                        Arguments = Shortcut.Arguments,
                        WorkingDirectory = Shortcut.WorkingDirectory
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"프로그램 테스트 실행 실패: {ex.Message}", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtShortcutName_Click(object sender, EventArgs e)
        {
            
            if (!IsSelected)
                ucProgramShortcut_Click(sender, e);
        }

        private void ucProgramShortcut_Enter(object sender, EventArgs e)
        {
            //IsSelected = true;
        }

        private void ucProgramShortcut_Leave(object sender, EventArgs e)
        {
            //IsSelected = false;
        }

    }
}
