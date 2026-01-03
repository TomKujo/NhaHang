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
        private string originalSDT;
        private NhanVienDTO currentStaffDetail;

        public ucTaiKhoan(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            ApplyStyles();

            this.Load += (s, e) => LoadData();
            this.Resize += UcTaiKhoan_Resize;
            txtDiaChi.TextChanged += CheckInputChange;
            txtSDT.TextChanged += CheckInputChange;
        }

        private void ApplyStyles()
        {
            this.Paint += (s, e) => UIHelper.SetRoundedCorner(pnlContainer, 20);
            UIHelper.StylePrimaryButton(btnCapNhat, "LƯU", Color.Gray);
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

            currentStaffDetail = bll.GetStaffDetail(currentUser.MaNguoiDung);

            if (currentStaffDetail != null)
            {
                if (string.IsNullOrEmpty(currentStaffDetail.MaNV))
                {
                    currentStaffDetail.MaNV = currentUser.MaNguoiDung;
                }

                txtTen.Text = currentStaffDetail.Ten;
                txtSDT.Text = currentStaffDetail.SDT;
                txtEmail.Text = currentStaffDetail.Email;
                txtDiaChi.Text = currentStaffDetail.DiaChi;
                originalAddress = currentStaffDetail.DiaChi;
                originalSDT = currentStaffDetail.SDT;
                cboVaiTro.Items.Clear();
                cboVaiTro.Items.Add(currentStaffDetail.VaiTro);
                cboVaiTro.SelectedIndex = 0;
                btnCapNhat.Enabled = false;
                btnCapNhat.BackColor = Color.Gray;
            }
            else
            {
                MessageBox.Show($"Không tìm thấy thông tin chi tiết cho User ID: {currentUser.MaNguoiDung}. \nVui lòng kiểm tra lại CSDL.", "Lỗi dữ liệu");
            }
        }

        private void CheckInputChange(object sender, EventArgs e)
        {
            string curAddr = txtDiaChi.Text.Trim();
            string curSDT = txtSDT.Text.Trim();
            bool isChanged = (txtDiaChi.Text.Trim() != originalAddress) ||
                             (txtSDT.Text.Trim() != originalSDT);

            if (isChanged)
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

        private void lblDoiMatKhau_Click(object sender, EventArgs e)
        {
            frmDoiMK f = new frmDoiMK(currentUser.TenDN);
            UIHelper.SetupDialog(f, "Đổi mật khẩu");

            if (f.ShowDialog() == DialogResult.OK)
            {
                currentUser.MatKhau = f.NewPassword;
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (currentStaffDetail == null) return;
            currentStaffDetail.Ten = txtTen.Text;
            currentStaffDetail.Email = txtEmail.Text;
            currentStaffDetail.SDT = txtSDT.Text.Trim();
            currentStaffDetail.DiaChi = txtDiaChi.Text.Trim();
            string result = bll.UpdateStaff(currentStaffDetail);
            MessageBox.Show(result);

            if (result.Contains("thành công") || result.Contains("OK"))
            {
                originalAddress = currentStaffDetail.DiaChi;
                originalSDT = currentStaffDetail.SDT;
                btnCapNhat.BackColor = Color.Gray;
                btnCapNhat.Enabled = false;
            }
        }
    }
}