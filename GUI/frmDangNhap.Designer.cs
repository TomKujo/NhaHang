namespace GUI
{
    partial class frmDangNhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblDangNhap = new Label();
            lblTenDN = new Label();
            txtTenDN = new TextBox();
            lblMK = new Label();
            txtMK = new TextBox();
            btnXacNhan = new Button();
            lblQuenMK = new Label();
            chkDuyTri = new CheckBox();
            SuspendLayout();
            // 
            // lblDangNhap
            // 
            lblDangNhap.Dock = DockStyle.Top;
            lblDangNhap.Font = new Font("Times New Roman", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblDangNhap.ForeColor = Color.Black;
            lblDangNhap.Location = new Point(0, 0);
            lblDangNhap.Name = "lblDangNhap";
            lblDangNhap.Size = new Size(300, 50);
            lblDangNhap.TabIndex = 0;
            lblDangNhap.Text = "ĐĂNG NHẬP";
            lblDangNhap.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTenDN
            // 
            lblTenDN.AutoSize = true;
            lblTenDN.Location = new Point(12, 56);
            lblTenDN.Name = "lblTenDN";
            lblTenDN.Size = new Size(36, 15);
            lblTenDN.TabIndex = 1;
            lblTenDN.Text = "Email";
            // 
            // txtTenDN
            // 
            txtTenDN.Location = new Point(75, 53);
            txtTenDN.Name = "txtTenDN";
            txtTenDN.Size = new Size(213, 23);
            txtTenDN.TabIndex = 2;
            // 
            // lblMK
            // 
            lblMK.AutoSize = true;
            lblMK.Location = new Point(12, 85);
            lblMK.Name = "lblMK";
            lblMK.Size = new Size(57, 15);
            lblMK.TabIndex = 3;
            lblMK.Text = "Mật khẩu";
            // 
            // txtMK
            // 
            txtMK.Location = new Point(75, 82);
            txtMK.Name = "txtMK";
            txtMK.PasswordChar = '*';
            txtMK.Size = new Size(213, 23);
            txtMK.TabIndex = 4;
            txtMK.KeyDown += txtMatKhau_KeyDown;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.FromArgb(255, 152, 0);
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Location = new Point(12, 136);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(276, 40);
            btnXacNhan.TabIndex = 5;
            btnXacNhan.Text = "XÁC NHẬN ĐĂNG NHẬP";
            btnXacNhan.UseVisualStyleBackColor = false;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // lblQuenMK
            // 
            lblQuenMK.Cursor = Cursors.Hand;
            lblQuenMK.Dock = DockStyle.Bottom;
            lblQuenMK.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblQuenMK.ForeColor = Color.Blue;
            lblQuenMK.Location = new Point(0, 179);
            lblQuenMK.Name = "lblQuenMK";
            lblQuenMK.Size = new Size(300, 15);
            lblQuenMK.TabIndex = 6;
            lblQuenMK.Text = "Quên mật khẩu";
            lblQuenMK.TextAlign = ContentAlignment.MiddleCenter;
            lblQuenMK.Click += lblQuenMatKhau_Click;
            // 
            // chkDuyTri
            // 
            chkDuyTri.AutoSize = true;
            chkDuyTri.Location = new Point(75, 111);
            chkDuyTri.Name = "chkDuyTri";
            chkDuyTri.Size = new Size(121, 19);
            chkDuyTri.TabIndex = 7;
            chkDuyTri.Text = "Duy trì đăng nhập";
            chkDuyTri.UseVisualStyleBackColor = true;
            // 
            // frmDangNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(300, 194);
            Controls.Add(chkDuyTri);
            Controls.Add(lblQuenMK);
            Controls.Add(btnXacNhan);
            Controls.Add(lblDangNhap);
            Controls.Add(lblMK);
            Controls.Add(txtMK);
            Controls.Add(lblTenDN);
            Controls.Add(txtTenDN);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += frmDangNhap_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblDangNhap;
        private Label lblTenDN;
        private TextBox txtTenDN;
        private Label lblMK;
        private TextBox txtMK;
        private Button btnXacNhan;
        private Label lblQuenMK;
        private CheckBox chkDuyTri;
    }
}