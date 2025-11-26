namespace GUI
{
    partial class frmLogin
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
            pnlCard = new Panel();
            lblQuenMatKhau = new Label();
            btnXacNhan = new Button();
            txtMatKhau = new TextBox();
            lblMK = new Label();
            txtMaNV = new TextBox();
            lblMaNV = new Label();
            lblTitle = new Label();
            btnThoat = new Button();
            pnlCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.White;
            pnlCard.Controls.Add(lblQuenMatKhau);
            pnlCard.Controls.Add(btnXacNhan);
            pnlCard.Controls.Add(txtMatKhau);
            pnlCard.Controls.Add(lblMK);
            pnlCard.Controls.Add(txtMaNV);
            pnlCard.Controls.Add(lblMaNV);
            pnlCard.Controls.Add(lblTitle);
            pnlCard.Location = new Point(121, 12);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new Size(450, 300);
            pnlCard.TabIndex = 0;
            // 
            // lblQuenMatKhau
            // 
            lblQuenMatKhau.AutoSize = true;
            lblQuenMatKhau.Cursor = Cursors.Hand;
            lblQuenMatKhau.ForeColor = Color.Blue;
            lblQuenMatKhau.Location = new Point(160, 200);
            lblQuenMatKhau.Name = "lblQuenMatKhau";
            lblQuenMatKhau.Size = new Size(94, 15);
            lblQuenMatKhau.TabIndex = 6;
            lblQuenMatKhau.Text = "Quên mật khẩu?";
            lblQuenMatKhau.Click += label1_Click;
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackColor = Color.FromArgb(255, 152, 0);
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Location = new Point(160, 157);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(120, 40);
            btnXacNhan.TabIndex = 5;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.UseVisualStyleBackColor = false;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // txtMatKhau
            // 
            txtMatKhau.Location = new Point(160, 128);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PasswordChar = '●';
            txtMatKhau.Size = new Size(200, 23);
            txtMatKhau.TabIndex = 4;
            // 
            // lblMK
            // 
            lblMK.AutoSize = true;
            lblMK.Location = new Point(50, 130);
            lblMK.Name = "lblMK";
            lblMK.Size = new Size(60, 15);
            lblMK.TabIndex = 3;
            lblMK.Text = "Mật khẩu:";
            // 
            // txtMaNV
            // 
            txtMaNV.Location = new Point(160, 78);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new Size(200, 23);
            txtMaNV.TabIndex = 2;
            // 
            // lblMaNV
            // 
            lblMaNV.AutoSize = true;
            lblMaNV.Location = new Point(50, 80);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new Size(82, 15);
            lblMaNV.TabIndex = 1;
            lblMaNV.Text = "Mã nhân viên:";
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.DimGray;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(450, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "NHẬP TÀI KHOẢN NHÂN VIÊN THU NGÂN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Red;
            btnThoat.Location = new Point(613, 12);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(75, 23);
            btnThoat.TabIndex = 1;
            btnThoat.Text = "X";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(700, 500);
            Controls.Add(btnThoat);
            Controls.Add(pnlCard);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmLogin";
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlCard;
        private Label lblTitle;
        private Label lblMaNV;
        private Button btnXacNhan;
        private TextBox txtMatKhau;
        private Label lblMK;
        private TextBox txtMaNV;
        private Button btnThoat;
        private Label lblQuenMatKhau;
    }
}