namespace GUI
{
    partial class frmThanhToan
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
            this.components = new System.ComponentModel.Container();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblLabelKM = new System.Windows.Forms.Label();
            this.cboApplyKM = new System.Windows.Forms.ComboBox();
            this.lblLabelSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblLabelDiem = new System.Windows.Forms.Label();
            this.lblDiemTichLuy = new System.Windows.Forms.Label();
            this.lblLabelChonKM = new System.Windows.Forms.Label();
            this.cboKhuyenMai = new System.Windows.Forms.ComboBox();
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.qrTimer = new System.Windows.Forms.Timer(this.components);
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.layoutMain.Controls.Add(this.lblTotal, 0, 0);
            this.layoutMain.Controls.Add(this.lblLabelKM, 0, 1);
            this.layoutMain.Controls.Add(this.cboApplyKM, 1, 1);
            this.layoutMain.Controls.Add(this.lblLabelSDT, 0, 2);
            this.layoutMain.Controls.Add(this.txtSDT, 1, 2);
            this.layoutMain.Controls.Add(this.lblLabelDiem, 0, 3);
            this.layoutMain.Controls.Add(this.lblDiemTichLuy, 1, 3);
            this.layoutMain.Controls.Add(this.lblLabelChonKM, 0, 4);
            this.layoutMain.Controls.Add(this.cboKhuyenMai, 1, 4);
            this.layoutMain.Controls.Add(this.pbQRCode, 0, 5);
            this.layoutMain.Controls.Add(this.btnThanhToan, 0, 6);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.Padding = new System.Windows.Forms.Padding(20);
            this.layoutMain.RowCount = 7;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.Size = new System.Drawing.Size(800, 950);
            this.layoutMain.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.layoutMain.SetColumnSpan(this.lblTotal, 2);
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.Black;
            this.lblTotal.Location = new System.Drawing.Point(23, 20);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(754, 40);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Tổng tiền: 0 VNĐ";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLabelKM
            // 
            this.lblLabelKM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLabelKM.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLabelKM.Location = new System.Drawing.Point(23, 60);
            this.lblLabelKM.Name = "lblLabelKM";
            this.lblLabelKM.Size = new System.Drawing.Size(222, 40);
            this.lblLabelKM.TabIndex = 1;
            this.lblLabelKM.Text = "Áp dụng KM/Điểm?";
            this.lblLabelKM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboApplyKM
            // 
            this.cboApplyKM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboApplyKM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboApplyKM.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboApplyKM.FormattingEnabled = true;
            this.cboApplyKM.Items.AddRange(new object[] {
            "Không",
            "Có"});
            this.cboApplyKM.Location = new System.Drawing.Point(251, 67);
            this.cboApplyKM.Name = "cboApplyKM";
            this.cboApplyKM.Size = new System.Drawing.Size(526, 25);
            this.cboApplyKM.TabIndex = 2;
            this.cboApplyKM.SelectedIndexChanged += new System.EventHandler(this.CboApplyKM_SelectedIndexChanged);
            // 
            // lblLabelSDT
            // 
            this.lblLabelSDT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLabelSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLabelSDT.Location = new System.Drawing.Point(23, 100);
            this.lblLabelSDT.Name = "lblLabelSDT";
            this.lblLabelSDT.Size = new System.Drawing.Size(222, 40);
            this.lblLabelSDT.TabIndex = 3;
            this.lblLabelSDT.Text = "SĐT Khách:";
            this.lblLabelSDT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSDT
            // 
            this.txtSDT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSDT.Location = new System.Drawing.Point(251, 107);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(526, 25);
            this.txtSDT.TabIndex = 4;
            this.txtSDT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSDT_KeyDown);
            this.txtSDT.Leave += new System.EventHandler(this.TxtSDT_Leave);
            // 
            // lblLabelDiem
            // 
            this.lblLabelDiem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLabelDiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLabelDiem.Location = new System.Drawing.Point(23, 140);
            this.lblLabelDiem.Name = "lblLabelDiem";
            this.lblLabelDiem.Size = new System.Drawing.Size(222, 40);
            this.lblLabelDiem.TabIndex = 5;
            this.lblLabelDiem.Text = "Điểm tích lũy:";
            this.lblLabelDiem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDiemTichLuy
            // 
            this.lblDiemTichLuy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiemTichLuy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblDiemTichLuy.ForeColor = System.Drawing.Color.Blue;
            this.lblDiemTichLuy.Location = new System.Drawing.Point(251, 140);
            this.lblDiemTichLuy.Name = "lblDiemTichLuy";
            this.lblDiemTichLuy.Size = new System.Drawing.Size(526, 40);
            this.lblDiemTichLuy.TabIndex = 6;
            this.lblDiemTichLuy.Text = "Điểm: 0";
            this.lblDiemTichLuy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLabelChonKM
            // 
            this.lblLabelChonKM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLabelChonKM.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLabelChonKM.Location = new System.Drawing.Point(23, 180);
            this.lblLabelChonKM.Name = "lblLabelChonKM";
            this.lblLabelChonKM.Size = new System.Drawing.Size(222, 40);
            this.lblLabelChonKM.TabIndex = 7;
            this.lblLabelChonKM.Text = "Chọn KM:";
            this.lblLabelChonKM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboKhuyenMai
            // 
            this.cboKhuyenMai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboKhuyenMai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhuyenMai.Enabled = false;
            this.cboKhuyenMai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboKhuyenMai.FormattingEnabled = true;
            this.cboKhuyenMai.Location = new System.Drawing.Point(251, 187);
            this.cboKhuyenMai.Name = "cboKhuyenMai";
            this.cboKhuyenMai.Size = new System.Drawing.Size(526, 25);
            this.cboKhuyenMai.TabIndex = 8;
            this.cboKhuyenMai.SelectedIndexChanged += new System.EventHandler(this.CboKhuyenMai_SelectedIndexChanged);
            // 
            // pbQRCode
            // 
            this.layoutMain.SetColumnSpan(this.pbQRCode, 2);
            this.pbQRCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbQRCode.Location = new System.Drawing.Point(23, 223);
            this.pbQRCode.MinimumSize = new System.Drawing.Size(0, 500);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(754, 654);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbQRCode.TabIndex = 9;
            this.pbQRCode.TabStop = false;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.OrangeRed;
            this.layoutMain.SetColumnSpan(this.btnThanhToan, 2);
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThanhToan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(23, 883);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(754, 44);
            this.btnThanhToan.TabIndex = 10;
            this.btnThanhToan.Text = "THANH TOÁN";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Click += new System.EventHandler(this.BtnThanhToan_Click);
            // 
            // qrTimer
            // 
            this.qrTimer.Interval = 500;
            this.qrTimer.Tick += new System.EventHandler(this.QrTimer_Tick);
            // 
            // frmThanhToan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 900);
            this.Controls.Add(this.layoutMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmThanhToan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thanh toán hóa đơn";
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblLabelKM;
        private System.Windows.Forms.ComboBox cboApplyKM;
        private System.Windows.Forms.Label lblLabelSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblLabelDiem;
        private System.Windows.Forms.Label lblDiemTichLuy;
        private System.Windows.Forms.Label lblLabelChonKM;
        private System.Windows.Forms.ComboBox cboKhuyenMai;
        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Timer qrTimer;
    }
}