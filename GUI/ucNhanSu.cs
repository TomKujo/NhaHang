using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucNhanSu : UserControl
    {
        ServiceBLL bll = new ServiceBLL();

        // Các control chính
        private DataGridView dgvNV;
        private Panel pnlInput; // Form nhập liệu bên phải
        private TextBox txtSearch;

        // Các control nhập liệu
        private TextBox txtTenNV, txtSDT, txtDiaChi;
        private Label lblInputTitle;
        private string currentMaNV = null; // null = Thêm mới, có giá trị = Sửa

        public ucNhanSu()
        {
            InitializeComponent();
            SetupManualUI();
            LoadData();
        }

        // --- 1. THIẾT KẾ GIAO DIỆN (Code-Behind) ---
        void SetupManualUI()
        {
            this.BackColor = Color.WhiteSmoke;

            // A. TOP BAR (Tìm kiếm + Nút thêm)
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White };

            Button btnAdd = new Button { Text = "+ Thêm nhân viên", Width = 160, Height = 40, Location = new Point(20, 15) };
            UIHelper.StyleButton(btnAdd, true);
            btnAdd.Click += (s, e) => OpenInputForm(null); // Null là thêm mới

            txtSearch = new TextBox { Width = 300, Height = 30, Location = new Point(200, 22), Font = new Font("Segoe UI", 11) };
            txtSearch.PlaceholderText = "Tìm kiếm nhân viên..."; // Chỉ chạy trên .NET 5+

            pnlTop.Controls.Add(btnAdd);
            pnlTop.Controls.Add(txtSearch);

            // B. RIGHT PANEL (Form nhập liệu - Mặc định ẩn/Width=0)
            pnlInput = new Panel { Dock = DockStyle.Right, Width = 0, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            SetupInputFormControls(); // Hàm vẽ chi tiết form bên dưới

            // C. GRID VIEW (Danh sách)
            dgvNV = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvNV);
            dgvNV.CellClick += DgvNV_CellClick; // Sự kiện click vào dòng để sửa

            // Add control vào UserControl
            this.Controls.Add(dgvNV);   // Fill
            this.Controls.Add(pnlInput); // Right
            this.Controls.Add(pnlTop);   // Top
        }

        // Vẽ chi tiết form nhập liệu bên phải
        void SetupInputFormControls()
        {
            lblInputTitle = new Label { Text = "THÊM NHÂN VIÊN", Dock = DockStyle.Top, Height = 60, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = UIHelper.PrimaryColor };

            pnlInput.Controls.Add(CreateInputGroup("Số điện thoại:", 180, out txtSDT));
            pnlInput.Controls.Add(CreateInputGroup("Địa chỉ:", 110, out txtDiaChi));
            pnlInput.Controls.Add(CreateInputGroup("Họ và tên:", 40, out txtTenNV));
            pnlInput.Controls.Add(lblInputTitle);

            // Nút Lưu / Hủy
            Button btnLuu = new Button { Text = "Lưu lại", Location = new Point(20, 260), Width = 100, Height = 40 };
            UIHelper.StyleButton(btnLuu, true);
            btnLuu.Click += BtnLuu_Click;

            Button btnHuy = new Button { Text = "Đóng", Location = new Point(140, 260), Width = 100, Height = 40 };
            UIHelper.StyleButton(btnHuy, false);
            btnHuy.Click += (s, e) => ToggleInputPanel(false); // Đóng form

            Button btnXoa = new Button { Text = "Xóa NV", Location = new Point(20, 320), Width = 220, Height = 40, BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnXoa.Click += BtnXoa_Click;

            pnlInput.Controls.Add(btnLuu);
            pnlInput.Controls.Add(btnHuy);
            pnlInput.Controls.Add(btnXoa);
        }

        // Hàm hỗ trợ tạo nhãn + ô nhập liệu nhanh
        Panel CreateInputGroup(string labelText, int topY, out TextBox txtOut)
        {
            Panel p = new Panel { Location = new Point(20, topY), Size = new Size(260, 60) };
            Label lbl = new Label { Text = labelText, Dock = DockStyle.Top, Height = 25, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray };
            txtOut = new TextBox { Dock = DockStyle.Bottom, Height = 30, Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };
            p.Controls.Add(lbl);
            p.Controls.Add(txtOut);
            return p;
        }

        // --- 2. XỬ LÝ LOGIC ---

        void LoadData()
        {
            dgvNV.DataSource = bll.GetStaff();

            // Đặt tên cột tiếng Việt
            if (dgvNV.Columns["MaNV"] != null) dgvNV.Columns["MaNV"].HeaderText = "Mã NV";
            if (dgvNV.Columns["TenNV"] != null) dgvNV.Columns["TenNV"].HeaderText = "Họ và tên";
            if (dgvNV.Columns["SDT"] != null) dgvNV.Columns["SDT"].HeaderText = "Điện thoại";
            if (dgvNV.Columns["DiaChi"] != null) dgvNV.Columns["DiaChi"].HeaderText = "Địa chỉ";
        }

        // Hàm mở form nhập liệu (Slide in)
        void ToggleInputPanel(bool open)
        {
            // Hiệu ứng đơn giản: thay đổi độ rộng
            pnlInput.Width = open ? 300 : 0;
        }

        void OpenInputForm(string maNV)
        {
            currentMaNV = maNV;
            ToggleInputPanel(true);

            if (maNV == null)
            {
                lblInputTitle.Text = "THÊM NHÂN VIÊN";
                txtTenNV.Clear(); txtSDT.Clear(); txtDiaChi.Clear();
            }
            else
            {
                lblInputTitle.Text = "CẬP NHẬT NV";
                // Lấy dữ liệu từ dòng đang chọn trên Grid điền vào ô
                if (dgvNV.CurrentRow != null)
                {
                    txtTenNV.Text = dgvNV.CurrentRow.Cells["TenNV"].Value.ToString();
                    txtSDT.Text = dgvNV.CurrentRow.Cells["SDT"].Value.ToString();
                    txtDiaChi.Text = dgvNV.CurrentRow.Cells["DiaChi"].Value.ToString();
                }
            }
        }

        // Sự kiện Click vào Grid -> Mở form sửa
        private void DgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maNV = dgvNV.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
                OpenInputForm(maNV);
            }
        }

        // Nút Lưu
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            NhanVienDTO nv = new NhanVienDTO
            {
                MaNV = currentMaNV,
                TenNV = txtTenNV.Text,
                SDT = txtSDT.Text,
                DiaChi = txtDiaChi.Text
            };

            string msg = "";
            if (currentMaNV == null)
                msg = bll.AddStaff(nv); // Thêm mới
            else
                msg = bll.UpdateStaff(nv); // Cập nhật

            MessageBox.Show(msg);
            LoadData(); // Load lại lưới
            ToggleInputPanel(false); // Đóng form
        }

        // Nút Xóa
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (currentMaNV != null)
            {
                if (MessageBox.Show("Xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteStaff(currentMaNV));
                    LoadData();
                    ToggleInputPanel(false);
                }
            }
        }
    }
}