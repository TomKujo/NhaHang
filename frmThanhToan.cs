using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GUI
{
    public partial class frmThanhToan : Form
    {
        // Output Properties
        public string SelectedMaKM { get; private set; }
        public string HinhThucThanhToan { get; private set; } = "Tiền mặt"; // Default
        public string CustomerSDT { get; private set; }

        // Controls
        private ComboBox cboApplyKM;
        private TextBox txtSDT;
        private ComboBox cboKhuyenMai;
        private Label lblDiemTichLuy;
        private Button btnThanhToan;
        private Label lblTotal;

        ServiceBLL bll = new ServiceBLL();
        private string maBan;
        private decimal currentTotal;
        private CultureInfo viVN = new CultureInfo("vi-VN");
        private string currentMaHD;
        private string currentMaKH;

        public frmThanhToan(string maBan, string maHD, decimal total)
        {
            this.maBan = maBan;
            this.currentTotal = total;
            this.currentMaHD = maHD;
            InitializeComponent();
            InitializeCustomComponent();
        }

        /* File: frmThanhToan.cs (Code đầy đủ cho InitializeCustomComponent) */

        private void InitializeCustomComponent()
        {
            // Cài đặt Form
            UIHelper.SetupDialog(this, "THANH TOÁN HÓA ĐƠN");
            this.Size = new Size(450, 480);
            this.viVN.NumberFormat.CurrencyDecimalDigits = 0; // Config format tiền tệ

            // Table Layout
            TableLayoutPanel layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(20) };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Total
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Apply KM
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // SDT
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Diem
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // ComboBox KM
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // HinhThucTT
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Spacer
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Button

            // Helper to add controls (Label on left, Control on right)
            void AddRow(string labelText, Control control, int row)
            {
                // Khắc phục lỗi CS0230: Truyền DockStyle.Fill rõ ràng
                Label lbl = UIHelper.CreateLabel(labelText, DockStyle.Fill, null, null, ContentAlignment.MiddleRight);
                lbl.Dock = DockStyle.Fill;
                layout.Controls.Add(lbl, 0, row);
                layout.Controls.Add(control, 1, row);
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }

            // --- 1. Tổng tiền ---
            // Khởi tạo lblTotal (Giả định rằng nó không được khởi tạo trong Designer)
            lblTotal = UIHelper.CreateLabel($"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ", DockStyle.Fill, new Font("Segoe UI", 12, FontStyle.Bold), Color.Black, ContentAlignment.MiddleLeft);
            layout.Controls.Add(lblTotal, 0, 0);
            layout.SetColumnSpan(lblTotal, 2);

            // --- 2. Áp dụng Khuyến mãi? ---
            // Khởi tạo cboApplyKM
            cboApplyKM = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Items = { "Không", "Có" }, SelectedIndex = 0 };
            cboApplyKM.SelectedIndexChanged += CboApplyKM_SelectedIndexChanged;
            AddRow("Áp dụng KM/Điểm?", cboApplyKM, 1);

            // --- 3. SĐT Khách hàng ---
            // Khởi tạo txtSDT
            txtSDT = new TextBox { Width = 200 };
            txtSDT.Leave += TxtSDT_Leave;
            txtSDT.KeyDown += TxtSDT_KeyDown;
            AddRow("SĐT Khách:", txtSDT, 2);

            // --- 4. Điểm tích lũy ---
            // Khởi tạo lblDiemTichLuy
            lblDiemTichLuy = UIHelper.CreateLabel("Điểm: 0", DockStyle.Fill, new Font("Segoe UI", 10, FontStyle.Italic), Color.Blue, ContentAlignment.MiddleLeft);
            layout.Controls.Add(lblDiemTichLuy, 1, 3);
            // Khắc phục lỗi CS0230: Truyền DockStyle.Fill rõ ràng cho Label tĩnh
            layout.Controls.Add(UIHelper.CreateLabel("Điểm tích lũy:", DockStyle.Fill, null, null, ContentAlignment.MiddleRight), 0, 3);

            // --- 5. Chọn Khuyến mãi ---
            // Khởi tạo cboKhuyenMai
            cboKhuyenMai = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            cboKhuyenMai.DisplayMember = "TenKM";
            cboKhuyenMai.ValueMember = "MaKM";
            cboKhuyenMai.SelectedIndexChanged += CboKhuyenMai_SelectedIndexChanged;
            AddRow("Chọn KM:", cboKhuyenMai, 4);

            // --- 6. Hình thức thanh toán ---
            ComboBox cboHinhThuc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Items = { "Tiền mặt", "Chuyển khoản", "Thẻ" }, SelectedIndex = 0 };
            cboHinhThuc.SelectedIndexChanged += (s, e) => { HinhThucThanhToan = cboHinhThuc.Text; };
            AddRow("Hình thức TT:", cboHinhThuc, 5);


            // --- 7. Nút Thanh toán ---
            // Khởi tạo btnThanhToan
            btnThanhToan = UIHelper.CreateButton("THANH TOÁN", 200, Color.OrangeRed);
            btnThanhToan.Click += BtnThanhToan_Click;
            btnThanhToan.Dock = DockStyle.Fill;
            layout.Controls.Add(btnThanhToan, 0, 7);
            layout.SetColumnSpan(btnThanhToan, 2);

            this.Controls.Add(layout);
            CboApplyKM_SelectedIndexChanged(null, null); // Setup initial state
        }

        private void CboApplyKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = cboApplyKM.Text == "Có";
            txtSDT.Enabled = enable;
            cboKhuyenMai.Enabled = enable;
            lblDiemTichLuy.Text = "Điểm: 0";
            cboKhuyenMai.DataSource = null;
            currentMaKH = null;

            if (!enable)
            {
                txtSDT.Text = "";
                lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
            }
        }

        private void TxtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtSDT_Leave(null, null);
                e.SuppressKeyPress = true; // Ngăn tiếng 'Ding'
            }
        }

        private void TxtSDT_Leave(object sender, EventArgs e)
        {
            if (!txtSDT.Enabled || string.IsNullOrWhiteSpace(txtSDT.Text)) return;

            string sdt = txtSDT.Text.Trim();
            CustomerSDT = sdt;

            // 1. Tìm MaKH bằng SĐT
            currentMaKH = bll.dal.GetKhachHangBySDT(sdt);

            if (currentMaKH == null)
            {
                MessageBox.Show("Khách hàng mới. Sẽ tạo tài khoản khách hàng mới khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblDiemTichLuy.Text = "Điểm: 0";
                cboKhuyenMai.DataSource = null;
                return;
            }

            // 2. Lấy điểm tích lũy
            int diem = bll.GetDiemTichLuy(currentMaKH);
            lblDiemTichLuy.Text = $"Điểm: {diem.ToString("N0", viVN)}";

            // 3. Lọc danh sách KM hợp lệ
            var listKM = bll.GetListKhuyenMai()
                            .FindAll(km => km.DiemCanThiet <= diem && (km.NgayKetThuc == null || km.NgayKetThuc.Value >= DateTime.Now))
                            .ToList();

            if (listKM.Count == 0)
            {
                MessageBox.Show("Khách hàng chưa đủ điểm để đổi KM nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            cboKhuyenMai.DataSource = listKM;
            cboKhuyenMai.SelectedIndex = -1; // Bỏ chọn để không áp dụng KM nào nếu không chọn
            SelectedMaKM = null;

            // Reset tổng tiền vì chưa áp dụng
            lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
        }

        private void CboKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhuyenMai.SelectedItem is KhuyenMaiDTO km)
            {
                SelectedMaKM = km.MaKM;

                // Cập nhật tổng tiền hiển thị (Lưu ý: Chỉ là hiển thị tạm, logic giảm giá chính nằm trong Stored Proc)
                decimal totalAfterDiscount = currentTotal;
                decimal discountAmount = 0;

                if (km.LoaiGiam == "Phần trăm")
                {
                    discountAmount = currentTotal * (km.GiaTriGiam / 100);
                    totalAfterDiscount = currentTotal - discountAmount;
                }
                else if (km.LoaiGiam == "Tiền mặt")
                {
                    discountAmount = km.GiaTriGiam;
                    totalAfterDiscount = currentTotal - discountAmount;
                    if (totalAfterDiscount < 0) totalAfterDiscount = 0;
                }

                lblTotal.Text = $"Tổng tiền: {totalAfterDiscount.ToString("#,##0", viVN)} VNĐ (Giảm {discountAmount.ToString("#,##0", viVN)})";
            }
            else
            {
                SelectedMaKM = null;
                lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (cboApplyKM.Text == "Có" && string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập SĐT khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // GỌI LOGIC XỬ LÝ THANH TOÁN
            try
            {
                // 1. Nếu có Khuyến mãi được chọn, gán MaKM vào HoaDon (trước khi gọi Checkout)
                if (!string.IsNullOrEmpty(SelectedMaKM))
                {
                    // Hàm mới: UpdateMaKMToBill
                    bll.UpdateMaKMToBill(currentMaHD, currentMaKH, SelectedMaKM);
                }

                // 2. Gọi hàm thanh toán chính
                string resultMessage = "";
                if (bll.Checkout(maBan, HinhThucThanhToan, out resultMessage))
                {
                    // LƯU Ý: Stored Proc USP_ThanhToanHoaDon TỰ ĐỘNG CỘNG ĐIỂM TÍCH LŨY MỚI,
                    // và logic TRỪ ĐIỂM KHUYẾN MÃI sẽ được xử lý tại đây (bước 3)

                    // 3. Xử lý Trừ Điểm cho Khách hàng nếu có sử dụng khuyến mãi (BƯỚC MỚI)
                    if (!string.IsNullOrEmpty(SelectedMaKM) && currentMaKH != null)
                    {
                        var km = bll.GetListKhuyenMai().Find(k => k.MaKM == SelectedMaKM);
                        if (km != null)
                        {
                            // Hàm mới: DeductPointOnUse
                            bll.DeductPointOnUse(currentMaHD, currentMaKH, SelectedMaKM, km.DiemCanThiet);
                        }
                    }

                    MessageBox.Show(resultMessage, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultMessage, "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}