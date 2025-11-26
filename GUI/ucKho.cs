using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucKho : UserControl
    {
        ServiceBLL bll = new ServiceBLL();
        NguoiDungDTO currentUser;

        // --- KHAI BÁO BIẾN TOÀN CỤC ---

        // 1. Controls Tab 1 (Kho)
        DataGridView dgvKho;
        // Đã sửa: Chỉ dùng duy nhất biến cboNCC_Import, bỏ biến cboNCC thừa
        private ComboBox cboNCC_Import;

        DataGridView dgvGioHang;
        List<CartItemDTO> gioHangNhap = new List<CartItemDTO>();
        Label lblTongTienNhap;

        // 2. Controls Tab 2 (NCC)
        DataGridView dgvNCC;
        Panel pnlInputNCC;
        TextBox txtTenNCC, txtDiaChiNCC, txtSDTNCC;
        string currentMaNCC = null;

        public ucKho(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;
            SetupUI();
            LoadDataKho();
            LoadDataNCC();
        }

        void SetupUI()
        {
            this.BackColor = Color.White;
            TabControl tab = new TabControl { Dock = DockStyle.Fill };

            // =================================================================
            // TAB 1: KHO & NHẬP HÀNG
            // =================================================================
            TabPage tabKho = new TabPage("Kho & Nhập Hàng");
            SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 500 };

            // --- TRÁI: DANH SÁCH TỒN KHO ---
            Panel pnlKhoLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            Label lblTitleKho = new Label { Text = "DANH SÁCH TỒN KHO", Dock = DockStyle.Top, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            Button btnAddTP = new Button { Text = "+ Khai báo Nguyên liệu mới", Dock = DockStyle.Top, Height = 30 };
            btnAddTP.Click += BtnAddTP_Click;

            dgvKho = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvKho);
            dgvKho.CellDoubleClick += DgvKho_CellDoubleClick;

            pnlKhoLeft.Controls.Add(dgvKho);
            pnlKhoLeft.Controls.Add(btnAddTP);
            pnlKhoLeft.Controls.Add(lblTitleKho);

            // --- PHẢI: FORM NHẬP HÀNG ---
            Panel pnlNhap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5), BackColor = Color.WhiteSmoke };
            Label lblTitleNhap = new Label { Text = "LẬP PHIẾU NHẬP", Dock = DockStyle.Top, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = UIHelper.PrimaryColor };

            // SỬA LỖI: Khởi tạo đúng biến cboNCC_Import (trước đây bạn new cboNCC)
            cboNCC_Import = new ComboBox { Dock = DockStyle.Top, Height = 30, DropDownStyle = ComboBoxStyle.DropDownList };
            Label lblChonNCC = new Label { Text = "Chọn Nhà cung cấp:", Dock = DockStyle.Top };

            dgvGioHang = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvGioHang);

            lblTongTienNhap = new Label { Text = "Tổng: 0 VNĐ", Dock = DockStyle.Bottom, Height = 30, TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            Button btnXacNhanNhap = new Button { Text = "NHẬP KHO & XUẤT PHIẾU", Dock = DockStyle.Bottom, Height = 40 };
            UIHelper.StyleButton(btnXacNhanNhap, true);
            btnXacNhanNhap.Click += BtnXacNhanNhap_Click;

            pnlNhap.Controls.Add(dgvGioHang);
            pnlNhap.Controls.Add(lblTongTienNhap);
            pnlNhap.Controls.Add(btnXacNhanNhap);
            pnlNhap.Controls.Add(cboNCC_Import); // Add đúng biến
            pnlNhap.Controls.Add(lblChonNCC);
            pnlNhap.Controls.Add(lblTitleNhap);

            split.Panel1.Controls.Add(pnlKhoLeft);
            split.Panel2.Controls.Add(pnlNhap);
            tabKho.Controls.Add(split);

            // =================================================================
            // TAB 2: QUẢN LÝ NHÀ CUNG CẤP
            // =================================================================
            TabPage tabNCC = new TabPage("Quản Lý Nhà Cung Cấp");

            // Panel Input (Slide in)
            pnlInputNCC = new Panel { Dock = DockStyle.Right, Width = 0, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };
            SetupInputNCC();

            // Grid
            dgvNCC = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvNCC);
            dgvNCC.CellClick += DgvNCC_CellClick;

            // Top Bar
            Panel pnlTopNCC = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.White };
            Button btnAddNCC = new Button { Text = "+ Thêm NCC", Width = 120, Height = 35, Location = new Point(10, 8) };
            UIHelper.StyleButton(btnAddNCC, true);
            btnAddNCC.Click += (s, e) => OpenPanelNCC(null);
            pnlTopNCC.Controls.Add(btnAddNCC);

            tabNCC.Controls.Add(dgvNCC);
            tabNCC.Controls.Add(pnlInputNCC);
            tabNCC.Controls.Add(pnlTopNCC);

            tab.TabPages.Add(tabKho);
            tab.TabPages.Add(tabNCC);
            this.Controls.Add(tab);
        }

        // --- LOGIC TAB KHO ---
        private void BtnAddTP_Click(object sender, EventArgs e)
        {
            string ten = Microsoft.VisualBasic.Interaction.InputBox("Tên nguyên liệu:", "Thêm mới");
            string dvt = Microsoft.VisualBasic.Interaction.InputBox("Đơn vị tính:", "Thêm mới");
            if (!string.IsNullOrEmpty(ten))
            {
                MessageBox.Show(bll.AddIngredient(ten, dvt));
                LoadDataKho();
            }
        }

        // Double click vào kho -> Thêm vào giỏ
        private void DgvKho_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string ma = dgvKho.Rows[e.RowIndex].Cells["MaThucPham"].Value.ToString();
            string ten = dgvKho.Rows[e.RowIndex].Cells["TenTP"].Value.ToString();

            string strSL = Microsoft.VisualBasic.Interaction.InputBox($"Số lượng nhập {ten}:", "Nhập hàng", "1");
            string strGia = Microsoft.VisualBasic.Interaction.InputBox($"Đơn giá nhập:", "Nhập hàng", "0");

            if (int.TryParse(strSL, out int sl) && decimal.TryParse(strGia, out decimal gia))
            {
                gioHangNhap.Add(new CartItemDTO { MaThucPham = ma, TenTP = ten, SoLuong = sl, DonGia = gia });
                RefreshGioHang();
            }
        }

        void RefreshGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = gioHangNhap;
            dgvGioHang.Columns["MaThucPham"].Visible = false;
            decimal tong = 0;
            foreach (var i in gioHangNhap) tong += i.ThanhTien;
            lblTongTienNhap.Text = $"Tổng: {tong:N0} VNĐ";
        }

        private void BtnXacNhanNhap_Click(object sender, EventArgs e)
        {
            if (cboNCC_Import.SelectedValue == null) { MessageBox.Show("Chọn NCC!"); return; }
            string msg = bll.ImportStock(cboNCC_Import.SelectedValue.ToString(), currentUser.MaNguoiDung, gioHangNhap, cboNCC_Import.Text);
            MessageBox.Show(msg);
            if (msg.Contains("thành công")) { gioHangNhap.Clear(); RefreshGioHang(); LoadDataKho(); }
        }

        // --- LOGIC TAB NCC ---
        void LoadDataKho() => dgvKho.DataSource = bll.GetInventory();

        void LoadDataNCC()
        {
            var dt = bll.GetListNCC();
            dgvNCC.DataSource = dt;

            // Refresh ComboBox nhập hàng
            cboNCC_Import.DataSource = dt;
            cboNCC_Import.DisplayMember = "TenNCC";
            cboNCC_Import.ValueMember = "MaNCC";
        }

        // --- FORM NHẬP LIỆU NCC ---
        void SetupInputNCC()
        {
            Label lblTitle = new Label { Text = "THÔNG TIN NCC", Dock = DockStyle.Top, Height = 50, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 12, FontStyle.Bold) };

            var p1 = CreateInputGroup("Tên Nhà CC:", 60, out txtTenNCC);
            var p2 = CreateInputGroup("Địa chỉ:", 130, out txtDiaChiNCC);
            var p3 = CreateInputGroup("Số điện thoại:", 200, out txtSDTNCC);

            Button btnSave = new Button { Text = "Lưu lại", Location = new Point(20, 280), Width = 100, Height = 35 };
            UIHelper.StyleButton(btnSave, true);
            btnSave.Click += BtnSaveNCC_Click;

            Button btnCancel = new Button { Text = "Hủy", Location = new Point(130, 280), Width = 100, Height = 35 };
            UIHelper.StyleButton(btnCancel, false);
            btnCancel.Click += (s, e) => pnlInputNCC.Width = 0;

            Button btnDel = new Button { Text = "Xóa NCC", Location = new Point(20, 330), Width = 210, Height = 35, BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnDel.Click += BtnDelNCC_Click;

            pnlInputNCC.Controls.Add(btnDel);
            pnlInputNCC.Controls.Add(btnCancel);
            pnlInputNCC.Controls.Add(btnSave);
            pnlInputNCC.Controls.Add(p3);
            pnlInputNCC.Controls.Add(p2);
            pnlInputNCC.Controls.Add(p1);
            pnlInputNCC.Controls.Add(lblTitle);
        }

        Panel CreateInputGroup(string label, int y, out TextBox txt)
        {
            Panel p = new Panel { Location = new Point(10, y), Size = new Size(280, 60) };
            Label l = new Label { Text = label, Dock = DockStyle.Top, Height = 20 };
            txt = new TextBox { Dock = DockStyle.Bottom, Height = 30, Font = new Font("Segoe UI", 10) };
            p.Controls.Add(l);
            p.Controls.Add(txt);
            return p;
        }

        void OpenPanelNCC(string maNCC)
        {
            currentMaNCC = maNCC;
            pnlInputNCC.Width = 300;

            if (maNCC == null) // Thêm mới
            {
                txtTenNCC.Clear(); txtDiaChiNCC.Clear(); txtSDTNCC.Clear();
                txtTenNCC.Focus();
            }
            else // Sửa
            {
                if (dgvNCC.CurrentRow != null)
                {
                    txtTenNCC.Text = dgvNCC.CurrentRow.Cells["TenNCC"].Value.ToString();
                    txtDiaChiNCC.Text = dgvNCC.CurrentRow.Cells["DiaChi"].Value.ToString();
                    txtSDTNCC.Text = dgvNCC.CurrentRow.Cells["SDT"].Value.ToString();
                }
            }
        }

        private void DgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string ma = dgvNCC.Rows[e.RowIndex].Cells["MaNCC"].Value.ToString();
                OpenPanelNCC(ma);
            }
        }

        private void BtnSaveNCC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenNCC.Text)) { MessageBox.Show("Tên không được trống"); return; }

            string msg;
            if (currentMaNCC == null)
                msg = bll.AddSupplier(txtTenNCC.Text, txtDiaChiNCC.Text, txtSDTNCC.Text);
            else
                msg = bll.UpdateSupplier(currentMaNCC, txtTenNCC.Text, txtDiaChiNCC.Text, txtSDTNCC.Text);

            MessageBox.Show(msg);
            if (msg.Contains("thành công"))
            {
                LoadDataNCC();
                pnlInputNCC.Width = 0;
            }
        }

        private void BtnDelNCC_Click(object sender, EventArgs e)
        {
            if (currentMaNCC != null && MessageBox.Show("Xóa NCC này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string msg = bll.DeleteSupplier(currentMaNCC);
                MessageBox.Show(msg);
                if (msg.Contains("thành công"))
                {
                    LoadDataNCC();
                    pnlInputNCC.Width = 0;
                }
            }
        }
    }
}