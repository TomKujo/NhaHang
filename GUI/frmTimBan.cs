using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmTimBan : Form
    {
        public string TenBan { get; private set; }
        public string LoaiBan { get; private set; }
        public string TrangThai { get; private set; }

        public frmTimBan()
        {
            InitializeComponent();
            UIHelper.SetupDialog(this, "Tìm kiếm bàn");

            cboLoai.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;

            btnTim.Click += BtnTim_Click;
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == pageLoai)
            {
                LoaiBan = cboLoai.Text;
                TrangThai = cboTrangThai.Text;
                TenBan = null;
            }
            else
            {
                TenBan = txtTen.Text.Trim();
                LoaiBan = null;
                TrangThai = null;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}