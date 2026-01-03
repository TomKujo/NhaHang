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
        ServiceBLL bll = new ServiceBLL();
        NguoiDungDTO currentUser;
        string selectedBanID = null;
        bool isFiltered = false;
        ToolTip tip = new ToolTip();
        CultureInfo viVN = new CultureInfo("vi-VN");
        Panel pnlWrapperMa;
        TabPage tabBooking;

        public ucBan(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            tabBooking = tabRight.TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Text == "Đặt bàn");
            viVN.NumberFormat.CurrencyDecimalDigits = 0;
            viVN.NumberFormat.CurrencyGroupSeparator = ".";
            viVN.NumberFormat.NumberGroupSeparator = ".";

            InitEvents();
            LoadBan();
            ApplyPermission();
            SetupDetailTabSecurity();

            timerUpdate.Interval = 30000;
            timerUpdate.Tick += (s, e) => {
                if (!isFiltered) LoadBan(null, null, null, true);
                if (!string.IsNullOrEmpty(selectedBanID) && tabRight.SelectedTab == tabInfo)
                {
                }
            };
            timerUpdate.Start();
        }

        void InitEvents()
        {
            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click;
            btnGoiMon.Click += BtnGoiMon_Click;
            btnThanhToan.Click += BtnThanhToan_Click;
            btnBookNew.Click += (s, e) => {
                if (string.IsNullOrEmpty(selectedBanID))
                {
                    MessageBox.Show("Vui lòng chọn bàn cần đặt trước!", "Chưa chọn bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                frmAddEditDatBan frm = new frmAddEditDatBan(selectedBanID);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadBookingGrid(selectedBanID);
                }
            };
            btnEditBooking.Click += BtnEditBooking_Click;
            btnCancelBooking.Click += BtnCancelBooking_Click;
            btnSaveBan.Click += BtnSaveBan_Click;
            btnDeleteBan.Click += BtnDeleteBan_Click;
            dgvOrder.CurrentCellDirtyStateChanged += DgvOrder_CurrentCellDirtyStateChanged;
            dgvOrder.CellValueChanged += DgvOrder_CellValueChanged;
        }

        void BtnDeleteBan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID)) return;

            var ban = bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID);
            string currentStatus = ban != null ? ban.TrangThai : "";

            if (currentStatus == "Có khách" || currentStatus == "Đã cọc")
            {
                MessageBox.Show($"Bàn đang ở trạng thái '{currentStatus}'.\nKhông thể xóa bàn đang hoạt động!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn XÓA bàn {txtEditTenBan.Text} không?\nHành động này không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                string result = bll.DeleteTable(selectedBanID);

                if (result.Contains("thành công"))
                {
                    MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetAndHideRightPanel();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvOrder_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOrder.IsCurrentCellDirty)
            {
                dgvOrder.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvOrder_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvOrder.Columns[e.ColumnIndex].Name == "colCheck")
            {
                bool isChecked = Convert.ToBoolean(dgvOrder.Rows[e.RowIndex].Cells["colCheck"].Value);
                int idChiTiet = Convert.ToInt32(dgvOrder.Rows[e.RowIndex].Cells["ID"].Value);
                string trangThaiMoi = isChecked ? "Đã phục vụ" : "Chưa phục vụ";
                bll.UpdateDishStatus(idChiTiet, trangThaiMoi);
                dgvOrder.Rows[e.RowIndex].Cells["TrangThai"].Value = trangThaiMoi;
                CheckPaymentStatus();
            }
        }

        void CheckPaymentStatus()
        {
            if (dgvOrder.Rows.Count == 0)
            {
                btnThanhToan.Enabled = false;
                btnThanhToan.BackColor = Color.Gray;
                return;
            }

            bool allChecked = true;
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["colCheck"].Value);
                if (!isChecked)
                {
                    allChecked = false;
                    break;
                }
            }

            btnThanhToan.Enabled = allChecked;
            btnThanhToan.BackColor = allChecked ? Color.OrangeRed : Color.Gray;
        }

        // =================================================================================
        // 1. QUẢN LÝ DANH SÁCH BÀN (LEFT PANEL)
        // =================================================================================
        void LoadBan(string type = null, string stt = null, string name = null, bool isAutoUpdate = false)
        {
            int scrollPos = flpBan.VerticalScroll.Value;
            var fullList = bll.GetListBan();
            var list = fullList.ToList();

            if (!string.IsNullOrEmpty(type) && type != "Tất cả") list = list.Where(x => x.Loai == type).ToList();
            if (!string.IsNullOrEmpty(stt) && stt != "Tất cả") list = list.Where(x => x.TrangThai == stt).ToList();
            if (!string.IsNullOrEmpty(name)) list = list.Where(x => x.Ten.ToLower().Contains(name.ToLower())).ToList();

            if (list.Count == 0 && !isAutoUpdate && (type != null || stt != null || name != null))
            {
                MessageBox.Show("Không tìm thấy bàn nào phù hợp với điều kiện tìm kiếm!", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);

                list = fullList;

                isFiltered = false;
            }

            if (!isAutoUpdate) flpBan.Controls.Clear();
            else flpBan.Controls.Clear();

            foreach (var ban in list)
            {
                Button btn = new Button { Size = new Size(110, 110), Margin = new Padding(10) };
                btn.Text = $"{ban.Ten}\n({ban.Loai})\n{ban.TrangThai}";
                UIHelper.StyleButton(btn, true);
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                if (ban.TrangThai == "Có khách") btn.BackColor = Color.OrangeRed;
                else if (ban.TrangThai == "Đã cọc") btn.BackColor = Color.Goldenrod;
                else btn.BackColor = Color.SeaGreen;

                btn.Click += (s, e) => SelectBan(ban);

                flpBan.Controls.Add(btn);
            }

            if (isAutoUpdate) flpBan.VerticalScroll.Value = scrollPos;
        }

        void SelectBan(BanDTO ban)
        {
            if (ban == null) return;
            selectedBanID = ban.MaBan;
            pnlRight.Visible = true;
            lblTitleBan.Text = $"BÀN: {ban.Ten.ToUpper()}";

            txtEditMaBan.Text = ban.MaBan;
            txtEditTenBan.Text = ban.Ten;
            cboEditLoaiBan.SelectedItem = ban.Loai;

            string role = currentUser.VaiTro;

            void SetLabelVisibility(bool isVisible)
            {
                if (txtEditTenBan.Parent != null)
                {
                    foreach (Control c in txtEditTenBan.Parent.Controls)
                    {
                        if (c is Label lbl)
                        {
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
                txtEditTenBan.Visible = false;
                cboEditLoaiBan.Visible = false;

                SetLabelVisibility(false);
            }
            else if (role == "Quản lý")
            {
                txtEditTenBan.Visible = true;
                cboEditLoaiBan.Visible = true;

                SetLabelVisibility(true);

                bool isOccupied = (ban.TrangThai == "Có khách");
                txtEditTenBan.Enabled = !isOccupied;
                cboEditLoaiBan.Enabled = !isOccupied;
            }

            LoadOrderGrid(ban.MaBan);
            LoadBookingGrid(ban.MaBan);
        }

        // =================================================================================
        // 2. FORM GỌI MÓN
        // =================================================================================
        void BtnGoiMon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID))
            {
                MessageBox.Show("Vui lòng chọn bàn để gọi món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ban = bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID);
            if (ban == null) return;

            DataTable dtBookings = bll.GetBookings(selectedBanID);
            DateTime? nextBookingTime = null;

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
                        return;
                    }
                }

                // LUẬT 2: Thông báo thanh toán trước 15 phút
                if (minutesLeft < 240)
                {
                    DateTime deadline = nextBookingTime.Value.AddMinutes(-15);
                    string notification = $"LƯU Ý QUAN TRỌNG CHO NHÂN VIÊN:\n\n" +
                                          $"Bàn có lịch đặt kế tiếp lúc: {nextBookingTime.Value:HH:mm}\n" +
                                          $"YÊU CẦU: Khách phải thanh toán và trả bàn trước {deadline:HH:mm} (Trước 15 phút).\n\n" +
                                          $"Vui lòng thông báo điều kiện này cho khách hàng.";

                    MessageBox.Show(notification, "Quy định trả bàn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            Form f = new Form { Size = new Size(743, 600), FormBorderStyle = FormBorderStyle.FixedToolWindow };
            UIHelper.SetupDialog(f, "Gọi món");

            Panel pnlBtn = new Panel { Height = 70, Dock = DockStyle.Bottom, Padding = new Padding(20, 10, 20, 10) };
            Button btnConfirm = UIHelper.CreateButton("XÁC NHẬN GỌI MÓN", 200, Color.DodgerBlue);
            btnConfirm.Dock = DockStyle.Fill;
            pnlBtn.Controls.Add(btnConfirm);

            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Panel pnlInput = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            Label lblMon = new Label { Text = "Món ăn:", Left = 50, Top = 23, AutoSize = true };
            ComboBox cboMon = new ComboBox { Width = 300, DataSource = bll.GetFullMenu(), DisplayMember = "Ten", ValueMember = "MaMon", DropDownStyle = ComboBoxStyle.DropDownList, Left = 120, Top = 20 };

            Label lblSL = new Label { Text = "Số lượng:", Left = 450, Top = 23, AutoSize = true };
            NumericUpDown txtSL = new NumericUpDown { Width = 155, Minimum = 1, Maximum = 100, Value = 1, Left = 520, Top = 20 };

            Label lblNote = new Label { Text = "Ghi chú:", Left = 50, Top = 63, AutoSize = true };
            TextBox txtNote = new TextBox { Width = 555, Left = 120, Top = 60 };

            Button btnAddTemp = UIHelper.CreateButton("THÊM VÀO LIST", 200, Color.Teal);
            btnAddTemp.Location = new Point(260, 110);

            pnlInput.Controls.AddRange(new Control[] { lblMon, cboMon, lblSL, txtSL, lblNote, txtNote, btnAddTemp });

            Panel pnlGridWrapper = new Panel { Dock = DockStyle.Fill, Padding = new Padding(50, 0, 50, 20) };
            DataGridView dgvTemp = UIHelper.CreateDataGridView();

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

            if (dgvTemp.Columns["SoLuong"] != null)
            {
                dgvTemp.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
                dgvTemp.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            pnlGridWrapper.Controls.Add(dgvTemp);

            btnAddTemp.Click += (s, a) => {
                string maMon = cboMon.SelectedValue.ToString();
                string tenMon = cboMon.Text;
                int sl = (int)txtSL.Value;
                string ghiChu = txtNote.Text.Trim();

                bool found = false;
                foreach (DataRow r in dtTemp.Rows)
                {
                    if (r["MaMon"].ToString() == maMon && r["GhiChu"].ToString() == ghiChu)
                    {
                        r["SoLuong"] = (int)r["SoLuong"] + sl;
                        found = true; break;
                    }
                }
                if (!found) dtTemp.Rows.Add(maMon, tenMon, sl, ghiChu);
            };

            btnConfirm.Click += (s, a) => {
                if (dtTemp.Rows.Count == 0) { MessageBox.Show("Danh sách gọi món đang trống!"); return; }
                try
                {
                    foreach (DataRow r in dtTemp.Rows)
                    {
                        bll.OrderMon(selectedBanID, r["MaMon"].ToString(), r["SoLuong"].ToString(), r["GhiChu"].ToString());
                    }
                    MessageBox.Show("Gọi món thành công!");
                    f.Close();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi hệ thống: " + ex.Message); }
            };

            layout.Controls.Add(pnlInput, 0, 0);
            layout.Controls.Add(pnlGridWrapper, 0, 1);
            f.Controls.Add(layout);
            f.Controls.Add(pnlBtn);
            f.ShowDialog();
            ResetAndHideRightPanel();

            if (!string.IsNullOrEmpty(selectedBanID))
            {
                LoadBan(null, null, null, true);
                var selectedBan = bll.GetListBan().FirstOrDefault(x => x.MaBan == selectedBanID);
                SelectBan(selectedBan);
            }
        }

        // =================================================================================
        // 3. TAB DANH SÁCH MÓN & THANH TOÁN
        // =================================================================================
        void LoadOrderGrid(string maBan)
        {
            var dt = bll.GetOrderDetails(maBan);
            dgvOrder.DataSource = dt;
            dgvOrder.Columns.Clear();
            dgvOrder.ReadOnly = false;

            UIHelper.SetGridColumns(dgvOrder,
                new[] { "ID", "Tên món", "SL", "Đơn giá", "Ghi chú", "Thành tiền", "Trạng Thái DB" },
                new[] { "ID", "TenMon", "SoLuong", "Gia", "GhiChu", "ThanhTien", "TrangThai" },
                new[] { 0, 200, 60, 120, 150, 150, 0 }
            );

            if (dgvOrder.Columns["ID"] != null)
                dgvOrder.Columns["ID"].Visible = false;

            if (dgvOrder.Columns["TrangThai"] != null)
                dgvOrder.Columns["TrangThai"].Visible = false;

            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
            checkCol.Name = "colCheck";
            checkCol.HeaderText = "Phục vụ";
            checkCol.Width = 80;
            checkCol.FalseValue = false;
            checkCol.TrueValue = true;
            dgvOrder.Columns.Add(checkCol);
            dgvOrder.Columns["colCheck"].DisplayIndex = 0;

            if (dgvOrder.Columns["SoLuong"] != null)
            {
                dgvOrder.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
                dgvOrder.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvOrder.Columns["Gia"] != null)
            {
                dgvOrder.Columns["Gia"].DefaultCellStyle.Format = "#,##0";
                dgvOrder.Columns["Gia"].DefaultCellStyle.FormatProvider = viVN;
                dgvOrder.Columns["Gia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvOrder.Columns["ThanhTien"] != null)
            {
                dgvOrder.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0";
                dgvOrder.Columns["ThanhTien"].DefaultCellStyle.FormatProvider = viVN;
                dgvOrder.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            decimal tong = 0;
            if (dt != null)
            {
                foreach (DataGridViewRow row in dgvOrder.Rows)
                {
                    if (row.Cells["ThanhTien"].Value != null)
                        tong += Convert.ToDecimal(row.Cells["ThanhTien"].Value);

                    string statusDB = row.Cells["TrangThai"].Value?.ToString();
                    if (statusDB == "Đã phục vụ")
                    {
                        row.Cells["colCheck"].Value = true;
                    }
                    else
                    {
                        row.Cells["colCheck"].Value = false;
                    }
                }
            }
            lblTongTien.Text = tong.ToString("#,##0", viVN) + " VNĐ";
            CheckPaymentStatus();
        }

        void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID)) return;
            if (dgvOrder.Rows.Count == 0) { MessageBox.Show("Bàn chưa gọi món!"); return; }

            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }

            string maHD = bll.dal.GetUnpaidBillID(selectedBanID);
            if (string.IsNullOrEmpty(maHD))
            {
                MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmThanhToan f = new frmThanhToan(selectedBanID, maHD, tongTien);

            if (f.ShowDialog() == DialogResult.OK)
            {
                ResetAndHideRightPanel();
            }
        }

        // =================================================================================
        // 4. XỬ LÝ SỰ KIỆN TAB ĐẶT BÀN (EDIT, CANCEL)
        // =================================================================================
        void LoadBookingGrid(string maBan)
        {
            var dt = bll.GetBookings(maBan);
            dgvBookingList.DataSource = dt;

            UIHelper.SetGridColumns(dgvBookingList,
                new[] { "ID", "Khách hàng", "SĐT", "Thời gian", "Trạng Thái" },
                new[] { "MaDatBan", "TenKhachHang", "SDT", "ThoiGian", "TrangThai" },
                new[] { 130, 0, 100, 120, 100 }
            );

            if (dgvBookingList.Columns["ThoiGian"] != null)
                dgvBookingList.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        void BtnEditBooking_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID))
            {
                MessageBox.Show("Vui lòng chọn bàn cần thao tác trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvBookingList.CurrentRow == null || dgvBookingList.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn một lịch đặt trong danh sách để sửa!", "Chưa chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView drv = dgvBookingList.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;

            string status = drv["TrangThai"].ToString();

            if (status == "Hủy" || status == "Hoàn tất")
            {
                MessageBox.Show("Không thể sửa lịch đã Hủy hoặc Hoàn tất.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = drv["MaDatBan"].ToString();
            DateTime dt;
            if (!DateTime.TryParse(drv["ThoiGian"].ToString(), out dt)) dt = DateTime.Now;

            string name = drv["TenKhachHang"].ToString();
            string sdt = drv["SDT"].ToString();

            frmAddEditDatBan frm = new frmAddEditDatBan(selectedBanID, id, dt, name, sdt);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadBookingGrid(selectedBanID);
            }
        }

        void BtnCancelBooking_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBanID))
            {
                MessageBox.Show("Vui lòng chọn bàn cần thao tác trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvBookingList.CurrentRow == null || dgvBookingList.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn một lịch đặt trong danh sách để hủy!", "Chưa chọn lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                LoadBookingGrid(selectedBanID);
            }
        }

        // =================================================================================
        // 5. TAB CHI TIẾT & BẢO MẬT (TOOLTIP, DISABLE CURSOR)
        // =================================================================================
        void SetupDetailTabSecurity()
        {
            pnlWrapperMa = new Panel
            {
                Size = txtEditMaBan.Size,
                Location = txtEditMaBan.Location,
                BackColor = Color.Transparent
            };

            txtEditMaBan.Parent.Controls.Add(pnlWrapperMa);

            pnlWrapperMa.Controls.Add(txtEditMaBan);
            txtEditMaBan.Dock = DockStyle.Fill;

            txtEditMaBan.Enabled = false;

            pnlWrapperMa.MouseMove += (s, e) => {
                Cursor.Current = Cursors.No;
                tip.Show("Không thể thay đổi Mã Bàn!", pnlWrapperMa, 10, -25, 1000);
            };

            pnlWrapperMa.MouseLeave += (s, e) => {
                Cursor.Current = Cursors.Default;
                tip.Hide(pnlWrapperMa);
            };
        }

        void BtnSaveBan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Lưu thay đổi cấu hình bàn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (currentUser.VaiTro == "Quản lý")
                {
                    bll.UpdateTable(selectedBanID, txtEditTenBan.Text, cboEditLoaiBan.Text);
                }

                MessageBox.Show("Đã lưu thành công!");
                ResetAndHideRightPanel();
            }
        }

        // =================================================================================
        // 6. CÁC FORM TÌM KIẾM VÀ THÊM BÀN (POPUP)
        // =================================================================================
        void BtnSearch_Click(object sender, EventArgs e)
        {
            frmTimBan f = new frmTimBan();
            if (f.ShowDialog() == DialogResult.OK)
            {
                isFiltered = true;
                LoadBan(f.LoaiBan, f.TrangThai, f.TenBan);
            }
        }

        void BtnAdd_Click(object sender, EventArgs e)
        {
            frmAddBan frm = new frmAddBan(null);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                BanDTO newBan = frm.BanData;

                string ketQua = bll.AddTable(newBan.Ten, newBan.Loai);

                if (ketQua == "OK")
                {
                    MessageBox.Show("Thêm bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isFiltered = false;
                    LoadBan();
                }
                else
                {
                    MessageBox.Show("Lỗi thêm bàn: " + ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void ApplyPermission()
        {
            string role = currentUser.VaiTro;

            btnAdd.Visible = (role == "Quản lý");
            btnSaveBan.Visible = (role == "Quản lý");
            btnDeleteBan.Visible = (role == "Quản lý");

            if (role == "Phục vụ")
            {
                btnThanhToan.Visible = false;

                if (tabBooking != null && tabRight.TabPages.Contains(tabBooking))
                {
                    tabRight.TabPages.Remove(tabBooking);
                }

                TabPage tabInfo = tabRight.TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Text == "Chi tiết bàn");
                if (tabInfo != null && tabRight.TabPages.Contains(tabInfo))
                {
                    tabRight.TabPages.Remove(tabInfo);
                }
            }
            else if (role == "Thu ngân")
            {
                btnGoiMon.Visible = false;
            }
        }

        void ResetAndHideRightPanel()
        {
            selectedBanID = null;
            pnlRight.Visible = false;
            LoadBan(null, null, null, true);
        }
    }
}