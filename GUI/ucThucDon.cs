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

        void ApplyStyles()
        {
            this.BackColor = Color.WhiteSmoke;

            lblCatHeader.ForeColor = UIHelper.PrimaryColor;
            lblDishHeader.ForeColor = UIHelper.PrimaryColor;

            UIHelper.StyleDataGridView(dgvCategory);
            UIHelper.StyleDataGridView(dgvDish);

            UIHelper.StyleButton(btnAddCat, true);
            UIHelper.StyleButton(btnEditCat, false);
            UIHelper.StyleButton(btnDelCat, false);

            UIHelper.StyleButton(btnAddDish, true);
            UIHelper.StyleButton(btnEditDish, false);
            UIHelper.StyleButton(btnDelDish, false);
        }

        void BindEvents()
        {
            dgvCategory.CellClick += DgvCategory_CellClick;

            btnAddCat.Click += (s, e) => ActionCategory("ADD");
            btnEditCat.Click += (s, e) => ActionCategory("EDIT");
            btnDelCat.Click += (s, e) => ActionCategory("DEL");

            btnAddDish.Click += (s, e) => ActionDish("ADD");
            btnEditDish.Click += (s, e) => ActionDish("EDIT");
            btnDelDish.Click += (s, e) => ActionDish("DEL");
        }

        private void DgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategory.Rows[e.RowIndex].Cells["MaThucDon"].Value != null)
            {
                selectedCatID = dgvCategory.Rows[e.RowIndex].Cells["MaThucDon"].Value.ToString();
                pnlRight.Visible = true;
                LoadDish(selectedCatID);
                UpdateButtonState();
            }
        }

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
                new int[] { 150, 0, 150, 150 }
            );
        }

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
                    selectedCatID = null;
                    UpdateButtonState();
                    pnlRight.Visible = false;
                }
                return;
            }

            ThucDonDTO tdHienTai = null;
            if (action == "EDIT")
            {
                tdHienTai = new ThucDonDTO
                {
                    MaThucDon = selectedCatID,
                    Ten = dgvCategory.CurrentRow.Cells["Ten"].Value.ToString(),
                    MoTa = dgvCategory.CurrentRow.Cells["MoTa"].Value?.ToString()
                };
            }

            frmAddEditTD f = new frmAddEditTD(tdHienTai);
            if (f.ShowDialog() == DialogResult.OK)
            {
                ThucDonDTO tdMoi = f.ThucDonData;
                string msg = "";

                if (action == "ADD")
                {
                    msg = bll.AddCategory(tdMoi.Ten, tdMoi.MoTa);
                }
                else
                {
                    msg = bll.UpdateCategory(tdMoi.MaThucDon, tdMoi.Ten, tdMoi.MoTa);
                }

                MessageBox.Show(msg);
                if (msg.Contains("thành công") || msg.Contains("OK")) // Kiểm tra theo thông báo trả về từ BLL
                {
                    LoadCategory();
                }
            }
        }

        void ActionDish(string action)
        {
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

            if (action == "DEL")
            {
                if (MessageBox.Show("Bạn chắc chắn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteDishSafe(maMon));
                    LoadDish(selectedCatID);
                }
                return;
            }

            Form f = new Form { Size = new Size(500, 680) };
            UIHelper.SetupDialog(f, action == "ADD" ? "Thêm món" : "Sửa món");

            Label lblTen = UIHelper.CreateLabel("Tên món:", DockStyle.None);
            lblTen.Location = new Point(20, 20);
            TextBox txtTen = new TextBox { Top = 45, Left = 20, Width = 443 };

            Label lblGia = new Label { Text = "Giá bán:", Top = 80, Left = 20, AutoSize = true };
            TextBox txtGia = new TextBox { Top = 105, Left = 20, Width = 443 };

            Label lblDiem = new Label { Text = "Điểm thưởng:", Top = 140, Left = 20, AutoSize = true };
            TextBox txtDiem = new TextBox { Top = 165, Left = 20, Width = 443 };

            Label lblChonNL = new Label { Text = "Chọn nguyên liệu:", Top = 205, Left = 20, AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
            Label lblNhapSL = new Label { Text = "Số lượng:", Top = 205, Left = 230, AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Italic) };

            ComboBox cboNL = new ComboBox { Top = 230, Left = 20, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboNL.DataSource = bll.GetInventory();
            cboNL.DisplayMember = "Ten";
            cboNL.ValueMember = "MaNguyenLieu";

            TextBox txtTieuHao = new TextBox { Top = 230, Left = 230, Width = 100, PlaceholderText = "Lượng" };

            Button btnAddIng = UIHelper.CreateButton("+", 121, Color.Gray);
            btnAddIng.Location = new Point(340, 225);

            Label lblListHeader = new Label { Text = "Công thức (Danh sách nguyên liệu):", Top = 270, Left = 20, AutoSize = true, ForeColor = UIHelper.PrimaryColor, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dgvIng = UIHelper.CreateDataGridView();
            dgvIng.Dock = DockStyle.None;
            dgvIng.Location = new Point(30, 300);
            dgvIng.Size = new Size(440, 220);

            tempRecipe = new List<CongThucDTO>();

            if (action == "EDIT")
            {
                txtTen.Text = dgvDish.CurrentRow.Cells["Ten"].Value.ToString();

                if (dgvDish.CurrentRow.Cells["Gia"].Value != null)
                {
                    decimal valGia = Convert.ToDecimal(dgvDish.CurrentRow.Cells["Gia"].Value);
                    txtGia.Text = valGia.ToString("N0");
                }

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

            RefreshRecipeGrid();

            btnAddIng.Click += (s, e) => {
                if (cboNL.SelectedValue == null) return;

                string inputSL = txtTieuHao.Text.Replace(',', '.');

                if (!decimal.TryParse(inputSL, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sl) || sl <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ (Ví dụ: 0.5 hoặc 1)!");
                    txtTieuHao.Focus();
                    return;
                }

                string maNL = cboNL.SelectedValue.ToString();
                var exist = tempRecipe.FirstOrDefault(x => x.MaNguyenLieu == maNL);

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

            dgvIng.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    tempRecipe.RemoveAt(e.RowIndex);
                    RefreshRecipeGrid();
                }
            };

            Panel pnlBtn = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.White
            };

            Button btnSave = UIHelper.CreateButton("LƯU", 200, Color.SeaGreen);
            btnSave.Dock = DockStyle.Fill;
            pnlBtn.Controls.Add(btnSave);

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Chưa nhập tên món!"); return; }
                string cleanGia = txtGia.Text.Replace(".", "").Replace(",", "").Trim();
                if (!decimal.TryParse(cleanGia, out decimal g))
                {
                    MessageBox.Show("Giá không hợp lệ!"); return;
                }

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

            f.Controls.AddRange(new Control[]{
                lblTen, txtTen,
                lblGia, txtGia,
                lblDiem, txtDiem,
                lblChonNL, lblNhapSL,
                cboNL, txtTieuHao, btnAddIng,
                lblListHeader, dgvIng,
                pnlBtn
            });

            if (action == "EDIT")
            {
                lblTen.Visible = false;
                txtTen.Visible = false;

                lblChonNL.Visible = false;
                lblNhapSL.Visible = false;
                cboNL.Visible = false;
                txtTieuHao.Visible = false;
                btnAddIng.Visible = false;
                lblListHeader.Visible = false;
                dgvIng.Visible = false;
                lblGia.Location = new Point(20, 20);
                txtGia.Location = new Point(20, 45);
                lblDiem.Location = new Point(20, 80);
                txtDiem.Location = new Point(20, 105);
                f.Height = 270;
                f.Text = "Sửa giá, điểm tích lũy";
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