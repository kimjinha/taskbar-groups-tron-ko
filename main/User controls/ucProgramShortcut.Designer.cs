using client.Classes;
using System.Drawing;
using System.Windows.Forms;

namespace client.User_controls
{
    partial class ucProgramShortcut
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

        // Hand-written; positions are set in LayoutRow() with Tron.S() so every DPI matches
        private void InitializeComponent()
        {
            this.cmdDelete = Tron.Button("삭제");
            this.cmdNumUp = new Button { Text = "▲" };
            this.cmdNumDown = new Button { Text = "▼" };
            this.picShortcut = new PictureBox();
            this.txtShortcutName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picShortcut)).BeginInit();
            this.SuspendLayout();

            this.cmdDelete.Size = Tron.S(64, 30);
            this.cmdDelete.Font = Tron.F(9.5f);
            this.cmdDelete.TabStop = false;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            this.cmdDelete.MouseEnter += new System.EventHandler(this.ucProgramShortcut_MouseEnter);
            this.cmdDelete.MouseLeave += new System.EventHandler(this.ucProgramShortcut_MouseLeave);

            foreach (Button b in new[] { cmdNumUp, cmdNumDown })
            {
                Tron.Style(b);
                b.FlatAppearance.BorderSize = 0;
                b.Font = Tron.F(7f);
                b.Size = Tron.S(22, 18);
                b.TabStop = false;
                b.MouseEnter += new System.EventHandler(this.ucProgramShortcut_MouseEnter);
                b.MouseLeave += new System.EventHandler(this.ucProgramShortcut_MouseLeave);
            }
            this.cmdNumUp.Click += new System.EventHandler(this.cmdNumUp_Click);
            this.cmdNumDown.Click += new System.EventHandler(this.cmdNumDown_Click);

            this.picShortcut.BackColor = Color.Transparent;
            this.picShortcut.BackgroundImageLayout = ImageLayout.Stretch;
            this.picShortcut.Size = Tron.S(28, 28);
            this.picShortcut.TabStop = false;
            this.picShortcut.Click += new System.EventHandler(this.ucProgramShortcut_Click);
            this.picShortcut.MouseEnter += new System.EventHandler(this.ucProgramShortcut_MouseEnter);
            this.picShortcut.MouseLeave += new System.EventHandler(this.ucProgramShortcut_MouseLeave);

            this.txtShortcutName.BackColor = Tron.Bg;
            this.txtShortcutName.BorderStyle = BorderStyle.None;
            this.txtShortcutName.Font = Tron.F(11f);
            this.txtShortcutName.ForeColor = Tron.Text;
            this.txtShortcutName.MaxLength = 50;
            this.txtShortcutName.Text = "프로그램 이름";
            this.txtShortcutName.Click += new System.EventHandler(this.txtShortcutName_Click);
            this.txtShortcutName.TextChanged += new System.EventHandler(this.lbTextbox_TextChanged);
            this.txtShortcutName.Enter += new System.EventHandler(this.ucProgramShortcut_Enter);
            this.txtShortcutName.KeyDown += new KeyEventHandler(this.ucProgramShortcut_KeyDown);
            this.txtShortcutName.Leave += new System.EventHandler(this.ucProgramShortcut_Leave);
            this.txtShortcutName.MouseEnter += new System.EventHandler(this.ucProgramShortcut_MouseEnter);
            this.txtShortcutName.MouseLeave += new System.EventHandler(this.ucProgramShortcut_MouseLeave);

            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Tron.Bg;
            this.Controls.Add(this.txtShortcutName);
            this.Controls.Add(this.cmdNumUp);
            this.Controls.Add(this.cmdNumDown);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.picShortcut);
            this.Name = "ucProgramShortcut";
            this.Size = Tron.S(560, 48);
            this.Load += new System.EventHandler(this.ucProgramShortcut_Load);
            this.Click += new System.EventHandler(this.ucProgramShortcut_Click);
            this.Enter += new System.EventHandler(this.ucProgramShortcut_Enter);
            this.Leave += new System.EventHandler(this.ucProgramShortcut_Leave);
            this.MouseEnter += new System.EventHandler(this.ucProgramShortcut_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ucProgramShortcut_MouseLeave);
            this.Resize += (s, e) => LayoutRow();
            ((System.ComponentModel.ISupportInitialize)(this.picShortcut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private PictureBox picShortcut;
        private Button cmdDelete;
        private Button cmdNumUp;
        private Button cmdNumDown;
        private TextBox txtShortcutName;
    }
}
