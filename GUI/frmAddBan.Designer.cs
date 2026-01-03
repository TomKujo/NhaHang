namespace GUI
{
    partial class frmAddBan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo các biến control
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.TextBox txtTen;
        private System.Windows.Forms.ComboBox cboLoai;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblLoai;

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
            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.lblLoai = new System.Windows.Forms.Label();
            this.cboLoai = new System.Windows.Forms.ComboBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.tblMain.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tblMain.Controls.Add(this.lblTen, 0, 0);
            this.tblMain.Controls.Add(this.txtTen, 1, 0);
            this.tblMain.Controls.Add(this.lblLoai, 0, 1);
            this.tblMain.Controls.Add(this.cboLoai, 1, 1);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMain.Location = new System.Drawing.Point(0, 0);
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.tblMain.RowCount = 2;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMain.Size = new System.Drawing.Size(400, 120);
            this.tblMain.TabIndex = 0;
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTen.Location = new System.Drawing.Point(23, 10);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(66, 50);
            this.lblTen.TabIndex = 0;
            this.lblTen.Text = "Tên bàn:";
            this.lblTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTen
            // 
            this.txtTen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTen.Location = new System.Drawing.Point(131, 22);
            this.txtTen.Name = "txtTen";
            this.txtTen.Size = new System.Drawing.Size(280, 25);
            this.txtTen.TabIndex = 1;
            // 
            // lblLoai
            // 
            this.lblLoai.AutoSize = true;
            this.lblLoai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoai.Location = new System.Drawing.Point(23, 60);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(102, 50);
            this.lblLoai.TabIndex = 2;
            this.lblLoai.Text = "Loại bàn:";
            this.lblLoai.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboLoai
            // 
            this.cboLoai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLoai.FormattingEnabled = true;
            this.cboLoai.Items.AddRange(new object[] {
            "Thường",
            "VIP"});
            this.cboLoai.Location = new System.Drawing.Point(131, 72);
            this.cboLoai.Name = "cboLoai";
            this.cboLoai.Size = new System.Drawing.Size(280, 25);
            this.cboLoai.TabIndex = 3;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnLuu);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 130);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.Size = new System.Drawing.Size(400, 70);
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
            this.btnLuu.Size = new System.Drawing.Size(360, 50);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.UseVisualStyleBackColor = false;
            // 
            // frmAddEditBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.tblMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddEditBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAddEditBan";
            this.tblMain.ResumeLayout(false);
            this.tblMain.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    }
}