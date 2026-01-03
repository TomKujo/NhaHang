using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmTimNV : Form
    {
        private Action<string> _onSearch;

        public frmTimNV(Action<string> onSearch)
        {
            InitializeComponent();
            this._onSearch = onSearch;
            cboStatus.SelectedIndex = 0;
        }

        private void btnTimTen_Click(object sender, EventArgs e)
        {
            string keyword = txtTuKhoa.Text.Trim();
            _onSearch?.Invoke(keyword);
            this.Close();
        }

        private void txtTuKhoa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimTen_Click(null, null);
                e.SuppressKeyPress = true;
            }
        }

        private void btnTimStatus_Click(object sender, EventArgs e)
        {
            string status = cboStatus.Text;
            _onSearch?.Invoke("STATUS:" + status);
            this.Close();
        }
    }
}