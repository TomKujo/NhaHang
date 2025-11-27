using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;
// Cần Reference: Microsoft.VisualBasic (Project -> Add Reference -> search Microsoft.VisualBasic)

namespace GUI
{
    public partial class ucBanHang : UserControl
    {
        // Khởi tạo tầng nghiệp vụ
        ServiceBLL bll = new ServiceBLL();

        // Lưu thông tin người dùng đăng nhập
        NguoiDungDTO currentUser;

        // Lưu mã bàn đang được chọn
        string selectedBanID = null;

        // Timer cập nhật trạng thái bàn tự động
        System.Windows.Forms.Timer timerUpdate;

        // --- CÁC CONTROL GIAO DIỆN ---
        private FlowLayoutPanel flpBan;
        private DataGridView dgvOrder;
        private Label lblTitleBan;
        private Label lblTongTien;

        // Nút bấm (Khai báo toàn cục để phân quyền)
        private Button btnThanhToan;
        private Button btnGoiMon;
        private Button btnDatBan;
        private Button btnHuy;

        // Tìm kiếm
        private TextBox txtTimKiem;

        // Menu chuột phải
        private ContextMenuStrip ctxMenuBan;
        private string contextBanID = null;

        public ucBanHang(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            // Setup Timer (Quét mỗi 1 phút để cập nhật trạng thái đặt bàn)
            timerUpdate = new System.Windows.Forms.Timer();
            timerUpdate.Interval = 60000;
            timerUpdate.Tick += (s, e) => {
                bll.RefreshTableStatus();
                LoadBan(txtTimKiem != null ? txtTimKiem.Text : "");
            };
            timerUpdate.Start();

            // Khởi tạo giao diện
            InitContextMenu();
            SetupManualUI();
            LoadBan();
            ApplyPermission();
        }

        // --- 1. MENU CHUỘT PHẢI (CRUD BÀN) ---
        void InitContextMenu()
        {
            ctxMenuBan = new ContextMenuStrip();

            // Mục 1: Đặt lịch
            ToolStripMenuItem itemDat = new ToolStripMenuItem("Đặt lịch bàn này");
            itemDat.Click += (s, e) => ShowBookingForm(contextBanID);

            // Mục 2: Sửa tên
            ToolStripMenuItem itemSua = new ToolStripMenuItem("Sửa tên/loại");
            itemSua.Click += (s, e) => {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Tên mới:", "Sửa Bàn", "");
                if (!string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show(bll.UpdateTable(contextBanID, newName, "Thường"));
                    LoadBan(txtTimKiem.Text);
                }
            };

            // Mục 3: Xóa bàn
            ToolStripMenuItem itemXoa = new ToolStripMenuItem("Xóa bàn");
            itemXoa.Click += (s, e) => {
                if (MessageBox.Show("Xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteTable(contextBanID));
                    LoadBan(txtTimKiem.Text);
                }
            };

            ctxMenuBan.Items.AddRange(new ToolStripItem[] { itemDat, new ToolStripSeparator(), itemSua, itemXoa });
        }

        // --- 2. VẼ GIAO DIỆN (LAYOUT) ---
        void SetupManualUI()
        {
            this.BackColor = Color.WhiteSmoke;
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // === CỘT TRÁI: DANH SÁCH BÀN ===
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            // Thanh tìm kiếm & Thêm bàn
            Panel pnlSearch = new Panel { Dock = DockStyle.Top, Height = 40 };
            txtTimKiem = new TextBox { Width = 250, Height = 30, Location = new Point(0, 5), Font = new Font("Segoe UI", 10) };
            txtTimKiem.PlaceholderText = "Tìm kiếm bàn...";
            txtTimKiem.TextChanged += (s, e) => LoadBan(txtTimKiem.Text);

            Button btnAddTable = new Button { Text = "+", Width = 40, Height = 30, Location = new Point(260, 4) };
            btnAddTable.Click += (s, e) => {
                string ten = Microsoft.VisualBasic.Interaction.InputBox("Tên bàn mới:", "Thêm Bàn");
                if (!string.IsNullOrEmpty(ten)) { bll.AddTable(ten, "Thường"); LoadBan(); }
            };

            pnlSearch.Controls.Add(txtTimKiem);
            // Chỉ quản lý mới thấy nút thêm bàn
            if (currentUser.VaiTro == "Quản lý") pnlSearch.Controls.Add(btnAddTable);

            flpBan = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };

            pnlLeft.Controls.Add(flpBan);
            pnlLeft.Controls.Add(pnlSearch);

            // === CỘT PHẢI: CHI TIẾT ORDER ===
            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = Color.White };

            lblTitleBan = new Label { Text = "Chọn bàn", Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 14, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };

            dgvOrder = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvOrder);

