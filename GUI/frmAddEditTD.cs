using System;
using System.Windows.Forms;
using DTO;

namespace GUI
{
    public partial class frmAddEditTD : Form
    {
        private ThucDonDTO _td;
        private bool isEdit;

        public ThucDonDTO ThucDonData { get; private set; }

        public frmAddEditTD(ThucDonDTO td = null)
        {
            InitializeComponent();
            _td = td;
            isEdit = (td != null);

            UIHelper.SetupDialog(this, isEdit ? "Sửa thực đơn" : "Thêm thực đơn");

            LoadData();
            btnLuu.Click += BtnLuu_Click;
        }

        private void LoadData()
        {
            if (isEdit)
            {
                txtTen.Text = _td.Ten;
                txtMoTa.Text = _td.MoTa;
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thực đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            ThucDonData = new ThucDonDTO
            {
                MaThucDon = isEdit ? _td.MaThucDon : null,
                Ten = txtTen.Text.Trim(),
                MoTa = txtMoTa.Text.Trim()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}