using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMain : Form
    {
        private NguoiDungDTO currentUser;
        private Button currentBtn; // Biến lưu nút đang được chọn để highlight

        public frmMain(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            // 1. Áp dụng Style chuẩn Figma (Gán Icon và cấu hình nút)
            SetupFigmaStyles();

            // 2. Phân quyền (Ẩn/Hiện nút dựa trên vai trò)
            ApplyAuthorization();

            // 3. Mặc định mở tab Bàn khi vào phần mềm
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
            // Cấp 1: Nút chính
            UIHelper.StyleSidebarButton(btnBan, Properties.Resources.Ban, "Đặt bàn");
            UIHelper.StyleSidebarButton(btnKho, Properties.Resources.Kho, "Nhập hàng");
            UIHelper.StyleSidebarButton(btnQuanLyParent, Properties.Resources.QuanLy, "Quản lý");

            // Nút Tài khoản & Đăng xuất
            // Dùng tạm icon QuanLy cho Tài khoản hoặc icon User nếu có
            UIHelper.StyleSidebarButton(btnTaiKhoan, Properties.Resources.QuanLy, "Tài khoản");
            // btnDangXuat có thể để style mặc định hoặc tùy chỉnh thêm trong UIHelper

            // Cấp 2: Nút con (Menu Dropdown)
            UIHelper.StyleSubMenuButton(btnKhuyenMai, null, "Khuyến mãi");
            UIHelper.StyleSubMenuButton(btnThucDon, null, "Thực đơn");
            UIHelper.StyleSubMenuButton(btnDoanhThu, null, "Doanh thu");
            UIHelper.StyleSubMenuButton(btnNV, null, "Nhân sự");

            // Canh giữa Logo
            if (picLogo.Image != null)
                picLogo.Location = new Point((pnlSidebar.Width - picLogo.Width) / 2, 20);
        }

        // Hàm xử lý hiệu ứng khi Click nút (Highlight Active)
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentBtn != (Button)btnSender)
                {
                    DisableButton();
                    currentBtn = (Button)btnSender;

                    // Nếu là nút con (Nằm trong pnlSubQuanLy)
                    if (currentBtn.Parent == pnlSubQuanLy)
                    {
                        currentBtn.ForeColor = UIHelper.PrimaryColor; // Cam
                        currentBtn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

                        // Giữ nút cha sáng đèn
                        UIHelper.SetActiveButton(btnQuanLyParent);
                        pnlSubQuanLy.Visible = true;
                    }
                    else // Nút thường
                    {
                        UIHelper.SetActiveButton(currentBtn);

                        // Nếu chuyển sang nút thường (khác Quản lý), đóng menu con
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

        // Hàm reset nút cũ
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                if (currentBtn.Parent == pnlSubQuanLy)
                {
                    // Reset style nút con
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

        // Hàm chuyển đổi UserControl vào Panel Content
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

        // --- MENU CHÍNH ---
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

            // Đổi style nút cha để biết nó đang mở hay đóng
            if (pnlSubQuanLy.Visible)
                UIHelper.SetActiveButton(btnQuanLyParent);
            else
            {
                // Chỉ tắt highlight nếu nút hiện tại KHÔNG phải là con của nó
                if (currentBtn == null || currentBtn.Parent != pnlSubQuanLy)
                    UIHelper.SetInactiveButton(btnQuanLyParent);
            }
        }

        // --- MENU CON ---
        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            MessageBox.Show("Chức năng Khuyến mãi đang phát triển", "Thông báo");
        }

        private void btnThucDon_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucThucDon());
        }

        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            MessageBox.Show("Chức năng Doanh thu đang phát triển", "Thông báo");
        }

        private void btnNV_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucNhanSu());
        }

        // --- BOTTOM BUTTONS ---
        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucTaiKhoan(currentUser)); // Chuyển sang màn hình tài khoản
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK; // Báo hiệu cho frmLogin biết
                this.Close();
            }
        }

        // ==================================================================================
        // PHÂN QUYỀN
        // ==================================================================================
        private void ApplyAuthorization()
        {
            string role = currentUser.VaiTro; // "Quản lý", "Thu ngân", "Phục vụ"

            // 1. Reset ẩn hết
            btnKho.Visible = false;
            btnQuanLyParent.Visible = false;
            pnlSubQuanLy.Visible = false;

            // Các nút con
            btnKhuyenMai.Visible = false;
            btnThucDon.Visible = false;
            btnDoanhThu.Visible = false;
            btnNV.Visible = false;

            // 2. Bật theo quyền
            if (role == "Quản lý")
            {
                btnKho.Visible = true;
                btnQuanLyParent.Visible = true;

                btnKhuyenMai.Visible = true;
                btnThucDon.Visible = true;
                btnDoanhThu.Visible = true;
                btnNV.Visible = true;
            }
            else if (role == "Thu ngân")
            {
                btnKho.Visible = true;
                btnQuanLyParent.Visible = true;

                btnKhuyenMai.Visible = true; // Thu ngân được xem KM
                // Ẩn: NV, DoanhThu, ThucDon
            }
            else if (role == "Phục vụ")
            {
                btnKho.Visible = true; // Phục vụ có thể cần xem kho? Tùy nghiệp vụ
                // Ẩn hoàn toàn nhóm Quản lý
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