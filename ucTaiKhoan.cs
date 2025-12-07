using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucTaiKhoan : UserControl
    {
        private NguoiDungDTO currentUser;
        private ServiceBLL bll = new ServiceBLL();
        private string originalAddress;

        public ucTaiKhoan(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            ApplyStyles();

            // Đảm bảo sự kiện LoadData được gọi
            this.Load += (s, e) => LoadData();
            this.Resize += UcTaiKhoan_Resize;
        }

        private void ApplyStyles()
        {
            this.Paint += (s, e) => UIHelper.SetRoundedCorner(pnlContainer, 20);
            UIHelper.StylePrimaryButton(btnCapNhat, "Cập nhật", Color.Gray);
        }

        private void UcTaiKhoan_Resize(object sender, EventArgs e)
        {
            if (pnlContainer != null)
            {
                int x = (this.Width - pnlContainer.Width) / 2;
                int y = (this.Height - pnlContainer.Height) / 2;
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                pnlContainer.Location = new Point(x, y);
            }
        }

        private void LoadData()
        {
            if (currentUser == null) return;

            // Gọi BLL lấy thông tin chi tiết
            NhanVienDTO detail = bll.GetStaffDetail(currentUser.MaNguoiDung);

            if (detail != null)
            {
                txtTen.Text = detail.Ten;
                txtSDT.Text = detail.SDT;
                txtDiaChi.Text = detail.DiaChi;
                originalAddress = detail.DiaChi; // Lưu địa chỉ gốc

                txtLuong.Text = detail.Luong.ToString("#,##0") + " VNĐ";

                cboVaiTro.Items.Clear();
                cboVaiTro.Items.Add(detail.VaiTro);
                cboVaiTro.SelectedIndex = 0;

                txtMatKhau.Text = currentUser.MatKhau;
            }
            else
            {
                // Thêm dòng này để debug nếu vẫn không thấy dữ liệu
                MessageBox.Show($"Không tìm thấy thông tin chi tiết cho User ID: {currentUser.MaNguoiDung}. \nVui lòng kiểm tra lại CSDL xem user này đã có trong bảng NhanVien hoặc QuanLy chưa.", "Lỗi dữ liệu");
            }
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {
            // So sánh với originalAddress để bật/tắt nút
            if (txtDiaChi.Text.Trim() != originalAddress)
            {
                btnCapNhat.Enabled = true;
                btnCapNhat.BackColor = UIHelper.PrimaryColor;
            }
            else
            {
                btnCapNhat.Enabled = false;
                btnCapNhat.BackColor = Color.Gray;
            }
        }

        private void lblHienMatKhau_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.UseSystemPasswordChar)
            {
                txtMatKhau.UseSystemPasswordChar = false;
                lblHienMatKhau.Text = "Ẩn mật khẩu";
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
                lblHienMatKhau.Text = "Hiện mật khẩu";
            }
        }

        private void lblDoiMatKhau_Click(object sender, EventArgs e)
        {
            frmDoiMK f = new frmDoiMK(currentUser); // Đảm bảo bạn có form này
            UIHelper.SetupDialog(f, "Đổi mật khẩu");

            if (f.ShowDialog() == DialogResult.OK)
            {
                currentUser.MatKhau = f.NewPassword;
                txtMatKhau.Text = f.NewPassword;
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            string newAddr = txtDiaChi.Text.Trim();
            string result = bll.UpdatePersonalAddress(currentUser.MaNguoiDung, newAddr);
            MessageBox.Show(result);

            if (result.Contains("thành công"))
            {
                originalAddress = newAddr;
                btnCapNhat.Enabled = false;
                btnCapNhat.BackColor = Color.Gray;
            }
        }
    }
}