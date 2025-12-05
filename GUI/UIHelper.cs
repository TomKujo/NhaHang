using System;
using System.Drawing;
using System.Windows.Forms;

public static class UIHelper
{
    public static Color PrimaryColor = ColorTranslator.FromHtml("#FF9800");
    public static Color SidebarColor = ColorTranslator.FromHtml("#1E1E1E");
    public static Color SecondaryColor = Color.WhiteSmoke;

    // --- FORM & DIALOG SETUP ---
    public static void SetupDialog(Form f, string title)
    {
        f.Text = title;
        f.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Style cửa sổ công cụ
        f.ControlBox = true;   // [SỬA]: Bật ControlBox để hiện nút X
        f.MinimizeBox = false; // Ẩn nút Min
        f.MaximizeBox = false; // Ẩn nút Max
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

    // --- FACTORY METHODS (Tạo Control nhanh) ---

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
            AutoSize = dock != DockStyle.Fill // Nếu Fill thì không AutoSize để canh lề chuẩn
        };
    }

    public static Button CreateButton(string text, int w, Color bg, bool isPrimary = true)
    {
        Button btn = new Button { Text = text, Width = w, Height = 35 };
        StyleButton(btn, isPrimary);
        btn.BackColor = bg; // Override màu nếu cần
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

            // Định dạng số tiền và số lượng
            if (headers[i].Contains("tiền") || headers[i].Contains("Giá"))
            {
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (headers[i].Contains("Lượng") || headers[i].Contains("Tồn") || headers[i].Contains("SL"))
            {
                col.DefaultCellStyle.Format = "N2"; // Hiển thị 2 số lẻ cho số lượng (kg, lít)
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgv.Columns.Add(col);
        }
    }

    public static void StyleSidebarButton(Button btn, Image icon, string text)
    {
        btn.Text = "  " + text; // Thêm khoảng trắng để tách chữ khỏi icon
        btn.Image = icon;
        btn.ImageAlign = ContentAlignment.MiddleLeft;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.TextImageRelation = TextImageRelation.ImageBeforeText; // Đặt icon trước chữ

        btn.Padding = new Padding(15, 0, 0, 0); // Canh lề trái cho nội dung bên trong
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular); // Font chữ hiện đại hơn Times New Roman
        btn.ForeColor = Color.White; // Chữ màu trắng
        btn.BackColor = SidebarColor; // Nền tiệp màu sidebar
        btn.Cursor = Cursors.Hand;

        // Kích thước nút (để icon không bị méo)
        btn.Height = 55;
        btn.Dock = DockStyle.Top; // Xếp chồng lên nhau
    }

    public static void SetInactiveButton(Button btn)
    {
        btn.BackColor = SidebarColor;
        btn.ForeColor = Color.White;
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
    }

    public static void SetActiveButton(Button btn)
    {
        btn.BackColor = Color.FromArgb(50, 50, 50); // Sáng hơn nền sidebar một chút
        btn.ForeColor = PrimaryColor; // Chữ chuyển sang màu Cam
        btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

        // (Tùy chọn) Thêm viền cam bên trái để giống thiết kế web
        btn.FlatAppearance.BorderSize = 0;
    }

    public static void StyleSubMenuButton(Button btn, Image icon, string text)
    {
        // Thêm nhiều khoảng trắng để thụt lề sâu hơn nút cha
        btn.Text = "      " + text;
        btn.Image = icon; // Để null nếu không muốn icon

        // Cấu hình Layout: Icon trước, Chữ sau
        btn.ImageAlign = ContentAlignment.MiddleLeft;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.TextImageRelation = TextImageRelation.ImageBeforeText;

        // Padding trái lớn (30) để tạo cảm giác cấp con
        btn.Padding = new Padding(30, 0, 0, 0);

        // Style giao diện phẳng, tối màu
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular); // Font nhỏ hơn nút chính
        btn.ForeColor = Color.Silver; // Màu chữ xám bạc (tối hơn trắng)
        btn.BackColor = Color.FromArgb(40, 40, 40); // Nền tối hơn Sidebar chính
        btn.Cursor = Cursors.Hand;

        // Kích thước
        btn.Height = 45; // Chiều cao nhỏ hơn nút chính (55)
        btn.Dock = DockStyle.Top;
    }
}