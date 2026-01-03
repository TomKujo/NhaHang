namespace GUI
{
    partial class ucKho
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
            this.tabCtrlKho = new System.Windows.Forms.TabControl();
            this.tabTonKho = new System.Windows.Forms.TabPage();
            this.dgvKho = new System.Windows.Forms.DataGridView();
            this.pnlToolKho = new System.Windows.Forms.Panel();
            this.btnXoaNL = new System.Windows.Forms.Button();
            this.btnSuaNL = new System.Windows.Forms.Button();
            this.btnThemNL = new System.Windows.Forms.Button();
            this.tabNhapHang = new System.Windows.Forms.TabPage();
            this.splitNhapHang = new System.Windows.Forms.SplitContainer();
            this.dgvKhoSelection = new System.Windows.Forms.DataGridView();
            this.lblHuongDanNhap = new System.Windows.Forms.Label();
            this.lblHuongDanEdit = new System.Windows.Forms.Label();
            this.dgvGioHang = new System.Windows.Forms.DataGridView();
            this.pnlActionNhap = new System.Windows.Forms.Panel();
            this.lblTongTienNhap = new System.Windows.Forms.Label();
            this.btnXacNhanNhap = new System.Windows.Forms.Button();
            this.cboNCC_Import = new System.Windows.Forms.ComboBox();
            this.lblChonNCC = new System.Windows.Forms.Label();
            this.tabPhieuNhap = new System.Windows.Forms.TabPage();
            this.splitLichSuNhap = new System.Windows.Forms.SplitContainer();
            this.dgvPhieuNhap = new System.Windows.Forms.DataGridView();
            this.pnlPhieuAction = new System.Windows.Forms.Panel();
            this.btnXoaPhieu = new System.Windows.Forms.Button();
            this.lblListPhieu = new System.Windows.Forms.Label();
            this.dgvChiTietPhieu = new System.Windows.Forms.DataGridView();
            this.lblChiTietPhieu = new System.Windows.Forms.Label();
            this.tabNCC = new System.Windows.Forms.TabPage();
            this.dgvNCC = new System.Windows.Forms.DataGridView();
            this.pnlNCCAction = new System.Windows.Forms.Panel();
            this.btnXoaNCC = new System.Windows.Forms.Button();
            this.btnSuaNCC = new System.Windows.Forms.Button();
            this.btnThemNCC = new System.Windows.Forms.Button();
            this.tabCtrlKho.SuspendLayout();
            this.tabTonKho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKho)).BeginInit();
            this.pnlToolKho.SuspendLayout();
            this.tabNhapHang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitNhapHang)).BeginInit();
            this.splitNhapHang.Panel1.SuspendLayout();
            this.splitNhapHang.Panel2.SuspendLayout();
            this.splitNhapHang.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhoSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
            this.pnlActionNhap.SuspendLayout();
            this.tabPhieuNhap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLichSuNhap)).BeginInit();
            this.splitLichSuNhap.Panel1.SuspendLayout();
            this.splitLichSuNhap.Panel2.SuspendLayout();
            this.splitLichSuNhap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).BeginInit();
            this.pnlPhieuAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietPhieu)).BeginInit();
            this.tabNCC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNCC)).BeginInit();
            this.pnlNCCAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCtrlKho
            // 
            this.tabCtrlKho.Controls.Add(this.tabTonKho);
            this.tabCtrlKho.Controls.Add(this.tabNhapHang);
            this.tabCtrlKho.Controls.Add(this.tabPhieuNhap);
            this.tabCtrlKho.Controls.Add(this.tabNCC);
            this.tabCtrlKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlKho.ItemSize = new System.Drawing.Size(100, 30);
            this.tabCtrlKho.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlKho.Name = "tabCtrlKho";
            this.tabCtrlKho.SelectedIndex = 0;
            this.tabCtrlKho.Size = new System.Drawing.Size(1000, 600);
            this.tabCtrlKho.TabIndex = 0;
            this.tabCtrlKho.SelectedIndexChanged += new System.EventHandler(this.TabCtrlKho_SelectedIndexChanged);
            // 
            // tabTonKho
            // 
            this.tabTonKho.Controls.Add(this.dgvKho);
            this.tabTonKho.Controls.Add(this.pnlToolKho);
            this.tabTonKho.Location = new System.Drawing.Point(4, 34);
            this.tabTonKho.Name = "tabTonKho";
            this.tabTonKho.Padding = new System.Windows.Forms.Padding(3);
            this.tabTonKho.Size = new System.Drawing.Size(992, 562);
            this.tabTonKho.TabIndex = 0;
            this.tabTonKho.Text = "Tồn Kho";
            this.tabTonKho.UseVisualStyleBackColor = true;
            // 
            // dgvKho
            // 
            this.dgvKho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKho.Location = new System.Drawing.Point(3, 43);
            this.dgvKho.Name = "dgvKho";
            this.dgvKho.Size = new System.Drawing.Size(986, 516);
            this.dgvKho.TabIndex = 1;
            // 
            // pnlToolKho
            // 
            this.pnlToolKho.Controls.Add(this.btnXoaNL);
            this.pnlToolKho.Controls.Add(this.btnSuaNL);
            this.pnlToolKho.Controls.Add(this.btnThemNL);
            this.pnlToolKho.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolKho.Location = new System.Drawing.Point(3, 3);
            this.pnlToolKho.Name = "pnlToolKho";
            this.pnlToolKho.Size = new System.Drawing.Size(986, 40);
            this.pnlToolKho.TabIndex = 0;
            // 
            // btnXoaNL
            // 
            this.btnXoaNL.Location = new System.Drawing.Point(220, 5);
            this.btnXoaNL.Name = "btnXoaNL";
            this.btnXoaNL.Size = new System.Drawing.Size(100, 30);
            this.btnXoaNL.TabIndex = 2;
            this.btnXoaNL.Text = "Xóa NL";
            // 
            // btnSuaNL
            // 
            this.btnSuaNL.Location = new System.Drawing.Point(114, 5);
            this.btnSuaNL.Name = "btnSuaNL";
            this.btnSuaNL.Size = new System.Drawing.Size(100, 30);
            this.btnSuaNL.TabIndex = 1;
            this.btnSuaNL.Text = "Sửa NL";
            // 
            // btnThemNL
            // 
            this.btnThemNL.Location = new System.Drawing.Point(8, 5);
            this.btnThemNL.Name = "btnThemNL";
            this.btnThemNL.Size = new System.Drawing.Size(100, 30);
            this.btnThemNL.TabIndex = 0;
            this.btnThemNL.Text = "Thêm NL";
            // 
            // tabNhapHang
            // 
            this.tabNhapHang.Controls.Add(this.splitNhapHang);
            this.tabNhapHang.Location = new System.Drawing.Point(4, 34);
            this.tabNhapHang.Name = "tabNhapHang";
            this.tabNhapHang.Padding = new System.Windows.Forms.Padding(3);
            this.tabNhapHang.Size = new System.Drawing.Size(992, 562);
            this.tabNhapHang.TabIndex = 1;
            this.tabNhapHang.Text = "Nhập Hàng";
            this.tabNhapHang.UseVisualStyleBackColor = true;
            // 
            // splitNhapHang
            // 
            this.splitNhapHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitNhapHang.Location = new System.Drawing.Point(3, 3);
            this.splitNhapHang.Name = "splitNhapHang";
            // 
            // splitNhapHang.Panel1
            // 
            this.splitNhapHang.Panel1.Controls.Add(this.dgvKhoSelection);
            this.splitNhapHang.Panel1.Controls.Add(this.lblHuongDanNhap);
            // 
            // splitNhapHang.Panel2
            // 
            this.splitNhapHang.Panel2.Controls.Add(this.dgvGioHang);
            this.splitNhapHang.Panel2.Controls.Add(this.lblHuongDanEdit);
            this.splitNhapHang.Panel2.Controls.Add(this.pnlActionNhap);
            this.splitNhapHang.Size = new System.Drawing.Size(986, 556);
            this.splitNhapHang.SplitterDistance = 450;
            this.splitNhapHang.TabIndex = 0;
            // 
            // lblHuongDanEdit
            // 
            this.lblHuongDanEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHuongDanEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblHuongDanEdit.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblHuongDanEdit.Location = new System.Drawing.Point(0, 100);
            this.lblHuongDanEdit.Name = "lblHuongDanEdit";
            this.lblHuongDanEdit.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblHuongDanEdit.Size = new System.Drawing.Size(532, 25);
            this.lblHuongDanEdit.TabIndex = 1;
            this.lblHuongDanEdit.Text = "Double click vào nguyên liệu để sửa số lượng, tình trạng, đơn giá";
            this.lblHuongDanEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvKhoSelection
            // 
            this.dgvKhoSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhoSelection.Location = new System.Drawing.Point(0, 30);
            this.dgvKhoSelection.Name = "dgvKhoSelection";
            this.dgvKhoSelection.Size = new System.Drawing.Size(450, 526);
            this.dgvKhoSelection.TabIndex = 1;
            // 
            // lblHuongDanNhap
            // 
            this.lblHuongDanNhap.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHuongDanNhap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHuongDanNhap.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblHuongDanNhap.Location = new System.Drawing.Point(0, 0);
            this.lblHuongDanNhap.Name = "lblHuongDanNhap";
            this.lblHuongDanNhap.Size = new System.Drawing.Size(450, 30);
            this.lblHuongDanNhap.TabIndex = 0;
            this.lblHuongDanNhap.Text = "Double click vào nguyên liệu để thêm vào giỏ nhập";
            this.lblHuongDanNhap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvGioHang
            // 
            this.dgvGioHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGioHang.Location = new System.Drawing.Point(0, 100);
            this.dgvGioHang.Name = "dgvGioHang";
            this.dgvGioHang.Size = new System.Drawing.Size(532, 456);
            this.dgvGioHang.TabIndex = 1;
            // 
            // pnlActionNhap
            // 
            this.pnlActionNhap.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlActionNhap.Controls.Add(this.lblTongTienNhap);
            this.pnlActionNhap.Controls.Add(this.btnXacNhanNhap);
            this.pnlActionNhap.Controls.Add(this.cboNCC_Import);
            this.pnlActionNhap.Controls.Add(this.lblChonNCC);
            this.pnlActionNhap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActionNhap.Location = new System.Drawing.Point(0, 0);
            this.pnlActionNhap.Name = "pnlActionNhap";
            this.pnlActionNhap.Size = new System.Drawing.Size(532, 100);
            this.pnlActionNhap.TabIndex = 0;
            // 
            // lblTongTienNhap
            // 
            this.lblTongTienNhap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienNhap.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTienNhap.ForeColor = System.Drawing.Color.Red;
            this.lblTongTienNhap.Location = new System.Drawing.Point(232, 60);
            this.lblTongTienNhap.Name = "lblTongTienNhap";
            this.lblTongTienNhap.Size = new System.Drawing.Size(290, 30);
            this.lblTongTienNhap.TabIndex = 3;
            this.lblTongTienNhap.Text = "Tổng: 0 VNĐ";
            this.lblTongTienNhap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnXacNhanNhap
            // 
            this.btnXacNhanNhap.Location = new System.Drawing.Point(6, 56);
            this.btnXacNhanNhap.Name = "btnXacNhanNhap";
            this.btnXacNhanNhap.Size = new System.Drawing.Size(180, 35);
            this.btnXacNhanNhap.TabIndex = 2;
            this.btnXacNhanNhap.Text = "Lưu";
            // 
            // cboNCC_Import
            // 
            this.cboNCC_Import.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNCC_Import.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNCC_Import.FormattingEnabled = true;
            this.cboNCC_Import.Location = new System.Drawing.Point(6, 27);
            this.cboNCC_Import.Name = "cboNCC_Import";
            this.cboNCC_Import.Size = new System.Drawing.Size(516, 21);
            this.cboNCC_Import.TabIndex = 1;
            // 
            // lblChonNCC
            // 
            this.lblChonNCC.AutoSize = true;
            this.lblChonNCC.Location = new System.Drawing.Point(3, 8);
            this.lblChonNCC.Name = "lblChonNCC";
            this.lblChonNCC.Size = new System.Drawing.Size(103, 13);
            this.lblChonNCC.TabIndex = 0;
            this.lblChonNCC.Text = "Chọn Nhà cung cấp";
            // 
            // tabPhieuNhap
            // 
            this.tabPhieuNhap.Controls.Add(this.splitLichSuNhap);
            this.tabPhieuNhap.Location = new System.Drawing.Point(4, 34);
            this.tabPhieuNhap.Name = "tabPhieuNhap";
            this.tabPhieuNhap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPhieuNhap.Size = new System.Drawing.Size(992, 562);
            this.tabPhieuNhap.TabIndex = 3;
            this.tabPhieuNhap.Text = "Lịch Sử Nhập";
            this.tabPhieuNhap.UseVisualStyleBackColor = true;
            // 
            // splitLichSuNhap
            // 
            this.splitLichSuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLichSuNhap.Location = new System.Drawing.Point(3, 3);
            this.splitLichSuNhap.Name = "splitLichSuNhap";
            // 
            // splitLichSuNhap.Panel1
            // 
            this.splitLichSuNhap.Panel1.Controls.Add(this.dgvPhieuNhap);
            this.splitLichSuNhap.Panel1.Controls.Add(this.pnlPhieuAction);
            this.splitLichSuNhap.Panel1.Controls.Add(this.lblListPhieu);
            // 
            // splitLichSuNhap.Panel2
            // 
            this.splitLichSuNhap.Panel2.Controls.Add(this.dgvChiTietPhieu);
            this.splitLichSuNhap.Panel2.Controls.Add(this.lblChiTietPhieu);
            this.splitLichSuNhap.Size = new System.Drawing.Size(986, 556);
            this.splitLichSuNhap.SplitterDistance = 550;
            this.splitLichSuNhap.TabIndex = 0;
            // 
            // dgvPhieuNhap
            // 
            this.dgvPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhieuNhap.Location = new System.Drawing.Point(0, 30);
            this.dgvPhieuNhap.Name = "dgvPhieuNhap";
            this.dgvPhieuNhap.Size = new System.Drawing.Size(550, 486);
            this.dgvPhieuNhap.TabIndex = 2;
            // 
            // pnlPhieuAction
            // 
            this.pnlPhieuAction.Controls.Add(this.btnXoaPhieu);
            this.pnlPhieuAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPhieuAction.Location = new System.Drawing.Point(0, 516);
            this.pnlPhieuAction.Name = "pnlPhieuAction";
            this.pnlPhieuAction.Size = new System.Drawing.Size(550, 40);
            this.pnlPhieuAction.TabIndex = 1;
            // 
            // btnXoaPhieu
            // 
            this.btnXoaPhieu.Location = new System.Drawing.Point(5, 5);
            this.btnXoaPhieu.Name = "btnXoaPhieu";
            this.btnXoaPhieu.Size = new System.Drawing.Size(120, 30);
            this.btnXoaPhieu.TabIndex = 0;
            this.btnXoaPhieu.Text = "Xóa Phiếu Nhập";
            // 
            // lblListPhieu
            // 
            this.lblListPhieu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListPhieu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblListPhieu.Location = new System.Drawing.Point(0, 0);
            this.lblListPhieu.Name = "lblListPhieu";
            this.lblListPhieu.Size = new System.Drawing.Size(550, 30);
            this.lblListPhieu.TabIndex = 0;
            this.lblListPhieu.Text = "DANH SÁCH PHIẾU NHẬP";
            this.lblListPhieu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvChiTietPhieu
            // 
            this.dgvChiTietPhieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTietPhieu.Location = new System.Drawing.Point(0, 30);
            this.dgvChiTietPhieu.Name = "dgvChiTietPhieu";
            this.dgvChiTietPhieu.Size = new System.Drawing.Size(432, 526);
            this.dgvChiTietPhieu.TabIndex = 1;
            // 
            // lblChiTietPhieu
            // 
            this.lblChiTietPhieu.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChiTietPhieu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChiTietPhieu.Location = new System.Drawing.Point(0, 0);
            this.lblChiTietPhieu.Name = "lblChiTietPhieu";
            this.lblChiTietPhieu.Size = new System.Drawing.Size(432, 30);
            this.lblChiTietPhieu.TabIndex = 0;
            this.lblChiTietPhieu.Text = "CHI TIẾT VẬT TƯ";
            this.lblChiTietPhieu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabNCC
            // 
            this.tabNCC.Controls.Add(this.dgvNCC);
            this.tabNCC.Controls.Add(this.pnlNCCAction);
            this.tabNCC.Location = new System.Drawing.Point(4, 34);
            this.tabNCC.Name = "tabNCC";
            this.tabNCC.Padding = new System.Windows.Forms.Padding(3);
            this.tabNCC.Size = new System.Drawing.Size(992, 562);
            this.tabNCC.TabIndex = 2;
            this.tabNCC.Text = "Nhà Cung Cấp";
            this.tabNCC.UseVisualStyleBackColor = true;
            // 
            // dgvNCC
            // 
            this.dgvNCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNCC.Location = new System.Drawing.Point(3, 43);
            this.dgvNCC.Name = "dgvNCC";
            this.dgvNCC.Size = new System.Drawing.Size(986, 516);
            this.dgvNCC.TabIndex = 2;
            // 
            // pnlNCCAction
            // 
            this.pnlNCCAction.Controls.Add(this.btnXoaNCC);
            this.pnlNCCAction.Controls.Add(this.btnSuaNCC);
            this.pnlNCCAction.Controls.Add(this.btnThemNCC);
            this.pnlNCCAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNCCAction.Location = new System.Drawing.Point(3, 3);
            this.pnlNCCAction.Name = "pnlNCCAction";
            this.pnlNCCAction.Size = new System.Drawing.Size(986, 40);
            this.pnlNCCAction.TabIndex = 1;
            // 
            // btnXoaNCC
            // 
            this.btnXoaNCC.Location = new System.Drawing.Point(220, 5);
            this.btnXoaNCC.Name = "btnXoaNCC";
            this.btnXoaNCC.Size = new System.Drawing.Size(100, 30);
            this.btnXoaNCC.TabIndex = 2;
            this.btnXoaNCC.Text = "Xóa NCC";
            // 
            // btnSuaNCC
            // 
            this.btnSuaNCC.Location = new System.Drawing.Point(114, 5);
            this.btnSuaNCC.Name = "btnSuaNCC";
            this.btnSuaNCC.Size = new System.Drawing.Size(100, 30);
            this.btnSuaNCC.TabIndex = 1;
            this.btnSuaNCC.Text = "Sửa NCC";
            // 
            // btnThemNCC
            // 
            this.btnThemNCC.Location = new System.Drawing.Point(8, 5);
            this.btnThemNCC.Name = "btnThemNCC";
            this.btnThemNCC.Size = new System.Drawing.Size(100, 30);
            this.btnThemNCC.TabIndex = 0;
            this.btnThemNCC.Text = "Thêm NCC";
            // 
            // ucKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabCtrlKho);
            this.Name = "ucKho";
            this.Size = new System.Drawing.Size(1000, 600);
            this.tabCtrlKho.ResumeLayout(false);
            this.tabTonKho.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKho)).EndInit();
            this.pnlToolKho.ResumeLayout(false);
            this.tabNhapHang.ResumeLayout(false);
            this.splitNhapHang.Panel1.ResumeLayout(false);
            this.splitNhapHang.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitNhapHang)).EndInit();
            this.splitNhapHang.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhoSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
            this.pnlActionNhap.ResumeLayout(false);
            this.pnlActionNhap.PerformLayout();
            this.tabPhieuNhap.ResumeLayout(false);
            this.splitLichSuNhap.Panel1.ResumeLayout(false);
            this.splitLichSuNhap.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLichSuNhap)).EndInit();
            this.splitLichSuNhap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).EndInit();
            this.pnlPhieuAction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietPhieu)).EndInit();
            this.tabNCC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNCC)).EndInit();
            this.pnlNCCAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrlKho;
        private System.Windows.Forms.TabPage tabTonKho;
        private System.Windows.Forms.TabPage tabNhapHang;
        private System.Windows.Forms.TabPage tabNCC;
        private System.Windows.Forms.Panel pnlToolKho;
        private System.Windows.Forms.DataGridView dgvKho;
        private System.Windows.Forms.Button btnThemNL;
        private System.Windows.Forms.Button btnXoaNL;
        private System.Windows.Forms.Button btnSuaNL;
        private System.Windows.Forms.SplitContainer splitNhapHang;
        private System.Windows.Forms.DataGridView dgvKhoSelection;
        private System.Windows.Forms.Label lblHuongDanNhap;
        private System.Windows.Forms.Label lblHuongDanEdit;
        private System.Windows.Forms.Panel pnlActionNhap;
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.Label lblChonNCC;
        private System.Windows.Forms.ComboBox cboNCC_Import;
        private System.Windows.Forms.Button btnXacNhanNhap;
        private System.Windows.Forms.Label lblTongTienNhap;
        private System.Windows.Forms.DataGridView dgvNCC;
        private System.Windows.Forms.Panel pnlNCCAction;
        private System.Windows.Forms.Button btnXoaNCC;
        private System.Windows.Forms.Button btnSuaNCC;
        private System.Windows.Forms.Button btnThemNCC;
        private System.Windows.Forms.TabPage tabPhieuNhap;
        private System.Windows.Forms.SplitContainer splitLichSuNhap;
        private System.Windows.Forms.DataGridView dgvPhieuNhap;
        private System.Windows.Forms.Panel pnlPhieuAction;
        private System.Windows.Forms.Label lblListPhieu;
        private System.Windows.Forms.DataGridView dgvChiTietPhieu;
        private System.Windows.Forms.Label lblChiTietPhieu;
        private System.Windows.Forms.Button btnXoaPhieu;
    }
}