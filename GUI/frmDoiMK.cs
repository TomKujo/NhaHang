using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmDoiMK : Form
    {
        private NguoiDungDTO user;
        private ServiceBLL bll = new ServiceBLL();
        private string serverOTP = "";
        private string usernameInput = "";

        public string NewPassword { get; private set; }

        public frmDoiMK(string username = "")
        {
            InitializeComponent();
            this.usernameInput = username;

            UIHelper.StyleButton(btnGuiOTP, false);
            UIHelper.StylePrimaryButton(btnXacNhan, "XÁC NHẬN ĐỔI MẬT KHẨU", UIHelper.PrimaryColor);
            btnXacNhan.Height = 50;
        }

        private void btnGuiOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Vui lòng nhập Email hợp lệ!");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string otpOut = "";
            string result = bll.CheckEmailAndSendOTP(usernameInput, email, out otpOut);

            Cursor.Current = Cursors.Default;

            if (result == "OK")
            {
                serverOTP = otpOut;
                MessageBox.Show($"OTP đã được gửi đến {email}. Vui lòng kiểm tra.");
                txtOTP.Enabled = true;
                txtOTP.Focus();

                txtEmail.Enabled = false;
                btnGuiOTP.Enabled = false;
                btnGuiOTP.Text = "Đã gửi";
            }
            else
            {
                MessageBox.Show(result, "Lỗi Gửi OTP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(serverOTP))
            {
                MessageBox.Show("Vui lòng nhấn Gửi OTP trước!");
                return;
            }
            if (txtOTP.Text.Trim() != serverOTP)
            {
                MessageBox.Show("Mã OTP không chính xác!");
                return;
            }

            string p1 = txtPassMoi.Text.Trim();
            string p2 = txtXacNhan.Text.Trim();

            if (string.IsNullOrEmpty(p1) || p1.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải từ 6 ký tự trở lên!");
                return;
            }

            if (p1 != p2)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            if (string.IsNullOrEmpty(usernameInput))
            {
                MessageBox.Show("Lỗi: Không xác định được tài khoản. Vui lòng nhập Tên đăng nhập ở màn hình trước.");
                return;
            }

            string result = bll.ChangePassword(usernameInput, p1);
            if (result.Contains("thành công"))
            {
                MessageBox.Show("Đổi mật khẩu thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}