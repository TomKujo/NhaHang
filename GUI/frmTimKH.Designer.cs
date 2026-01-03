namespace GUI
{
    partial class frmTimKH
    {
        private System.Windows.Forms.TabControl tabControlSearch;
        private System.Windows.Forms.TabPage tabTen;
        private System.Windows.Forms.TabPage tabSDT;
        private System.Windows.Forms.TabPage tabEmail;
        public System.Windows.Forms.TextBox txtTen;
        public System.Windows.Forms.TextBox txtSDT;
        public System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Panel pnlBtn;

        private void InitializeComponent()
        {
            this.tabControlSearch = new System.Windows.Forms.TabControl();
            this.tabTen = new System.Windows.Forms.TabPage();
            this.tabSDT = new System.Windows.Forms.TabPage();
            this.tabEmail = new System.Windows.Forms.TabPage();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnTim = new System.Windows.Forms.Button();
            this.pnlBtn = new System.Windows.Forms.Panel();
            // 
            // tabControlSearch
            // 
            this.tabControlSearch.Controls.Add(this.tabTen);
            this.tabControlSearch.Controls.Add(this.tabSDT);
            this.tabControlSearch.Controls.Add(this.tabEmail);
            this.tabControlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlSearch.Location = new System.Drawing.Point(0, 0);
            this.tabControlSearch.Name = "tabControlSearch";
            this.tabControlSearch.SelectedIndex = 0;
            this.tabControlSearch.Size = new System.Drawing.Size(400, 125);
            this.tabControlSearch.TabIndex = 0;
            SetupTab(tabTen, txtTen, "Tên khách hàng:", "tabTen");
            SetupTab(tabSDT, txtSDT, "Số điện thoại:", "tabSDT");
            SetupTab(tabEmail, txtEmail, "Email:", "tabEmail");
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnTim);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Height = 70;
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.TabIndex = 1;
            // 
            // btnTim
            // 
            this.btnTim.BackColor = System.Drawing.Color.SeaGreen;
            this.btnTim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTim.FlatAppearance.BorderSize = 0;
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Text = "TÌM";
            this.btnTim.UseVisualStyleBackColor = false;
            //
            // frmTimKH
            //
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.tabControlSearch);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this.pnlBtn);
        }

        private void SetupTab(System.Windows.Forms.TabPage tab, System.Windows.Forms.TextBox txt, string lblText, string tabName)
        {
            tab.Text = tabName == "tabTen" ? "Tên" : (tabName == "tabSDT" ? "SĐT" : "Email");
            tab.Name = tabName;
            tab.BackColor = System.Drawing.Color.White;

            var lbl = new System.Windows.Forms.Label { Text = lblText, Location = new System.Drawing.Point(20, 15), AutoSize = true };
            txt.Location = new System.Drawing.Point(20, 40);
            txt.Size = new System.Drawing.Size(340, 30);

            tab.Controls.Add(lbl);
            tab.Controls.Add(txt);
        }
    }
}