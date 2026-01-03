using System;
using System.Windows.Forms;
using DTO;

namespace GUI
{
    public partial class frmAddBan : Form
    {
        private BanDTO _ban;
        private bool isEdit;

        public BanDTO BanData { get; private set; }

        public frmAddBan(BanDTO ban = null)
        {
            InitializeComponent();
            _ban = ban;
            isEdit = (ban != null);

            UIHelper.SetupDialog(this, isEdit ? "Sửa bàn" : "Thêm bàn");

            LoadData();
            btnLuu.Click += BtnLuu_Click;
        }

        private void LoadData()
        {
            if (isEdit)
            {
                txtTen.Text = _ban.Ten;
                cboLoai.SelectedItem = _ban.Loai;
            }
            else
            {
                if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            if (cboLoai.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BanData = new BanDTO
            {
                MaBan = isEdit ? _ban.MaBan : null,
                Ten = txtTen.Text.Trim(),
                Loai = cboLoai.Text,
                TrangThai = isEdit ? _ban.TrangThai : "Trống"
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}