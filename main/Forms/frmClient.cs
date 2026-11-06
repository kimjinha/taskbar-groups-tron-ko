using client.Classes;
using client.User_controls;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Data.Json;

namespace client.Forms
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            System.Runtime.ProfileOptimization.StartProfile("frmClient.Profile");
            InitializeComponent();
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            Reload();
            CreateTray();

            Task.Run(async () =>
            {
                string v = await getVersionData();
                try
                {
                    if (this.IsHandleCreated)
                        this.BeginInvoke(new Action(() => githubVersion.Text = "원본 최신: " + v));
                }
                catch { }
            });
        }

        public void Reload()
        {
            // flush and reload existing groups
            pnlExistingGroups.SuspendLayout();
            pnlExistingGroups.Controls.Clear();
            pnlExistingGroups.RowStyles.Clear();

            string configPath = @MainPath.path + @"\config";
            foreach (string dir in Directory.GetDirectories(configPath))
            {
                try
                {
                    LoadCategory(dir);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            pnlExistingGroups.ResumeLayout();

            lblAddGroup.Text = pnlExistingGroups.HasChildren
                ? "+  새 그룹 추가"
                : "+  첫 그룹 만들기 — 여기를 누르세요";
        }

        public void LoadCategory(string dir)
        {
            Category category = new Category(dir);
            ucCategoryPanel newCategory = new ucCategoryPanel(this, category)
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, Tron.S(8))
            };
            pnlExistingGroups.Controls.Add(newCategory);
        }

        // kept for callers in frmGroup; the docked layout needs no manual sizing
        public void Reset() { }

        private void cmdAddGroup_Click(object sender, EventArgs e)
        {
            frmGroup newGroup = new frmGroup(this);
            newGroup.Show();
            newGroup.BringToFront();
        }

        // TaskbarX (taskbar centering/style) ships in the TaskbarX subfolder next to this exe
        private void cmdTaskbarX_Click(object sender, EventArgs e)
        {
            string exe = Path.Combine(Application.StartupPath, "TaskbarX", "TaskbarX Configurator.exe");
            if (!File.Exists(exe))
            {
                MessageBox.Show("TaskbarX 설정 프로그램을 찾을 수 없습니다.\n\n" + exe + "\n\nbuild.bat 로 다시 빌드해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(exe) { WorkingDirectory = Path.GetDirectoryName(exe) });
        }

        private void cmdOpenShortcuts_Click(object sender, EventArgs e)
        {
            string dir = Path.Combine(MainPath.path, "Shortcuts");
            Directory.CreateDirectory(dir);
            System.Diagnostics.Process.Start("explorer.exe", "\"" + dir + "\"");
        }

        private void pnlAddGroup_MouseEnter(object sender, EventArgs e)
        {
            pnlAddGroup.BackColor = Tron.Hover;
        }

        private void pnlAddGroup_MouseLeave(object sender, EventArgs e)
        {
            pnlAddGroup.BackColor = Tron.Bg;
        }

        public void EnterControl(object sender, EventArgs e, Control control)
        {
            control.BackColor = Tron.Hover;
            control.Invalidate();
        }

        public void LeaveControl(object sender, EventArgs e, Control control)
        {
            // MouseLeave also fires when moving onto a child control; keep the highlight while the cursor is still inside
            if (control.ClientRectangle.Contains(control.PointToClient(Cursor.Position)))
                return;
            control.BackColor = Tron.Bg;
            control.Invalidate();
        }

        private static async Task<String> getVersionData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    client.DefaultRequestHeaders.Add("User-Agent", "taskbar-groups");
                    var res = await client.GetAsync("https://api.github.com/repos/tjackenpacken/taskbar-groups/releases");
                    res.EnsureSuccessStatusCode();
                    string responseBody = await res.Content.ReadAsStringAsync();

                    JsonArray responseJSON = JsonArray.Parse(responseBody);
                    JsonObject jsonObjectData = responseJSON[0].GetObject();

                    return jsonObjectData["tag_name"].GetString();
                }
            }
            catch { return "확인 불가"; }
        }

        //--------------------------------------
        // TRAY ICON: X hides to the tray, "종료" in the tray menu really quits
        //--------------------------------------
        private NotifyIcon tray;
        private bool exiting;
        private bool trayTipShown;

        private void CreateTray()
        {
            ContextMenuStrip menu = new ContextMenuStrip { ShowImageMargin = false, Font = Tron.F(10f) };
            menu.Items.Add("관리자 열기", null, (s, e) => ShowFromTray());
            menu.Items.Add("새 그룹 만들기", null, (s, e) => { ShowFromTray(); cmdAddGroup_Click(s, e); });
            menu.Items.Add("⚡ 작업 표시줄 꾸미기", null, cmdTaskbarX_Click);
            menu.Items.Add("Shortcuts 폴더 열기", null, cmdOpenShortcuts_Click);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("종료", null, (s, e) => { exiting = true; tray.Visible = false; Application.Exit(); });

            tray = new NotifyIcon
            {
                Icon = TronTrayIcon(),
                Text = "TaskbarGroups — 작업 표시줄 그룹 관리자",
                ContextMenuStrip = menu,
                Visible = true
            };
            tray.DoubleClick += (s, e) => ShowFromTray();
            this.FormClosing += frmClient_FormClosing;
            this.FormClosed += (s, e) => tray.Dispose();
        }

        private void ShowFromTray()
        {
            Show();
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
            Activate();
        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exiting || e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            Hide();
            if (!trayTipShown)
            {
                tray.ShowBalloonTip(3000, "TaskbarGroups", "트레이에서 계속 실행 중입니다. 완전히 끄려면 트레이 아이콘 우클릭 → 종료", ToolTipIcon.Info);
                trayTipShown = true;
            }
        }

        // Small TRON tile drawn in code: obsidian square, neon frame, 2x2 grid
        private static Icon TronTrayIcon()
        {
            int n = Math.Max(16, (int)Math.Round(16 * Tron.Scale)) * 2; // drawn at 2x, Windows scales it down
            using (Bitmap bmp = new Bitmap(n, n))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                float w = n / 12f;
                using (SolidBrush bg = new SolidBrush(Tron.Bg))
                    g.FillRectangle(bg, w, w, n - 2 * w, n - 2 * w);
                using (Pen p = new Pen(Tron.Cyan, w * 1.6f))
                {
                    g.DrawRectangle(p, w, w, n - 2 * w, n - 2 * w);
                    float a = n * 0.3f, b = n * 0.7f, m = n * 0.5f;
                    g.DrawRectangle(p, a, a, b - a, b - a);
                    g.DrawLine(p, m, a, m, b);
                    g.DrawLine(p, a, m, b, m);
                }
                return Icon.FromHandle(bmp.GetHicon());
            }
        }

        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/tjackenpacken/taskbar-groups/releases");
        }
    }
}
