using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucBan : UserControl
    {
        // --- KHAI BÁO BIẾN ---
        ServiceBLL bll = new ServiceBLL();
        NguoiDungDTO currentUser;
        string selectedBanID = null;
        bool isFiltered = false;
        ToolTip tip = new ToolTip();

        // Định dạng văn hóa Việt Nam (Dùng dấu chấm phân cách hàng nghìn, không số thập phân)
        CultureInfo viVN = new CultureInfo("vi-VN");

        // Panel bao bọc để xử lý sự kiện chuột cho TextBox bị Disable
        Panel pnlWrapperMa;

        // Add this field to your ucBan class (at the top with other controls)
        TabPage tabBooking;

        public ucBan(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            // Assign tabBooking from tabRight if it exists
            tabBooking = tabRight.TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Text == "Đặt bàn"); // Mới -> Đúng

            // Cấu hình format số: 120.000 (Bỏ số thập phân)
            viVN.NumberFormat.CurrencyDecimalDigits = 0;
            viVN.NumberFormat.CurrencyGroupSeparator = ".";
            viVN.NumberFormat.NumberGroupSeparator = ".";

            InitEvents();
            LoadBan();
            ApplyPermission();
            SetupDetailTabSecurity();

            // Timer tự động cập nhật trạng thái bàn mỗi 30 giây
            timerUpdate.Interval = 30000;
            timerUpdate.Tick += (s, e) => {
                // Chỉ update danh sách bên trái nếu không đang lọc
                if (!isFiltered) LoadBan(null, null, null, true);

                // Nếu đang chọn bàn, update lại thông tin chi tiết (trừ khi đang nhập liệu)
                if (!string.IsNullOrEmpty(selectedBanID) && tabRight.SelectedTab == tabInfo)
                {
                    // Có thể thêm logic refresh form info nhẹ nhàng tại đây nếu cần
                }
            };
            timerUpdate.Start();
        }

        void InitEvents()
        {
            // Các nút công cụ trái
            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click;

            // Tab Gọi món
            btnGoiMon.Click += BtnGoiMon_Click;
            btnThanhToan.Click += BtnThanhToan_Click;

            // Tab Đặt bàn
            btnBookNew.Click += (s, e) => ShowBookingForm(selectedBanID, false);
            btnEditBooking.Click += BtnEditBooking_Click;
            btnCancelBooking.Click += BtnCancelBooking_Click;

            // Tab Chi tiết
            btnSaveBan.Click += BtnSaveBan_Click;
        }

        // =================================================================================
        // 1. QUẢN LÝ DANH SÁCH BÀN (LEFT PANEL)
        // =================================================================================
        void LoadBan(string type = null, string stt = null, string name = null, bool isAutoUpdate = false)
        {
            // Giữ vị trí cuộn chuột
            int scrollPos = flpBan.VerticalScroll.Value;

            if (!isAutoUpdate) flpBan.Controls.Clear();
            else
            {
                // Nếu auto update, chỉ xóa controls nếu số lượng thay đổi hoặc load lại toàn bộ cho an toàn
                flpBan.Controls.Clear();
            }

            var list = bll.GetListBan();

            // Lọc dữ liệu
            if (!string.IsNullOrEmpty(type) && type != "Tất cả") list = list.Where(x => x.Loai == type).ToList();
            if (!string.IsNullOrEmpty(stt) && stt != "Tất cả") list = list.Where(x => x.TrangThai == stt).ToList();
            if (!string.IsNullOrEmpty(name)) list = list.Where(x => x.Ten.ToLower().Contains(name.ToLower())).ToList();

            foreach (var ban in list)
            {
                Button btn = new Button { Size = new Size(110, 110), Margin = new Padding(10) };
                btn.Text = $"{ban.Ten}\n({ban.Loai})\n{ban.TrangThai}";
                UIHelper.StyleButton(btn, true);
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                // Màu sắc theo trạng thái
                if (ban.TrangThai == "Có khách") btn.BackColor = Color.OrangeRed;
                else if (ban.TrangThai == "Đã cọc") btn.BackColor = Color.Goldenrod;
                else btn.BackColor = Color.SeaGreen;

                // Sự kiện click
                btn.Click += (s, e) => SelectBan(ban);

                flpBan.Controls.Add(btn);
            }

            // Khôi phục vị trí cuộn
            if (isAutoUpdate) flpBan.VerticalScroll.Value = scrollPos;
        }

        void SelectBan(BanDTO ban)
        {
            if (ban == null) return;
            selectedBanID = ban.MaBan;
            pnlRight.Visible = true;
            lblTitleBan.Text = $"BÀN: {ban.Ten.ToUpper()}";

            // Fill dữ liệu
            txtEditMaBan.Text = ban.MaBan;
            txtEditTenBan.Text = ban.Ten;
            cboEditLoaiBan.SelectedItem = ban.Loai;
            cboEditTrangThai.SelectedItem = ban.TrangThai;

            string role = currentUser.VaiTro;

            // --- [MỚI] Hàm hỗ trợ ẩn/hiện Label theo Text ---
            // Vì Label "Tên bàn", "Loại bàn" không có tên biến (l1, l2 là biến cục bộ), 
            // ta phải tìm chúng trong danh sách Controls của cha (tabInfo).
            void SetLabelVisibility(bool isVisible)
            {
                if (txtEditTenBan.Parent != null)
                {
                    foreach (Control c in txtEditTenBan.Parent.Controls)
                    {
                        if (c is Label lbl)
                        {
                            // Tìm Label dựa vào nội dung chữ hiển thị
                            if (lbl.Text == "Tên bàn:" || lbl.Text == "Loại bàn:")
                            {
                                lbl.Visible = isVisible;
                            }
                        }
                    }
                }
            }

            if (role == "Thu ngân")
            {
                // Thu ngân: Ẩn input
                txtEditTenBan.Visible = false;
                cboEditLoaiBan.Visible = false;

                // [MỚI] Ẩn luôn Label đi kèm
                SetLabelVisibility(false);
            }
            else if (role == "Quản lý")
            {
                // Quản lý: Hiện tất cả
                txtEditTenBan.Visible = true;
                cboEditLoaiBan.Visible = true;

                // [MỚI] Đảm bảo Label hiện lại (nếu chuyển từ user khác sang)
                SetLabelVisibility(true);

                bool isOccupied = (ban.TrangThai == "Có khách");
                txtEditTenBan.Enabled = !isOccupied;
                cboEditLoaiBan.Enabled = !isOccupied;
            }

            // Load dữ liệu các Tab khác
            LoadOrderGrid(ban.MaBan);
            LoadBookingGrid(ban.MaBan);
        }

        // =================================================================================
        // 2. FORM GỌI MÓN (POPUP CĂN GIỮA, GHI CHÚ, GỘP DÒNG)
        // =================================================================================
        void BtnGoiMon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID)) return;

            // --- [MỚI] BẮT ĐẦU ĐOẠN LOGIC KIỂM TRA LUẬT ---
            var ban = bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID);
            if (ban == null) return;

            // Lấy danh sách lịch đặt
            DataTable dtBookings = bll.GetBookings(selectedBanID);
            DateTime? nextBookingTime = null;

            // Tìm lịch đặt gần nhất trong tương lai
            foreach (DataRow r in dtBookings.Rows)
            {
                string state = r["TrangThai"].ToString();
                if (state != "Hủy" && state != "Hoàn tất")
                {
                    DateTime t = Convert.ToDateTime(r["ThoiGian"]);
                    if (t > DateTime.Now)
                    {
                        if (nextBookingTime == null || t < nextBookingTime)
                            nextBookingTime = t;
                    }
                }
            }

            if (nextBookingTime != null)
            {
                double minutesLeft = (nextBookingTime.Value - DateTime.Now).TotalMinutes;

                // LUẬT 1: Chặn khách vãng lai nếu có lịch đặt trong vòng 1 giờ
                if (ban.TrangThai == "Trống")
                {
                    if (minutesLeft <= 60)
                    {
                        MessageBox.Show($"CHẶN KHÁCH!\n\nBàn này có lịch đặt lúc: {nextBookingTime.Value:HH:mm}.\nQuy tắc: Không nhận khách vãng lai trước giờ đặt 1 tiếng.",
                            "Cảnh báo quy tắc", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return; // <-- Lệnh này chặn không cho mở form gọi món
                    }
                }

                // LUẬT 2: Thông báo thanh toán trước 15 phút
                if (minutesLeft < 240) // Chỉ nhắc nếu lịch đặt trong vòng 4 tiếng tới
                {
                    DateTime deadline = nextBookingTime.Value.AddMinutes(-15);
                    string notification = $"LƯU Ý QUAN TRỌNG CHO NHÂN VIÊN:\n\n" +
                                          $"Bàn có lịch đặt kế tiếp lúc: {nextBookingTime.Value:HH:mm}\n" +
                                          $"YÊU CẦU: Khách phải thanh toán và trả bàn trước {deadline:HH:mm} (Trước 15 phút).\n\n" +
                                          $"Vui lòng thông báo điều kiện này cho khách hàng.";

                    MessageBox.Show(notification, "Quy định trả bàn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Tạo Form Gọi Món bằng Code
            Form f = new Form { Size = new Size(743, 600), FormBorderStyle = FormBorderStyle.FixedToolWindow };
            UIHelper.SetupDialog(f, "GỌI MÓN");

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180)); // Vùng nhập liệu
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Vùng lưới

            // --- Vùng nhập liệu ---
            Panel pnlInput = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            // Control chọn món
            Label lblMon = new Label { Text = "Món ăn:", Left = 50, Top = 23, AutoSize = true };
            ComboBox cboMon = new ComboBox { Width = 300, DataSource = bll.GetFullMenu(), DisplayMember = "Ten", ValueMember = "MaMon", DropDownStyle = ComboBoxStyle.DropDownList, Left = 120, Top = 20 };

            // Control số lượng
            Label lblSL = new Label { Text = "Số lượng:", Left = 450, Top = 23, AutoSize = true };
            NumericUpDown txtSL = new NumericUpDown { Width = 155, Minimum = 1, Maximum = 100, Value = 1, Left = 520, Top = 20 };

            // Control Ghi chú
            Label lblNote = new Label { Text = "Ghi chú:", Left = 50, Top = 63, AutoSize = true };
            TextBox txtNote = new TextBox { Width = 555, Left = 120, Top = 60 };

            Button btnAddTemp = UIHelper.CreateButton("THÊM VÀO LIST", 200, Color.Teal);
            btnAddTemp.Location = new Point(250, 110);

            pnlInput.Controls.AddRange(new Control[] { lblMon, cboMon, lblSL, txtSL, lblNote, txtNote, btnAddTemp });

            // --- Vùng Lưới Tạm (Căn giữa lưới bằng Padding) ---
            Panel pnlGridWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(50, 0, 50, 20) };
            DataGridView dgvTemp = UIHelper.CreateDataGridView();

            // Tạo DataTable tạm để lưu danh sách gọi trước khi lưu xuống DB
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("MaMon");
            dtTemp.Columns.Add("TenMon");
            dtTemp.Columns.Add("SoLuong", typeof(int));
            dtTemp.Columns.Add("GhiChu");

            dgvTemp.DataSource = dtTemp;
            UIHelper.SetGridColumns(dgvTemp,
                new[] { "Tên Món", "SL", "Ghi Chú" },
                new[] { "TenMon", "SoLuong", "GhiChu" },
                new[] { 300, 80, 200 }
            );

            pnlGridWrapper.Controls.Add(dgvTemp);

            // Logic nút THÊM VÀO LIST
            btnAddTemp.Click += (s, a) => {
                string maMon = cboMon.SelectedValue.ToString();
                string tenMon = cboMon.Text;
                int sl = (int)txtSL.Value;
                string ghiChu = txtNote.Text.Trim();

                bool found = false;
                // Logic gộp dòng: Nếu trùng Món VÀ trùng Ghi chú -> Cộng dồn số lượng
                foreach (DataRow r in dtTemp.Rows)
                {
                    if (r["MaMon"].ToString() == maMon && r["GhiChu"].ToString() == ghiChu)
                    {
                        r["SoLuong"] = (int)r["SoLuong"] + sl;
                        found = true; break;
                    }
                }
                // Nếu chưa có hoặc khác ghi chú -> Thêm dòng mới
                if (!found) dtTemp.Rows.Add(maMon, tenMon, sl, ghiChu);
            };

            // Nút Xác Nhận (Dưới cùng form)
            Button btnConfirm = UIHelper.CreateButton("XÁC NHẬN GỌI MÓN", 200, Color.DodgerBlue);
            btnConfirm.Dock = DockStyle.Bottom;
            btnConfirm.Click += (s, a) => {
                if (dtTemp.Rows.Count == 0) { MessageBox.Show("Danh sách gọi món đang trống!"); return; }
                try
                {
                    foreach (DataRow r in dtTemp.Rows)
                    {
                        // Gọi DAL (DAL đã sửa để Insert dòng riêng biệt)
                        bll.OrderMon(selectedBanID, r["MaMon"].ToString(), r["SoLuong"].ToString(), r["GhiChu"].ToString());
                    }
                    MessageBox.Show("Gọi món thành công!");
                    f.Close(); // Đóng form popup
                }
                catch (Exception ex) { MessageBox.Show("Lỗi hệ thống: " + ex.Message); }
            };

            layout.Controls.Add(pnlInput, 0, 0);
            layout.Controls.Add(pnlGridWrapper, 0, 1);
            f.Controls.Add(layout);
            f.Controls.Add(btnConfirm);
            f.ShowDialog();

            // [SỬA]: Hiển thị form và sau khi đóng form thì Reload lại dữ liệu bàn
            f.ShowDialog();
            ResetAndHideRightPanel();

            if (!string.IsNullOrEmpty(selectedBanID))
            {
                // Reload lại danh sách bàn (để cập nhật trạng thái màu sắc nếu bàn từ Trống -> Có khách)
                LoadBan(null, null, null, true);

                // Reload lại chi tiết bàn đang chọn (để hiện danh sách món vừa gọi lên lưới dgvOrder)
                var selectedBan = bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID);
                SelectBan(selectedBan);
            }
        }

        // =================================================================================
        // 3. TAB DANH SÁCH MÓN & THANH TOÁN (FORMAT TIỀN, GHI CHÚ)
        // =================================================================================
        void LoadOrderGrid(string maBan)
        {
            var dt = bll.GetOrderDetails(maBan);
            dgvOrder.DataSource = dt;

            UIHelper.SetGridColumns(dgvOrder,
                new[] { "Tên món", "SL", "Đơn giá", "Ghi chú", "Thành tiền" },
                new[] { "TenMon", "SoLuong", "Gia", "GhiChu", "ThanhTien" },
                new[] { 200, 60, 120, 150, 150 }
            );

            // [SỬA]: Format tiền tệ dạng số nguyên, dùng dấu phân cách hàng nghìn
            // Format "#,##0" sẽ hiển thị 120.000 (nếu Culture là VN) và không hiện phần thập phân
            if (dgvOrder.Columns["Gia"] != null)
            {
                dgvOrder.Columns["Gia"].DefaultCellStyle.Format = "#,##0";
                dgvOrder.Columns["Gia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvOrder.Columns["ThanhTien"] != null)
            {
                dgvOrder.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0";
                dgvOrder.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            decimal tong = 0;
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows) tong += Convert.ToDecimal(r["ThanhTien"]);
            }
            lblTongTien.Text = tong.ToString("#,##0", viVN) + " VNĐ";
        }

        void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID)) return;
            if (dgvOrder.Rows.Count == 0) { MessageBox.Show("Bàn chưa gọi món!"); return; }

            if (MessageBox.Show("Xác nhận thanh toán và in hóa đơn cho bàn này?", "Thanh Toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Parse ngược chuỗi tiền tệ (bỏ " VNĐ" và dấu chấm) để lấy giá trị Decimal
                string rawMoney = lblTongTien.Text.Replace(" VNĐ", "").Replace(".", "").Trim();
                if (decimal.TryParse(rawMoney, out decimal tongTien))
                {
                    if (bll.Checkout(selectedBanID, tongTien))
                    {
                        MessageBox.Show("Thanh toán thành công! Bàn đã trống.");
                        SelectBan(bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID));
                    }
                    else
                    {
                        MessageBox.Show("Lỗi trong quá trình thanh toán.");
                    }
                }
            }

            ResetAndHideRightPanel();
        }

        // =================================================================================
        // 4. FORM ĐẶT BÀN (CĂN GIỮA, ẨN HIỆN LAYOUT THÔNG MINH)
        // =================================================================================
        // =================================================================================
        // FORM ĐẶT BÀN (CĂN GIỮA HOÀN TOÀN, TXT TÊN = TXT SĐT)
        // =================================================================================
        // =================================================================================
        // FORM ĐẶT BÀN (KHÔNG DÙNG HÀM CON, SỬ DỤNG TABLE LAYOUT)
        // =================================================================================
        // =================================================================================
        // FORM ĐẶT BÀN (CĂN CHỈNH LABEL VÀ TEXTBOX THẲNG HÀNG)
        // =================================================================================
        void ShowBookingForm(string tableID, bool isEdit, string bookingID = null, DateTime? editDate = null, string name = "", string phone = "")
        {
            // 1. Khởi tạo Form
            Form f = new Form { Size = new Size(450, 420), FormBorderStyle = FormBorderStyle.FixedToolWindow, StartPosition = FormStartPosition.CenterParent, BackColor = Color.White };
            UIHelper.SetupDialog(f, isEdit ? "SỬA LỊCH ĐẶT" : "ĐẶT BÀN MỚI");

            // 2. Layout chính
            TableLayoutPanel tblMain = new TableLayoutPanel();
            tblMain.Dock = DockStyle.Fill;
            tblMain.RowCount = 5;
            tblMain.ColumnCount = 1;

            // Chiều cao các dòng
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Ngày
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Giờ
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Tên
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // SĐT
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Nút

            // --- HELPER STYLE ---
            // Hàm này giúp hạ thấp Label xuống 8px để ngang với TextBox
            void StyleLabel(Label lbl)
            {
                lbl.Margin = new Padding(0, 8, 0, 0); // Đẩy xuống 8px
            }

            // --- DÒNG 1: NGÀY THÁNG NĂM ---
            FlowLayoutPanel flpDate = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };

            var lblD = UIHelper.CreateLabel("Ngày"); StyleLabel(lblD);
            var txtD = new NumericUpDown { Width = 45, Minimum = 1, Maximum = 31, TextAlign = HorizontalAlignment.Center };

            var lblM = UIHelper.CreateLabel("Tháng"); StyleLabel(lblM);
            var txtM = new NumericUpDown { Width = 45, Minimum = 1, Maximum = 12, TextAlign = HorizontalAlignment.Center };

            var lblY = UIHelper.CreateLabel("Năm"); StyleLabel(lblY);
            var txtY = new NumericUpDown { Width = 65, Minimum = DateTime.Now.Year, Maximum = DateTime.Now.Year + 2, TextAlign = HorizontalAlignment.Center };

            DateTime init = editDate ?? DateTime.Now.AddMinutes(30);
            txtD.Value = init.Day; txtM.Value = init.Month; txtY.Value = init.Year;

            flpDate.Controls.AddRange(new Control[] { lblD, txtD, lblM, txtM, lblY, txtY });
            flpDate.Anchor = AnchorStyles.None;
            tblMain.Controls.Add(flpDate, 0, 0);

            // --- DÒNG 2: GIỜ PHÚT ---
            FlowLayoutPanel flpTime = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };

            var lblH = UIHelper.CreateLabel("Giờ"); StyleLabel(lblH);
            var txtH = new NumericUpDown { Width = 50, Minimum = 0, Maximum = 23, TextAlign = HorizontalAlignment.Center };

            var lblMi = UIHelper.CreateLabel("Phút"); StyleLabel(lblMi);
            var txtMi = new NumericUpDown { Width = 50, Minimum = 0, Maximum = 59, TextAlign = HorizontalAlignment.Center };

            txtH.Value = init.Hour; txtMi.Value = init.Minute;

            flpTime.Controls.AddRange(new Control[] { lblH, txtH, lblMi, txtMi });
            flpTime.Anchor = AnchorStyles.None;
            tblMain.Controls.Add(flpTime, 0, 1);

            // --- KHAI BÁO TEXTBOX ---
            TextBox txtName = new TextBox { Width = 220, Text = name };
            TextBox txtPhone = new TextBox { Width = 220, Text = phone };

            if (!isEdit)
            {
                // --- DÒNG 3: TÊN KHÁCH ---
                FlowLayoutPanel flpName = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };

                var lblName = UIHelper.CreateLabel("Tên Khách:");
                StyleLabel(lblName);
                // FIX: Đặt độ rộng cố định cho Label Tên để căn lề
                lblName.AutoSize = false;
                lblName.Width = 90;
                lblName.TextAlign = ContentAlignment.MiddleRight; // Căn chữ sang phải sát ô nhập

                flpName.Controls.AddRange(new Control[] { lblName, txtName });
                flpName.Anchor = AnchorStyles.None;
                tblMain.Controls.Add(flpName, 0, 2);

                // --- DÒNG 4: SĐT ---
                FlowLayoutPanel flpPhone = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.Transparent };

                var lblPhone = UIHelper.CreateLabel("SĐT:");
                StyleLabel(lblPhone);
                // FIX: Đặt độ rộng Label SĐT BẰNG Label Tên (90px) -> TextBox sẽ tự động thẳng hàng
                lblPhone.AutoSize = false;
                lblPhone.Width = 90;
                lblPhone.TextAlign = ContentAlignment.MiddleRight;

                flpPhone.Controls.AddRange(new Control[] { lblPhone, txtPhone });
                flpPhone.Anchor = AnchorStyles.None;
                tblMain.Controls.Add(flpPhone, 0, 3);
            }
            else
            {
                // Ẩn dòng nếu là sửa
                tblMain.RowStyles[2].Height = 0;
                tblMain.RowStyles[3].Height = 0;
            }

            // --- DÒNG 5: NÚT LƯU ---
            Button btnSave = UIHelper.CreateButton("LƯU", 150, Color.OrangeRed);
            btnSave.Anchor = AnchorStyles.None;
            tblMain.Controls.Add(btnSave, 0, 4);

            // --- SỰ KIỆN ---
            btnSave.Click += (s, a) => {
                try
                {
                    // Lấy ngày giờ từ input
                    int d = (int)txtD.Value;
                    int m = (int)txtM.Value;
                    int y = (int)txtY.Value;
                    int h = (int)txtH.Value;
                    int mi = (int)txtMi.Value;
                    DateTime fullDate = new DateTime(y, m, d, h, mi, 0);

                    // --- [MỚI] BẮT ĐẦU ĐOẠN LOGIC CHECK LUẬT ĐẶT BÀN ---

                    // Luật 1: Không đặt về quá khứ
                    if (fullDate < DateTime.Now)
                    {
                        MessageBox.Show("LỖI THỜI GIAN:\nKhông thể đặt bàn lùi về quá khứ!",
                            "Quy tắc đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // <-- Chặn lưu
                    }

                    // Luật 2: Đặt trước ít nhất 1 ngày (24 giờ)
                    if (fullDate < DateTime.Now.AddDays(1))
                    {
                        MessageBox.Show("QUY ĐỊNH NHÀ HÀNG:\nPhải đặt bàn trước ít nhất 1 ngày (24 giờ)!",
                            "Quy tắc đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // <-- Chặn lưu
                    }

                    if (isEdit)
                    {
                        string msg = bll.UpdateBookingSmart(bookingID, tableID, fullDate);
                        MessageBox.Show(msg);
                        if (msg.Contains("thành công")) f.Close();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
                        {
                            MessageBox.Show("Vui lòng nhập tên và số điện thoại khách!"); return;
                        }
                        string msg = bll.BookTableSmart(tableID, txtName.Text, txtPhone.Text, fullDate);
                        MessageBox.Show(msg);
                        if (msg.Contains("thành công")) f.Close();
                    }
                    LoadBookingGrid(tableID);
                }
                catch { MessageBox.Show("Định dạng ngày giờ không hợp lệ!"); }
            };

            f.Controls.Add(tblMain);
            f.ShowDialog();
        }

        // =================================================================================
        // 5. XỬ LÝ SỰ KIỆN TAB ĐẶT BÀN (EDIT, CANCEL)
        // =================================================================================
        void LoadBookingGrid(string maBan)
        {
            var dt = bll.GetBookings(maBan);
            dgvBookingList.DataSource = dt;

            // Cấu hình cột hiển thị
            UIHelper.SetGridColumns(dgvBookingList,
                new[] { "ID", "Khách hàng", "SĐT", "Thời gian", "Trạng Thái" },
                new[] { "MaDatBan", "TenKhachHang", "SDT", "ThoiGian", "TrangThai" },
                new[] { 50, 150, 100, 120, 100 }
            );

            // Format ngày giờ
            if (dgvBookingList.Columns["ThoiGian"] != null)
                dgvBookingList.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            // Ẩn cột ID nhưng VẪN GIỮ DỮ LIỆU để xử lý Sửa/Hủy
            if (dgvBookingList.Columns["MaDatBan"] != null)
                dgvBookingList.Columns["MaDatBan"].Visible = false;
        }

        void BtnEditBooking_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn bàn chưa (đề phòng trường hợp nút nằm ngoài vùng ẩn hiện)
            if (string.IsNullOrEmpty(selectedBanID))
            {
                MessageBox.Show("Vui lòng chọn bàn cần thao tác trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra đã chọn dòng lịch đặt trên lưới chưa
            if (dgvBookingList.CurrentRow == null || dgvBookingList.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn một lịch đặt trong danh sách để sửa!", "Chưa chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // [FIX]: Lấy dữ liệu an toàn từ DataRowView
            DataRowView drv = dgvBookingList.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            string status = drv["TrangThai"].ToString();

            // Kiểm tra trạng thái
            if (status == "Hủy" || status == "Hoàn tất")
            {
                MessageBox.Show("Không thể sửa lịch đã Hủy hoặc Hoàn tất.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin
            string id = drv["MaDatBan"].ToString();
            DateTime dt;
            if (!DateTime.TryParse(drv["ThoiGian"].ToString(), out dt)) dt = DateTime.Now;

            // Mở form Edit
            ShowBookingForm(selectedBanID, true, id, dt);
        }

        void BtnCancelBooking_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn bàn chưa
            if (string.IsNullOrEmpty(selectedBanID))
            {
                MessageBox.Show("Vui lòng chọn bàn cần thao tác trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra đã chọn dòng lịch đặt trên lưới chưa
            if (dgvBookingList.CurrentRow == null || dgvBookingList.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn một lịch đặt trong danh sách để hủy!", "Chưa chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // [FIX]: Lấy dữ liệu an toàn từ DataRowView
            DataRowView drv = dgvBookingList.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            string status = drv["TrangThai"].ToString();
            string id = drv["MaDatBan"].ToString();

            if (status == "Hủy")
            {
                MessageBox.Show("Lịch này đã bị hủy trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (status == "Hoàn tất")
            {
                MessageBox.Show("Lịch này đã hoàn tất, không thể hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn hủy lịch đặt này?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string result = bll.CancelBooking(id);
                MessageBox.Show(result);
                LoadBookingGrid(selectedBanID); // Load lại lưới
            }
        }

        // =================================================================================
        // 6. TAB CHI TIẾT & BẢO MẬT (TOOLTIP, DISABLE CURSOR)
        // =================================================================================
        void SetupDetailTabSecurity()
        {
            // TextBox Disable sẽ không nhận sự kiện MouseHover.
            // Giải pháp: Tạo một Panel trong suốt đè lên hoặc bao bọc TextBox để bắt sự kiện chuột.

            pnlWrapperMa = new Panel
            {
                Size = txtEditMaBan.Size,
                Location = txtEditMaBan.Location,
                BackColor = Color.Transparent
            };

            // Thêm Panel vào Parent của textbox
            txtEditMaBan.Parent.Controls.Add(pnlWrapperMa);

            // Đưa Textbox vào trong Panel và Dock Fill
            pnlWrapperMa.Controls.Add(txtEditMaBan);
            txtEditMaBan.Dock = DockStyle.Fill;

            // Disable textbox
            txtEditMaBan.Enabled = false;

            // Xử lý sự kiện chuột trên Panel Wrapper
            pnlWrapperMa.MouseMove += (s, e) => {
                // Đổi con trỏ chuột thành hình tròn gạch chéo (No)
                Cursor.Current = Cursors.No;
                // Hiện Tooltip
                tip.Show("Không thể thay đổi Mã Bàn!", pnlWrapperMa, 10, -25, 1000);
            };

            pnlWrapperMa.MouseLeave += (s, e) => {
                // Trả lại con trỏ chuột mặc định
                Cursor.Current = Cursors.Default;
                tip.Hide(pnlWrapperMa);
            };
        }

        void BtnSaveBan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Lưu thay đổi cấu hình bàn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // 1. Luôn cập nhật trạng thái (Cả Quản lý và Thu ngân đều được quyền)
                bll.UpdateTableState(selectedBanID, cboEditTrangThai.Text);

                // 2. Chỉ cập nhật Tên và Loại nếu là Quản lý
                // (Vì Thu ngân đã bị ẩn textbox, giá trị text có thể không chính xác hoặc không chủ đích sửa)
                if (currentUser.VaiTro == "Quản lý")
                {
                    bll.UpdateTable(selectedBanID, txtEditTenBan.Text, cboEditLoaiBan.Text);
                }

                MessageBox.Show("Đã lưu thành công!");
                ResetAndHideRightPanel();
            }
        }

        // =================================================================================
        // 7. CÁC FORM TÌM KIẾM VÀ THÊM BÀN (POPUP)
        // =================================================================================
        void BtnSearch_Click(object sender, EventArgs e)
        {
            Form f = new Form { Size = new Size(350, 300) };
            UIHelper.SetupDialog(f, "TÌM KIẾM BÀN");

            TabControl tab = new TabControl { Dock = DockStyle.Fill };

            // Tab 1: Theo Loại/Trạng thái
            TabPage page1 = new TabPage("Theo Loại/TT");
            TableLayoutPanel tbl1 = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(20) };

            ComboBox cbLoai = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cbLoai.Items.AddRange(new object[] { "Tất cả", "Thường", "VIP" }); cbLoai.SelectedIndex = 0;

            ComboBox cbTT = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cbTT.Items.AddRange(new object[] { "Tất cả", "Trống", "Có khách", "Đã cọc" }); cbTT.SelectedIndex = 0;

            Button btnOk1 = UIHelper.CreateButton("LỌC", 100, Color.SeaGreen);
            btnOk1.Click += (s, a) => {
                isFiltered = true;
                LoadBan(cbLoai.Text, cbTT.Text, null);
                f.Close();
            };

            // Helper add control center
            void Add(Control c, Control p) { c.Anchor = AnchorStyles.None; p.Controls.Add(c); }

            tbl1.Controls.Add(UIHelper.CreateLabel("Loại:")); tbl1.Controls.Add(cbLoai);
            tbl1.Controls.Add(UIHelper.CreateLabel("Trạng thái:")); tbl1.Controls.Add(cbTT);
            tbl1.Controls.Add(new Panel { Height = 10 }); tbl1.Controls.Add(btnOk1);

            foreach (Control c in tbl1.Controls) c.Anchor = AnchorStyles.None; // Center All
            page1.Controls.Add(tbl1);

            // Tab 2: Theo Tên
            TabPage page2 = new TabPage("Theo Tên");
            TableLayoutPanel tbl2 = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(20) };
            TextBox txtName = new TextBox { Width = 200 };
            Button btnOk2 = UIHelper.CreateButton("TÌM", 100, Color.SeaGreen);
            btnOk2.Click += (s, a) => {
                isFiltered = true;
                LoadBan(null, null, txtName.Text);
                f.Close();
            };

            tbl2.Controls.Add(UIHelper.CreateLabel("Nhập tên bàn:")); tbl2.Controls.Add(txtName);
            tbl2.Controls.Add(new Panel { Height = 10 }); tbl2.Controls.Add(btnOk2);
            foreach (Control c in tbl2.Controls) c.Anchor = AnchorStyles.None;
            page2.Controls.Add(tbl2);

            tab.TabPages.Add(page1); tab.TabPages.Add(page2);
            f.Controls.Add(tab);
            f.ShowDialog();
        }

        void BtnAdd_Click(object sender, EventArgs e)
        {
            Form f = new Form { Size = new Size(300, 250) };
            UIHelper.SetupDialog(f, "THÊM BÀN");

            TableLayoutPanel tbl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(20) };

            ComboBox cb = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cb.Items.AddRange(new object[] { "Thường", "VIP" }); cb.SelectedIndex = 0;

            TextBox txt = new TextBox { Width = 200, PlaceholderText = "Tên bàn" };

            Button btn = UIHelper.CreateButton("LƯU", 100, Color.SeaGreen);
            btn.Click += (s, a) => {
                if (string.IsNullOrWhiteSpace(txt.Text)) { MessageBox.Show("Chưa nhập tên!"); return; }
                bll.AddTable(txt.Text, cb.Text);
                isFiltered = false;
                LoadBan();
                f.Close();
            };

            tbl.Controls.Add(UIHelper.CreateLabel("Loại bàn:")); tbl.Controls.Add(cb);
            tbl.Controls.Add(UIHelper.CreateLabel("Tên bàn:")); tbl.Controls.Add(txt);
            tbl.Controls.Add(new Panel { Height = 10 }); tbl.Controls.Add(btn);
            foreach (Control c in tbl.Controls) c.Anchor = AnchorStyles.None;

            f.Controls.Add(tbl);
            f.ShowDialog();
        }

        void ApplyPermission()
        {
            string role = currentUser.VaiTro;

            btnAdd.Visible = (role == "Quản lý");

            if (role == "Phục vụ")
            {
                // --- PHỤC VỤ ---
                // 1. Chỉ được gọi món -> Ẩn nút Thanh toán
                btnThanhToan.Visible = false;

                // 2. Không được xem/sửa lịch đặt -> Ẩn Tab Đặt bàn
                // --- [SỬA ĐỔI]: Thêm kiểm tra 'tabBooking != null' trước khi xóa ---
                if (tabBooking != null && tabRight.TabPages.Contains(tabBooking))
                {
                    tabRight.TabPages.Remove(tabBooking);
                }

                // 3. Không được cấu hình bàn -> Ẩn Tab Chi tiết
                // Tìm tab chi tiết theo tiêu đề (Text) để đảm bảo tìm thấy
                TabPage tabInfo = tabRight.TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Text == "Chi tiết bàn");
                if (tabInfo != null && tabRight.TabPages.Contains(tabInfo))
                {
                    tabRight.TabPages.Remove(tabInfo);
                }
            }
            else if (role == "Thu ngân")
            {
                // --- THU NGÂN ---
                // 1. Chỉ thanh toán, không gọi món -> Ẩn nút Gọi món
                btnGoiMon.Visible = false;

                // 2. Được phép CRUD Lịch đặt -> Giữ nguyên Tab Đặt bàn (Visible = true)

                // 3. Tab Chi tiết: Chỉ sửa trạng thái, KHÔNG sửa tên/loại
                // Logic ẩn control input sẽ được xử lý kỹ hơn trong hàm SelectBan
            }
        }

        void ResetAndHideRightPanel()
        {
            selectedBanID = null;            // 1. Xóa biến lưu mã bàn đang chọn
            pnlRight.Visible = false;        // 2. Ẩn toàn bộ panel bên phải (Tab gọi món/đặt bàn...)

            // 3. Load lại danh sách bàn ở bên trái
            // Tham số cuối = true để giữ vị trí cuộn chuột, không bị nhảy lên đầu trang
            LoadBan(null, null, null, true);
        }
    }
}