using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmTimKH : Form
    {
        public string Keyword { get; private set; }
        public string SearchType { get; private set; }

        public frmTimKH()
        {
            InitializeComponent();
            UIHelper.SetupDialog(this, "Tìm kiếm khách hàng");

            btnTim.Click += BtnTim_Click;
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            if (tabControlSearch.SelectedTab == tabTen)
            {
                Keyword = txtTen.Text.Trim();
                SearchType = "TEN";
            }
            else if (tabControlSearch.SelectedTab == tabSDT)
            {
                Keyword = txtSDT.Text.Trim();
                SearchType = "SDT";
            }
            else
            {
                Keyword = txtEmail.Text.Trim();
                SearchType = "EMAIL";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}