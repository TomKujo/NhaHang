using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class ucKhuyenMai : UserControl
    {
        private ServiceBLL bll = new ServiceBLL();

        public ucKhuyenMai()
        {
            InitializeComponent();
            this.Load += UcKhuyenMai_Load; // Gắn sự kiện Load
            btnThem.Click += btnThem_Click; // Gắn sự kiện cho nút Thêm
            btnSua.Click += btnSua_Click;   // Gắn sự kiện cho nút Sửa
            btnXoa.Click += btnXoa_Click;   // Gắn sự kiện cho nút Xóa
        }

        private void UcKhuyenMai_Load(object sender, EventArgs e)
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            // Thiết lập DataGridView
            UIHelper.StyleDataGridView(dgvKhuyenMai);
            string[] headers = { "Mã KM", "Tên Khuyến Mãi", "Mô Tả", "Điểm Yêu Cầu", "Giá Trị Giảm", "Loại Giảm", "Ngày Bắt Đầu", "Ngày Kết Thúc", "Trạng Thái" };
            string[] dataFields = { "MaKM", "TenKM", "MoTa", "DiemCanThiet", "GiaTriGiam", "LoaiGiam", "NgayBatDau", "NgayKetThuc", "TrangThai" };
            UIHelper.SetGridColumns(dgvKhuyenMai, headers, dataFields);

            // Thiết lập Style cho các nút
            UIHelper.StyleButton(btnThem, true); // Nút chính
            UIHelper.StyleButton(btnSua, false); // Nút phụ
            UIHelper.StyleButton(btnXoa, false); // Nút phụ
        }

        private void LoadData()
        {
            dgvKhuyenMai.DataSource = bll.GetListKhuyenMai();
        }

        // File: ucKhuyenMai.cs (Tiếp tục)

        // Hàm giả định để mở Form Thêm/Sửa (Form này cần được bạn tự tạo)
        private void ShowAddEditForm(KhuyenMaiDTO km, Button sender)
        {
            // 1. Lấy tọa độ màn hình của góc dưới bên trái nút bấm
            Point btnLocation = sender.PointToScreen(new Point(0, sender.Height + 5)); // +5px offset

            // 2. Tạo Form Thêm/Sửa Khuyến Mãi (Sử dụng Form mới)
            frmAddEditKM f = new frmAddEditKM(km);

            // Thiết lập vị trí (top, left) theo yêu cầu
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Location = btnLocation;

            // Thay vì ShowDialog(parentForm), dùng ShowDialog() và kiểm tra kết quả
            if (f.ShowDialog() == DialogResult.OK)
            {
                // 3. Sau khi Form đóng, làm mới DataGridView
                LoadData();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ShowAddEditForm(null, (Button)sender); // Thêm mới (km = null)
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhuyenMai.SelectedRows.Count > 0)
            {
                // Lấy dữ liệu Khuyến Mãi từ dòng được chọn
                DataGridViewRow row = dgvKhuyenMai.SelectedRows[0];
                KhuyenMaiDTO selectedKM = new KhuyenMaiDTO
                {
                    MaKM = row.Cells["MaKM"].Value.ToString(),
                    TenKM = row.Cells["TenKM"].Value.ToString(),
                    MoTa = row.Cells["MoTa"].Value.ToString(),
                    DiemCanThiet = Convert.ToInt32(row.Cells["DiemCanThiet"].Value),
                    GiaTriGiam = Convert.ToDecimal(row.Cells["GiaTriGiam"].Value),
                    LoaiGiam = row.Cells["LoaiGiam"].Value.ToString(),
                    NgayBatDau = Convert.ToDateTime(row.Cells["NgayBatDau"].Value),
                    NgayKetThuc = row.Cells["NgayKetThuc"].Value != DBNull.Value ? (DateTime?)row.Cells["NgayKetThuc"].Value : null,
                    TrangThai = row.Cells["TrangThai"].Value.ToString()
                };

                ShowAddEditForm(selectedKM, (Button)sender); // Sửa (truyền DTO)
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhuyenMai.SelectedRows.Count > 0)
            {
                string maKM = dgvKhuyenMai.SelectedRows[0].Cells["MaKM"].Value.ToString();
                string tenKM = dgvKhuyenMai.SelectedRows[0].Cells["TenKM"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khuyến mãi '{tenKM}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string message = bll.DeleteKhuyenMai(maKM);
                    MessageBox.Show(message);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}