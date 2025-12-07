using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        ServiceBLL bll = new ServiceBLL();

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string tenDN = txtTenDN.Text.Trim();
            string mk = txtMK.Text.Trim();

            if (string.IsNullOrEmpty(tenDN) || string.IsNullOrEmpty(mk))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            NguoiDungDTO user;
            var result = bll.LoginAdvanced(tenDN, mk, out user);

            switch (result)
            {
                case LoginResult.Success:
                    frmMain f = new frmMain(user);
                    this.Hide();
                    DialogResult dr = f.ShowDialog();

                    // Kiểm tra xem form Main đóng vì lý do gì?
                    if (dr == DialogResult.OK)
                    {
                        // Nếu là do bấm nút Đăng xuất (DialogResult.OK) -> Hiện lại Login
                        this.Show();
                        txtMK.Text = ""; // Xóa mật khẩu cũ
                        txtTenDN.Focus();
                    }
                    else
                    {
                        // Nếu đóng bằng nút X hoặc Alt+F4 -> Thoát luôn ứng dụng
                        this.Close();
                    }
                    break;

                case LoginResult.InvalidCredentials:
                    ShowError("Mật khẩu không đúng!\nLưu ý: Nhập sai 3 lần tài khoản sẽ bị KHÓA.");
                    txtMK.Focus();
                    break;

                case LoginResult.Locked:
                    ShowError("Tài khoản này đã bị KHÓA do vi phạm bảo mật\nVui lòng liên hệ Quản lý để mở khóa.");
                    break;

                case LoginResult.CustomerDenied:
                    ShowWarning("Tài khoản Khách hàng chỉ dùng cho đặt bàn Online/App.\nKhông thể truy cập hệ thống quản lý.");
                    txtTenDN.Focus();
                    break;

                case LoginResult.UserNotFound:
                    ShowError("Tài khoản không tồn tại trong hệ thống.");
                    txtTenDN.Focus();
                    break;
            }
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            string user = txtTenDN.Text.Trim();
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập vào để lấy lại mật khẩu.");
                return;
            }
            string msg = bll.ForgotPassword(user);
            MessageBox.Show(msg);
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnXacNhan.PerformClick();
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}