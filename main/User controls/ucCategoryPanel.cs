using System;
using System.Drawing;
using System.Windows.Forms;
using client.Classes;
using client.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace client.User_controls
{
    public partial class ucCategoryPanel : UserControl
    {
        public Category Category;
        public frmClient Client;
        public ucCategoryPanel(frmClient client, Category category)
        {
            InitializeComponent();
            Client = client;
            Category = category;
            lblTitle.Text = Regex.Replace(category.Name, @"(_)+", " ");
            picGroupIcon.BackgroundImage = Category.LoadIconImage();

            if (!Directory.Exists(@"config\" + category.Name + @"\Icons\"))
            {
                category.cacheIcons();
            }

            foreach (ProgramShortcut psc in Category.ShortcutList)
            {
                CreateShortcut(0, 0, psc);
                shortcutIcons.Add(this.shortcutPanel);
            }

            this.Resize += (s, e) => LayoutCard();
            LayoutCard();
        }

        private readonly System.Collections.Generic.List<PictureBox> shortcutIcons = new System.Collections.Generic.List<PictureBox>();

        // Header row (icon, name, edit button) + wrapped row(s) of program icons; all sizes via Tron.S
        private void LayoutCard()
        {
            int pad = Tron.S(16), left = Tron.S(68), icon = Tron.S(24), step = Tron.S(34);
            picGroupIcon.Location = new Point(pad, Tron.S(14));
            lblTitle.Location = new Point(left, Tron.S(14));
            cmdDelete.Location = new Point(Math.Max(left, this.Width - pad - cmdDelete.Width), Tron.S(16));
            lblTitle.MaximumSize = new Size(Math.Max(Tron.S(60), cmdDelete.Left - left - Tron.S(8)), 0);

            int columns = Math.Max(1, (this.Width - left - pad) / step);
            int top = Tron.S(56);
            for (int i = 0; i < shortcutIcons.Count; i++)
            {
                shortcutIcons[i].Bounds = new Rectangle(left + (i % columns) * step, top + (i / columns) * step, icon, icon);
            }
            int rows = (shortcutIcons.Count + columns - 1) / columns;
            int h = top + Math.Max(1, rows) * step + Tron.S(6);
            if (this.Height != h) this.Height = h;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            bool hot = this.BackColor == Tron.Hover;
            using (Pen p = new Pen(hot ? Tron.Cyan : Tron.Line, 1f))
                e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            if (hot)
                using (SolidBrush b = new SolidBrush(Tron.Cyan))
                    e.Graphics.FillRectangle(b, 0, 0, Tron.S(3), this.Height);
        }

        private void CreateShortcut(int x, int y, ProgramShortcut programShortcut)
        {
            // creating shortcut picturebox from shortcut
            this.shortcutPanel = new System.Windows.Forms.PictureBox
            {
                BackColor = System.Drawing.Color.Transparent,
                Location = new System.Drawing.Point(x, y),
                Size = Tron.S(24, 24),
                Cursor = Cursors.Hand,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false
            };
            this.shortcutPanel.MouseEnter += new System.EventHandler((sender, e) => Client.EnterControl(sender, e, this));
            this.shortcutPanel.MouseLeave += new System.EventHandler((sender, e) => Client.LeaveControl(sender, e, this));
            this.shortcutPanel.Click += new System.EventHandler((sender, e) => OpenFolder(sender, e));

            // Check if file is stil existing and if so render it
            if (File.Exists(programShortcut.FilePath) || Directory.Exists(programShortcut.FilePath) || programShortcut.isWindowsApp)
            {
                this.shortcutPanel.BackgroundImage = Category.loadImageCache(programShortcut);
            }
            else // if file does not exist
            {
                this.shortcutPanel.BackgroundImage = global::client.Properties.Resources.Error;
                ToolTip tt = new ToolTip
                {
                    InitialDelay = 0,
                    ShowAlways = true
                };
                tt.SetToolTip(this.shortcutPanel, "프로그램이 존재하지 않습니다");
            }

            this.Controls.Add(this.shortcutPanel);
            this.shortcutPanel.Show();
            this.shortcutPanel.BringToFront();
        }

        private void ucNewCategory_Load(object sender, EventArgs e)
        {

        }

        public void OpenFolder(object sender, EventArgs e)
        {
            // Open the shortcut folder for the group when click on category panel

            // Build path based on the directory of the main .exe file
            string filePath = Path.GetFullPath(new Uri($"{MainPath.path}\\Shortcuts").LocalPath + "\\" + Category.Name + ".lnk");

            // Open directory in explorer and highlighting file
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", @filePath));
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            frmGroup editGroup = new frmGroup(Client, Category);
            editGroup.Show();
            editGroup.BringToFront();
        }

        public static Bitmap LoadBitmap(string path) // needed to access img without occupying read/write
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                reader.Close();
                stream.Close();
                return new Bitmap(memoryStream);
            }
        }

        private void lblTitle_MouseEnter(object sender, EventArgs e)
        {
            Client.EnterControl(sender, e, this);

        }

        private void lblTitle_MouseLeave(object sender, EventArgs e)
        {
            Client.LeaveControl(sender, e, this);
        }

        //
        // endregion
        //
        public System.Windows.Forms.PictureBox shortcutPanel;

    }
}
