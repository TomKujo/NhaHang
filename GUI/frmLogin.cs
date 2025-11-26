using BLL; // Gọi lớp nghiệp vụ
using DTO; // Gọi lớp dữ liệu
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmLogin : Form
    {
        // Khởi tạo lớp xử lý nghiệp vụ
        ServiceBLL bll = new ServiceBLL();

        public frmLogin()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        // Hàm này để căn giữa khung trắng (pnlCard) mỗi khi mở form
        private void SetupCustomUI()
        {
            // Căn giữa Panel Card
            pnlCard.Location = new Point(
                (this.Width - pnlCard.Width) / 2,
                (this.Height - pnlCard.Height) / 2
            );

            // Bo tròn góc cho Button (Nếu muốn đẹp hơn) - Tùy chọn
            // btnXacNhan.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnXacNhan.Width, btnXacNhan.Height, 10, 10));
        }

        // Sự kiện click nút Xác Nhận
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string u = txtMaNV.Text.Trim();
            string p = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            NguoiDungDTO user;
            var result = bll.LoginAdvanced(u, p, out user);

            // ... bên trong btnXacNhan_Click ...

            switch (result)
            {
                // Thêm ServiceBLL. vào trước LoginResult
                case ServiceBLL.LoginResult.Success:
                    frmMain f = new frmMain(user);
                    this.Hide();
                    f.ShowDialog();
                    this.Close();
                    break;

                case ServiceBLL.LoginResult.InvalidCredentials:
                    ShowError("Mật khẩu không đúng! \nLưu ý: Nhập sai 3 lần tài khoản sẽ bị KHÓA.");
                    txtMatKhau.Clear();
                    txtMatKhau.Focus();
                    break;

                case ServiceBLL.LoginResult.Locked:
                    ShowError("Tài khoản này đã bị KHÓA do vi phạm bảo mật.\nVui lòng liên hệ Quản lý để mở khóa.");
                    break;

                case ServiceBLL.LoginResult.CustomerDenied:
                    ShowWarning("Tài khoản Khách hàng chỉ dùng cho đặt bàn Online/App.\nKhông thể truy cập hệ thống quản lý.");
                    break;

                case ServiceBLL.LoginResult.UserNotFound:
                    ShowError("Tài khoản không tồn tại trong hệ thống.");
                    txtMaNV.Focus();
                    break;
            }
        }

        // Thêm một Label "lblQuenMatKhau" vào form Login và gán sự kiện này
        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            string user = txtMaNV.Text.Trim();
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập vào ô mã nhân viên để lấy lại mật khẩu.");
                return;
            }
            string msg = bll.ForgotPassword(user);
            MessageBox.Show(msg);
        }

        // Sự kiện nút Thoát (X)
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Sự kiện nhấn Enter thì tự động bấm nút Xác Nhận (UX tốt hơn)
        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnXacNhan.PerformClick();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            string user = txtMaNV.Text.Trim();
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập vào ô mã nhân viên để lấy lại mật khẩu.");
                return;
            }
            string msg = bll.ForgotPassword(user);
            MessageBox.Show(msg);
        }

        // --- CÁC HÀM HỖ TRỢ HIỂN THỊ THÔNG BÁO ---

        // Hàm hiển thị lỗi (Icon X đỏ)
        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hàm hiển thị cảnh báo (Icon tam giác vàng)
        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}