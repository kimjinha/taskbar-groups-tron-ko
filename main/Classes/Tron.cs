using System;
using System.Drawing;
using System.Windows.Forms;

namespace client.Classes
{
    // TRON: Legacy look shared by every window, plus DPI-safe pixel scaling.
    static class Tron
    {
        public static readonly Color Bg = ColorTranslator.FromHtml("#0A0F14");     // obsidian black
        public static readonly Color Panel = ColorTranslator.FromHtml("#0F1A22");  // one step lighter
        public static readonly Color Hover = ColorTranslator.FromHtml("#123A46");  // hover / selected row
        public static readonly Color Line = ColorTranslator.FromHtml("#0E3A44");   // dividers
        public static readonly Color Cyan = ColorTranslator.FromHtml("#00E5FF");   // neon accent
        public static readonly Color Text = ColorTranslator.FromHtml("#D8F6FF");
        public static readonly Color Muted = ColorTranslator.FromHtml("#7FA9B5");
        public static readonly Color Error = ColorTranslator.FromHtml("#FF6B6B");

        // ponytail: one DPI for the whole process (primary monitor); per-monitor rescale only if mixed-DPI setups need it
        public static readonly float Scale = GetScale();

        private static float GetScale()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                return g.DpiX / 96f;
        }

        // 100%-design pixels -> real pixels
        public static int S(int px) { return (int)Math.Round(px * Scale); }
        public static Size S(int w, int h) { return new Size(S(w), S(h)); }
        public static Padding P(int all) { return new Padding(S(all)); }
        public static Padding P(int l, int t, int r, int b) { return new Padding(S(l), S(t), S(r), S(b)); }

        public static Font F(float pt, bool bold = false)
        {
            return new Font("Segoe UI", pt, bold ? FontStyle.Bold : FontStyle.Regular);
        }

        public static bool IsTron(Category c)
        {
            return c != null && (c.isTronTheme || "#0A0F14".Equals(c.ColorString, StringComparison.OrdinalIgnoreCase));
        }

        public static Label Label(string text, float pt = 10f, bool muted = false, bool bold = false)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = F(pt, bold),
                ForeColor = muted ? Muted : Text,
                BackColor = Color.Transparent,
                Margin = P(0, 0, 0, 4)
            };
        }

        // primary = filled cyan, otherwise cyan outline
        public static void Style(ButtonBase b, bool primary = false)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Cyan;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = primary ? ColorTranslator.FromHtml("#5FF0FF") : Hover;
            b.FlatAppearance.MouseDownBackColor = primary ? ColorTranslator.FromHtml("#00B8CC") : Line;
            b.BackColor = primary ? Cyan : Bg;
            b.ForeColor = primary ? Bg : Cyan;
            b.Font = F(10f, primary);
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        public static Button Button(string text, bool primary = false)
        {
            Button b = new Button { Text = text, Size = S(120, 36), Margin = P(0, 0, 0, 8) };
            Style(b, primary);
            return b;
        }

        // Radio button drawn as a toggle tile: outline when off, filled cyan when on
        public static void Toggle(RadioButton r)
        {
            r.Appearance = Appearance.Button;
            r.TextAlign = ContentAlignment.MiddleCenter;
            Style(r);
            r.FlatAppearance.CheckedBackColor = Cyan;
            EventHandler sync = (s, e) => r.ForeColor = r.Checked ? Bg : Cyan;
            r.CheckedChanged += sync;
            sync(r, EventArgs.Empty);
        }

        public static void Style(TextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = Panel;
            t.ForeColor = Text;
            t.Font = F(11f);
        }
    }
}
