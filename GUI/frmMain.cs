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
                // Gọi sự kiện click để active tab Bàn
                btnBan_Click(btnBan, null);
            }
        }

        // ==================================================================================
        // SETUP GIAO DIỆN & STYLE
        // ==================================================================================
        private void SetupFigmaStyles()
        {
            // Cấp 1: Nút chính
            UIHelper.StyleSidebarButton(btnBan, Properties.Resources.Ban, "Bàn");
            UIHelper.StyleSidebarButton(btnKho, Properties.Resources.Kho, "Kho"); //
            UIHelper.StyleSidebarButton(btnQuanLyParent, Properties.Resources.QuanLy, "Quản lý"); //

            // Cấp 2: Nút con (Menu Dropdown)
            UIHelper.StyleSubMenuButton(btnKhuyenMai, null, "Khuyến mãi");
            UIHelper.StyleSubMenuButton(btnThucDon, null, "Thực đơn");
            UIHelper.StyleSubMenuButton(btnDoanhThu, null, "Doanh thu");
            UIHelper.StyleSubMenuButton(btnNV, null, "Nhân viên");

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
                    }
                    else // Nút thường
                    {
                        UIHelper.SetActiveButton(currentBtn);
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
                UIHelper.SetInactiveButton(currentBtn);
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

        private void btnBan_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucBan(currentUser)); // Truyền user nếu cần phân quyền sâu hơn
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            // SwitchView(new ucKhuyenMai()); // Chưa có UC này
            MessageBox.Show("Chức năng Khuyến mãi đang phát triển", "Thông báo");
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            SwitchView(new ucKho(currentUser));
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

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // [QUAN TRỌNG] Gán DialogResult = OK để báo hiệu cho frmLogin biết là Logout
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // ==================================================================================
        // PHÂN QUYỀN (LOGIC CŨ)
        // ==================================================================================
        private void ApplyAuthorization()
        {
            string role = currentUser.VaiTro; // "Quản lý", "Thu ngân", "Phục vụ"

            // --- BƯỚC 1: RESET TRẠNG THÁI (Ẩn tất cả trước khi bật lại) ---
            // 1. Sidebar chính
            btnBan.Visible = true;          // Ai cũng cần vào Bàn
            btnKho.Visible = false;
            btnQuanLyParent.Visible = false; // Nút cha của nhóm quản lý

            // 2. Menu con (trong pnlSubQuanLy) - Mặc định ẩn hết
            btnKhuyenMai.Visible = false;
            btnThucDon.Visible = false;
            btnDoanhThu.Visible = false;
            btnNV.Visible = false;

            // --- BƯỚC 2: BẬT CHỨC NĂNG THEO VAI TRÒ ---
            if (role == "Quản lý")
            {
                // Full quyền
                btnKho.Visible = true;
                btnQuanLyParent.Visible = true;

                // Hiện tất cả menu con
                btnKhuyenMai.Visible = true;
                btnThucDon.Visible = true;
                btnDoanhThu.Visible = true;
                btnNV.Visible = true;
            }
            else if (role == "Thu ngân")
            {
                // Yêu cầu: Thanh toán, Lịch đặt, Trạng thái bàn, Kho, Xem Khuyến mãi
                // Logic: Ẩn Thực đơn, Doanh thu, Nhân sự
                btnKho.Visible = true;
                btnQuanLyParent.Visible = true;

                btnKhuyenMai.Visible = true;     // Chỉ hiện Khuyến mãi
                                                 // Các nút btnThucDon, btnDoanhThu, btnNV vẫn False (Ẩn)
            }
            else if (role == "Phục vụ")
            {
                // Yêu cầu: Gọi món, Truy cập kho
                // Logic: Ẩn hoàn toàn nhóm Quản lý
                btnKho.Visible = true;
                btnQuanLyParent.Visible = false;
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

        private void btnQuanLyParent_Click(object sender, EventArgs e)
        {
            pnlSubQuanLy.Visible = !pnlSubQuanLy.Visible;

            // Đổi style nút cha để biết nó đang mở hay đóng
            if (pnlSubQuanLy.Visible)
                UIHelper.SetActiveButton(btnQuanLyParent);
            else
                UIHelper.SetInactiveButton(btnQuanLyParent);
        }
    }
}