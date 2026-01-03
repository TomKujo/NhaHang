using BLL;
using DTO;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucKhuyenMai : UserControl
    {
        private ServiceBLL bll = new ServiceBLL();
        private System.Windows.Forms.Timer refreshTimer;
        private NguoiDungDTO currentUser;

        public ucKhuyenMai(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.Load += UcKhuyenMai_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 60000;
            refreshTimer.Tick += (s, e) => LoadData();
            refreshTimer.Start();
        }

        private void UcKhuyenMai_Load(object sender, EventArgs e)
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            UIHelper.StyleDataGridView(dgvKhuyenMai);
            string[] headers = { "Mã KM", "Tên Khuyến Mãi", "Mô Tả", "Điểm Yêu Cầu", "Giá Trị Giảm", "Loại Giảm", "Thời Gian Bắt Đầu", "Thời Gian Kết Thúc", "Trạng Thái" };
            string[] dataFields = { "MaKM", "Ten", "MoTa", "DiemCan", "GiaTriGiam", "LoaiGiam", "NgayBD", "NgayKT", "TrangThai" };
            int[] widths = { 80, 200, 0, 130, 130, 100, 140, 150, 120 };
            UIHelper.SetGridColumns(dgvKhuyenMai, headers, dataFields, widths);
            UIHelper.StyleButton(btnThem, true);
            UIHelper.StyleButton(btnSua, false);
            UIHelper.StyleButton(btnXoa, false);
            if (currentUser != null)
            {
                if (currentUser.VaiTro == "Thu ngân")
                {
                    btnXoa.Visible = false;
                }
            }
        }

        private void LoadData()
        {
            dgvKhuyenMai.DataSource = bll.GetListKhuyenMai();
        }

        private void ShowAddEditForm(KhuyenMaiDTO km, Button sender)
        {
            Point btnLocation = sender.PointToScreen(new Point(0, sender.Height + 5));
            frmAddEditKM f = new frmAddEditKM(km);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Location = btnLocation;
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ShowAddEditForm(null, (Button)sender);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhuyenMai.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvKhuyenMai.SelectedRows[0];
                KhuyenMaiDTO selectedKM = new KhuyenMaiDTO
                {
                    MaKM = row.Cells["MaKM"].Value.ToString(),
                    Ten = row.Cells["Ten"].Value.ToString(),
                    MoTa = row.Cells["MoTa"].Value.ToString(),
                    DiemCan = Convert.ToInt32(row.Cells["DiemCan"].Value),
                    GiaTriGiam = Convert.ToDecimal(row.Cells["GiaTriGiam"].Value),
                    LoaiGiam = row.Cells["LoaiGiam"].Value.ToString(),
                    NgayBD = Convert.ToDateTime(row.Cells["NgayBD"].Value),
                    NgayKT = row.Cells["NgayKT"].Value != DBNull.Value ? (DateTime?)row.Cells["NgayKT"].Value : null,
                    TrangThai = row.Cells["TrangThai"].Value.ToString()
                };

                ShowAddEditForm(selectedKM, (Button)sender);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhuyenMai.SelectedRows.Count > 0)
            {
                string maKM = dgvKhuyenMai.SelectedRows[0].Cells["MaKM"].Value.ToString();
                string tenKM = dgvKhuyenMai.SelectedRows[0].Cells["Ten"].Value.ToString();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khuyến mãi '{tenKM}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string message = bll.DeleteKhuyenMai(maKM);
                    MessageBox.Show(message);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}