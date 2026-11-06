using client.Classes;
using System.Drawing;
using System.Windows.Forms;

namespace client.User_controls
{
    partial class ucCategoryPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Hand-written; positions are set in LayoutCard() with Tron.S() so every DPI matches
        private void InitializeComponent()
        {
            this.lblTitle = new Label { AutoSize = true, BackColor = Color.Transparent, Font = Tron.F(13f), ForeColor = Tron.Text, Text = "그룹 이름", Cursor = Cursors.Hand };
            this.cmdDelete = Tron.Button("그룹 편집");
            this.cmdDelete.Size = Tron.S(96, 32);
            this.cmdDelete.Margin = new Padding(0);
            this.picGroupIcon = new PictureBox { BackColor = Color.Transparent, BackgroundImageLayout = ImageLayout.Stretch, Size = Tron.S(36, 36), TabStop = false, Cursor = Cursors.Hand };

            this.SuspendLayout();
            this.lblTitle.Click += new System.EventHandler(this.OpenFolder);
            this.lblTitle.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.lblTitle.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            this.cmdDelete.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.cmdDelete.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            this.picGroupIcon.Click += new System.EventHandler(this.OpenFolder);
            this.picGroupIcon.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.picGroupIcon.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);

            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Tron.Bg;
            this.Cursor = Cursors.Hand;
            this.DoubleBuffered = true;
            this.Controls.Add(this.picGroupIcon);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.lblTitle);
            this.Name = "ucCategoryPanel";
            this.Size = Tron.S(600, 96);
            this.Click += new System.EventHandler(this.OpenFolder);
            this.MouseEnter += new System.EventHandler(this.lblTitle_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Button cmdDelete;
        private PictureBox picGroupIcon;
    }
}
