using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class UIHelper
{
    public static Color PrimaryColor = ColorTranslator.FromHtml("#FF9800");
    public static Color SidebarColor = ColorTranslator.FromHtml("#1E1E1E");
    public static Color SecondaryColor = Color.WhiteSmoke;
    public static Color DangerColor = ColorTranslator.FromHtml("#F44336");

    public static void SetupDialog(Form f, string title)
    {
        f.Text = title;
        f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        f.ControlBox = true;
        f.MinimizeBox = false;
        f.MaximizeBox = false;
        f.StartPosition = FormStartPosition.CenterParent;
        f.BackColor = Color.White;
        f.Font = new Font("Segoe UI", 10);
    }
    public static Button CreateCloseButton(Form f)
    {
        Button btn = CreateButton("Đóng", 100, Color.Gray);
        btn.Click += (s, e) => f.Close();
        return btn;
    }

    public static Panel CreatePanel(DockStyle dock, int height = 0, Padding? padding = null)
    {
        return new Panel { Dock = dock, Height = height, Padding = padding ?? new Padding(0), BackColor = Color.Transparent };
    }

    public static Label CreateLabel(string text, DockStyle dock = DockStyle.None, Font font = null, Color? color = null, ContentAlignment align = ContentAlignment.MiddleLeft)
    {
        return new Label
        {
            Text = text,
            Dock = dock,
            Font = font ?? new Font("Segoe UI", 10),
            ForeColor = color ?? Color.Black,
            TextAlign = align,
            AutoSize = dock != DockStyle.Fill
        };
    }

    public static Button CreateButton(string text, int w, Color bg, bool isPrimary = true)
    {
        Button btn = new Button { Text = text, Width = w, Height = 35 };
        StyleButton(btn, isPrimary);
        btn.BackColor = bg;
        return btn;
    }

    public static DataGridView CreateDataGridView()
    {
        var dgv = new DataGridView { Dock = DockStyle.Fill };
        StyleDataGridView(dgv);
        return dgv;
    }

    public static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BackgroundColor = Color.White;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.RowTemplate.Height = 35;
        dgv.ColumnHeadersHeight = 40;
        dgv.BorderStyle = BorderStyle.None;
    }

    public static void StyleButton(Button btn, bool isPrimary = true)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btn.Cursor = Cursors.Hand;
        if (isPrimary) { btn.BackColor = PrimaryColor; btn.ForeColor = Color.White; }
        else { btn.BackColor = Color.White; btn.ForeColor = Color.FromArgb(64, 64, 64); btn.FlatAppearance.BorderSize = 1; btn.FlatAppearance.BorderColor = Color.Silver; }
    }

    public static void SetGridColumns(DataGridView dgv, string[] headers, string[] dataFields, int[] widths = null)
    {
        dgv.AutoGenerateColumns = false;
        dgv.Columns.Clear();
        for (int i = 0; i < headers.Length; i++)
        {
            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = headers[i];
            col.DataPropertyName = dataFields[i];
            col.Name = dataFields[i];

            if (widths != null && i < widths.Length && widths[i] > 0)
            {
                col.Width = widths[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            string headerLower = headers[i].ToLower();
            if (headerLower.Contains("ngày") || headerLower.Contains("thời gian") || headerLower.Contains("hạn"))
            {
                col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            else if (headerLower.Contains("tiền") || headerLower.Contains("giá") ||
                     headerLower.Contains("doanh thu") || headerLower.Contains("chi phí") ||
                     headerLower.Contains("lợi nhuận") || headerLower.Contains("lương"))
            {
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (headerLower.Contains("lượng") || headerLower.Contains("tồn") ||
                     headerLower.Contains("sl") || headerLower.Contains("điểm"))
            {
                col.DefaultCellStyle.Format = headerLower.Contains("điểm") ? "N0" : "N2";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgv.Columns.Add(col);
        }
    }

    public static void StyleSidebarButton(Button btn, Image icon, string text)
    {
        btn.Text = "  " + text;
        btn.Image = icon;
        btn.ImageAlign = ContentAlignment.MiddleLeft;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.TextImageRelation = TextImageRelation.ImageBeforeText;
        btn.Padding = new Padding(15, 0, 0, 0);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
        btn.ForeColor = Color.White;
        btn.BackColor = SidebarColor;
        btn.Cursor = Cursors.Hand;
        btn.Height = 55;
        btn.Dock = DockStyle.Top;
    }

    public static void SetInactiveButton(Button btn)
    {
        btn.BackColor = SidebarColor;
        btn.ForeColor = Color.White;
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
    }

    public static void SetActiveButton(Button btn)
    {
        btn.BackColor = Color.FromArgb(50, 50, 50);
        btn.ForeColor = PrimaryColor;
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btn.FlatAppearance.BorderSize = 0;
    }

    public static void StyleSubMenuButton(Button btn, Image icon, string text)
    {
        btn.Text = "      " + text;
        btn.Image = icon;
        btn.ImageAlign = ContentAlignment.MiddleLeft;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.TextImageRelation = TextImageRelation.ImageBeforeText;
        btn.Padding = new Padding(30, 0, 0, 0);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        btn.ForeColor = Color.Silver;
        btn.BackColor = Color.FromArgb(40, 40, 40);
        btn.Cursor = Cursors.Hand;
        btn.Height = 45;
        btn.Dock = DockStyle.Top;
    }

    public static void SetRoundedCorner(Control control, int radius)
    {
        if (control == null) return;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
        path.CloseAllFigures();
        control.Region = new Region(path);
    }

    public static void StylePrimaryButton(Button btn, string text, Color bgColor)
    {
        btn.Text = text;
        btn.BackColor = bgColor;
        btn.ForeColor = Color.White;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btn.Cursor = Cursors.Hand;
        if (btn.Width < 100) btn.Width = 100;
        if (btn.Height < 35) btn.Height = 35;
    }
}