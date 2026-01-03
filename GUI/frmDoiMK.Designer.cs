namespace GUI
{
    partial class frmDoiMK
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlButtonContainer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblOTP = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.btnGuiOTP = new System.Windows.Forms.Button();
            this.lblPassMoi = new System.Windows.Forms.Label();
            this.txtPassMoi = new System.Windows.Forms.TextBox();
            this.lblXacNhan = new System.Windows.Forms.Label();
            this.txtXacNhan = new System.Windows.Forms.TextBox();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.pnlButtonContainer = new System.Windows.Forms.Panel();
            this.pnlButtonContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.Location = new System.Drawing.Point(20, 15);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(44, 19);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(20, 37);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(290, 25);
            this.txtEmail.TabIndex = 1;
            // 
            // lblOTP
            // 
            this.lblOTP.AutoSize = true;
            this.lblOTP.Location = new System.Drawing.Point(20, 70);
            this.lblOTP.Text = "Mã xác thực (OTP):";
            this.lblOTP.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // txtOTP
            // 
            this.txtOTP.Location = new System.Drawing.Point(20, 92);
            this.txtOTP.Size = new System.Drawing.Size(150, 25);
            // 
            // btnGuiOTP
            // 
            this.btnGuiOTP.Location = new System.Drawing.Point(180, 91);
            this.btnGuiOTP.Size = new System.Drawing.Size(130, 28);
            this.btnGuiOTP.Text = "Gửi OTP";
            this.btnGuiOTP.Click += new System.EventHandler(this.btnGuiOTP_Click);
            // 
            // lblPassMoi
            // 
            this.lblPassMoi.AutoSize = true;
            this.lblPassMoi.Location = new System.Drawing.Point(20, 130);
            this.lblPassMoi.Text = "Mật khẩu mới:";
            this.lblPassMoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // txtPassMoi
            // 
            this.txtPassMoi.Location = new System.Drawing.Point(20, 152);
            this.txtPassMoi.Size = new System.Drawing.Size(290, 25);
            this.txtPassMoi.UseSystemPasswordChar = true;
            // 
            // lblXacNhan
            // 
            this.lblXacNhan.AutoSize = true;
            this.lblXacNhan.Location = new System.Drawing.Point(20, 190);
            this.lblXacNhan.Text = "Nhập lại mật khẩu:";
            this.lblXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // txtXacNhan
            // 
            this.txtXacNhan.Location = new System.Drawing.Point(20, 212);
            this.txtXacNhan.Size = new System.Drawing.Size(290, 25);
            this.txtXacNhan.UseSystemPasswordChar = true;
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXacNhan.Location = new System.Drawing.Point(0, 260);
            this.btnXacNhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhan.FlatAppearance.BorderSize = 0;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(330, 50);
            this.btnXacNhan.TabIndex = 9;
            this.btnXacNhan.Text = "XÁC NHẬN ĐỔI MẬT KHẨU";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // pnlButtonContainer
            //
            this.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonContainer.Height = 80;
            this.pnlButtonContainer.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlButtonContainer.Controls.Add(this.btnXacNhan);
            // 
            // frmDoiMK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 330);
            this.Controls.Add(this.pnlButtonContainer);
            this.Controls.Add(this.txtXacNhan);
            this.Controls.Add(this.lblXacNhan);
            this.Controls.Add(this.txtPassMoi);
            this.Controls.Add(this.lblPassMoi);
            this.Controls.Add(this.btnGuiOTP);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDoiMK";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
            this.pnlButtonContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button btnGuiOTP;
        private System.Windows.Forms.Label lblPassMoi;
        private System.Windows.Forms.TextBox txtPassMoi;
        private System.Windows.Forms.Label lblXacNhan;
        private System.Windows.Forms.TextBox txtXacNhan;
        private System.Windows.Forms.Button btnXacNhan;
    }
}