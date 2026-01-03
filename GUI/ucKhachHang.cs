using BLL;
using DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucKhachHang : UserControl
    {
        private ServiceBLL bll = new ServiceBLL();

        public ucKhachHang()
        {
            InitializeComponent();
            this.Load += (s, e) => SetupUI();
            btnThem.Click += (s, e) => ShowForm(null);
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnTim.Click += BtnTim_Click;
        }

        private void SetupUI()
        {
            string[] headers = { "Mã KH", "Tên Khách Hàng", "Số Điện Thoại", "Email", "Điểm Tích Lũy" };
            string[] fields = { "MaKH", "Ten", "SDT", "Email", "DiemTichLuy" };
            UIHelper.SetGridColumns(dgvKhachHang, headers, fields);
            LoadData();
        }

        private void LoadData() => dgvKhachHang.DataSource = bll.dal.GetListKhachHang();

        private void ShowForm(KhachHangDTO kh)
        {
            using (frmAddEditKH f = new frmAddEditKH(kh))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    bool success;
                    if (kh == null)
                        success = bll.dal.InsertKhachHang(f.KhachHangData);
                    else
                        success = bll.dal.UpdateKhachHang(f.KhachHangData);

                    if (success) MessageBox.Show("Cập nhật dữ liệu thành công!");
                    LoadData();
                }
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                var row = dgvKhachHang.SelectedRows[0];
                var kh = new KhachHangDTO
                {
                    MaKH = row.Cells["MaKH"]?.Value?.ToString(),
                    Ten = row.Cells["Ten"].Value.ToString(),
                    SDT = row.Cells["SDT"].Value.ToString(),
                    Email = row.Cells["Email"].Value.ToString()
                };
                ShowForm(kh);
            }
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            using (frmTimKH frm = new frmTimKH())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string k = frm.Keyword;
                    string type = frm.SearchType;

                    var dt = bll.FindCustomers(k, type);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dgvKhachHang.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy khách hàng nào theo {type}: {k}",
                                        "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                string ma = dgvKhachHang.SelectedRows[0].Cells["MaKH"].Value.ToString();
                string ten = dgvKhachHang.SelectedRows[0].Cells["Ten"].Value.ToString();

                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {ten}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (bll.dal.DeleteKhachHang(ma))
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadData();
                    }
                    else MessageBox.Show("Không thể xóa khách hàng này (có thể đã có lịch sử hóa đơn)!");
                }
            }
        }
    }
}