using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmNhanVienChiTiet : Form
    {
        private ServiceBLL bll;
        private string currentMaNV;
        private string currentTenNV;

        public frmNhanVienChiTiet(string maNV, ServiceBLL serviceBLL)
        {
            // Gọi hàm khởi tạo giao diện từ file Designer
            InitializeComponent();

            this.currentMaNV = maNV;
            this.bll = serviceBLL;

            // Cấu hình Logic ban đầu
            SetupInitialLogic();

            // Thiết lập tiêu đề form
            this.Text = maNV == null ? "THÊM NHÂN VIÊN MỚI" : "CẬP NHẬT THÔNG TIN NHÂN VIÊN";
        }

        private void SetupInitialLogic()
        {
            // Đổ dữ liệu ComboBox
            cmbVaiTro.Items.AddRange(new string[] { "Phục vụ", "Thu ngân", "Quản lý" });
            cmbVaiTro.SelectedIndex = 0;

            cmbTrangThai.Items.AddRange(new string[] { "Kích hoạt", "Khóa" });
            cmbTrangThai.SelectedIndex = 0;

            // Nếu là Thêm mới -> Ẩn chọn trạng thái (Mặc định Kích hoạt)
            if (currentMaNV == null)
            {
                lblTrangThai.Visible = false;
                cmbTrangThai.Visible = false;
                // Form ngắn lại một chút vì mất 1 dòng
                this.Height -= 40;
                btnLuu.Location = new Point(btnLuu.Location.X, btnLuu.Location.Y - 40);
            }
        }

        // --- SỰ KIỆN UI ---
        private void txtLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLuong_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtLuong.Text.Replace(",", "").Replace(".", ""), out decimal v))
            {
                txtLuong.Text = v.ToString("N0");
            }
        }

        public void SetData(string maNV, string ten, string sdt, string diaChi, string trangThaiTK, string vaiTroTK, string luong, string email)
        {
            this.currentTenNV = ten;

            txtHoTen.Text = ten;
            txtSDT.Text = sdt;
            txtDiaChi.Text = diaChi;
            txtEmail.Text = email;

            if (decimal.TryParse(luong, out decimal val))
                txtLuong.Text = val.ToString("N0");
            else
                txtLuong.Text = "0";

            if (cmbTrangThai.Items.Contains(trangThaiTK)) cmbTrangThai.SelectedItem = trangThaiTK;
            if (cmbVaiTro.Items.Contains(vaiTroTK)) cmbVaiTro.SelectedItem = vaiTroTK;

            // Khi sửa thì không cho đổi SĐT (vì là Tên đăng nhập)
            txtSDT.ReadOnly = true;
            txtSDT.BackColor = Color.WhiteSmoke;
        }

        // --- XỬ LÝ LƯU ---
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // 1. VALIDATE DỮ LIỆU
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // 2. LẤY DỮ LIỆU TỪ UI
            decimal luongInput = 0;
            // Xóa dấu phẩy format trước khi parse
            decimal.TryParse(txtLuong.Text.Replace(",", "").Replace(".", "").Trim(), out luongInput);

            NhanVienDTO nv = new NhanVienDTO
            {
                MaNV = currentMaNV,
                Ten = txtHoTen.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                Luong = luongInput,
                VaiTro = cmbVaiTro.SelectedItem.ToString(),
                TrangThaiTK = currentMaNV == null ? "Kích hoạt" : cmbTrangThai.SelectedItem.ToString()
            };

            // 3. GỌI BLL
            string msg = "";
            if (currentMaNV == null)
            {
                msg = bll.AddStaff(nv);
            }
            else
            {
                msg = bll.UpdateStaff(nv);
            }

            MessageBox.Show(msg);

            if (msg.Contains("thành công"))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}