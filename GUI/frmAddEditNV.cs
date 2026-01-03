using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace GUI
{
    public partial class frmAddEditNV : Form
    {
        private ServiceBLL bll;
        private string currentMaNV;
        private string currentTenNV;
        private CultureInfo viVN = new CultureInfo("vi-VN");

        public frmAddEditNV(string maNV, ServiceBLL serviceBLL)
        {
            InitializeComponent();

            viVN.NumberFormat.CurrencyDecimalDigits = 0;
            viVN.NumberFormat.NumberGroupSeparator = ".";

            this.currentMaNV = maNV;
            this.bll = serviceBLL;

            SetupInitialLogic();

            if (maNV == null)
            {
                this.Text = "Thêm nhân viên";
            }
            else
            {
                this.Text = "Sửa nhân viên";
                this.Height = 430;
            }
        }

        private void SetupInitialLogic()
        {
            cmbVaiTro.Items.AddRange(new string[] { "Phục vụ", "Thu ngân", "Quản lý" });
            cmbVaiTro.SelectedIndex = 0;

            cmbTrangThai.Items.AddRange(new string[] { "Kích hoạt", "Khóa" });
            cmbTrangThai.SelectedIndex = 0;

            if (currentMaNV == null)
            {
                lblTrangThai.Visible = false;
                cmbTrangThai.Visible = false;
                this.Height = 460;
            }
            else
            {
                lblHoTen.Visible = false; txtHoTen.Visible = false;
                lblEmail.Visible = false; txtEmail.Visible = false;

                int shiftSDT = -50;
                MoveControlUp(lblSDT, shiftSDT);
                MoveControlUp(txtSDT, shiftSDT);

                int yShift = 90;
                MoveControlUp(lblSDT, yShift); MoveControlUp(txtSDT, yShift);
                MoveControlUp(lblDiaChi, yShift); MoveControlUp(txtDiaChi, yShift);
                MoveControlUp(lblLuong, yShift); MoveControlUp(txtLuong, yShift);
                MoveControlUp(lblVaiTro, yShift); MoveControlUp(cmbVaiTro, yShift);
                MoveControlUp(lblTrangThai, yShift); MoveControlUp(cmbTrangThai, yShift);

                this.Height = 400;
            }
        }

        private void MoveControlUp(Control c, int y)
        {
            c.Location = new Point(c.Location.X, c.Location.Y - y);
        }

        private void txtLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLuong_Leave(object sender, EventArgs e)
        {
            string raw = txtLuong.Text.Replace(".", "").Replace(",", "").Trim();
            if (decimal.TryParse(raw, out decimal v))
            {
                txtLuong.Text = v.ToString("N0", viVN);
            }
        }

        public void SetData(string maNV, string ten, string sdt, string diaChi, string trangThaiTK, string vaiTroTK, string luong, string email)
        {
            this.currentTenNV = ten;

            txtHoTen.Text = ten;
            txtSDT.Text = sdt;
            txtDiaChi.Text = diaChi;
            txtEmail.Text = email;

            decimal val = 0;
            try
            {
                val = Convert.ToDecimal(luong);
            }
            catch
            {
                val = 0;
            }

            txtLuong.Text = val.ToString("N0", viVN);

            if (cmbTrangThai.Items.Contains(trangThaiTK)) cmbTrangThai.SelectedItem = trangThaiTK;
            if (cmbVaiTro.Items.Contains(vaiTroTK)) cmbVaiTro.SelectedItem = vaiTroTK;

            txtSDT.BackColor = Color.White;
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSDT.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text)) { MessageBox.Show("Vui lòng nhập đủ Tên, SĐT, Email!"); return; }

            string luongRaw = txtLuong.Text.Replace(".", "").Replace(",", "").Replace(" VNĐ", "").Trim();
            decimal.TryParse(luongRaw, out decimal luongInput);

            NhanVienDTO nv = new NhanVienDTO
            {
                MaNV = currentMaNV,
                Ten = txtHoTen.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                Luong = luongInput,
                VaiTro = cmbVaiTro.SelectedItem.ToString(),
                TrangThaiTK = currentMaNV == null ? "Kích hoạt" : cmbTrangThai.SelectedItem.ToString()
            };

            string msg = "";
            if (currentMaNV == null)
            {
                msg = bll.AddStaff(nv);
            }
            else
            {
                msg = bll.UpdateStaff(nv);
            }

            MessageBox.Show(msg);

            if (msg.Contains("thành công"))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}