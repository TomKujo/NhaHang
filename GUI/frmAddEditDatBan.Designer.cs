namespace GUI
{
    partial class frmAddEditDatBan
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtTenKhach = new System.Windows.Forms.TextBox();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.dtpThoiGian = new System.Windows.Forms.DateTimePicker();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtSDT);
            this.pnlContent.Controls.Add(this.lblSDT);
            this.pnlContent.Controls.Add(this.txtTenKhach);
            this.pnlContent.Controls.Add(this.lblTenKhach);
            this.pnlContent.Controls.Add(this.dtpThoiGian);
            this.pnlContent.Controls.Add(this.lblThoiGian);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContent.Size = new System.Drawing.Size(434, 230);
            this.pnlContent.TabIndex = 0;
            // 
            // txtSDT
            // 
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSDT.Location = new System.Drawing.Point(23, 178);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(388, 27);
            this.txtSDT.TabIndex = 5;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSDT.Location = new System.Drawing.Point(23, 156);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(92, 19);
            this.lblSDT.TabIndex = 4;
            this.lblSDT.Text = "Số điện thoại:";
            // 
            // txtTenKhach
            // 
            this.txtTenKhach.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTenKhach.Location = new System.Drawing.Point(23, 112);
            this.txtTenKhach.Name = "txtTenKhach";
            this.txtTenKhach.Size = new System.Drawing.Size(388, 27);
            this.txtTenKhach.TabIndex = 3;
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTenKhach.Location = new System.Drawing.Point(23, 90);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(108, 19);
            this.lblTenKhach.TabIndex = 2;
            this.lblTenKhach.Text = "Tên Khách hàng:";
            // 
            // dtpThoiGian
            // 
            this.dtpThoiGian.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpThoiGian.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpThoiGian.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpThoiGian.Location = new System.Drawing.Point(23, 46);
            this.dtpThoiGian.Name = "dtpThoiGian";
            this.dtpThoiGian.Size = new System.Drawing.Size(388, 27);
            this.dtpThoiGian.TabIndex = 1;
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblThoiGian.Location = new System.Drawing.Point(23, 24);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(164, 19);
            this.lblThoiGian.TabIndex = 0;
            this.lblThoiGian.Text = "Thời gian đặt (Ngày - Giờ):";
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnSave);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 230);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlBtn.Size = new System.Drawing.Size(434, 70);
            this.pnlBtn.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(394, 50);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "LƯU";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // frmAddEditDatBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(434, 301);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditDatBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đặt bàn";
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.DateTimePicker dtpThoiGian;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Button btnSave;
    }
}