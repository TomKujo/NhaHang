namespace GUI
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            pnlSidebar = new Panel();
            btnTaiKhoan = new Button();
            btnDangXuat = new Button();
            pnlSubQuanLy = new Panel();
            btnTaiKhoan = new Button();
            btnKhachHang = new Button();
            btnNV = new Button();
            btnDoanhThu = new Button();
            btnThucDon = new Button();
            btnKhuyenMai = new Button();
            btnQuanLyParent = new Button();
            btnKho = new Button();
            btnBan = new Button();
            pnlLogo = new Panel();
            picLogo = new PictureBox();
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            pnlSubQuanLy.SuspendLayout();
            pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(30, 30, 30);
            pnlSidebar.Controls.Add(btnTaiKhoan);
            pnlSidebar.Controls.Add(btnDangXuat);
            pnlSidebar.Controls.Add(pnlSubQuanLy);
            pnlSidebar.Controls.Add(btnQuanLyParent);
            pnlSidebar.Controls.Add(btnKho);
            pnlSidebar.Controls.Add(btnBan);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(260, 600);
            pnlSidebar.TabIndex = 0;
            // 
            // btnTaiKhoan
            // 
            btnTaiKhoan.BackColor = Color.FromArgb(30, 30, 30);
            btnTaiKhoan.Cursor = Cursors.Hand;
            btnTaiKhoan.Dock = DockStyle.Bottom;
            btnTaiKhoan.FlatAppearance.BorderSize = 0;
            btnTaiKhoan.FlatStyle = FlatStyle.Flat;
            btnTaiKhoan.Font = new Font("Segoe UI", 11F);
            btnTaiKhoan.ForeColor = Color.White;
            btnTaiKhoan.Location = new Point(0, 490);
            btnTaiKhoan.Name = "btnTaiKhoan";
            btnTaiKhoan.Size = new Size(260, 55);
            btnTaiKhoan.TabIndex = 8;
            btnTaiKhoan.Text = "Tài khoản";
            btnTaiKhoan.UseVisualStyleBackColor = false;
            btnTaiKhoan.Click += btnTaiKhoan_Click;
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.FromArgb(40, 40, 40);
            btnDangXuat.Cursor = Cursors.Hand;
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 11F);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(0, 545);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(260, 55);
            btnDangXuat.TabIndex = 9;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // pnlSubQuanLy
            // 
            pnlSubQuanLy.AutoSize = true;
            pnlSubQuanLy.BackColor = Color.FromArgb(40, 40, 40);
            pnlSubQuanLy.Controls.Add(this.btnKhachHang);
            pnlSubQuanLy.Controls.Add(btnNV);
            pnlSubQuanLy.Controls.Add(btnDoanhThu);
            pnlSubQuanLy.Controls.Add(btnThucDon);
            pnlSubQuanLy.Controls.Add(btnKhuyenMai);
            pnlSubQuanLy.Dock = DockStyle.Top;
            pnlSubQuanLy.Location = new Point(0, 285);
            pnlSubQuanLy.Name = "pnlSubQuanLy";
            pnlSubQuanLy.Size = new Size(260, 225);
            pnlSubQuanLy.TabIndex = 10;
            pnlSubQuanLy.Visible = false;
            //
            // btnKhachHang
            //
            btnKhachHang.Dock = System.Windows.Forms.DockStyle.Top;
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Size = new System.Drawing.Size(260, 45);
            btnKhachHang.TabIndex = 4; // Thứ tự tab
            btnKhachHang.Text = "Khách hàng";
            btnKhachHang.UseVisualStyleBackColor = true;
            btnKhachHang.Click += new System.EventHandler(this.btnKhachHang_Click);
            // 
            // btnNV
            // 
            btnNV.Dock = DockStyle.Top;
            btnNV.Location = new Point(0, 135);
            btnNV.Name = "btnNV";
            btnNV.Size = new Size(260, 45);
            btnNV.TabIndex = 0;
            btnNV.Text = "Nhân sự";
            btnNV.UseVisualStyleBackColor = true;
            btnNV.Click += btnNV_Click;
            // 
            // btnDoanhThu
            // 
            btnDoanhThu.Dock = DockStyle.Top;
            btnDoanhThu.Location = new Point(0, 90);
            btnDoanhThu.Name = "btnDoanhThu";
            btnDoanhThu.Size = new Size(260, 45);
            btnDoanhThu.TabIndex = 1;
            btnDoanhThu.Text = "Doanh thu";
            btnDoanhThu.UseVisualStyleBackColor = true;
            btnDoanhThu.Click += btnDoanhThu_Click;
            // 
            // btnThucDon
            // 
            btnThucDon.Dock = DockStyle.Top;
            btnThucDon.Location = new Point(0, 45);
            btnThucDon.Name = "btnThucDon";
            btnThucDon.Size = new Size(260, 45);
            btnThucDon.TabIndex = 2;
            btnThucDon.Text = "Thực đơn";
            btnThucDon.UseVisualStyleBackColor = true;
            btnThucDon.Click += btnThucDon_Click;
            // 
            // btnKhuyenMai
            // 
            btnKhuyenMai.Dock = DockStyle.Top;
            btnKhuyenMai.Location = new Point(0, 0);
            btnKhuyenMai.Name = "btnKhuyenMai";
            btnKhuyenMai.Size = new Size(260, 45);
            btnKhuyenMai.TabIndex = 3;
            btnKhuyenMai.Text = "Khuyến mãi";
            btnKhuyenMai.UseVisualStyleBackColor = true;
            btnKhuyenMai.Click += btnKhuyenMai_Click;
            // 
            // btnQuanLyParent
            // 
            btnQuanLyParent.Dock = DockStyle.Top;
            btnQuanLyParent.Location = new Point(0, 230);
            btnQuanLyParent.Name = "btnQuanLyParent";
            btnQuanLyParent.Size = new Size(260, 55);
            btnQuanLyParent.TabIndex = 11;
            btnQuanLyParent.Text = "Quản lý";
            btnQuanLyParent.UseVisualStyleBackColor = true;
            btnQuanLyParent.Click += btnQuanLyParent_Click;
            // 
            // btnKho
            // 
            btnKho.Dock = DockStyle.Top;
            btnKho.Location = new Point(0, 175);
            btnKho.Name = "btnKho";
            btnKho.Size = new Size(260, 55);
            btnKho.TabIndex = 3;
            btnKho.Text = "Kho";
            btnKho.UseVisualStyleBackColor = true;
            btnKho.Click += btnKho_Click;
            // 
            // btnBan
            // 
            btnBan.Dock = DockStyle.Top;
            btnBan.Location = new Point(0, 120);
            btnBan.Name = "btnBan";
            btnBan.Size = new Size(260, 55);
            btnBan.TabIndex = 1;
            btnBan.Text = "Bàn";
            btnBan.UseVisualStyleBackColor = true;
            btnBan.Click += btnBan_Click;
            // 
            // pnlLogo
            // 
            pnlLogo.Controls.Add(picLogo);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(260, 120);
            pnlLogo.TabIndex = 0;
            // 
            // picLogo
            // 
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(70, 20);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(120, 80);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(260, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(740, 60);
            pnlHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(20, 0, 0, 0);
            lblTitle.Size = new Size(740, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Trang chủ";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.WhiteSmoke;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(260, 60);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(740, 540);
            pnlContent.TabIndex = 2;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ thống quản lý nhà hàng";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlSubQuanLy.ResumeLayout(false);
            pnlLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.PictureBox picLogo;

        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Button btnTaiKhoan;

        private System.Windows.Forms.Button btnKho;
        private System.Windows.Forms.Button btnBan;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;

        private System.Windows.Forms.Button btnQuanLyParent;
        private System.Windows.Forms.Panel pnlSubQuanLy;
        private System.Windows.Forms.Button btnNV;
        private System.Windows.Forms.Button btnDoanhThu;
        private System.Windows.Forms.Button btnThucDon;
        private System.Windows.Forms.Button btnKhuyenMai;
        private System.Windows.Forms.Button btnKhachHang;
    }
}