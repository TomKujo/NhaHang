using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMain : Form
    {
        private NguoiDungDTO currentUser;
        private Button currentBtn;

        public frmMain(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            SetupFigmaStyles();

            ApplyAuthorization();

            if (btnBan.Visible)
            {
                btnBan_Click(btnBan, null);
            }
        }

        // ==================================================================================
        // SETUP GIAO DIỆN & STYLE
        // ==================================================================================
        private void SetupFigmaStyles()
        {
            UIHelper.StyleSidebarButton(btnBan, Properties.Resources.Ban, "Bàn");
            UIHelper.StyleSidebarButton(btnKho, Properties.Resources.Kho, "Kho");
            UIHelper.StyleSidebarButton(btnQuanLyParent, Properties.Resources.QuanLy, "Quản lý");
            UIHelper.StyleSidebarButton(btnTaiKhoan, Properties.Resources.QuanLy, "Tài khoản");
            UIHelper.StyleSubMenuButton(btnKhuyenMai, null, "Khuyến mãi");
            UIHelper.StyleSubMenuButton(btnThucDon, null, "Thực đơn");
            UIHelper.StyleSubMenuButton(btnDoanhThu, null, "Doanh thu");
            UIHelper.StyleSubMenuButton(btnNV, null, "Nhân sự");
            UIHelper.StyleSubMenuButton(btnKhachHang, null, "Khách hàng");

            if (picLogo.Image != null)
                picLogo.Location = new Point((pnlSidebar.Width - picLogo.Width) / 2, 20);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentBtn != (Button)btnSender)
                {
                    DisableButton();
                    currentBtn = (Button)btnSender;

                    if (currentBtn.Parent == pnlSubQuanLy)
                    {
                        currentBtn.ForeColor = UIHelper.PrimaryColor;
                        currentBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                        UIHelper.SetActiveButton(btnQuanLyParent);
                        pnlSubQuanLy.Visible = true;
                    }
                    else
                    {
                        UIHelper.SetActiveButton(currentBtn);

                        if (currentBtn != btnQuanLyParent && currentBtn.Parent != pnlSubQuanLy)
                        {
                            pnlSubQuanLy.Visible = false;
                            UIHelper.SetInactiveButton(btnQuanLyParent);
                        }
                    }
                    lblTitle.Text = currentBtn.Text.Trim();
                }
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                if (currentBtn.Parent == pnlSubQuanLy)
                {
                    currentBtn.ForeColor = Color.Silver;
                    currentBtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    currentBtn.BackColor = Color.FromArgb(40, 40, 40);
                }
                else
                {
                    UIHelper.SetInactiveButton(currentBtn);
                }
            }
        }

        private void SwitchView(UserControl uc)
        {
            if (uc == null) return;

            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        // ==================================================================================
        // EVENTS CLICK (NAVIGATION)
        // ==================================================================================
        private void btnBan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucBan(currentUser));
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucKho(currentUser));
        }

        private void btnQuanLyParent_Click(object sender, EventArgs e)
        {
            pnlSubQuanLy.Visible = !pnlSubQuanLy.Visible;

            if (pnlSubQuanLy.Visible)
                UIHelper.SetActiveButton(btnQuanLyParent);
            else
            {
                if (currentBtn == null || currentBtn.Parent != pnlSubQuanLy)
                    UIHelper.SetInactiveButton(btnQuanLyParent);
            }
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucKhuyenMai(currentUser));
        }

        private void btnThucDon_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucThucDon());
        }

        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucDoanhThu());
        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucNhanSu());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucKhachHang());
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucTaiKhoan(currentUser));
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // ==================================================================================
        // PHÂN QUYỀN
        // ==================================================================================
        private void ApplyAuthorization()
        {
            string role = currentUser.VaiTro;

            btnQuanLyParent.Visible = false;
            pnlSubQuanLy.Visible = false;

            btnKhuyenMai.Visible = false;
            btnThucDon.Visible = false;
            btnDoanhThu.Visible = false;
            btnNV.Visible = false;
            btnKhachHang.Visible = false;

                if (role == "Quản lý")
            {
                btnQuanLyParent.Visible = true;
                btnKhuyenMai.Visible = true;
                btnThucDon.Visible = true;
                btnDoanhThu.Visible = true;
                btnNV.Visible = true;
                btnKhachHang.Visible = true;
            }
            else if (role == "Thu ngân")
            {
                btnQuanLyParent.Visible = true;
                btnKhuyenMai.Visible = true;
                btnKhachHang.Visible = true;
            }
            else if (role == "Phục vụ")
            {
            }
            else
            {
                DisableAllFeatures();
            }
        }

        private void DisableAllFeatures()
        {
            pnlSidebar.Visible = false;
            MessageBox.Show("Tài khoản không có quyền truy cập hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}