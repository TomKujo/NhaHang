namespace GUI
{
    partial class frmAddEditKH
    {
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.TextBox txtTen, txtSDT, txtEmail;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Panel pnlBtn;

        private void InitializeComponent()
        {
            tblMain = new TableLayoutPanel();
            txtTen = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            btnLuu = new Button();
            pnlBtn = new Panel();
            tblMain.SuspendLayout();
            pnlBtn.SuspendLayout();
            SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 2;
            this.tblMain.RowCount = 3;
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMain.Height = 150;
            this.tblMain.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            this.tblMain.RowStyles.Clear();
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            // 
            // txtTen
            // 
            txtTen.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTen.Location = new Point(161, 23);
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(316, 23);
            txtTen.TabIndex = 1;
            // 
            // txtSDT
            // 
            txtSDT.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSDT.Location = new Point(161, 43);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(316, 23);
            txtSDT.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtEmail.Location = new Point(161, 88);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(316, 23);
            txtEmail.TabIndex = 5;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.SeaGreen;
            btnLuu.Dock = DockStyle.Fill;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(20, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(460, 50);
            btnLuu.TabIndex = 0;
            btnLuu.Text = "LƯU";
            btnLuu.UseVisualStyleBackColor = false;
            //
            // lblTen
            //
            Label lblTen = new System.Windows.Forms.Label { Text = "Tên KH:", TextAlign = System.Drawing.ContentAlignment.MiddleRight, Dock = System.Windows.Forms.DockStyle.Fill, AutoSize = true };
            this.tblMain.Controls.Add(lblTen, 0, 0);
            this.tblMain.Controls.Add(txtTen, 1, 0);
            //
            // lblSDT
            //
            Label lblSDT = new System.Windows.Forms.Label { Text = "Số điện thoại:", TextAlign = System.Drawing.ContentAlignment.MiddleRight, Dock = System.Windows.Forms.DockStyle.Fill, AutoSize = true };
            this.tblMain.Controls.Add(lblSDT, 0, 1);
            this.tblMain.Controls.Add(txtSDT, 1, 1);
            //
            // lblEmail
            //
            Label lblEmail = new System.Windows.Forms.Label { Text = "Email:", TextAlign = System.Drawing.ContentAlignment.MiddleRight, Dock = System.Windows.Forms.DockStyle.Fill, AutoSize = true };
            this.tblMain.Controls.Add(lblEmail, 0, 2);
            this.tblMain.Controls.Add(txtEmail, 1, 2);
            // 
            // pnlBtn
            // 
            pnlBtn.BackColor = Color.White;
            pnlBtn.Controls.Add(btnLuu);
            pnlBtn.Dock = DockStyle.Bottom;
            pnlBtn.Location = new Point(0, 160);
            pnlBtn.Name = "pnlBtn";
            pnlBtn.Padding = new Padding(20, 10, 20, 10);
            pnlBtn.Size = new Size(500, 70);
            pnlBtn.TabIndex = 1;
            // 
            // frmAddEditKH
            // 
            ClientSize = new Size(500, 230);
            Controls.Add(tblMain);
            Controls.Add(pnlBtn);
            Name = "frmAddEditKH";
            StartPosition = FormStartPosition.CenterParent;
            tblMain.ResumeLayout(false);
            tblMain.PerformLayout();
            pnlBtn.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}