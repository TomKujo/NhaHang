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

        // Property để form cha lấy mật khẩu mới nếu cần
        public string NewPassword { get; private set; }

        public frmDoiMK(NguoiDungDTO u)
        {
            InitializeComponent();
            this.user = u;

            // Style
            UIHelper.StyleButton(btnGuiOTP, false); // Nút thường
            UIHelper.StylePrimaryButton(btnXacNhan, "Xác nhận", UIHelper.PrimaryColor);
            UIHelper.StylePrimaryButton(btnHuy, "Hủy", UIHelper.DangerColor);
        }

        private void btnGuiOTP_Click(object sender, EventArgs e)
        {
            // Lấy SĐT từ DB dựa trên user để đảm bảo bảo mật (hoặc lấy từ session)
            // Ở đây gọi BLL sinh OTP
            serverOTP = bll.SendOTP(user.TenDN); // Giả lập gửi OTP

            // DEMO: Hiển thị OTP lên MessageBox vì không có SMS thực
            MessageBox.Show($"[DEMO] Mã OTP của bạn là: {serverOTP}", "Tin nhắn từ hệ thống");

            txtOTP.Focus();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra OTP
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

            // 2. Kiểm tra mật khẩu
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

            // 3. Thực hiện đổi
            string result = bll.ChangePassword(user.TenDN, p1);
            if (result.Contains("thành công"))
            {
                MessageBox.Show(result);
                this.NewPassword = p1;
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