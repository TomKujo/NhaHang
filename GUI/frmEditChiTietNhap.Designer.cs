namespace GUI
{
    partial class frmEditChiTietNhap
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
            this.lblYC = new System.Windows.Forms.Label();
            this.txtYC = new System.Windows.Forms.TextBox();
            this.lblTT = new System.Windows.Forms.Label();
            this.cboTT = new System.Windows.Forms.ComboBox();
            this.lblGia = new System.Windows.Forms.Label();
            this.txtGia = new System.Windows.Forms.TextBox();
            this.lblTT_SL = new System.Windows.Forms.Label();
            this.txtTT_SL = new System.Windows.Forms.TextBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblYC
            // 
            this.lblYC.AutoSize = true;
            this.lblYC.Location = new System.Drawing.Point(20, 35);
            this.lblYC.Name = "lblYC";
            this.lblYC.Size = new System.Drawing.Size(83, 13);
            this.lblYC.TabIndex = 0;
            this.lblYC.Text = "Lượng yêu cầu:";
            // 
            // txtYC
            // 
            this.txtYC.Location = new System.Drawing.Point(130, 30);
            this.txtYC.Name = "txtYC";
            this.txtYC.Size = new System.Drawing.Size(150, 20);
            this.txtYC.TabIndex = 1;
            this.txtYC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtYC.TextChanged += new System.EventHandler(this.TxtYC_TextChanged);
            // 
            // lblTT
            // 
            this.lblTT.AutoSize = true;
            this.lblTT.Location = new System.Drawing.Point(20, 80);
            this.lblTT.Name = "lblTT";
            this.lblTT.Size = new System.Drawing.Size(58, 13);
            this.lblTT.TabIndex = 2;
            this.lblTT.Text = "Tình trạng:";
            // 
            // cboTT
            // 
            this.cboTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTT.FormattingEnabled = true;
            this.cboTT.Items.AddRange(new object[] {
            "Đủ",
            "Thiếu"});
            this.cboTT.Location = new System.Drawing.Point(130, 75);
            this.cboTT.Name = "cboTT";
            this.cboTT.Size = new System.Drawing.Size(150, 21);
            this.cboTT.TabIndex = 3;
            this.cboTT.SelectedIndexChanged += new System.EventHandler(this.CboTT_SelectedIndexChanged);
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.Location = new System.Drawing.Point(20, 125);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(47, 13);
            this.lblGia.TabIndex = 4;
            this.lblGia.Text = "Đơn giá:";
            // 
            // txtGia
            // 
            this.txtGia.Location = new System.Drawing.Point(130, 120);
            this.txtGia.Name = "txtGia";
            this.txtGia.Size = new System.Drawing.Size(150, 20);
            this.txtGia.TabIndex = 5;
            this.txtGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTT_SL
            // 
            this.lblTT_SL.AutoSize = true;
            this.lblTT_SL.Location = new System.Drawing.Point(20, 170);
            this.lblTT_SL.Name = "lblTT_SL";
            this.lblTT_SL.Size = new System.Drawing.Size(79, 13);
            this.lblTT_SL.TabIndex = 6;
            this.lblTT_SL.Text = "Lượng thực tế:";
            // 
            // txtTT_SL
            // 
            this.txtTT_SL.Enabled = false;
            this.txtTT_SL.Location = new System.Drawing.Point(130, 165);
            this.txtTT_SL.Name = "txtTT_SL";
            this.txtTT_SL.Size = new System.Drawing.Size(150, 20);
            this.txtTT_SL.TabIndex = 7;
            this.txtTT_SL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnSave);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 211);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.Size = new System.Drawing.Size(334, 70);
            this.pnlBtn.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.SeaGreen;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(294, 50);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "LƯU";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // frmEditChiTietNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 281);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.txtTT_SL);
            this.Controls.Add(this.lblTT_SL);
            this.Controls.Add(this.txtGia);
            this.Controls.Add(this.lblGia);
            this.Controls.Add(this.cboTT);
            this.Controls.Add(this.lblTT);
            this.Controls.Add(this.txtYC);
            this.Controls.Add(this.lblYC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditChiTietNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi Tiết Nhập";
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblYC;
        private System.Windows.Forms.TextBox txtYC;
        private System.Windows.Forms.Label lblTT;
        private System.Windows.Forms.ComboBox cboTT;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.TextBox txtGia;
        private System.Windows.Forms.Label lblTT_SL;
        private System.Windows.Forms.TextBox txtTT_SL;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Button btnSave;
    }
}