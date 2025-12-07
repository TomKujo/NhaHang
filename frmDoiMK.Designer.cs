namespace GUI
{
    partial class frmDoiMK
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.btnGuiOTP = new System.Windows.Forms.Button();

            this.lblPassMoi = new System.Windows.Forms.Label();
            this.txtPassMoi = new System.Windows.Forms.TextBox();

            this.lblXacNhan = new System.Windows.Forms.Label();
            this.txtXacNhan = new System.Windows.Forms.TextBox();

            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.Location = new System.Drawing.Point(20, 20);
            this.lblOTP.Text = "Mã xác thực (OTP):";
            this.lblOTP.Font = new System.Drawing.Font("Segoe UI", 10F);

            // 
            // txtOTP
            // 
            this.txtOTP.Location = new System.Drawing.Point(20, 45);
            this.txtOTP.Size = new System.Drawing.Size(150, 25);

            // 
            // btnGuiOTP
            // 
            this.btnGuiOTP.Location = new System.Drawing.Point(180, 43);
            this.btnGuiOTP.Size = new System.Drawing.Size(120, 28);
            this.btnGuiOTP.Text = "Gửi OTP";
            this.btnGuiOTP.Click += new System.EventHandler(this.btnGuiOTP_Click);

            // 
            // lblPassMoi
            // 
            this.lblPassMoi.AutoSize = true;
            this.lblPassMoi.Location = new System.Drawing.Point(20, 90);
            this.lblPassMoi.Text = "Mật khẩu mới:";
            this.lblPassMoi.Font = new System.Drawing.Font("Segoe UI", 10F);

            // 
            // txtPassMoi
            // 
            this.txtPassMoi.Location = new System.Drawing.Point(20, 115);
            this.txtPassMoi.Size = new System.Drawing.Size(280, 25);
            this.txtPassMoi.UseSystemPasswordChar = true;

            // 
            // lblXacNhan
            // 
            this.lblXacNhan.AutoSize = true;
            this.lblXacNhan.Location = new System.Drawing.Point(20, 150);
            this.lblXacNhan.Text = "Nhập lại mật khẩu:";
            this.lblXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F);

            // 
            // txtXacNhan
            // 
            this.txtXacNhan.Location = new System.Drawing.Point(20, 175);
            this.txtXacNhan.Size = new System.Drawing.Size(280, 25);
            this.txtXacNhan.UseSystemPasswordChar = true;

            // 
            // btnXacNhan
            // 
            this.btnXacNhan.Location = new System.Drawing.Point(20, 220);
            this.btnXacNhan.Size = new System.Drawing.Size(130, 35);
            this.btnXacNhan.Text = "Xác nhận đổi";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);

            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(170, 220);
            this.btnHuy.Size = new System.Drawing.Size(130, 35);
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // frmDoiMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 280);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.txtXacNhan);
            this.Controls.Add(this.lblXacNhan);
            this.Controls.Add(this.txtPassMoi);
            this.Controls.Add(this.lblPassMoi);
            this.Controls.Add(this.btnGuiOTP);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lblOTP);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDoiMatKhau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button btnGuiOTP;
        private System.Windows.Forms.Label lblPassMoi;
        private System.Windows.Forms.TextBox txtPassMoi;
        private System.Windows.Forms.Label lblXacNhan;
        private System.Windows.Forms.TextBox txtXacNhan;
        private System.Windows.Forms.Button btnXacNhan;
        private System.Windows.Forms.Button btnHuy;
    }
}