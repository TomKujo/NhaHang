using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmEditChiTietNhap : Form
    {
        public CartItemViewModel ResultItem { get; private set; }

        private string _maNL;
        private string _tenNL;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maNL">Mã nguyên liệu</param>
        /// <param name="tenNL">Tên nguyên liệu</param>
        /// <param name="editingItem">Nếu null là thêm mới, có giá trị là sửa</param>
        public frmEditChiTietNhap(string maNL, string tenNL, CartItemViewModel editingItem = null)
        {
            InitializeComponent();

            this._maNL = maNL;
            this._tenNL = tenNL;

            UIHelper.SetupDialog(this, editingItem != null ? $"Sửa: {tenNL}" : $"Nhập: {tenNL}");
            btnSave.Text = editingItem != null ? "LƯU" : "LƯU";

            if (editingItem != null)
            {
                txtYC.Text = editingItem.LuongYeuCau.ToString();
                txtGia.Text = editingItem.DonGia.ToString("N0");
                cboTT.SelectedItem = editingItem.TinhTrang;
                txtTT_SL.Text = editingItem.LuongThucTe.ToString();

                if (editingItem.TinhTrang == "Thiếu")
                    txtTT_SL.Enabled = true;
            }
            else
            {
                txtYC.Text = "1";
                txtGia.Text = "0";
                cboTT.SelectedIndex = 0;
                txtTT_SL.Text = "1";
            }
        }

        private void TxtYC_TextChanged(object sender, EventArgs e)
        {
            if (cboTT.SelectedIndex == 0)
            {
                txtTT_SL.Text = txtYC.Text;
            }
        }

        private void CboTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTT.SelectedIndex == 0)
            {
                txtTT_SL.Enabled = false;
                txtTT_SL.Text = txtYC.Text;
            }
            else
            {
                txtTT_SL.Enabled = true;
                txtTT_SL.Focus();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtYC.Text, out decimal yc) || yc <= 0)
            {
                MessageBox.Show("Lượng yêu cầu không hợp lệ!");
                return;
            }

            string strGia = txtGia.Text.Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(strGia, out decimal gia) || gia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return;
            }

            if (!decimal.TryParse(txtTT_SL.Text, out decimal tt) || tt < 0)
            {
                MessageBox.Show("Lượng thực tế không hợp lệ!");
                return;
            }

            string tinhTrang = cboTT.Text;
            if (tinhTrang == "Thiếu")
            {
                if (tt >= yc)
                {
                    MessageBox.Show("Bạn đang chọn tình trạng 'Thiếu' nhưng Lượng thực tế lại lớn hơn hoặc bằng Lượng yêu cầu.\n\nVui lòng kiểm tra lại số lượng!",
                                    "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTT_SL.Focus();
                    return;
                }
            }
            else
            {
                tt = yc;
            }

            ResultItem = new CartItemViewModel
            {
                MaNguyenLieu = _maNL,
                Ten = _tenNL,
                LuongYeuCau = yc,
                LuongThucTe = tt,
                DonGia = gia,
                TinhTrang = tinhTrang
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}