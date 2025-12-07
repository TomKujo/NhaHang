using BLL;
using DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucNhanSu : UserControl
    {
        ServiceBLL bll = new ServiceBLL();
        private Panel pnlTop; // Panel Top (chứa các nút và tìm kiếm)
        private string currentMaNV = null; // null = Thêm mới, có giá trị = Sửa

        public ucNhanSu()
        {
            // Thay thế SetupManualUI() bằng InitializeComponent()
            InitializeComponent();
            SetupCustomControls(); // Cấu hình thêm các control phức tạp (Top Panel, Input Groups)
            LoadData();
        }

        // --- 1. THIẾT KẾ GIAO DIỆN PHỤ (Code-Behind) ---
        // Hàm này thay thế cho việc vẽ Panel Top và Input Group trước đó
        void SetupCustomControls()
        {
            this.BackColor = Color.WhiteSmoke;

            // A. TOP BAR (Đã tạo sẵn trong Designer, chỉ cần lấy ra)
            this.Controls.Remove(pnlTop); // Xóa pnlTop cũ nếu có
            pnlTop = CreateTopPanel();
            this.Controls.Add(pnlTop);
        }

        Panel CreateTopPanel()
        {
            Panel p = UIHelper.CreatePanel(DockStyle.Top, 70, new Padding(0, 0, 10, 0));
            p.BackColor = Color.White;

            p.Controls.Add(this.btnSearch);
            p.Controls.Add(this.btnXoa);
            p.Controls.Add(this.btnSua);
            p.Controls.Add(this.btnAdd);

            return p;
        }

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
            dgvNV.DataSource = bll.GetAllStaffFull();

            string[] headers = { "Mã NV", "Họ và tên", "Điện thoại", "Địa chỉ", "Email", "Lương", "Trạng thái TK", "Vai trò" };
            string[] dataFields = { "MaNV", "Ten", "SDT", "DiaChi", "Email", "Luong", "TrangThai", "VaiTro" };

            UIHelper.SetGridColumns(dgvNV, headers, dataFields);
        }

        void OpenInputForm(string maNV)
        {
            frmNhanVienChiTiet form = new frmNhanVienChiTiet(maNV, bll);

            UIHelper.SetupDialog(form, maNV == null ? "THÊM NHÂN VIÊN MỚI" : "CẬP NHẬT THÔNG TIN NHÂN VIÊN");

            if (maNV != null && dgvNV.CurrentRow != null)
            {
                form.SetData(
                    dgvNV.CurrentRow.Cells["MaNV"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["Ten"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["SDT"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["DiaChi"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["TrangThai"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["VaiTro"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["Luong"].Value.ToString(),
                    dgvNV.CurrentRow.Cells["Email"].Value != null ? dgvNV.CurrentRow.Cells["Email"].Value.ToString() : ""
                );
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        // --- 3. XỬ LÝ SỰ KIỆN CỦA NÚT/GRID ---

        private void DgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Click vào grid để xem/sửa thông tin
            if (e.RowIndex >= 0)
            {
                string maNV = dgvNV.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
                OpenInputForm(maNV);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenInputForm(null); // Thêm mới
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNV.CurrentRow != null)
            {
                string maNV = dgvNV.CurrentRow.Cells["MaNV"].Value.ToString();
                OpenInputForm(maNV); // Sửa thông tin nhân viên đang chọn
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNV.CurrentRow != null)
            {
                string maNV = dgvNV.CurrentRow.Cells["MaNV"].Value.ToString();
                if (MessageBox.Show($"Xóa nhân viên có mã {maNV}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string msg = bll.DeleteStaff(maNV);
                    MessageBox.Show(msg);
                    if (msg.Contains("thành công"))
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (dgvNV.CurrentRow != null)
            {
                // Lấy SĐT (được dùng làm TenDN trong logic InsertKhachHang)
                string tenDN = dgvNV.CurrentRow.Cells["SDT"].Value.ToString();

                if (MessageBox.Show($"Xác nhận mở khóa tài khoản cho NV có SĐT {tenDN}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Dùng hàm UnlockAccount trong BLL, hàm này nhận TenDN
                    string msg = bll.UnlockAccount(tenDN);
                    MessageBox.Show(msg);
                    if (msg.Contains("mở khóa") || msg.Contains("Kích hoạt"))
                    {
                        LoadData();
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // Mở form tìm kiếm mới
            Form searchForm = new Form();
            UIHelper.SetupDialog(searchForm, "Tìm kiếm nhân viên");
            searchForm.Width = 450;
            searchForm.Height = 450;

            // Cấu hình Tabs
            // SỬA: Khởi tạo đối tượng mà không dùng cú pháp { ... } để tránh hiểu nhầm
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // --- TAB 1: Tìm theo Tên ---
            TabPage tabByName = new TabPage("Tìm theo Tên");
            tabByName.Padding = new Padding(10);

            // Panel chứa các control tìm kiếm trên Tab 1
            Panel pnlSearchName = new Panel { Dock = DockStyle.Top, Height = 50 };

            TextBox txtSearchName = new TextBox { Width = 250, Height = 30, Location = new Point(10, 10), Font = new Font("Segoe UI", 11) };
            Button btnSearchByName = UIHelper.CreateButton("Tìm kiếm", 100, UIHelper.PrimaryColor);
            btnSearchByName.Location = new Point(280, 10);

            pnlSearchName.Controls.Add(txtSearchName);
            pnlSearchName.Controls.Add(btnSearchByName);

            DataGridView dgvResultsByName = UIHelper.CreateDataGridView();
            dgvResultsByName.Dock = DockStyle.Fill;

            tabByName.Controls.Add(dgvResultsByName); // Fill trước
            tabByName.Controls.Add(pnlSearchName);   // Top sau

            // Xử lý tìm kiếm theo tên
            btnSearchByName.Click += (s1, e1) =>
            {
                DataTable fullData = (DataTable)dgvNV.DataSource;
                if (fullData == null) return;

                string filter = $"Ten LIKE '%{txtSearchName.Text.Replace("'", "''")}%'";
                DataRow[] filteredRows = fullData.Select(filter);

                DataTable dtResults = fullData.Clone();
                foreach (DataRow row in filteredRows) dtResults.ImportRow(row);

                dgvResultsByName.DataSource = dtResults;
                string[] headers = { "Mã NV", "Họ và tên", "Điện thoại", "Trạng thái TK" };
                string[] dataFields = { "MaNV", "Ten", "SDT", "TrangThai" };
                UIHelper.SetGridColumns(dgvResultsByName, headers, dataFields);
            };

            // --- TAB 2: Tìm theo Trạng thái Tài khoản ---
            TabPage tabByStatus = new TabPage("Tìm theo Trạng thái");
            tabByStatus.Padding = new Padding(10);

            // Panel chứa các control tìm kiếm trên Tab 2
            Panel pnlSearchStatus = new Panel { Dock = DockStyle.Top, Height = 50 };

            ComboBox cmbStatus = new ComboBox { Width = 200, Location = new Point(10, 10), Font = new Font("Segoe UI", 11), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new string[] { "Kích hoạt", "Khóa" });
            cmbStatus.SelectedIndex = 0;
            Button btnSearchByStatus = UIHelper.CreateButton("Tìm kiếm", 100, UIHelper.PrimaryColor);
            btnSearchByStatus.Location = new Point(220, 10);

            pnlSearchStatus.Controls.Add(cmbStatus);
            pnlSearchStatus.Controls.Add(btnSearchByStatus);

            DataGridView dgvResultsByStatus = UIHelper.CreateDataGridView();
            dgvResultsByStatus.Dock = DockStyle.Fill;

            tabByStatus.Controls.Add(dgvResultsByStatus); // Fill trước
            tabByStatus.Controls.Add(pnlSearchStatus);  // Top sau

            // Xử lý tìm kiếm theo trạng thái
            btnSearchByStatus.Click += (s2, e2) =>
            {
                DataTable fullData = (DataTable)dgvNV.DataSource;
                if (fullData == null) return;

                string status = cmbStatus.SelectedItem.ToString();
                string filter = $"TrangThai = '{status}'";
                DataRow[] filteredRows = fullData.Select(filter);

                DataTable dtResults = fullData.Clone();
                foreach (DataRow row in filteredRows) dtResults.ImportRow(row);

                dgvResultsByStatus.DataSource = dtResults;
                string[] headers = { "Mã NV", "Họ và tên", "Điện thoại", "Trạng thái TK" };
                string[] dataFields = { "MaNV", "Ten", "SDT", "TrangThai" };
                UIHelper.SetGridColumns(dgvResultsByStatus, headers, dataFields);
            };

            tabControl.TabPages.Add(tabByName);
            tabControl.TabPages.Add(tabByStatus);

            searchForm.Controls.Add(tabControl);
            searchForm.ShowDialog(this.ParentForm);
        }
    }
}