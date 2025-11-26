using System.Drawing;
using System.Windows.Forms;

public static class UIHelper
{
    // Mã màu lấy từ Figma
    public static Color PrimaryColor = ColorTranslator.FromHtml("#FF9800"); // Màu cam chủ đạo
    public static Color SidebarColor = ColorTranslator.FromHtml("#F5F5F5"); // Màu nền menu trái (Cream nhạt)
    public static Color HeaderColor = Color.Black; // Màu đen phần Logo
    public static Color TextColor = Color.FromArgb(64, 64, 64);

    // Hàm format Button theo style phẳng
    public static void StyleButton(Button btn, bool isPrimary = true)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btn.Cursor = Cursors.Hand;

        if (isPrimary)
        {
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
        }
        else
        {
            btn.BackColor = Color.White; // Nền trắng
            btn.ForeColor = TextColor;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.Silver;
        }
    }

    // Hàm format DataGridView giống ảnh 2 (Nhân sự)
    public static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        // Header
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        dgv.ColumnHeadersHeight = 40;

        // Row
        dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 224, 178); // Cam nhạt khi chọn
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        dgv.RowTemplate.Height = 35;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AllowUserToAddRows = false;
    }
}