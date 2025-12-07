namespace GUI
{
    partial class ucTaiKhoan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();

            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();

            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();

            this.lblLuong = new System.Windows.Forms.Label();
            this.txtLuong = new System.Windows.Forms.TextBox();

            this.lblVaiTro = new System.Windows.Forms.Label();
            this.cboVaiTro = new System.Windows.Forms.ComboBox();

            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.lblHienMatKhau = new System.Windows.Forms.Label();
            this.lblDoiMatKhau = new System.Windows.Forms.Label();

            this.btnCapNhat = new System.Windows.Forms.Button();

            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlContainer (Panel trắng bo góc chứa nội dung)
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.White;
            this.pnlContainer.Location = new System.Drawing.Point(20, 60);
            this.pnlContainer.Size = new System.Drawing.Size(650, 480);
            this.pnlContainer.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Orange; // Primary Color
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Thông tin tài khoản";

            // --- Hàng 1: Tên ---
            this.lblTen.Text = "Họ và tên:";
            this.lblTen.Location = new System.Drawing.Point(30, 80);
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtTen.Location = new System.Drawing.Point(30, 105);
            this.txtTen.Size = new System.Drawing.Size(250, 30);
            this.txtTen.ReadOnly = true;
            this.txtTen.Enabled = false; // Yêu cầu disable

            // --- Hàng 1: SDT ---
            this.lblSDT.Text = "Số điện thoại:";
            this.lblSDT.Location = new System.Drawing.Point(310, 80);
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtSDT.Location = new System.Drawing.Point(310, 105);
            this.txtSDT.Size = new System.Drawing.Size(250, 30);
            this.txtSDT.ReadOnly = true;
            this.txtSDT.Enabled = false;

            // --- Hàng 2: Địa chỉ (Cho phép sửa) ---
            this.lblDiaChi.Text = "Địa chỉ:";
            this.lblDiaChi.Location = new System.Drawing.Point(30, 150);
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtDiaChi.Location = new System.Drawing.Point(30, 175);
            this.txtDiaChi.Size = new System.Drawing.Size(530, 30);
            this.txtDiaChi.TextChanged += new System.EventHandler(this.txtDiaChi_TextChanged);

            // --- Hàng 3: Lương ---
            this.lblLuong.Text = "Lương cơ bản:";
            this.lblLuong.Location = new System.Drawing.Point(30, 220);
            this.lblLuong.AutoSize = true;
            this.lblLuong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtLuong.Location = new System.Drawing.Point(30, 245);
            this.txtLuong.Size = new System.Drawing.Size(250, 30);
            this.txtLuong.ReadOnly = true;
            this.txtLuong.Enabled = false;
            this.txtLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // --- Hàng 3: Vai trò ---
            this.lblVaiTro.Text = "Vai trò:";
            this.lblVaiTro.Location = new System.Drawing.Point(310, 220);
            this.lblVaiTro.AutoSize = true;
            this.lblVaiTro.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.cboVaiTro.Location = new System.Drawing.Point(310, 245);
            this.cboVaiTro.Size = new System.Drawing.Size(250, 30);
            this.cboVaiTro.Enabled = false;

            // --- Hàng 4: Mật khẩu ---
            this.lblMatKhau.Text = "Mật khẩu:";
            this.lblMatKhau.Location = new System.Drawing.Point(30, 290);
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtMatKhau.Location = new System.Drawing.Point(30, 315);
            this.txtMatKhau.Size = new System.Drawing.Size(250, 30);
            this.txtMatKhau.ReadOnly = true;
            this.txtMatKhau.UseSystemPasswordChar = true; // Hiện dấu *

            // Label Hiện mật khẩu
            this.lblHienMatKhau.Text = "Hiện mật khẩu";
            this.lblHienMatKhau.Location = new System.Drawing.Point(30, 350);
            this.lblHienMatKhau.AutoSize = true;
            this.lblHienMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))));
            this.lblHienMatKhau.ForeColor = System.Drawing.Color.Blue;
            this.lblHienMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblHienMatKhau.Click += new System.EventHandler(this.lblHienMatKhau_Click);

            // Label Đổi mật khẩu
            this.lblDoiMatKhau.Text = "Đổi mật khẩu?";
            this.lblDoiMatKhau.Location = new System.Drawing.Point(180, 350); // Cạnh nút hiện MK
            this.lblDoiMatKhau.AutoSize = true;
            this.lblDoiMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))));
            this.lblDoiMatKhau.ForeColor = System.Drawing.Color.Blue;
            this.lblDoiMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDoiMatKhau.Click += new System.EventHandler(this.lblDoiMatKhau_Click);

            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Text = "Cập nhật thông tin";
            this.btnCapNhat.Location = new System.Drawing.Point(30, 390);
            this.btnCapNhat.Size = new System.Drawing.Size(150, 40);
            this.btnCapNhat.Enabled = false; // Ban đầu disable
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);

            // Add controls to Panel
            this.pnlContainer.Controls.Add(lblTitle);
            this.pnlContainer.Controls.Add(lblTen); this.pnlContainer.Controls.Add(txtTen);
            this.pnlContainer.Controls.Add(lblSDT); this.pnlContainer.Controls.Add(txtSDT);
            this.pnlContainer.Controls.Add(lblDiaChi); this.pnlContainer.Controls.Add(txtDiaChi);
            this.pnlContainer.Controls.Add(lblLuong); this.pnlContainer.Controls.Add(txtLuong);
            this.pnlContainer.Controls.Add(lblVaiTro); this.pnlContainer.Controls.Add(cboVaiTro);
            this.pnlContainer.Controls.Add(lblMatKhau); this.pnlContainer.Controls.Add(txtMatKhau);
            this.pnlContainer.Controls.Add(lblHienMatKhau);
            this.pnlContainer.Controls.Add(lblDoiMatKhau);
            this.pnlContainer.Controls.Add(btnCapNhat);

            this.Controls.Add(this.pnlContainer);
            this.Size = new System.Drawing.Size(900, 600);
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTen;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblLuong;
        private System.Windows.Forms.TextBox txtLuong;
        private System.Windows.Forms.Label lblVaiTro;
        private System.Windows.Forms.ComboBox cboVaiTro;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Label lblHienMatKhau;
        private System.Windows.Forms.Label lblDoiMatKhau;
        private System.Windows.Forms.Button btnCapNhat;
    }
}