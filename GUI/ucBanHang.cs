using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

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

        // Các Control giao diện (Khai báo biến toàn cục để dùng trong các hàm)
        private FlowLayoutPanel flpBan;
        private DataGridView dgvOrder;
        private Label lblTitleBan;
        private Label lblTongTien;

        // QUAN TRỌNG: Khai báo Button ở đây để hàm Phân quyền nhìn thấy
        private Button btnThanhToan;
        private Button btnGoiMon;
        private Button btnHuy;

        // 1. Khai báo ContextMenuStrip (Menu chuột phải)
        private ContextMenuStrip ctxMenuBan;
        private string contextBanID = null; // Lưu mã bàn đang được click chuột phải

        public ucBanHang(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            // Khởi tạo Menu chuột phải
            InitContextMenu();

            SetupManualUI();
            LoadBan();
            ApplyPermission();
        }

        // --- HÀM 1: VẼ GIAO DIỆN CHIA CỘT (60% TRÁI - 40% PHẢI) ---
        void SetupManualUI()
        {
            this.BackColor = Color.WhiteSmoke;

            // Layout chính chia 2 cột
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 2;
            // Cột 1 (Bàn): 60%, Cột 2 (Hóa đơn): 40%
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // --- CỘT TRÁI: DANH SÁCH BÀN ---
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            // FlowLayoutPanel chứa các ô bàn
            flpBan = new FlowLayoutPanel();
            flpBan.Dock = DockStyle.Fill;
            flpBan.AutoScroll = true; // Cho phép cuộn nếu nhiều bàn
            flpBan.BackColor = Color.White;

            // Thanh tìm kiếm (Option) - Có thể thêm sau

            pnlLeft.Controls.Add(flpBan);

            // --- CỘT PHẢI: CHI TIẾT HÓA ĐƠN ---
            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = Color.White };

            // 1. Header Mã bàn (Trên cùng)
            lblTitleBan = new Label();
            lblTitleBan.Text = "Vui lòng chọn bàn";
            lblTitleBan.Dock = DockStyle.Top;
            lblTitleBan.Height = 50;
            lblTitleBan.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitleBan.TextAlign = ContentAlignment.MiddleCenter;
            lblTitleBan.ForeColor = UIHelper.TextColor;

            // 2. Footer Action (Dưới cùng: Nút bấm + Tổng tiền)
            Panel pnlAction = new Panel { Dock = DockStyle.Bottom, Height = 100 };

            lblTongTien = new Label();
            lblTongTien.Text = "Tổng tiền: 0 VNĐ";
            lblTongTien.Dock = DockStyle.Top;
            lblTongTien.Height = 30;
            lblTongTien.TextAlign = ContentAlignment.MiddleRight;
            lblTongTien.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTongTien.ForeColor = UIHelper.PrimaryColor;

            // Khu vực chứa nút bấm
            Panel pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 50 };

            Button btnHuy = new Button { Text = "Hủy", Dock = DockStyle.Left, Width = 80 };
            UIHelper.StyleButton(btnHuy, false); // Style màu trắng

            Button btnThanhToan = new Button { Text = "Thanh toán", Dock = DockStyle.Right, Width = 120 };
            UIHelper.StyleButton(btnThanhToan, true); // Style màu cam
            btnThanhToan.Click += BtnThanhToan_Click;

            Button btnGoiMon = new Button { Text = "Gọi món", Dock = DockStyle.Fill }; // Nút giữa
            UIHelper.StyleButton(btnGoiMon, false);
            btnGoiMon.Margin = new Padding(5, 0, 5, 0); // Cách lề
            btnGoiMon.Click += BtnGoiMon_Click;

            pnlButtons.Controls.Add(btnGoiMon); // Add nút giữa trước nếu dùng Dock Fill
            pnlButtons.Controls.Add(btnThanhToan);
            pnlButtons.Controls.Add(btnHuy);

            pnlAction.Controls.Add(lblTongTien);
            pnlAction.Controls.Add(pnlButtons);

            // 3. Grid Order (Ở giữa)
            dgvOrder = new DataGridView();
            dgvOrder.Dock = DockStyle.Fill;
            UIHelper.StyleDataGridView(dgvOrder); // Áp dụng Style đẹp

            // Ráp vào cột phải
            pnlRight.Controls.Add(dgvOrder); // Add Grid trước (Fill)
            pnlRight.Controls.Add(pnlAction); // Add Footer (Bottom)
            pnlRight.Controls.Add(lblTitleBan); // Add Header (Top)

            // Ráp 2 cột vào Layout chính
            layout.Controls.Add(pnlLeft, 0, 0);
            layout.Controls.Add(pnlRight, 1, 0);

            this.Controls.Add(layout);
        }

        // --- HÀM 2: LOAD DANH SÁCH BÀN TỪ CSDL ---
        void LoadBan()
        {
            flpBan.Controls.Clear();
            var listBan = bll.GetListBan();

            foreach (var ban in listBan)
            {
                Button btn = new Button();
                btn.Size = new Size(110, 110);

                // Hiển thị thông tin chi tiết hơn
                btn.Text = $"{ban.TenBan}\n({ban.Loai})\n{ban.TrangThai}";

                btn.Tag = ban.MaBan;
                btn.Margin = new Padding(10);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.ForeColor = Color.White;
                btn.Cursor = Cursors.Hand;

                switch (ban.TrangThai)
                {
                    case "Có khách":
                        btn.BackColor = UIHelper.PrimaryColor; // Cam
                        break;
                    case "Đặt trước": // Cần thêm trạng thái này vào Enum DB nếu muốn
                        btn.BackColor = Color.RoyalBlue;
                        break;
                    default:
                        btn.BackColor = Color.DarkGray; // Trống
                        break;
                }

                // Sự kiện Click trái: Chọn bàn
                btn.Click += (s, e) => {
                    selectedBanID = ban.MaBan;
                    lblTitleBan.Text = $"BÀN: {ban.TenBan} - {ban.TrangThai}";
                    LoadOrder(ban.MaBan);
                };

                // Sự kiện Chuột phải: Mở menu
                btn.MouseDown += (s, e) => {
                    if (e.Button == MouseButtons.Right)
                    {
                        contextBanID = ban.MaBan; // Lưu lại ID bàn đang thao tác
                        ctxMenuBan.Show(btn, e.Location); // Hiện menu tại vị trí chuột
                    }
                };

                flpBan.Controls.Add(btn);
            }

            // Thêm một nút đặc biệt "THÊM BÀN MỚI" ở cuối danh sách
            // Chỉ hiện nếu là Quản lý
            if (currentUser.VaiTro == "Quản lý")
            {
                Button btnAdd = new Button();
                btnAdd.Size = new Size(110, 110);
                btnAdd.Text = "+ THÊM BÀN";
                btnAdd.BackColor = Color.White;
                btnAdd.ForeColor = Color.Gray;
                btnAdd.FlatStyle = FlatStyle.Flat;
                btnAdd.FlatAppearance.BorderColor = Color.Silver;
                btnAdd.Click += (s, e) => {
                    string ten = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên bàn mới:", "Thêm bàn", "Bàn New");
                    if (!string.IsNullOrEmpty(ten))
                    {
                        MessageBox.Show(bll.AddTable(ten, "Thường"));
                        LoadBan();
                    }
                };
                flpBan.Controls.Add(btnAdd);
            }
        }

        // --- HÀM 3: LOAD CHI TIẾT GỌI MÓN (Order) ---
        void LoadOrder(string maBan)
        {
            var dt = bll.GetOrderDetails(maBan);
            dgvOrder.DataSource = dt;

            // Tính tổng tiền
            decimal tong = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    tong += Convert.ToDecimal(row["ThanhTien"]);
                }
            }
            lblTongTien.Text = $"Tổng tiền: {tong.ToString("N0")} VNĐ";

            // Đổi tên cột hiển thị cho đẹp
            if (dgvOrder.Columns["TenMonAn"] != null) dgvOrder.Columns["TenMonAn"].HeaderText = "Tên món";
            if (dgvOrder.Columns["SoLuong"] != null) dgvOrder.Columns["SoLuong"].HeaderText = "SL";
            if (dgvOrder.Columns["ThanhTien"] != null) dgvOrder.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }

        // --- SỰ KIỆN: GỌI MÓN ---
        private void BtnGoiMon_Click(object sender, EventArgs e)
        {
            if (selectedBanID == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ở đây đơn giản hóa: Dùng InputBox để nhập món demo
            // Thực tế bạn cần hiển thị danh sách thực đơn (ucThucDon) để chọn
            // Đây là code test nhanh chức năng gọi món:
            string maMon = Microsoft.VisualBasic.Interaction.InputBox("Nhập Mã Món (VD: MA-001):", "Gọi món", "MA-001");
            string soLuong = Microsoft.VisualBasic.Interaction.InputBox("Nhập Số lượng:", "Gọi món", "1");

            if (!string.IsNullOrEmpty(maMon) && int.TryParse(soLuong, out int sl))
            {
                string ketQua = bll.OrderMon(selectedBanID, maMon, sl);
                MessageBox.Show(ketQua);

                LoadOrder(selectedBanID); // Load lại list món
                LoadBan(); // Load lại màu bàn (nếu từ Trống -> Có khách)
            }
        }

        // --- SỰ KIỆN: THANH TOÁN ---
        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (selectedBanID == null) return;

            // Lấy tổng tiền từ Label (cắt chuỗi để lấy số)
            string strTien = lblTongTien.Text.Replace("Tổng tiền: ", "").Replace(" VNĐ", "").Replace(".", "").Replace(",", "");
            if (decimal.TryParse(strTien, out decimal tongTien) && tongTien > 0)
            {
                DialogResult dr = MessageBox.Show($"Xác nhận thanh toán bàn {selectedBanID}?\nTổng tiền: {tongTien:N0} VNĐ",
                    "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    bool result = bll.Checkout(selectedBanID, tongTien);
                    if (result)
                    {
                        MessageBox.Show("Thanh toán thành công!");
                        LoadBan(); // Refresh lại bàn về màu xám
                        LoadOrder(selectedBanID); // Xóa grid order
                    }
                    else
                    {
                        MessageBox.Show("Thanh toán thất bại!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Bàn này chưa có món nào hoặc lỗi tính tiền!");
            }
        }

        void AuthorizeActions()
        {
            string role = currentUser.VaiTro;

            // Tìm các nút trong giao diện (đã khai báo biến toàn cục ở bài trước)
            // btnThanhToan, btnGoiMon

            if (role == "Phục vụ")
            {
                // Phục vụ: Chỉ CRUD Order, không thanh toán
                // btnThanhToan (nếu đã tạo biến global)
                // Lưu ý: Cần chuyển biến btnThanhToan thành biến toàn cục (private Button btnThanhToan;)

                // Giả sử bạn đã promote biến btnThanhToan ra ngoài hàm SetupManualUI
                if (btnThanhToan != null) btnThanhToan.Enabled = false;
            }

            if (role == "Thu ngân")
            {
                // Thu ngân: Được thanh toán, được xem bàn
                // Nhưng có thể không được sửa món (tùy nghiệp vụ, ở đây đề bài cho phép Full Bàn)
            }
        }

        // 2. Hàm tạo Menu chuột phải
        void InitContextMenu()
        {
            ctxMenuBan = new ContextMenuStrip();

            ToolStripMenuItem itemSua = new ToolStripMenuItem("Sửa tên bàn");
            itemSua.Click += (s, e) => {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên mới:", "Sửa bàn", "");
                if (!string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show(bll.UpdateTable(contextBanID, newName, "Thường"));
                    LoadBan();
                }
            };

            ToolStripMenuItem itemXoa = new ToolStripMenuItem("Xóa bàn này");
            itemXoa.Click += (s, e) => {
                if (MessageBox.Show("Chắc chắn xóa?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteTable(contextBanID));
                    LoadBan();
                }
            };

            ToolStripMenuItem itemDatBan = new ToolStripMenuItem("Đặt bàn này");
            itemDatBan.Click += (s, e) => {
                // Mở form đặt bàn (hoặc InputBox đơn giản demo)
                string khach = Microsoft.VisualBasic.Interaction.InputBox("Tên khách đặt:", "Booking", "");
                if (!string.IsNullOrEmpty(khach))
                {
                    MessageBox.Show(bll.BookTable(contextBanID, khach, "090...", DateTime.Now.AddHours(2)));
                    // Logic đổi màu bàn sang Xanh Dương cần update trong LoadBan dựa vào bảng DatBan
                }
            };

            ctxMenuBan.Items.Add(itemDatBan);
            ctxMenuBan.Items.Add(new ToolStripSeparator());
            ctxMenuBan.Items.Add(itemSua);
            ctxMenuBan.Items.Add(itemXoa);
        }

        void ApplyPermission()
        {
            string role = currentUser.VaiTro;

            // Yêu cầu: Phục vụ chỉ được truy cập chức năng CRUD order, thu ngân được thanh toán
            if (role == "Phục vụ")
            {
                // Ẩn hoặc Vô hiệu hóa nút Thanh toán
                if (btnThanhToan != null)
                {
                    btnThanhToan.Enabled = false;
                    btnThanhToan.BackColor = Color.Gray;
                    btnThanhToan.Text = "Không có quyền";
                }

                // Nút Gọi món (CRUD Order) vẫn Enabled (Mặc định)
            }

            // Thu ngân và Quản lý: Full quyền trong màn hình này
        }
    }
}