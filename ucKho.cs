using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public class CartItemViewModel
    {
        public string MaNguyenLieu { get; set; }
        public string Ten { get; set; }

        public decimal LuongYeuCau { get; set; }
        public decimal LuongThucTe { get; set; }

        public decimal DonGia { get; set; }
        public string TinhTrang { get; set; }

        public decimal ThanhTien => LuongThucTe * DonGia;
    }

    public partial class ucKho : UserControl
    {
        ServiceBLL bll = new ServiceBLL();
        NguoiDungDTO currentUser;
        List<CartItemViewModel> gioHangNhap = new List<CartItemViewModel>();

        public ucKho(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            StyleControls();
            BindEvents();

            // Load mặc định
            LoadDataKho();
            LoadDataNCC();
        }

        private Button btnXoaDongGH; // Khai báo biến cấp class

        private void StyleControls()
        {
            UIHelper.StyleDataGridView(dgvKho);
            UIHelper.StyleDataGridView(dgvKhoSelection);
            UIHelper.StyleDataGridView(dgvGioHang);
            UIHelper.StyleDataGridView(dgvNCC);
            UIHelper.StyleDataGridView(dgvPhieuNhap);
            UIHelper.StyleDataGridView(dgvChiTietPhieu);

            UIHelper.StyleButton(btnThemNL, true);
            UIHelper.StyleButton(btnSuaNL, false);
            UIHelper.StyleButton(btnXoaNL, false);

            UIHelper.StyleButton(btnXacNhanNhap, true);

            // [MỚI]: Tạo nút Xóa bằng code & thêm vào Panel
            btnXoaDongGH = UIHelper.CreateButton("Xóa Dòng", 100, Color.IndianRed);
            pnlActionNhap.Controls.Add(btnXoaDongGH);

            // [QUAN TRỌNG]: Đặt vị trí nút Xóa nằm cạnh nút Lưu (Lưu đang ở 6, 56)
            btnXoaDongGH.Location = new Point(225, 67);
            UIHelper.StyleButton(btnThemNCC, true);
            UIHelper.StyleButton(btnSuaNCC, false);
            UIHelper.StyleButton(btnXoaNCC, false);

            UIHelper.StyleButton(btnXoaPhieu, false);
        }

        private void BindEvents()
        {
            // Tab 1: Kho
            btnThemNL.Click += BtnThemNL_Click;
            btnSuaNL.Click += BtnSuaNL_Click;
            btnXoaNL.Click += BtnXoaNL_Click;

            // Tab 2: Nhập hàng
            dgvKhoSelection.CellDoubleClick += DgvKhoSelection_CellDoubleClick;
            btnXacNhanNhap.Click += BtnXacNhanNhap_Click;
            dgvGioHang.CellDoubleClick += DgvGioHang_CellDoubleClick;
            btnXoaDongGH.Click += BtnXoaDongGH_Click;

            // Tab 3: Phiếu nhập
            dgvPhieuNhap.CellClick += DgvPhieuNhap_CellClick;
            btnXoaPhieu.Click += BtnXoaPhieu_Click;

            // Tab 4: NCC
            btnThemNCC.Click += (s, e) => ShowFormNCC(null);
            btnSuaNCC.Click += BtnSuaNCC_Click;
            btnXoaNCC.Click += BtnXoaNCC_Click;
        }

        // =========================================================
        // LOGIC CHUNG & TAB LOAD
        // =========================================================
        private void TabCtrlKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlKho.SelectedTab == tabTonKho) LoadDataKho();
            else if (tabCtrlKho.SelectedTab == tabNhapHang)
            {
                LoadDataKhoSelection();
                LoadDataNCC();
            }
            else if (tabCtrlKho.SelectedTab == tabPhieuNhap) LoadDataPhieuNhap();
            else if (tabCtrlKho.SelectedTab == tabNCC) LoadDataNCC();
        }

        private void LoadDataKho()
        {
            dgvKho.DataSource = bll.GetInventory();
            UIHelper.SetGridColumns(dgvKho,
                new[] { "Mã NL", "Tên Nguyên Liệu", "Tồn Kho", "Đơn Vị" },
                new[] { "MaNguyenLieu", "Ten", "SoLuongTon", "DonVi" },
                new[] { 100, 300, 150, 100 });
        }

        // =========================================================
        // 1. MODULE NGUYÊN LIỆU (THÊM/SỬA/XÓA)
        // =========================================================
        private void BtnThemNL_Click(object sender, EventArgs e)
        {
            ShowFormNL(null);
        }

        private void BtnSuaNL_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow == null) return;
            string maNL = dgvKho.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            ShowFormNL(maNL);
        }

        private void ShowFormNL(string maNL)
        {
            // 1. Xác định chế độ (Thêm hay Sửa)
            bool isEdit = !string.IsNullOrEmpty(maNL);

            // 2. Khởi tạo Form dialog
            // Kích thước 350x250 là đủ gọn cho 2 trường nhập liệu và 1 nút bấm
            Form f = new Form { Size = new Size(350, 250) };
            UIHelper.SetupDialog(f, isEdit ? "SỬA NGUYÊN LIỆU" : "THÊM NGUYÊN LIỆU");

            // 3. Định nghĩa các hằng số toạ độ (Layout thủ công)
            int leftLbl = 20;   // Lề trái của Label
            int leftTxt = 120;  // Lề trái của TextBox/ComboBox
            int topStart = 30;  // Vị trí Y bắt đầu của dòng đầu tiên
            int gap = 45;       // Khoảng cách dọc giữa các dòng (Row Height + Margin)
            int txtWidth = 180; // Chiều rộng của ô nhập liệu

            // --- Row 1: Tên Nguyên Liệu ---
            Label lblTen = new Label
            {
                Text = "Tên NL:",
                Location = new Point(leftLbl, topStart + 5), // +5 để text của Label ngang hàng với TextBox
                AutoSize = true
            };

            TextBox txtTen = new TextBox
            {
                Location = new Point(leftTxt, topStart),
                Width = txtWidth
            };

            // --- Row 2: Đơn vị tính ---
            Label lblDV = new Label
            {
                Text = "Đơn vị tính:",
                Location = new Point(leftLbl, topStart + gap + 5),
                AutoSize = true
            };

            ComboBox cboDV = new ComboBox
            {
                Location = new Point(leftTxt, topStart + gap),
                Width = txtWidth,
                DropDownStyle = ComboBoxStyle.DropDownList // Chỉ cho chọn, không cho gõ linh tinh
            };
            // Thêm danh sách đơn vị tính phổ biến
            cboDV.Items.AddRange(new object[] { "kg", "l", "hộp", "chai", "gói", "quả", "lon" });
            cboDV.SelectedIndex = 0; // Mặc định chọn cái đầu tiên

            // --- Row 3: Button Lưu ---
            Button btnSave = UIHelper.CreateButton("LƯU", 100, Color.SeaGreen);

            // Tính toán vị trí để nút nằm giữa Form
            // Công thức: (Width Form - Width Button) / 2
            // Trừ thêm khoảng 10px để bù trừ border của Window
            int btnLeft = (350 - 100) / 2 - 10;
            int btnTop = topStart + gap * 2 + 10; // Nằm dưới dòng 2 một khoảng gap

            btnSave.Location = new Point(btnLeft, btnTop);

            // 4. Load dữ liệu cũ vào Form nếu đang ở chế độ Sửa
            if (isEdit)
            {
                var row = dgvKho.CurrentRow;
                if (row != null)
                {
                    txtTen.Text = row.Cells["Ten"].Value.ToString();
                    cboDV.Text = row.Cells["DonVi"].Value.ToString();
                }
            }

            // 5. Xử lý sự kiện Click nút Lưu
            btnSave.Click += (s, e) =>
            {
                // Validate dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nguyên liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTen.Focus();
                    return;
                }

                string msg;
                if (isEdit)
                {
                    // Gọi BLL để cập nhật
                    msg = bll.UpdateNL(maNL, txtTen.Text, cboDV.Text);
                }
                else
                {
                    // Gọi BLL để thêm mới
                    msg = bll.AddNL(txtTen.Text, cboDV.Text);
                }

                // Kiểm tra kết quả trả về
                if (msg.Contains("OK") || msg.Contains("thành công"))
                {
                    MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.Close();     // Đóng form popup
                    LoadDataKho(); // Load lại dữ liệu trên grid chính
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // 6. Thêm tất cả control vào Form và hiển thị
            f.Controls.AddRange(new Control[] { lblTen, txtTen, lblDV, cboDV, btnSave });

            // Hiển thị Form dưới dạng Dialog (chặn tương tác form chính khi chưa đóng form này)
            f.ShowDialog();
        }

        private void BtnXoaNL_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow != null)
            {
                string ma = dgvKho.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
                if (MessageBox.Show("Xóa nguyên liệu này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteIngredient(ma));
                    LoadDataKho();
                }
            }
        }

        // =========================================================
        // 2. MODULE NHẬP HÀNG
        // =========================================================
        private void LoadDataKhoSelection()
        {
            dgvKhoSelection.DataSource = bll.GetInventory();
            UIHelper.SetGridColumns(dgvKhoSelection,
                new[] { "Mã NL", "Tên" },
                new[] { "MaNguyenLieu", "Ten" },
                new[] { 80, 200 });
        }

        private void DgvKhoSelection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string ma = dgvKhoSelection.Rows[e.RowIndex].Cells["MaNguyenLieu"].Value.ToString();
            string ten = dgvKhoSelection.Rows[e.RowIndex].Cells["Ten"].Value.ToString();

            ShowImportDialog(ma, ten);
        }

        private void RefreshGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = gioHangNhap;

            // [SỬA]: Thêm cột MaNguyenLieu (có thể để width = 0 để ẩn nếu muốn, ở đây tôi để hiện để dễ debug)
            UIHelper.SetGridColumns(dgvGioHang,
                new[] { "Mã NL", "Tên NL", "Lượng Yêu Cầu", "Lượng Thực Tế", "Đơn Giá", "Tình Trạng", "Thành Tiền" },
                new[] { "MaNguyenLieu", "Ten", "LuongYeuCau", "LuongThucTe", "DonGia", "TinhTrang", "ThanhTien" },
                new[] { 80, 140, 90, 90, 90, 80, 100 });

            lblTongTienNhap.Text = $"Tổng: {gioHangNhap.Sum(x => x.ThanhTien):N0} VNĐ";
        }

        private void BtnXacNhanNhap_Click(object sender, EventArgs e)
        {
            if (cboNCC_Import.SelectedValue == null) { MessageBox.Show("Chưa chọn NCC!"); return; }
            if (gioHangNhap.Count == 0) { MessageBox.Show("Giỏ hàng trống!"); return; }

            List<ChiTietNhapDTO> listDTO = gioHangNhap.Select(x => new ChiTietNhapDTO
            {
                MaNguyenLieu = x.MaNguyenLieu,
                LuongYeuCau = x.LuongYeuCau,
                LuongThucTe = x.LuongThucTe,
                DonGia = x.DonGia,
                TinhTrang = x.TinhTrang
            }).ToList();

            try
            {
                string msg = bll.ImportStock(
                    cboNCC_Import.SelectedValue.ToString(),
                    currentUser.MaNguoiDung,
                    listDTO,
                    cboNCC_Import.Text
                );

                // Kiểm tra kết quả trả về
                if (msg.Contains("thành công") || msg.Contains("OK"))
                {
                    MessageBox.Show("Nhập hàng thành công!");
                    gioHangNhap.Clear();
                    RefreshGioHang();
                    LoadDataKho();
                }
                else
                {
                    // [QUAN TRỌNG]: Hiện thông báo lỗi cụ thể từ SQL/DAL trả về
                    MessageBox.Show("Hệ thống trả về lỗi:\n" + msg, "Lỗi Lưu Phiếu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi ngoại lệ:\n{ex.Message}", "Lỗi Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // 3. MODULE QUẢN LÝ PHIẾU NHẬP
        // =========================================================
        private void LoadDataPhieuNhap()
        {
            dgvPhieuNhap.DataSource = bll.GetAllImportSlips();
            UIHelper.SetGridColumns(dgvPhieuNhap,
                new[] { "Mã Phiếu", "Ngày Nhập", "NCC", "Nhân Viên", "Tổng Tiền" },
                new[] { "MaPhieu", "ThoiGian", "TenNCC", "MaNV", "TongTien" }, // <--- Các trường mới
                new[] { 80, 120, 150, 80, 100 });
            dgvChiTietPhieu.DataSource = null;
        }

        private void DgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maPhieu = dgvPhieuNhap.Rows[e.RowIndex].Cells["MaPhieu"].Value.ToString();

                // Dùng RepositoryDAL trực tiếp để lấy chi tiết (Do BLL không expose hàm này theo yêu cầu cũ)
                // Lưu ý: DAL trả về các cột: TenNguyenLieu, SoLuong (LuongThucTe), LuongYeuCau, DonGia, TinhTrang
                RepositoryDAL dalTemp = new RepositoryDAL();
                dgvChiTietPhieu.DataSource = dalTemp.GetChiTietPhieuNhap(maPhieu);

                // [CẬP NHẬT GRID]: Hiển thị đầy đủ thông tin chi tiết nhập
                UIHelper.SetGridColumns(dgvChiTietPhieu,
                    new[] { "Tên NL", "Yêu Cầu", "Thực Nhập", "Đơn Giá", "Tình Trạng" },
                    new[] { "TenNguyenLieu", "LuongYeuCau", "SoLuong", "DonGia", "TinhTrang" }, // <--- Thêm TinhTrang
                    new[] { 150, 70, 70, 90, 80 });
            }
        }

        private void BtnXoaPhieu_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.CurrentRow == null) return;
            string maPhieu = dgvPhieuNhap.CurrentRow.Cells["MaPhieu"].Value.ToString();

            if (MessageBox.Show($"Xóa phiếu {maPhieu}? (Kho sẽ KHÔNG tự động trừ lại số lượng)",
                "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string res = bll.DeleteImportSlip(maPhieu);
                MessageBox.Show(res);
                LoadDataPhieuNhap();
                dgvChiTietPhieu.DataSource = null;
            }
        }

        // =========================================================
        // 4. MODULE NHÀ CUNG CẤP
        // =========================================================
        private void LoadDataNCC()
        {
            var dt = bll.GetListNCC();
            dgvNCC.DataSource = dt;
            UIHelper.SetGridColumns(dgvNCC,
                new[] { "Mã NCC", "Tên NCC", "Địa Chỉ", "SĐT" },
                new[] { "MaNCC", "Ten", "DiaChi", "SDT" },
                new[] { 80, 200, 250, 100 });

            cboNCC_Import.DataSource = dt;
            cboNCC_Import.DisplayMember = "Ten";
            cboNCC_Import.ValueMember = "MaNCC";
        }

        private void ShowFormNCC(string maNCC)
        {
            // --- 1. CẤU HÌNH FORM ---
            bool isEdit = !string.IsNullOrEmpty(maNCC);
            // Kích thước 400x320 để chứa đủ 3 trường và nút bấm thoải mái
            Form f = new Form { Size = new Size(400, 320) };
            UIHelper.SetupDialog(f, isEdit ? "SỬA NHÀ CUNG CẤP" : "THÊM NHÀ CUNG CẤP");

            // --- 2. THIẾT LẬP TỌA ĐỘ ---
            int leftLbl = 20;
            int leftTxt = 100;   // Dịch sang trái xíu vì label ngắn
            int topStart = 30;
            int gap = 45;
            int txtWidth = 250;  // TextBox dài hơn để nhập địa chỉ

            // --- 3. KHỞI TẠO CONTROLS ---

            // Dòng 1: Tên NCC
            Label lblTen = new Label
            {
                Text = "Tên NCC:",
                Location = new Point(leftLbl, topStart + 5),
                AutoSize = true
            };
            TextBox txtTen = new TextBox
            {
                Location = new Point(leftTxt, topStart),
                Width = txtWidth
            };

            // Dòng 2: Địa chỉ
            Label lblDC = new Label
            {
                Text = "Địa chỉ:",
                Location = new Point(leftLbl, topStart + gap + 5),
                AutoSize = true
            };
            TextBox txtDC = new TextBox
            {
                Location = new Point(leftTxt, topStart + gap),
                Width = txtWidth
            };

            // Dòng 3: Số điện thoại
            Label lblSDT = new Label
            {
                Text = "SĐT:",
                Location = new Point(leftLbl, topStart + gap * 2 + 5),
                AutoSize = true
            };
            TextBox txtSDT = new TextBox
            {
                Location = new Point(leftTxt, topStart + gap * 2),
                Width = txtWidth
            };

            // Dòng 4: Nút Lưu
            Button btnSave = UIHelper.CreateButton("LƯU", 100, Color.SeaGreen);
            // Căn giữa nút: (400 - 100) / 2 - 10
            int btnX = 140;
            int btnY = topStart + gap * 3 + 10;
            btnSave.Location = new Point(btnX, btnY);

            // --- 4. LOAD DỮ LIỆU (NẾU SỬA) ---
            if (isEdit)
            {
                if (dgvNCC.CurrentRow != null)
                {
                    txtTen.Text = dgvNCC.CurrentRow.Cells["Ten"].Value.ToString();
                    txtDC.Text = dgvNCC.CurrentRow.Cells["DiaChi"].Value.ToString();
                    txtSDT.Text = dgvNCC.CurrentRow.Cells["SDT"].Value.ToString();
                }
            }

            // --- 5. SỰ KIỆN LƯU ---
            btnSave.Click += (s, e) =>
            {
                // Validate cơ bản
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên Nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTen.Focus();
                    return;
                }

                string msg;
                if (isEdit)
                {
                    // Gọi BLL Update
                    msg = bll.UpdateSupplier(maNCC, txtTen.Text, txtDC.Text, txtSDT.Text);
                }
                else
                {
                    // Gọi BLL Add
                    msg = bll.AddSupplier(txtTen.Text, txtDC.Text, txtSDT.Text);
                }

                // Kiểm tra kết quả
                if (msg.Contains("thành công") || msg.Contains("OK"))
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.Close();
                    LoadDataNCC(); // Refresh lại grid NCC
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // --- 6. HIỂN THỊ FORM ---
            f.Controls.AddRange(new Control[] { lblTen, txtTen, lblDC, txtDC, lblSDT, txtSDT, btnSave });
            f.ShowDialog();
        }

        private void BtnSuaNCC_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;
            string ma = dgvNCC.CurrentRow.Cells["MaNCC"].Value.ToString();
            ShowFormNCC(ma);
        }

        private void BtnXoaNCC_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;
            string ma = dgvNCC.CurrentRow.Cells["MaNCC"].Value.ToString();
            if (MessageBox.Show("Xóa NCC này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(bll.DeleteSupplier(ma));
                LoadDataNCC();
            }
        }

        private void ShowImportDialog(string maNL, string tenNL, CartItemViewModel editingItem = null)
        {
            bool isEditMode = (editingItem != null);
            Form f = new Form { Size = new Size(350, 320) };
            UIHelper.SetupDialog(f, isEditMode ? $"Sửa: {tenNL}" : $"Nhập: {tenNL}");

            int leftLbl = 20, leftTxt = 130, top = 30, gap = 45;

            // --- CÁC CONTROL (Giữ nguyên cấu trúc) ---
            Label lblYC = new Label { Text = "Lượng yêu cầu:", Location = new Point(leftLbl, top + 5), AutoSize = true };
            TextBox txtYC = new TextBox { Location = new Point(leftTxt, top), Width = 150, TextAlign = HorizontalAlignment.Right };

            Label lblTT = new Label { Text = "Tình trạng:", Location = new Point(leftLbl, top + gap + 5), AutoSize = true };
            ComboBox cboTT = new ComboBox { Location = new Point(leftTxt, top + gap), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTT.Items.AddRange(new object[] { "Đủ", "Thiếu" });

            Label lblGia = new Label { Text = "Đơn giá:", Location = new Point(leftLbl, top + gap * 2 + 5), AutoSize = true };
            TextBox txtGia = new TextBox { Location = new Point(leftTxt, top + gap * 2), Width = 150, TextAlign = HorizontalAlignment.Right };

            Label lblTT_SL = new Label { Text = "Lượng thực tế:", Location = new Point(leftLbl, top + gap * 3 + 5), AutoSize = true };
            TextBox txtTT_SL = new TextBox { Location = new Point(leftTxt, top + gap * 3), Width = 150, TextAlign = HorizontalAlignment.Right, Enabled = false };

            // --- ĐIỀN DỮ LIỆU MẶC ĐỊNH HOẶC DỮ LIỆU CŨ ---
            if (isEditMode)
            {
                txtYC.Text = editingItem.LuongYeuCau.ToString();
                txtGia.Text = editingItem.DonGia.ToString("N0"); // Format bỏ số thập phân thừa
                cboTT.SelectedItem = editingItem.TinhTrang;
                txtTT_SL.Text = editingItem.LuongThucTe.ToString();
                // Nếu đang là Thiếu thì mở ô nhập
                if (editingItem.TinhTrang == "Thiếu")
                    txtTT_SL.Enabled = true;
            }
            else
            {
                txtYC.Text = "1";
                txtGia.Text = "0";
                cboTT.SelectedIndex = 0; // Mặc định Đủ
                txtTT_SL.Text = "1";
            }

            // --- EVENTS LOGIC TỰ ĐỘNG ---
            txtYC.TextChanged += (s, e) => {
                if (cboTT.SelectedIndex == 0) txtTT_SL.Text = txtYC.Text;
            };

            cboTT.SelectedIndexChanged += (s, e) => {
                if (cboTT.SelectedIndex == 0) // Đủ
                {
                    txtTT_SL.Enabled = false;
                    txtTT_SL.Text = txtYC.Text;
                }
                else // Thiếu
                {
                    txtTT_SL.Enabled = true;
                    txtTT_SL.Focus();
                }
            };

            // --- NÚT XÁC NHẬN ---
            Button btnAdd = UIHelper.CreateButton(isEditMode ? "CẬP NHẬT" : "THÊM VÀO", 120, Color.OrangeRed);
            btnAdd.Location = new Point((350 - 120) / 2 - 10, top + gap * 4 + 10);

            btnAdd.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtYC.Text, out decimal yc) || yc <= 0) { MessageBox.Show("Lượng yêu cầu lỗi!"); return; }
                if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia < 0) { MessageBox.Show("Đơn giá lỗi!"); return; }
                if (!decimal.TryParse(txtTT_SL.Text, out decimal tt) || tt < 0) { MessageBox.Show("Lượng thực tế lỗi!"); return; }

                // [YÊU CẦU 1]: Kiểm tra logic Đủ/Thiếu
                if (cboTT.SelectedIndex == 1 && tt == yc) // 1 = Thiếu
                {
                    MessageBox.Show("Lượng thực tế BẰNG lượng yêu cầu.\nHệ thống sẽ tự động chuyển trạng thái về 'Đủ'.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboTT.SelectedIndex = 0; // Chuyển về Đủ
                    txtTT_SL.Text = txtYC.Text;
                    return; // Dừng lại để người dùng nhìn thấy thay đổi (hoặc bỏ return để tự lưu luôn)
                }

                // [YÊU CẦU 2]: Kiểm tra trùng lặp (Chỉ khi Thêm mới)
                if (!isEditMode)
                {
                    bool isExist = gioHangNhap.Any(x => x.MaNguyenLieu == maNL);
                    if (isExist)
                    {
                        MessageBox.Show($"Nguyên liệu '{tenNL}' đã có trong giỏ hàng!\nVui lòng sửa dòng cũ thay vì thêm mới.", "Trùng lặp");
                        return;
                    }

                    // Thêm mới
                    gioHangNhap.Add(new CartItemViewModel
                    {
                        MaNguyenLieu = maNL,
                        Ten = tenNL,
                        LuongYeuCau = yc,
                        LuongThucTe = tt,
                        DonGia = gia,
                        TinhTrang = cboTT.Text
                    });
                }
                else
                {
                    // Cập nhật (Sửa)
                    editingItem.LuongYeuCau = yc;
                    editingItem.LuongThucTe = tt;
                    editingItem.DonGia = gia;
                    editingItem.TinhTrang = cboTT.Text;
                }

                RefreshGioHang();
                f.Close();
            };

            f.Controls.AddRange(new Control[] { lblYC, txtYC, lblTT, cboTT, lblGia, txtGia, lblTT_SL, txtTT_SL, btnAdd });
            f.Shown += (s, e) => txtYC.Focus();
            f.ShowDialog();
        }

        private void BtnXoaDongGH_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                return;
            }

            string maNL = dgvGioHang.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            var item = gioHangNhap.FirstOrDefault(x => x.MaNguyenLieu == maNL);

            if (item != null)
            {
                gioHangNhap.Remove(item);
                RefreshGioHang();
            }
        }

        private void DgvGioHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy thông tin dòng đang chọn
            string ma = dgvGioHang.Rows[e.RowIndex].Cells["MaNguyenLieu"].Value.ToString();
            string ten = dgvGioHang.Rows[e.RowIndex].Cells["Ten"].Value.ToString();

            // Tìm item trong list và mở form ở chế độ Sửa
            var item = gioHangNhap.FirstOrDefault(x => x.MaNguyenLieu == ma);
            if (item != null)
            {
                ShowImportDialog(ma, ten, item); // Truyền item vào để sửa
            }
        }
    }
}