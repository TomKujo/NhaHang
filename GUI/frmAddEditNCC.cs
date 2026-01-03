using System;
using System.Windows.Forms;
using DTO;

namespace GUI
{
    public partial class frmAddEditNCC : Form
    {
        private NhaCungCapDTO _ncc;
        private bool isEdit;

        public NhaCungCapDTO NCCData { get; private set; }

        public frmAddEditNCC(NhaCungCapDTO ncc = null)
        {
            InitializeComponent();
            _ncc = ncc;
            isEdit = (ncc != null);

            UIHelper.SetupDialog(this, isEdit ? "Sửa nhà cung cấp" : "Thêm nhà cung cấp");

            LoadData();
            btnLuu.Click += BtnLuu_Click;
        }

        private void LoadData()
        {
            if (isEdit)
            {
                txtTen.Text = _ncc.Ten;
                txtDiaChi.Text = _ncc.DiaChi;
                txtSDT.Text = _ncc.SDT;
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            NCCData = new NhaCungCapDTO
            {
                MaNCC = isEdit ? _ncc.MaNCC : null,
                Ten = txtTen.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                SDT = txtSDT.Text.Trim()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}