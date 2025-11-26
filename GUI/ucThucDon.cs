using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;
// Cần Reference: Microsoft.VisualBasic

namespace GUI
{
    public partial class ucThucDon : UserControl
    {
        ServiceBLL bll = new ServiceBLL();

        private DataGridView dgvCategory;
        private DataGridView dgvDish;
        private string selectedCatID = null;

        public ucThucDon()
        {
            InitializeComponent();
            SetupUI();
            LoadCategory();
        }

        void SetupUI()
        {
            this.BackColor = Color.WhiteSmoke;
            SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 400 };

            // --- TRÁI: DANH MỤC ---
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Label lblCat = new Label { Text = "DANH MỤC THỰC ĐƠN", Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = UIHelper.PrimaryColor };

            dgvCategory = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvCategory);
            dgvCategory.CellClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    selectedCatID = dgvCategory.Rows[e.RowIndex].Cells["MaThucDon"].Value.ToString();
                    LoadDish(selectedCatID);
                }
            };

            // Nút thao tác Danh mục
            FlowLayoutPanel flpCatAction = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50 };
            Button btnAddCat = CreateButton("Thêm Nhóm", (s, e) => ActionCategory("ADD"));
            Button btnEditCat = CreateButton("Sửa Nhóm", (s, e) => ActionCategory("EDIT"));
            Button btnDelCat = CreateButton("Xóa Nhóm", (s, e) => ActionCategory("DEL"));
            flpCatAction.Controls.AddRange(new Control[] { btnAddCat, btnEditCat, btnDelCat });

            pnlLeft.Controls.Add(dgvCategory);
            pnlLeft.Controls.Add(flpCatAction);
            pnlLeft.Controls.Add(lblCat);

            // --- PHẢI: MÓN ĂN ---
            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            Label lblDish = new Label { Text = "DANH SÁCH MÓN ĂN", Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = UIHelper.PrimaryColor };

            dgvDish = new DataGridView { Dock = DockStyle.Fill };
            UIHelper.StyleDataGridView(dgvDish);

            // Nút thao tác Món ăn
            FlowLayoutPanel flpDishAction = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50 };
            Button btnAddDish = CreateButton("Thêm Món", (s, e) => ActionDish("ADD"));
            Button btnEditDish = CreateButton("Sửa Món", (s, e) => ActionDish("EDIT"));
            Button btnDelDish = CreateButton("Xóa Món", (s, e) => ActionDish("DEL"));
            flpDishAction.Controls.AddRange(new Control[] { btnAddDish, btnEditDish, btnDelDish });

            pnlRight.Controls.Add(dgvDish);
            pnlRight.Controls.Add(flpDishAction);
            pnlRight.Controls.Add(lblDish);

            split.Panel1.Controls.Add(pnlLeft);
            split.Panel2.Controls.Add(pnlRight);
            this.Controls.Add(split);
        }

        Button CreateButton(string text, EventHandler click)
        {
            Button btn = new Button { Text = text, Width = 100, Height = 35, Margin = new Padding(3) };
            UIHelper.StyleButton(btn, text.Contains("Thêm")); // Màu cam cho nút Thêm
            btn.Click += click;
            return btn;
        }

        // --- LOGIC LOAD DATA ---
        void LoadCategory()
        {
            dgvCategory.DataSource = bll.GetListThucDon();
        }

        void LoadDish(string maTD)
        {
            dgvDish.DataSource = bll.GetListMonAn(maTD);
        }

        // --- LOGIC CRUD DANH MỤC ---
        void ActionCategory(string action)
        {
            if (action == "ADD")
            {
                string ten = Microsoft.VisualBasic.Interaction.InputBox("Tên nhóm:", "Thêm Nhóm");
                string mota = Microsoft.VisualBasic.Interaction.InputBox("Mô tả:", "Thêm Nhóm");
                if (!string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show(bll.AddCategory(ten, mota));
                    LoadCategory();
                }
            }
            else if (selectedCatID != null)
            {
                if (action == "DEL")
                {
                    if (MessageBox.Show("Xóa nhóm này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MessageBox.Show(bll.DeleteCategory(selectedCatID));
                        LoadCategory();
                        dgvDish.DataSource = null;
                    }
                }
                // Edit tương tự...
            }
        }

        // --- LOGIC CRUD MÓN ĂN ---
        void ActionDish(string action)
        {
            if (selectedCatID == null) { MessageBox.Show("Chọn nhóm thực đơn trước!"); return; }

            if (action == "ADD")
            {
                // Demo nhập liệu nhanh (Thực tế nên làm Form riêng)
                string ten = Microsoft.VisualBasic.Interaction.InputBox("Tên món:", "Thêm Món");
                string gia = Microsoft.VisualBasic.Interaction.InputBox("Giá:", "Thêm Món", "0");
                string diem = Microsoft.VisualBasic.Interaction.InputBox("Điểm thưởng:", "Thêm Món", "0");

                if (!string.IsNullOrEmpty(ten))
                {
                    MonAnDTO m = new MonAnDTO
                    {
                        MaThucDon = selectedCatID,
                        TenMonAn = ten,
                        Gia = decimal.Parse(gia),
                        DiemThuong = int.Parse(diem)
                    };
                    MessageBox.Show(bll.AddDish(m));
                    LoadDish(selectedCatID);
                }
            }
            else if (dgvDish.CurrentRow != null)
            {
                string maMon = dgvDish.CurrentRow.Cells["MaMonAn"].Value.ToString();
                if (action == "DEL")
                {
                    if (MessageBox.Show("Xóa món này?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MessageBox.Show(bll.DeleteDish(maMon));
                        LoadDish(selectedCatID);
                    }
                }
                // Edit tương tự...
            }
        }
    }
}