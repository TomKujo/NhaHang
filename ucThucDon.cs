using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace GUI
{
    public partial class ucThucDon : UserControl
    {
        ServiceBLL bll = new ServiceBLL();
        private string selectedCatID = null;
        private List<CongThucDTO> tempRecipe;
        private DataGridView dgvIng;

        public ucThucDon()
        {
            InitializeComponent();
            ApplyStyles();
            BindEvents();
            LoadCategory();
            UpdateButtonState();
        }

        // Áp dụng UIHelper (màu sắc, style) sau khi InitializeComponent
        void ApplyStyles()
        {
            this.BackColor = Color.WhiteSmoke;

            // Header Color
            lblCatHeader.ForeColor = UIHelper.PrimaryColor;
            lblDishHeader.ForeColor = UIHelper.PrimaryColor;

            // Grid Style
            UIHelper.StyleDataGridView(dgvCategory);
            UIHelper.StyleDataGridView(dgvDish);

            // Button Style (Category)
            UIHelper.StyleButton(btnAddCat, true);
            UIHelper.StyleButton(btnEditCat, false);
            UIHelper.StyleButton(btnDelCat, false);

            // Button Style (Dish)
            UIHelper.StyleButton(btnAddDish, true);
            UIHelper.StyleButton(btnEditDish, false);
            UIHelper.StyleButton(btnDelDish, false);
        }

        // Gán sự kiện (tách biệt khỏi logic khởi tạo giao diện)
        void BindEvents()
        {
            // Grid Events
            dgvCategory.CellClick += DgvCategory_CellClick;

            // Category Actions
            btnAddCat.Click += (s, e) => ActionCategory("ADD");
            btnEditCat.Click += (s, e) => ActionCategory("EDIT");
            btnDelCat.Click += (s, e) => ActionCategory("DEL");

            // Dish Actions
            btnAddDish.Click += (s, e) => ActionDish("ADD");
            btnEditDish.Click += (s, e) => ActionDish("EDIT");
            btnDelDish.Click += (s, e) => ActionDish("DEL");
        }

        private void DgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategory.Rows[e.RowIndex].Cells["MaThucDon"].Value != null)
            {
                selectedCatID = dgvCategory.Rows[e.RowIndex].Cells["MaThucDon"].Value.ToString();
                LoadDish(selectedCatID);
                UpdateButtonState();
            }
        }

        // --- LOGIC LOAD DATA ---
        void LoadCategory()
        {
            dgvCategory.DataSource = bll.GetListThucDon();
            UIHelper.SetGridColumns(dgvCategory,
                new string[] { "Mã Thực Đơn", "Tên Thực Đơn", "Mô Tả" },
                new string[] { "MaThucDon", "Ten", "MoTa" },
                new int[] { 80, 150, 0 }
            );
        }

        void LoadDish(string maTD)
        {
            dgvDish.DataSource = bll.GetListMonAn(maTD);
            UIHelper.SetGridColumns(dgvDish,
                new string[] { "Mã Món", "Tên Món", "Giá Bán", "Điểm" },
                new string[] { "MaMon", "Ten", "Gia", "DiemTichLuy" },
                new int[] { 80, 200, 100, 80 }
            );
        }

        // --- LOGIC CRUD DANH MỤC (Giữ nguyên logic popup form động vì không được tạo file mới) ---
        void ActionCategory(string action)
        {
            if ((action == "EDIT" || action == "DEL") && string.IsNullOrEmpty(selectedCatID))
            {
                MessageBox.Show("Vui lòng chọn thực đơn!");
                LoadCategory();
                dgvDish.DataSource = null;
                return;
            }

            if (action == "DEL")
            {
                if (MessageBox.Show("Bạn chắc chắn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteCategory(selectedCatID));
                    LoadCategory();
                    dgvDish.DataSource = null;
                    selectedCatID = null; // Xóa ID đang chọn
                    UpdateButtonState();
                }
                return;
            }

            // Popup Form cho ADD/EDIT
            Form f = new Form { Size = new Size(350, 300) };
            UIHelper.SetupDialog(f, action == "ADD" ? "Thêm Thực Đơn" : "Sửa Thực Đơn");

            Label l1 = new Label { Text = "Tên thực đơn:", Top = 20, Left = 20, AutoSize = true};
            TextBox t1 = new TextBox { Top = 45, Left = 20, Width = 280 };
            Label l2 = new Label { Text = "Mô tả:", Top = 80, Left = 20, AutoSize = true };
            TextBox t2 = new TextBox { Top = 105, Left = 20, Width = 280 };

            // Nếu là Edit, fill dữ liệu cũ (Tạm thời lấy từ Grid cho nhanh)
            if (action == "EDIT")
            {
                t1.Text = dgvCategory.CurrentRow.Cells["Ten"].Value.ToString();
                t2.Text = dgvCategory.CurrentRow.Cells["MoTa"].Value.ToString();
            }

            Button btnSave = UIHelper.CreateButton("Lưu", 100, UIHelper.PrimaryColor);
            btnSave.Location = new Point(118, 150);
            btnSave.Click += (s, e) =>
            {
                string msg = action == "ADD"
                    ? bll.AddCategory(t1.Text, t2.Text)
                    : bll.UpdateCategory(selectedCatID, t1.Text, t2.Text);

                MessageBox.Show(msg);
                if (msg.Contains("Thành công"))
                {
                    f.Close();
                    LoadCategory(); // [SỬA] Load lại Grid Danh mục ngay lập tức
                }
            };

            f.Controls.AddRange(new Control[] { l1, t1, l2, t2, btnSave});
            f.ShowDialog();
        }

        // --- LOGIC CRUD MÓN ĂN ---
        void ActionDish(string action)
        {
            // 1. Kiểm tra điều kiện đầu vào
            if (action == "ADD" && string.IsNullOrEmpty(selectedCatID))
            {
                MessageBox.Show("Vui lòng chọn danh mục để thêm món vào!");
                return;
            }

            if ((action == "EDIT" || action == "DEL"))
            {
                if (dgvDish.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn món!");
                    return;
                }
            }

            string maMon = dgvDish.CurrentRow?.Cells["MaMon"]?.Value.ToString();

            // 2. Xử lý Xóa
            if (action == "DEL")
            {
                if (MessageBox.Show("Bạn chắc chắn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteDishSafe(maMon));
                    LoadDish(selectedCatID);
                }
                return;
            }

            // 3. Khởi tạo Form Popup
            Form f = new Form { Size = new Size(500, 680) }; // Tăng chiều cao form một chút
            UIHelper.SetupDialog(f, action == "ADD" ? "Thêm Món" : "Sửa Món");

            // --- KHU VỰC 1: THÔNG TIN CƠ BẢN (Top: 20 -> 170) ---
            Label lblTen = UIHelper.CreateLabel("Tên món:", DockStyle.None);
            lblTen.Location = new Point(20, 20);
            TextBox txtTen = new TextBox { Top = 45, Left = 20, Width = 443 };

            Label lblGia = new Label { Text = "Giá bán:", Top = 80, Left = 20, AutoSize = true };
            TextBox txtGia = new TextBox { Top = 105, Left = 20, Width = 443 };

            Label lblDiem = new Label { Text = "Điểm thưởng:", Top = 140, Left = 20, AutoSize = true };
            TextBox txtDiem = new TextBox { Top = 165, Left = 20, Width = 443 };

            // --- KHU VỰC 2: NHẬP LIỆU NGUYÊN LIỆU (Top: 200 -> 240) ---
            // Label tiêu đề cho ô nhập
            Label lblChonNL = new Label { Text = "Chọn nguyên liệu:", Top = 205, Left = 20, AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
            Label lblNhapSL = new Label { Text = "Số lượng:", Top = 205, Left = 230, AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Italic) };

            // Control nhập liệu
            ComboBox cboNL = new ComboBox { Top = 230, Left = 20, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboNL.DataSource = bll.GetInventory();
            cboNL.DisplayMember = "Ten";
            cboNL.ValueMember = "MaNguyenLieu";

            TextBox txtTieuHao = new TextBox { Top = 230, Left = 230, Width = 100, PlaceholderText = "Lượng" };

            Button btnAddIng = UIHelper.CreateButton("+", 121, Color.Gray);
            btnAddIng.Location = new Point(340, 230); // Canh chỉnh ngang hàng với TextBox

            // --- KHU VỰC 3: DANH SÁCH NGUYÊN LIỆU (Top: 270 -> End) ---
            Label lblListHeader = new Label { Text = "Công thức (Danh sách nguyên liệu):", Top = 270, Left = 20, AutoSize = true, ForeColor = UIHelper.PrimaryColor, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dgvIng = UIHelper.CreateDataGridView();
            dgvIng.Dock = DockStyle.None;       // Quan trọng: Không dùng Dock Fill
            dgvIng.Location = new Point(30, 300); // Đặt vị trí dưới phần nhập liệu
            dgvIng.Size = new Size(440, 220);   // Kích thước cố định

            // 4. Xử lý Logic dữ liệu
            tempRecipe = new List<CongThucDTO>();

            // Nếu là Edit -> Fill Data
            if (action == "EDIT")
            {
                txtTen.Text = dgvDish.CurrentRow.Cells["Ten"].Value.ToString();
                txtGia.Text = decimal.Parse(dgvDish.CurrentRow.Cells["Gia"].Value.ToString()).ToString("0");
                txtDiem.Text = dgvDish.CurrentRow.Cells["DiemTichLuy"].Value.ToString();

                DataTable dtRecipe = bll.GetDishRecipe(maMon);
                foreach (DataRow r in dtRecipe.Rows)
                {
                    tempRecipe.Add(new CongThucDTO
                    {
                        MaNguyenLieu = r["MaNguyenLieu"].ToString(),
                        TenNguyenLieu = r["TenNguyenLieu"].ToString(),
                        LuongTieuHao = Convert.ToDecimal(r["LuongTieuHao"]),
                        DonVi = r["DonVi"].ToString()
                    });
                }
            }

            RefreshRecipeGrid(); // Vẽ lại grid ngay lập tức

            // Logic nút Thêm Nguyên Liệu (+)
            btnAddIng.Click += (s, e) => {
                if (cboNL.SelectedValue == null) return;

                // [SỬA 1]: Thay thế dấu phẩy bằng dấu chấm để đồng bộ
                string inputSL = txtTieuHao.Text.Replace(',', '.');

                // [SỬA 2]: Dùng CultureInfo.InvariantCulture để đảm bảo "0.5" luôn được hiểu là số lẻ dù máy tính cài đặt Tiếng Việt hay Anh
                if (!decimal.TryParse(inputSL, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sl) || sl <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ (Ví dụ: 0.5 hoặc 1)!");
                    txtTieuHao.Focus();
                    return;
                }

                string maNL = cboNL.SelectedValue.ToString();
                var exist = tempRecipe.FirstOrDefault(x => x.MaNguyenLieu == maNL);

                // Logic cộng dồn hoặc thêm mới giữ nguyên
                if (exist != null) exist.LuongTieuHao += sl;
                else tempRecipe.Add(new CongThucDTO
                {
                    MaNguyenLieu = maNL,
                    TenNguyenLieu = cboNL.Text,
                    LuongTieuHao = sl,
                    DonVi = (cboNL.SelectedItem as DataRowView)?["DonVi"]?.ToString()
                });

                RefreshRecipeGrid();
                txtTieuHao.Clear();
            };

            // Logic Xóa dòng trong Grid (Double click)
            dgvIng.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    tempRecipe.RemoveAt(e.RowIndex);
                    RefreshRecipeGrid();
                }
            };

            // 5. Nút Lưu và Đóng
            Button btnSave = UIHelper.CreateButton("LƯU MÓN", 200, Color.Green);
            btnSave.Dock = DockStyle.Bottom;

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Chưa nhập tên món!"); return; }
                if (!decimal.TryParse(txtGia.Text, out decimal g)) { MessageBox.Show("Giá không hợp lệ!"); return; }
                int.TryParse(txtDiem.Text, out int d);

                MonDTO m = new MonDTO
                {
                    MaMon = action == "EDIT" ? maMon : null,
                    Ten = txtTen.Text,
                    Gia = g,
                    DiemTichLuy = d,
                    MaThucDon = selectedCatID
                };

                Dictionary<string, decimal> ingredients = new Dictionary<string, decimal>();
                foreach (var item in tempRecipe)
                {
                    if (ingredients.ContainsKey(item.MaNguyenLieu))
                        ingredients[item.MaNguyenLieu] += item.LuongTieuHao;
                    else
                        ingredients.Add(item.MaNguyenLieu, item.LuongTieuHao);
                }

                string result = action == "ADD"
                    ? bll.AddDishWithRecipe(m, ingredients)
                    : bll.UpdateDishWithRecipe(m, ingredients);

                MessageBox.Show(result);
                if (result.Contains("thành công") || result.Contains("OK"))
                {
                    f.Close();
                    LoadDish(selectedCatID);
                }
            };

            // 6. Add Control vào Form (Thứ tự thêm quan trọng để hiển thị đúng Z-Index nếu cần)
            f.Controls.AddRange(new Control[]{
                lblTen, txtTen,
                lblGia, txtGia,
                lblDiem, txtDiem,

                lblChonNL, lblNhapSL,       // Label tiêu đề cột nhập
                cboNL, txtTieuHao, btnAddIng, // Control nhập liệu
        
                lblListHeader, dgvIng,      // Grid và Header Grid
        
                btnSave
            });

            f.Controls.AddRange(new Control[]{
                lblTen, txtTen,
                lblGia, txtGia,
                lblDiem, txtDiem,
                lblChonNL, lblNhapSL,
                cboNL, txtTieuHao, btnAddIng,
                lblListHeader, dgvIng,
                btnSave
            });

            // --- [THÊM ĐOẠN NÀY] --- 
            // Xử lý ẩn phần công thức nếu là EDIT
            if (action == "EDIT")
            {
                // 1. Ẩn Tên món (User không được sửa tên)
                lblTen.Visible = false;
                txtTen.Visible = false;

                // 2. Ẩn toàn bộ phần Công thức/Nguyên liệu
                lblChonNL.Visible = false;
                lblNhapSL.Visible = false;
                cboNL.Visible = false;
                txtTieuHao.Visible = false;
                btnAddIng.Visible = false;
                lblListHeader.Visible = false;
                dgvIng.Visible = false;

                // 3. Đẩy vị trí GIÁ và ĐIỂM lên trên (lấp chỗ Tên món vừa ẩn)
                // Di chuyển lblGia, txtGia lên vị trí cũ của Tên (Top 20 và 45)
                lblGia.Location = new Point(20, 20);
                txtGia.Location = new Point(20, 45);

                // Di chuyển lblDiem, txtDiem lên vị trí cũ của Giá (Top 80 và 105)
                lblDiem.Location = new Point(20, 80);
                txtDiem.Location = new Point(20, 105);

                // 4. Thu gọn chiều cao Form
                // Chỉ còn hiển thị Giá, Điểm và Nút Lưu -> Height tầm 230 là đẹp
                f.Height = 230;

                // Đổi tiêu đề Form cho phù hợp ngữ cảnh
                f.Text = "Cập nhật giá & điểm";
            }

            f.ShowDialog();
        }

        void RefreshRecipeGrid()
        {
            dgvIng.DataSource = null;
            dgvIng.DataSource = tempRecipe
                .Select(x => new
                {
                    x.MaNguyenLieu,
                    x.TenNguyenLieu,
                    x.LuongTieuHao,
                    x.DonVi
                })
                .ToList();

            // Định nghĩa cột rõ ràng để Grid không tự sinh cột lung tung
            UIHelper.SetGridColumns(dgvIng,
                new string[] { "Nguyên Liệu", "Lượng", "Đơn Vị" },
                new string[] { "TenNguyenLieu", "LuongTieuHao", "DonVi" },
                new int[] { 200, 100, 80 }
            );
        }

        void UpdateButtonState()
        {
            bool hasCategory = !string.IsNullOrEmpty(selectedCatID);

            btnAddDish.Visible = hasCategory;
            btnEditDish.Visible = hasCategory;
            btnDelDish.Visible = hasCategory;
        }
    }
}