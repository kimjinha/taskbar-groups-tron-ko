using client.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace client.Forms
{
    partial class frmGroup
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

        // Hand-written layout (not editable in the VS designer). One TableLayoutPanel column, every row AutoSize,
        // so nothing overlaps at any DPI. The form height follows the content (see UpdateLayout).
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroup));
            this.SuspendLayout();
            colorDialog = new ColorDialog();

            tlpRoot = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, Padding = Tron.P(22, 18, 22, 16), BackColor = Color.Transparent };
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ---------- header: icon | name | width ----------
            TableLayoutPanel tlpHead = new TableLayoutPanel { AutoSize = true, ColumnCount = 3, RowCount = 1, Dock = DockStyle.Fill, Margin = Tron.P(0, 0, 0, 14) };
            tlpHead.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpHead.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tlpHead.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            pnlGroupIcon = new Panel { Size = Tron.S(72, 72), BackColor = Tron.Panel, AllowDrop = true, Cursor = Cursors.Hand, Margin = Tron.P(0, 0, 14, 0), Padding = Tron.P(10) };
            cmdAddGroupIcon = new PictureBox { Dock = DockStyle.Fill, BackgroundImage = global::client.Properties.Resources.AddWhite, BackgroundImageLayout = ImageLayout.Zoom, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            pnlGroupIcon.Controls.Add(cmdAddGroupIcon);
            pnlGroupIcon.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Tron.Cyan, 1f))
                    e.Graphics.DrawRectangle(p, 0, 0, pnlGroupIcon.Width - 1, pnlGroupIcon.Height - 1);
            };
            pnlGroupIcon.Click += new EventHandler(this.cmdAddGroupIcon_Click);
            pnlGroupIcon.DragDrop += new DragEventHandler(this.pnlDragDropImg);
            pnlGroupIcon.DragEnter += new DragEventHandler(this.pnlDragDropEnterImg);
            pnlGroupIcon.DragLeave += new EventHandler(this.pnlGroupIcon_MouseLeave);
            pnlGroupIcon.MouseEnter += new EventHandler(this.pnlGroupIcon_MouseEnter);
            pnlGroupIcon.MouseLeave += new EventHandler(this.pnlGroupIcon_MouseLeave);
            cmdAddGroupIcon.Click += new EventHandler(this.cmdAddGroupIcon_Click);
            cmdAddGroupIcon.MouseEnter += new EventHandler(this.pnlGroupIcon_MouseEnter);
            cmdAddGroupIcon.MouseLeave += new EventHandler(this.pnlGroupIcon_MouseLeave);

            TableLayoutPanel tlpName = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Fill, Margin = new Padding(0) };
            tlpName.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            Label lblName = Tron.Label("그룹 이름", 9.5f, true);
            txtGroupName = new TextBox { Dock = DockStyle.Fill, Text = "새 그룹 이름 입력...", MaxLength = 50, Margin = Tron.P(0, 0, 0, 4) };
            Tron.Style(txtGroupName);
            txtGroupName.Font = Tron.F(13f);
            txtGroupName.MouseClick += new MouseEventHandler(this.txtGroupName_MouseClick);
            txtGroupName.TextChanged += new EventHandler(this.txtGroupName_TextChanged);
            txtGroupName.Leave += new EventHandler(this.txtGroupName_Leave);
            lblErrorTitle = ErrorLabel();
            lblAddGroupIcon = Tron.Label("그룹 아이콘 선택 — 왼쪽 칸을 누르거나 이미지를 끌어다 놓기", 9f, true);
            lblAddGroupIcon.Cursor = Cursors.Hand;
            lblAddGroupIcon.Click += new EventHandler(this.cmdAddGroupIcon_Click);
            lblErrorIcon = ErrorLabel();
            tlpName.Controls.Add(lblName);
            tlpName.Controls.Add(txtGroupName);
            tlpName.Controls.Add(lblErrorTitle);
            tlpName.Controls.Add(lblAddGroupIcon);
            tlpName.Controls.Add(lblErrorIcon);

            // width: label, - , value, +
            TableLayoutPanel tlpWidth = new TableLayoutPanel { AutoSize = true, ColumnCount = 3, Margin = Tron.P(14, 0, 0, 0), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            Label lblWith = Tron.Label("팝업 너비 (칸)", 9.5f, true);
            tlpWidth.Controls.Add(lblWith, 0, 0);
            tlpWidth.SetColumnSpan(lblWith, 3);
            cmdWidthDown = SmallButton("−");
            lblNum = new Label { Text = "6", AutoSize = false, Size = Tron.S(40, 30), TextAlign = ContentAlignment.MiddleCenter, Font = Tron.F(12f, true), ForeColor = Tron.Text, Margin = new Padding(0) };
            cmdWidthUp = SmallButton("+");
            cmdWidthDown.Click += new EventHandler(this.cmdWidthDown_Click);
            cmdWidthUp.Click += new EventHandler(this.cmdWidthUp_Click);
            tlpWidth.Controls.Add(cmdWidthDown, 0, 1);
            tlpWidth.Controls.Add(lblNum, 1, 1);
            tlpWidth.Controls.Add(cmdWidthUp, 2, 1);
            lblErrorNum = ErrorLabel();
            lblErrorNum.MaximumSize = new Size(Tron.S(130), 0);
            tlpWidth.Controls.Add(lblErrorNum, 0, 2);
            tlpWidth.SetColumnSpan(lblErrorNum, 3);

            tlpHead.Controls.Add(pnlGroupIcon, 0, 0);
            tlpHead.Controls.Add(tlpName, 1, 0);
            tlpHead.Controls.Add(tlpWidth, 2, 0);

            // ---------- shortcuts ----------
            Label lblShortcuts = Tron.Label("바로 가기  ·  더블클릭하면 시험 실행, 클릭하면 실행 인수 설정", 9.5f, true);
            pnlShortcuts = new Panel { Name = "pnlShortcuts", Dock = DockStyle.Fill, Height = 0, AutoScroll = true, BackColor = Tron.Bg, Margin = new Padding(0) };

            pnlAddShortcut = new Panel { Dock = DockStyle.Fill, Height = Tron.S(52), AllowDrop = true, Cursor = Cursors.Hand, BackColor = Tron.Bg, Margin = Tron.P(0, 8, 0, 0) };
            Label lblAddShortcut = new Label { Text = "+  프로그램 추가 — 누르거나 파일·폴더를 끌어다 놓기", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = Tron.F(10.5f), ForeColor = Tron.Cyan, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            Button cmdAddFolder = Tron.Button("📁  폴더 추가");
            cmdAddFolder.Dock = DockStyle.Right;
            cmdAddFolder.Width = Tron.S(140);
            cmdAddFolder.Margin = new Padding(0);
            cmdAddFolder.Click += new EventHandler(this.cmdAddFolder_Click);
            pnlAddShortcut.Controls.Add(lblAddShortcut);
            pnlAddShortcut.Controls.Add(cmdAddFolder);
            pnlAddShortcut.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Tron.Cyan, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    e.Graphics.DrawRectangle(p, 0, 0, pnlAddShortcut.Width - 1, pnlAddShortcut.Height - 1);
            };
            pnlAddShortcut.Click += new EventHandler(this.pnlAddShortcut_Click);
            pnlAddShortcut.DragDrop += new DragEventHandler(this.pnlDragDropExt);
            pnlAddShortcut.DragEnter += new DragEventHandler(this.pnlDragDropEnterExt);
            pnlAddShortcut.DragLeave += new EventHandler(this.pnlAddShortcut_MouseLeave);
            pnlAddShortcut.MouseEnter += new EventHandler(this.pnlAddShortcut_MouseEnter);
            pnlAddShortcut.MouseLeave += new EventHandler(this.pnlAddShortcut_MouseLeave);
            lblAddShortcut.Click += new EventHandler(this.pnlAddShortcut_Click);
            lblAddShortcut.MouseEnter += new EventHandler(this.pnlAddShortcut_MouseEnter);
            lblAddShortcut.MouseLeave += new EventHandler(this.pnlAddShortcut_MouseLeave);
            lblErrorShortcut = ErrorLabel();

            // ---------- settings: theme panel or (when a shortcut is selected) arguments panel ----------
            pnlColor = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Fill, Margin = Tron.P(0, 14, 0, 0) };
            pnlColor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlColor.Controls.Add(Tron.Label("팝업 테마", 9.5f, true));

            TableLayoutPanel tlpThemes = new TableLayoutPanel { AutoSize = true, ColumnCount = 3, Dock = DockStyle.Fill, Margin = Tron.P(0, 0, 0, 10) };
            for (int i = 0; i < 3; i++) tlpThemes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            radioDark = new RadioButton { Text = "어두운 테마" };
            radioLight = new RadioButton { Text = "밝은 테마" };
            radioTron = new RadioButton { Text = "⚡ 트론 테마" };
            foreach (RadioButton r in new[] { radioDark, radioLight, radioTron })
            {
                Tron.Toggle(r);
                r.Dock = DockStyle.Fill;
                r.Height = Tron.S(38);
                r.Margin = Tron.P(0, 0, 8, 0);
                tlpThemes.Controls.Add(r);
            }
            radioTron.Margin = new Padding(0);
            radioDark.Click += new EventHandler(this.radioDark_Click);
            radioLight.Click += new EventHandler(this.radioLight_Click);
            radioTron.Click += new EventHandler(this.radioTron_Click);
            pnlColor.Controls.Add(tlpThemes);

            // custom colour + opacity on one line
            FlowLayoutPanel flpColor = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Dock = DockStyle.Fill, Margin = Tron.P(0, 0, 0, 8) };
            radioCustom = new RadioButton { Text = "사용자 지정 색", AutoSize = true, Font = Tron.F(10f), ForeColor = Tron.Text, Margin = Tron.P(0, 6, 6, 0), Cursor = Cursors.Hand };
            radioCustom.Click += new EventHandler(this.radioCustom_Click);
            pnlCustomColor = new Panel { Size = Tron.S(22, 22), Margin = Tron.P(0, 6, 28, 0), Visible = false };
            Label lblOpacityTooltip = Tron.Label("불투명도", 10f);
            lblOpacityTooltip.Margin = Tron.P(0, 6, 8, 0);
            numOpacDown = SmallButton("−");
            lblOpacity = new Label { Text = "10", AutoSize = false, Size = Tron.S(36, 30), TextAlign = ContentAlignment.MiddleRight, Font = Tron.F(11f, true), ForeColor = Tron.Text, Margin = new Padding(0) };
            Label lblPercent = new Label { Text = "%", AutoSize = false, Size = Tron.S(22, 30), TextAlign = ContentAlignment.MiddleLeft, Font = Tron.F(10f), ForeColor = Tron.Muted, Margin = new Padding(0) };
            numOpacUp = SmallButton("+");
            numOpacDown.Click += new EventHandler(this.numOpacDown_Click);
            numOpacUp.Click += new EventHandler(this.numOpacUp_Click);
            flpColor.Controls.AddRange(new Control[] { radioCustom, pnlCustomColor, lblOpacityTooltip, numOpacDown, lblOpacity, lblPercent, numOpacUp });
            pnlColor.Controls.Add(flpColor);

            pnlAllowOpenAll = new CheckBox { Text = "모든 바로 가기 동시 실행 허용 (팝업에서 Ctrl + Enter)", AutoSize = true, Font = Tron.F(10f), ForeColor = Tron.Text, Cursor = Cursors.Hand, Margin = new Padding(0) };
            pnlAllowOpenAll.CheckedChanged += new EventHandler(this.pnlAllowOpenAll_CheckedChanged);
            pnlColor.Controls.Add(pnlAllowOpenAll);

            pnlArguments = new TableLayoutPanel { AutoSize = true, ColumnCount = 2, Dock = DockStyle.Fill, Visible = false, Margin = Tron.P(0, 14, 0, 0) };
            pnlArguments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlArguments.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            Label lblArgs = Tron.Label("실행 인수 (Arguments) — 선택한 바로 가기", 9.5f, true);
            pnlArgumentTextbox = new TextBox { Dock = DockStyle.Fill, Enabled = false, Margin = Tron.P(0, 0, 0, 10) };
            Tron.Style(pnlArgumentTextbox);
            pnlArgumentTextbox.TextChanged += new EventHandler(this.pnlArgumentTextbox_TextChanged);
            pnlArgumentTextbox.KeyDown += new KeyEventHandler(this.pnlArgumentTextbox_KeyDown);
            Label lblDir = Tron.Label("시작 위치 (작업 폴더)", 9.5f, true);
            pnlWorkingDirectory = new TextBox { Dock = DockStyle.Fill, Enabled = false, Margin = Tron.P(0, 0, 8, 0) };
            Tron.Style(pnlWorkingDirectory);
            pnlWorkingDirectory.TextChanged += new EventHandler(this.pnlWorkingDirectory_TextChanged);
            cmdSelectDirectory = Tron.Button("폴더 선택");
            cmdSelectDirectory.Size = Tron.S(100, 32);
            cmdSelectDirectory.Margin = new Padding(0);
            cmdSelectDirectory.Enabled = false;
            cmdSelectDirectory.Click += new EventHandler(this.cmdSelectDirectory_Click);
            pnlArguments.Controls.Add(lblArgs, 0, 0);
            pnlArguments.SetColumnSpan(lblArgs, 2);
            pnlArguments.Controls.Add(pnlArgumentTextbox, 0, 1);
            pnlArguments.SetColumnSpan(pnlArgumentTextbox, 2);
            pnlArguments.Controls.Add(lblDir, 0, 2);
            pnlArguments.SetColumnSpan(lblDir, 2);
            pnlArguments.Controls.Add(pnlWorkingDirectory, 0, 3);
            pnlArguments.Controls.Add(cmdSelectDirectory, 1, 3);

            // ---------- bottom buttons, right aligned: [삭제] [닫기] [저장] ----------
            pnlEnd = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Dock = DockStyle.Fill, Margin = Tron.P(0, 22, 0, 0) };
            cmdSave = Tron.Button("저장 (Ctrl+S)", true);
            cmdExit = Tron.Button("닫기 (ESC)");
            cmdDelete = Tron.Button("그룹 삭제");
            cmdDelete.ForeColor = Tron.Error;
            cmdDelete.FlatAppearance.BorderColor = Tron.Error;
            foreach (Button b in new[] { cmdSave, cmdExit, cmdDelete })
            {
                b.Size = Tron.S(125, 38);
                b.Margin = Tron.P(20, 0, 0, 0);
            }
            cmdSave.Click += new EventHandler(this.cmdSave_Click);
            cmdExit.Click += new EventHandler(this.cmdExit_Click);
            cmdDelete.Click += new EventHandler(this.cmdDelete_Click);
            pnlEnd.Controls.Add(cmdSave);
            pnlEnd.Controls.Add(cmdExit);
            pnlEnd.Controls.Add(cmdDelete);

            tlpRoot.Controls.Add(tlpHead);
            tlpRoot.Controls.Add(lblShortcuts);
            tlpRoot.Controls.Add(pnlShortcuts);
            tlpRoot.Controls.Add(pnlAddShortcut);
            tlpRoot.Controls.Add(lblErrorShortcut);
            tlpRoot.Controls.Add(pnlColor);
            tlpRoot.Controls.Add(pnlArguments);
            tlpRoot.Controls.Add(pnlEnd);

            // ---------- form ----------
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Tron.Bg;
            this.ForeColor = Tron.Text;
            this.Font = Tron.F(10f);
            this.ClientSize = Tron.S(660, 520);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.Controls.Add(tlpRoot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGroup";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "새 그룹 만들기";
            this.Load += new EventHandler(this.frmGroup_Load);
            this.MouseClick += new MouseEventHandler(this.frmGroup_MouseClick);
            this.ResumeLayout(false);
        }

        private static Label ErrorLabel()
        {
            Label l = Tron.Label("", 9.5f);
            l.ForeColor = Tron.Error;
            l.Visible = false;
            return l;
        }

        private static Button SmallButton(string text)
        {
            Button b = Tron.Button(text);
            b.Size = Tron.S(30, 30);
            b.Margin = new Padding(0);
            b.Font = Tron.F(11f, true);
            return b;
        }

        private TableLayoutPanel tlpRoot;
        private ColorDialog colorDialog;
        private Panel pnlGroupIcon;
        private PictureBox cmdAddGroupIcon;
        private Label lblAddGroupIcon;
        private Label lblErrorIcon;
        private Label lblErrorTitle;
        private TextBox txtGroupName;
        private Button cmdWidthDown;
        private Button cmdWidthUp;
        private Label lblNum;
        private Label lblErrorNum;
        private Panel pnlShortcuts;
        private Panel pnlAddShortcut;
        private Label lblErrorShortcut;
        private TableLayoutPanel pnlColor;
        private RadioButton radioDark;
        private RadioButton radioLight;
        private RadioButton radioTron;
        private RadioButton radioCustom;
        private Panel pnlCustomColor;
        private Label lblOpacity;
        private Button numOpacUp;
        private Button numOpacDown;
        private CheckBox pnlAllowOpenAll;
        private TableLayoutPanel pnlArguments;
        private TextBox pnlArgumentTextbox;
        private TextBox pnlWorkingDirectory;
        private Button cmdSelectDirectory;
        private FlowLayoutPanel pnlEnd;
        private Button cmdSave;
        private Button cmdExit;
        private Button cmdDelete;
    }
}
