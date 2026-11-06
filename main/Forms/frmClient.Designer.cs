using client.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace client.Forms
{
    partial class frmClient
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

        // Hand-written layout (not editable in the VS designer): docked panels + TableLayoutPanels
        // so nothing is positioned by absolute pixels and every DPI lays out the same way.
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClient));
            this.SuspendLayout();

            // ---------- left column ----------
            pnlLeftColumn = new Panel { Dock = DockStyle.Left, Width = Tron.S(290), BackColor = Tron.Panel, Padding = Tron.P(20, 22, 20, 18) };
            Panel leftLine = new Panel { Dock = DockStyle.Left, Width = Math.Max(1, Tron.S(1)), BackColor = Tron.Line };

            TableLayoutPanel tlpLeft = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, BackColor = Color.Transparent };
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            Label lblTitle = Tron.Label("TASKBAR GROUPS", 15f, false, true);
            lblTitle.ForeColor = Tron.Cyan;
            currentVersion = Tron.Label("v2026.11.06 (kjh)", 9f, true);
            currentVersion.Margin = Tron.P(0, 0, 0, 22);

            cmdNewGroup = Tron.Button("+  새 그룹 만들기", true);
            cmdTaskbarX = Tron.Button("⚡  작업 표시줄 꾸미기");
            cmdOpenShortcuts = Tron.Button("Shortcuts 폴더 열기");
            foreach (Button b in new[] { cmdNewGroup, cmdTaskbarX, cmdOpenShortcuts })
            {
                b.Dock = DockStyle.Fill;
                b.Height = Tron.S(40);
                b.Margin = Tron.P(0, 0, 0, 10);
            }
            cmdNewGroup.Click += new System.EventHandler(this.cmdAddGroup_Click);
            cmdTaskbarX.Click += new System.EventHandler(this.cmdTaskbarX_Click);
            cmdOpenShortcuts.Click += new System.EventHandler(this.cmdOpenShortcuts_Click);

            // help
            pnlHelp = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Fill, BackColor = Color.Transparent, Margin = Tron.P(0, 0, 0, 16) };
            lblHelpTitle = Tron.Label("사용법", 10f, false, true);
            lblHelpTitle.ForeColor = Tron.Cyan;
            Label lblHelp1 = Tron.Label("1. 그룹 클릭 → Shortcuts 폴더 열림", 9.5f, true);
            Label lblHelp2 = Tron.Label("2. 바로 가기 우클릭 → 작업 표시줄에 고정", 9.5f, true);
            Label lblHelp3 = Tron.Label("3. 작업 표시줄에서 그룹 아이콘 클릭", 9.5f, true);
            foreach (Label l in new[] { lblHelpTitle, lblHelp1, lblHelp2, lblHelp3 })
            {
                l.MaximumSize = new Size(Tron.S(250), 0);
                pnlHelp.Controls.Add(l);
            }

            // version / links
            FlowLayoutPanel pnlVersionInfo = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Dock = DockStyle.Fill, BackColor = Color.Transparent };
            githubVersion = Tron.Label("원본 최신: 확인 중", 9f, true);
            githubLink = new LinkLabel
            {
                Text = "원본 GitHub 열기",
                AutoSize = true,
                Font = Tron.F(9f),
                LinkColor = Tron.Cyan,
                ActiveLinkColor = Tron.Text,
                VisitedLinkColor = Tron.Cyan,
                BackColor = Color.Transparent
            };
            githubLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.githubLink_LinkClicked);
            pnlVersionInfo.Controls.Add(githubVersion);
            pnlVersionInfo.Controls.Add(githubLink);

            tlpLeft.Controls.Add(lblTitle);
            tlpLeft.Controls.Add(currentVersion);
            tlpLeft.Controls.Add(cmdNewGroup);
            tlpLeft.Controls.Add(cmdTaskbarX);
            tlpLeft.Controls.Add(cmdOpenShortcuts);
            Panel spacer = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            tlpLeft.Controls.Add(spacer);
            tlpLeft.Controls.Add(pnlHelp);
            tlpLeft.Controls.Add(pnlVersionInfo);
            tlpLeft.RowCount = tlpLeft.Controls.Count;
            for (int i = 0; i < tlpLeft.RowCount; i++)
                tlpLeft.RowStyles.Add(tlpLeft.Controls[i] == spacer ? new RowStyle(SizeType.Percent, 100f) : new RowStyle(SizeType.AutoSize));
            pnlLeftColumn.Controls.Add(tlpLeft);

            // ---------- right side: group list ----------
            pnlRight = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Tron.Bg, Padding = Tron.P(28, 24, 28, 24) };

            TableLayoutPanel tlpRight = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, BackColor = Color.Transparent };
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            Label lblGroups = Tron.Label("내 그룹", 18f);
            Label lblGroupsSub = Tron.Label("자주 쓰는 프로그램을 그룹으로 묶어 작업 표시줄에 고정하세요", 10f, true);
            lblGroupsSub.Margin = Tron.P(0, 0, 0, 18);

            pnlExistingGroups = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Fill, BackColor = Color.Transparent, Margin = new Padding(0) };
            pnlExistingGroups.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // dashed "add group" tile at the end of the list
            pnlAddGroup = new Panel { Height = Tron.S(64), Dock = DockStyle.Fill, Cursor = Cursors.Hand, BackColor = Tron.Bg, Margin = Tron.P(0, 12, 0, 0) };
            lblAddGroup = new Label { Text = "+  새 그룹 추가", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = Tron.F(11f), ForeColor = Tron.Cyan, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            pnlAddGroup.Controls.Add(lblAddGroup);
            pnlAddGroup.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Tron.Cyan, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    e.Graphics.DrawRectangle(p, 0, 0, pnlAddGroup.Width - 1, pnlAddGroup.Height - 1);
            };
            foreach (Control c in new Control[] { pnlAddGroup, lblAddGroup })
            {
                c.Click += new System.EventHandler(this.cmdAddGroup_Click);
                c.MouseEnter += new System.EventHandler(this.pnlAddGroup_MouseEnter);
                c.MouseLeave += new System.EventHandler(this.pnlAddGroup_MouseLeave);
            }

            tlpRight.Controls.Add(lblGroups);
            tlpRight.Controls.Add(lblGroupsSub);
            tlpRight.Controls.Add(pnlExistingGroups);
            tlpRight.Controls.Add(pnlAddGroup);
            pnlRight.Controls.Add(tlpRight);

            // ---------- form ----------
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Tron.Bg;
            this.ForeColor = Tron.Text;
            this.Font = Tron.F(10f);
            this.ClientSize = Tron.S(960, 640);
            this.MinimumSize = Tron.S(780, 520);
            this.Controls.Add(pnlRight);
            this.Controls.Add(leftLine);
            this.Controls.Add(pnlLeftColumn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmClient";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TaskbarGroups — 작업 표시줄 그룹 관리자";
            this.ResumeLayout(false);
        }

        private Panel pnlLeftColumn;
        private Panel pnlRight;
        private TableLayoutPanel pnlHelp;
        private Label lblHelpTitle;
        private Label currentVersion;
        private Label githubVersion;
        private LinkLabel githubLink;
        private Button cmdNewGroup;
        private Button cmdTaskbarX;
        private Button cmdOpenShortcuts;
        private TableLayoutPanel pnlExistingGroups;
        private Panel pnlAddGroup;
        private Label lblAddGroup;
    }
}
