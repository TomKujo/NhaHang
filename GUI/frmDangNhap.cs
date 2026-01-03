using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmDangNhap : Form
    {
        private bool isLogout = false;

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            CheckAutoLogin();
        }

        ServiceBLL bll = new ServiceBLL();

        private void CheckAutoLogin()
        {
            if (Properties.Settings.Default.RememberMe && !isLogout)
            {
                txtTenDN.Text = Properties.Settings.Default.UserName;
                txtMK.Text = Properties.Settings.Default.Password;
                chkDuyTri.Checked = true;

                XuLyDangNhap(txtTenDN.Text, txtMK.Text);
            }
            else if (Properties.Settings.Default.RememberMe)
            {
                txtTenDN.Text = Properties.Settings.Default.UserName;
                txtMK.Text = Properties.Settings.Default.Password;
                chkDuyTri.Checked = true;
            }
        }

        private void XuLyDangNhap(string tenDN, string mk)
        {
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
                    SaveCredentials(tenDN, mk);

                    frmMain f = new frmMain(user);
                    this.Hide();
                    DialogResult dr = f.ShowDialog();

                    if (dr == DialogResult.OK)
                    {
                        this.isLogout = true;
                        ClearCredentials();
                        chkDuyTri.Checked = false;
                        txtTenDN.Text = "";
                        txtMK.Text = "";    
                        this.Show();
                        txtTenDN.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                    break;

                case LoginResult.InvalidCredentials:
                    ShowError("Mật khẩu không đúng hoặc đã bị thay đổi!");
                    if (Properties.Settings.Default.RememberMe)
                    {
                        ClearCredentials();
                        chkDuyTri.Checked = false;
                    }
                    this.Show();
                    txtMK.Focus();
                    break;

                case LoginResult.Locked:
                    ShowError("Tài khoản này đã bị KHÓA.");
                    this.Show();
                    break;

                case LoginResult.CustomerDenied:
                    ShowWarning("Tài khoản Khách hàng không có quyền truy cập.");
                    this.Show();
                    break;

                case LoginResult.UserNotFound:
                    ShowError("Tài khoản không tồn tại.");
                    this.Show();
                    break;
            }
        }

        private void SaveCredentials(string username, string password)
        {
            if (chkDuyTri.Checked)
            {
                Properties.Settings.Default.UserName = username;
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                ClearCredentials();
            }
            Properties.Settings.Default.Save();
        }

        private void ClearCredentials()
        {
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            XuLyDangNhap(txtTenDN.Text.Trim(), txtMK.Text.Trim());
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            string currentInputUser = txtTenDN.Text.Trim();
            this.Hide();
            frmDoiMK f = new frmDoiMK(currentInputUser);
            f.ShowDialog();
            this.Show();
            isLogout = true;
            if (!string.IsNullOrEmpty(txtTenDN.Text))
            {
                txtMK.Focus();
            }
            else
            {
                txtTenDN.Focus();
            }
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