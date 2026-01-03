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
            this.lblVaiTro = new System.Windows.Forms.Label();
            this.cboVaiTro = new System.Windows.Forms.ComboBox();
            this.lblDoiMatKhau = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.pnlContainer.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.White;
            this.pnlContainer.Controls.Add(this.lblTitle);
            this.pnlContainer.Controls.Add(this.lblTen);
            this.pnlContainer.Controls.Add(this.txtTen);
            this.pnlContainer.Controls.Add(this.lblSDT);
            this.pnlContainer.Controls.Add(this.txtSDT);
            this.pnlContainer.Controls.Add(this.lblEmail);
            this.pnlContainer.Controls.Add(this.txtEmail);
            this.pnlContainer.Controls.Add(this.lblDiaChi);
            this.pnlContainer.Controls.Add(this.txtDiaChi);
            this.pnlContainer.Controls.Add(this.lblVaiTro);
            this.pnlContainer.Controls.Add(this.cboVaiTro);
            this.pnlContainer.Controls.Add(this.lblDoiMatKhau);
            this.pnlContainer.Controls.Add(this.pnlBtn);
            this.pnlContainer.Location = new System.Drawing.Point(20, 60);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(650, 480);
            this.pnlContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblTitle.ForeColor = System.Drawing.Color.Orange;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(199, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin tài khoản";
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTen.Location = new System.Drawing.Point(30, 80);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(70, 19);
            this.lblTen.TabIndex = 1;
            this.lblTen.Text = "Họ và tên:";
            // 
            // txtTen
            // 
            this.txtTen.Enabled = false;
            this.txtTen.Location = new System.Drawing.Point(30, 105);
            this.txtTen.Name = "txtTen";
            this.txtTen.ReadOnly = true;
            this.txtTen.Size = new System.Drawing.Size(280, 20);
            this.txtTen.TabIndex = 2;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSDT.Location = new System.Drawing.Point(340, 80);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(122, 19);
            this.lblSDT.TabIndex = 3;
            this.lblSDT.Text = "Số điện thoại:";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(340, 105);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(280, 20);
            this.txtSDT.TabIndex = 4;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.Location = new System.Drawing.Point(30, 150);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(44, 19);
            this.lblEmail.TabIndex = 5;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Enabled = false;
            this.txtEmail.Location = new System.Drawing.Point(30, 175);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(590, 20);
            this.txtEmail.TabIndex = 6;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDiaChi.Location = new System.Drawing.Point(30, 220);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(53, 19);
            this.lblDiaChi.TabIndex = 7;
            this.lblDiaChi.Text = "Địa chỉ:";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(30, 245);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(590, 20);
            this.txtDiaChi.TabIndex = 8;
            // 
            // lblVaiTro
            // 
            this.lblVaiTro.AutoSize = true;
            this.lblVaiTro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVaiTro.Location = new System.Drawing.Point(30, 290);
            this.lblVaiTro.Name = "lblVaiTro";
            this.lblVaiTro.Size = new System.Drawing.Size(52, 19);
            this.lblVaiTro.TabIndex = 11;
            this.lblVaiTro.Text = "Vai trò:";
            // 
            // cboVaiTro
            // 
            this.cboVaiTro.Enabled = false;
            this.cboVaiTro.Location = new System.Drawing.Point(30, 315);
            this.cboVaiTro.Name = "cboVaiTro";
            this.cboVaiTro.Size = new System.Drawing.Size(590, 20);
            this.cboVaiTro.TabIndex = 12;
            // 
            // lblDoiMatKhau
            // 
            this.lblDoiMatKhau.AutoSize = false;
            this.lblDoiMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDoiMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))));
            this.lblDoiMatKhau.ForeColor = System.Drawing.Color.Blue;
            this.lblDoiMatKhau.Location = new System.Drawing.Point(0, 360);
            this.lblDoiMatKhau.Name = "lblDoiMatKhau";
            this.lblDoiMatKhau.Size = new System.Drawing.Size(650, 30);
            this.lblDoiMatKhau.TabIndex = 13;
            this.lblDoiMatKhau.Text = "Đổi mật khẩu";
            this.lblDoiMatKhau.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDoiMatKhau.Click += new System.EventHandler(this.lblDoiMatKhau_Click);
            // 
            // pnlBtn
            // 
            this.pnlBtn.Controls.Add(this.btnCapNhat);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 410);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.Size = new System.Drawing.Size(650, 70);
            this.pnlBtn.TabIndex = 14;
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCapNhat.Enabled = false;
            this.btnCapNhat.Location = new System.Drawing.Point(20, 10);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(610, 50);
            this.btnCapNhat.TabIndex = 0;
            this.btnCapNhat.Text = "LƯU";
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // ucTaiKhoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pnlContainer);
            this.Name = "ucTaiKhoan";
            this.Size = new System.Drawing.Size(900, 600);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblVaiTro;
        private System.Windows.Forms.ComboBox cboVaiTro;
        private System.Windows.Forms.Label lblDoiMatKhau;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel pnlBtn;
    }
}