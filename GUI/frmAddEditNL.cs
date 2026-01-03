using System;
using System.Windows.Forms;
using DTO;

namespace GUI
{
    public partial class frmAddEditNL : Form
    {
        private NguyenLieuDTO _nl;
        private bool isEdit;

        public NguyenLieuDTO NguyenLieuData { get; private set; }

        public frmAddEditNL(NguyenLieuDTO nl = null)
        {
            InitializeComponent();
            _nl = nl;
            isEdit = (nl != null);

            UIHelper.SetupDialog(this, isEdit ? "Sửa nguyên liệu" : "Thêm nguyên liệu");

            LoadData();
            btnLuu.Click += BtnLuu_Click;
        }

        private void LoadData()
        {
            if (isEdit)
            {
                txtTen.Text = _nl.Ten;
                cboDonVi.Text = _nl.DonVi;
            }
            else
            {
                if (cboDonVi.Items.Count > 0) cboDonVi.SelectedIndex = 0;
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nguyên liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cboDonVi.Text))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập đơn vị tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonVi.Focus();
                return;
            }

            NguyenLieuData = new NguyenLieuDTO
            {
                MaNguyenLieu = isEdit ? _nl.MaNguyenLieu : null,
                Ten = txtTen.Text.Trim(),
                DonVi = cboDonVi.Text.Trim(),
                SoLuongTon = isEdit ? _nl.SoLuongTon : 0
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}