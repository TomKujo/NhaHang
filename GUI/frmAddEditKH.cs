using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DTO;

namespace GUI
{
    public partial class frmAddEditKH : Form
    {
        private KhachHangDTO _kh;
        private bool isEdit;

        public frmAddEditKH(KhachHangDTO kh)
        {
            InitializeComponent();
            _kh = kh;
            isEdit = kh != null;
            UIHelper.SetupDialog(this, isEdit ? "Sửa khách hàng" : "Thêm khách hàng");
            if (isEdit) { txtTen.Text = kh.Ten; txtSDT.Text = kh.SDT; txtEmail.Text = kh.Email; }
            btnLuu.Click += BtnLuu_Click;
        }

        public KhachHangDTO KhachHangData { get; private set; }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và Email!");
                return;
            }

            KhachHangData = new KhachHangDTO
            {
                MaKH = isEdit ? _kh.MaKH : null,
                Ten = txtTen.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };

            this.DialogResult = DialogResult.OK;
        }
    }
}