namespace GUI
{
    partial class frmAddEditNCC
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tblMain;

        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTen;

        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;

        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;

        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Button btnLuu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.tblMain.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tblMain.Controls.Add(this.lblTen, 0, 0);
            this.tblMain.Controls.Add(this.txtTen, 1, 0);
            this.tblMain.Controls.Add(this.lblDiaChi, 0, 1);
            this.tblMain.Controls.Add(this.txtDiaChi, 1, 1);
            this.tblMain.Controls.Add(this.lblSDT, 0, 2);
            this.tblMain.Controls.Add(this.txtSDT, 1, 2);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.tblMain.RowCount = 3;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblMain.Size = new System.Drawing.Size(450, 160);
            this.tblMain.TabIndex = 0;
            // 
            // Ten
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTen.Location = new System.Drawing.Point(23, 10);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(117, 46);
            this.lblTen.TabIndex = 0;
            this.lblTen.Text = "Tên NCC:";
            this.lblTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTen.Location = new System.Drawing.Point(146, 20);
            this.txtTen.Name = "txtTen";
            this.txtTen.Size = new System.Drawing.Size(281, 25);
            this.txtTen.TabIndex = 1;
            // 
            // DiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiaChi.Location = new System.Drawing.Point(23, 56);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(117, 46);
            this.lblDiaChi.TabIndex = 2;
            this.lblDiaChi.Text = "Địa chỉ:";
            this.lblDiaChi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDiaChi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDiaChi.Location = new System.Drawing.Point(146, 66);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(281, 25);
            this.txtDiaChi.TabIndex = 3;
            // 
            // SDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSDT.Location = new System.Drawing.Point(23, 102);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(117, 48);
            this.lblSDT.TabIndex = 4;
            this.lblSDT.Text = "SĐT:";
            this.lblSDT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSDT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSDT.Location = new System.Drawing.Point(146, 113);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(281, 25);
            this.txtSDT.TabIndex = 5;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnLuu);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 170);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.Size = new System.Drawing.Size(450, 70);
            this.pnlBtn.TabIndex = 1;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.SeaGreen;
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(20, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(410, 50);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.UseVisualStyleBackColor = false;
            // 
            // frmAddEditNCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(450, 240);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.tblMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddEditNCC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAddEditNCC";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}