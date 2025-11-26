using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMain : Form
    {
        // Biến lưu thông tin người dùng đang đăng nhập
        private NguoiDungDTO currentUser;

        // Biến lưu nút menu đang được chọn (để đổi màu active)
        private Button currentButton;

        public frmMain(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;
            SetupMenu();

            // Áp dụng phân quyền ngay khi mở form
            ApplyAuthorization();

            // Mở màn hình mặc định hợp lý cho từng vai trò
            if (user.VaiTro == "Phục vụ") SwitchView(new ucBanHang(user)); // Phục vụ vào thẳng Đặt bàn
            else if (user.VaiTro == "Thu ngân") SwitchView(new ucBanHang(user));
            else SwitchView(new ucNhanSu()); // Quản lý vào Nhân sự hoặc Dashboard
        }

        // --- HÀM 1: THIẾT KẾ MENU TỰ ĐỘNG ---
        private void SetupMenu()
        {
            // ... (Phần Logo giữ nguyên) ...

            AddMenuButton("Đăng xuất", 99); // Ai cũng thấy

            // --- LOGIC PHÂN QUYỀN HIỂN THỊ MENU ---

            string role = currentUser.VaiTro;

            // 1. NHÓM QUẢN LÝ (Admin thấy hết)
            if (role == "Quản lý")
            {
                AddLabelCategory("QUẢN LÝ");
                AddMenuButton("   Website", 8);
                AddMenuButton("   Khách hàng", 7);
                AddMenuButton("   Voucher", 6);
                AddMenuButton("   Thực đơn", 5);
                AddMenuButton("   Doanh thu", 4);
                AddMenuButton("   Kho", 3);
                AddMenuButton("   Nhân sự", 2);
            }

            // 2. NHÓM CHỨC NĂNG CHUNG & RIÊNG
            AddLabelCategory("CHỨC NĂNG");

            // Thu Ngân: Bàn, Thanh toán, Nhập hàng, Voucher
            // Phục vụ: Order (trong Bàn), Nhập hàng
            // Quản lý: Tất cả

            bool allowNhapHang = (role == "Quản lý" || role == "Thu ngân" || role == "Phục vụ");
            if (allowNhapHang)
                AddMenuButton("Nhập hàng", 1);

            bool allowBan = (role == "Quản lý" || role == "Thu ngân" || role == "Phục vụ");
            if (allowBan)
                AddMenuButton("Đặt bàn", 0);

            // Nếu là Thu ngân thì được xem thêm Voucher (nhưng không sửa - xử lý trong UC)
            if (role == "Thu ngân")
            {
                AddMenuButton("   Voucher", 6); // Cho phép xem voucher
            }
        }

        // --- HÀM 2: TẠO MỘT NÚT MENU CHUẨN STYLE PHẲNG ---
        private Button AddMenuButton(string text, int tag)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Tag = tag; // Gán thẻ để nhận biết nút nào
            btn.Dock = DockStyle.Top;
            btn.Height = 45;

            // Style giao diện phẳng
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0); // Thụt đầu dòng văn bản
            btn.Cursor = Cursors.Hand;

            // Màu sắc mặc định
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.Black;

            // Gán sự kiện Click
            btn.Click += Menu_Click;

            pnlSidebar.Controls.Add(btn);
            pnlSidebar.Controls.SetChildIndex(btn, 0); // Đẩy nút mới add xuống dưới cùng của nhóm Dock Top
            return btn;
        }

        // --- HÀM 3: TẠO NHÃN DANH MỤC (Vd: QUẢN LÝ, CHỨC NĂNG) ---
        private void AddLabelCategory(string text)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Dock = DockStyle.Top;
            lbl.Height = 30;
            lbl.TextAlign = ContentAlignment.BottomLeft;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lbl.ForeColor = Color.Gray;
            lbl.Padding = new Padding(10, 0, 0, 5);

            pnlSidebar.Controls.Add(lbl);
            pnlSidebar.Controls.SetChildIndex(lbl, 0);
        }

        // --- HÀM 4: XỬ LÝ SỰ KIỆN CLICK MENU ---
        private void Menu_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ActiveButton(btn);

            int tag = (int)btn.Tag;
            switch (tag)
            {
                case 0: // Đặt bàn
                    SwitchView(new ucBanHang(currentUser));
                    break;

                case 1: // Nhập hàng
                        // SwitchView(new ucNhapHang()); // (Chưa làm)
                    break;

                case 2: // Nhân sự
                    SwitchView(new ucNhanSu());
                    break;

                // --- THÊM ĐOẠN NÀY VÀO ---
                case 5: // Quản lý Thực đơn
                    SwitchView(new ucThucDon());
                    break;
                // -------------------------

                case 6: // Voucher (Ví dụ)
                        // SwitchView(new ucVoucher()); 
                    break;

                case 3: // Kho
                    SwitchView(new ucKho(currentUser)); // Truyền currentUser để biết ai nhập hàng
                    break;

                case 99: // Đăng xuất
                    if (MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;

                default:
                    // Clear màn hình nếu chưa có chức năng
                    if (pnlContent.Controls.Count > 0) pnlContent.Controls.Clear();
                    break;
            }
        }

        // --- HÀM 5: HIỆU ỨNG ĐỔI MÀU KHI CHỌN NÚT ---
        private void ActiveButton(Button newBtn)
        {
            if (newBtn == null) return;

            // 1. Trả nút cũ về màu bình thường (nếu có)
            if (currentButton != null)
            {
                currentButton.BackColor = Color.Transparent;
                currentButton.ForeColor = Color.Black;
                currentButton.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }

            // 2. Highlight nút mới
            currentButton = newBtn;
            currentButton.BackColor = Color.FromArgb(255, 224, 178); // Màu cam nhạt (Background)
            currentButton.ForeColor = Color.Chocolate; // Màu chữ cam đậm
            currentButton.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Chữ đậm lên
        }

        // --- HÀM 6: CHUYỂN ĐỔI MÀN HÌNH (UserControl) ---
        // Đây là hàm quan trọng nhất để thay thế TabControl
        private void SwitchView(UserControl uc)
        {
            if (uc == null) return;

            // Xóa hết nội dung đang hiển thị ở pnlContent
            pnlContent.Controls.Clear();

            // Cấu hình UserControl mới để lấp đầy panel
            uc.Dock = DockStyle.Fill;

            // Thêm vào panel
            pnlContent.Controls.Add(uc);
        }

        private void ApplyAuthorization()
        {
            string role = currentUser.VaiTro;

            // Mặc định ẩn hết, sau đó hiện lại theo vai trò (White-list approach)
            // Giả sử các nút Menu của bạn được đặt tên biến là: btnNhanSu, btnKho, btnDoanhThu, btnThucDon...
            // Nếu bạn dùng hàm CreateMenuButton động như bài trước, bạn cần sửa logic hàm SetupMenu
            // để kiểm tra 'role' trước khi Add nút vào Panel.

            // Dưới đây là logic giả định việc Add nút vào Panel:
        }
    }
}