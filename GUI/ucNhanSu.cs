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
        private Panel pnlTop;
        private string currentMaNV = null;

        public ucNhanSu()
        {
            InitializeComponent();
            SetupCustomControls();
            LoadData();
        }

        void SetupCustomControls()
        {
            this.BackColor = Color.WhiteSmoke;

            this.Controls.Remove(pnlTop);
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

        void LoadData(string keyword = "")
        {
            DataTable dt = bll.GetAllStaffFull();

            if (!string.IsNullOrEmpty(keyword))
            {
                string filter = "";

                if (keyword.StartsWith("STATUS:"))
                {
                    string statusVal = keyword.Replace("STATUS:", "");
                    filter = string.Format("TrangThai = '{0}'", statusVal);
                }
                else
                {
                    filter = string.Format("Ten LIKE '%{0}%'", keyword);
                }

                DataRow[] rows = dt.Select(filter);
                if (rows.Length > 0)
                {
                    dt = rows.CopyToDataTable();
                }
                else
                {
                    string displayKey = keyword.StartsWith("STATUS:") ? "Trạng thái: " + keyword.Replace("STATUS:", "") : "Tên: " + keyword;

                    MessageBox.Show($"Không tìm thấy nhân viên nào theo {displayKey}",
                                    "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            dgvNV.DataSource = dt;

            string[] headers = { "Mã NV", "Họ và tên", "Điện thoại", "Địa chỉ", "Email", "Lương", "Trạng thái TK", "Vai trò" };
            string[] dataFields = { "MaNV", "Ten", "SDT", "DiaChi", "Email", "Luong", "TrangThai", "VaiTro" };
            int[] widths = { 100, 0, 150, 300, 150, 150, 150, 150 };
            UIHelper.SetGridColumns(dgvNV, headers, dataFields);

            if (dgvNV.Columns["Luong"] != null)
            {
                dgvNV.Columns["Luong"].DefaultCellStyle.Format = "N0";
            }
        }

        void OpenInputForm(string maNV)
        {
            frmAddEditNV form = new frmAddEditNV(maNV, bll);

            UIHelper.SetupDialog(form, maNV == null ? "Thêm nhân viên" : "Sửa nhân viên");

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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenInputForm(null);
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNV.CurrentRow != null)
            {
                string maNV = dgvNV.CurrentRow.Cells["MaNV"].Value.ToString();
                OpenInputForm(maNV);
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            frmTimNV f = new frmTimNV((resultKeyword) =>
            {
                LoadData(resultKeyword);
            });
            f.ShowDialog();
        }
    }
}