            // Footer chứa nút bấm
            Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 120 };
            lblTongTien = new Label { Text = "0 VNĐ", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = UIHelper.PrimaryColor };

            btnGoiMon = new Button { Text = "GỌI MÓN", Width = 100, Height = 40, Top = 40, Left = 10 };
            UIHelper.StyleButton(btnGoiMon, false);
            btnGoiMon.Click += BtnGoiMon_Click;

            btnDatBan = new Button { Text = "ĐẶT LỊCH", Width = 100, Height = 40, Top = 40, Left = 120 };
            UIHelper.StyleButton(btnDatBan, false);
            btnDatBan.Click += (s, e) => { if (selectedBanID != null) ShowBookingForm(selectedBanID); };

            btnThanhToan = new Button { Text = "THANH TOÁN", Width = 200, Height = 40, Top = 40, Left = 230 };
            UIHelper.StyleButton(btnThanhToan, true);
            btnThanhToan.Click += BtnThanhToan_Click;
            btnThanhToan.Enabled = false; // Mặc định khóa nút thanh toán

            pnlFooter.Controls.AddRange(new Control[] { lblTongTien, btnGoiMon, btnDatBan, btnThanhToan });

            pnlRight.Controls.Add(dgvOrder);
            pnlRight.Controls.Add(pnlFooter);
            pnlRight.Controls.Add(lblTitleBan);

            layout.Controls.Add(pnlLeft, 0, 0);
            layout.Controls.Add(pnlRight, 1, 0);
            this.Controls.Add(layout);
        }

        // --- 3. LOGIC LOAD DỮ LIỆU ---
        void LoadBan(string keyword = "")
        {
            flpBan.Controls.Clear();
            var listBan = bll.GetListBan();

            foreach (var ban in listBan)
            {
                // Lọc
                if (!string.IsNullOrEmpty(keyword) &&
                    !ban.TenBan.ToLower().Contains(keyword.ToLower()) &&
                    !ban.Loai.ToLower().Contains(keyword.ToLower()))
                    continue;

                Button btn = new Button();
                btn.Size = new Size(100, 100);
                btn.Text = $"{ban.TenBan}\n({ban.Loai})\n{ban.TrangThai}";
                btn.Tag = ban.MaBan;
                btn.Margin = new Padding(10);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;

                // Màu sắc trạng thái
                if (ban.TrangThai == "Có khách")
                    btn.BackColor = Color.OrangeRed;
                else
                    btn.BackColor = Color.SeaGreen; // Trống

                // Click trái: Chọn bàn
                btn.Click += (s, e) => {
                    selectedBanID = ban.MaBan;
                    lblTitleBan.Text = $"BÀN: {ban.TenBan}";
                    LoadOrder(ban.MaBan);

                    // Chỉ cho phép thanh toán nếu bàn đang có khách
                    btnThanhToan.Enabled = (ban.TrangThai == "Có khách");
                    ApplyPermission(); // Áp dụng lại quyền sau khi bật nút
                };

                // Click phải: Mở menu
                btn.MouseDown += (s, e) => {
                    if (e.Button == MouseButtons.Right)
                    {
                        contextBanID = ban.MaBan;
                        ctxMenuBan.Show(btn, e.Location);
                    }
                };

                flpBan.Controls.Add(btn);
            }
        }

        void LoadOrder(string maBan)
        {
            var dt = bll.GetOrderDetails(maBan);
            dgvOrder.DataSource = dt;

            decimal tong = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows) tong += Convert.ToDecimal(row["ThanhTien"]);
                btnThanhToan.Enabled = true;
            }
            else
            {
                btnThanhToan.Enabled = false; // Không có món -> Khóa thanh toán
            }
            lblTongTien.Text = $"{tong:N0} VNĐ";

            // Format cột
            if (dgvOrder.Columns["TenMonAn"] != null) dgvOrder.Columns["TenMonAn"].HeaderText = "Tên món";
            if (dgvOrder.Columns["ThanhTien"] != null) dgvOrder.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }

        // --- 4. CÁC SỰ KIỆN NÚT BẤM ---

        private void BtnGoiMon_Click(object sender, EventArgs e)
        {
            if (selectedBanID == null) return;

            // Kiểm tra Rule 2h trước khi cho phép ngồi vào bàn
            string reason;
            if (!bll.CanSitAtTable(selectedBanID, out reason))
            {
                MessageBox.Show(reason, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi món demo
            string maMon = Microsoft.VisualBasic.Interaction.InputBox("Nhập mã món (VD: MA01):", "Gọi món", "MA01");
            string sl = Microsoft.VisualBasic.Interaction.InputBox("Số lượng:", "Gọi món", "1");

            if (!string.IsNullOrEmpty(maMon) && int.TryParse(sl, out int iSL))
            {
                MessageBox.Show(bll.OrderMon(selectedBanID, maMon, iSL));
                LoadBan(txtTimKiem.Text); // Cập nhật màu bàn
                LoadOrder(selectedBanID);
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (selectedBanID == null) return;

            if (MessageBox.Show("Xác nhận thanh toán?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                decimal tong = decimal.Parse(lblTongTien.Text.Replace(" VNĐ", "").Replace(".", "").Replace(",", ""));

                if (bll.Checkout(selectedBanID, tong))
                {
                    MessageBox.Show("Thanh toán thành công. Bàn đã TRỐNG.");
                    LoadBan(txtTimKiem.Text);
                    LoadOrder(selectedBanID);
                }
            }
        }

        void ShowBookingForm(string maBan)
        {
            string ngayStr = Microsoft.VisualBasic.Interaction.InputBox("Ngày đặt (yyyy-MM-dd):", "Đặt lịch", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            string gioStr = Microsoft.VisualBasic.Interaction.InputBox("Giờ đến (HH:mm):", "Đặt lịch", "18:00");

            if (DateTime.TryParse(ngayStr, out DateTime ngay) && TimeSpan.TryParse(gioStr, out TimeSpan gioDen))
            {
                TimeSpan gioDi = gioDen.Add(TimeSpan.FromHours(2)); // Mặc định dùng 2 tiếng

                // Gọi BLL kiểm tra Rule 1 ngày và Trùng lịch
                string msg = bll.BookTableAdvanced(maBan, "Khách A", "090...", ngay, gioDen, gioDi);
                MessageBox.Show(msg);
            }
            else MessageBox.Show("Định dạng ngày giờ không hợp lệ!");
        }

        // --- 5. PHÂN QUYỀN ---
        void ApplyPermission()
        {
            // Nếu là Phục vụ -> Khóa nút thanh toán (dù bàn có khách)
            if (currentUser.VaiTro == "Phục vụ")
            {
                if (btnThanhToan != null) btnThanhToan.Enabled = false;
            }
        }
    }
}