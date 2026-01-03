namespace GUI
{
    partial class frmAddEditNV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlButtonContainer;

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
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblLuong = new System.Windows.Forms.Label();
            this.txtLuong = new System.Windows.Forms.TextBox();
            this.lblVaiTro = new System.Windows.Forms.Label();
            this.cmbVaiTro = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cmbTrangThai = new System.Windows.Forms.ComboBox();
            this.pnlButtonContainer = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.pnlButtonContainer.SuspendLayout();
            this.SuspendLayout();

            System.Drawing.Font fontLbl = new System.Drawing.Font("Segoe UI", 10);
            System.Drawing.Font fontInput = new System.Drawing.Font("Segoe UI", 11);
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = fontLbl;
            this.lblHoTen.Location = new System.Drawing.Point(20, 23);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(105, 19);
            this.lblHoTen.TabIndex = 0;
            this.lblHoTen.Text = "Họ tên NV:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Font = fontInput;
            this.txtHoTen.Location = new System.Drawing.Point(160, 20);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(250, 27);
            this.txtHoTen.TabIndex = 1;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = fontLbl;
            this.lblSDT.Location = new System.Drawing.Point(20, 68);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(123, 19);
            this.lblSDT.TabIndex = 2;
            this.lblSDT.Text = "SĐT:";
            // 
            // txtSDT
            // 
            this.txtSDT.Font = fontInput;
            this.txtSDT.Location = new System.Drawing.Point(160, 65);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(250, 27);
            this.txtSDT.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = fontLbl;
            this.lblEmail.Location = new System.Drawing.Point(20, 113);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(71, 19);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = fontInput;
            this.txtEmail.Location = new System.Drawing.Point(160, 110);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(250, 27);
            this.txtEmail.TabIndex = 5;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = fontLbl;
            this.lblDiaChi.Location = new System.Drawing.Point(20, 158);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(60, 19);
            this.lblDiaChi.TabIndex = 6;
            this.lblDiaChi.Text = "Địa chỉ:";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Font = fontInput;
            this.txtDiaChi.Location = new System.Drawing.Point(160, 155);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(250, 27);
            this.txtDiaChi.TabIndex = 7;
            // 
            // lblLuong
            // 
            this.lblLuong.AutoSize = true;
            this.lblLuong.Font = fontLbl;
            this.lblLuong.Location = new System.Drawing.Point(20, 203);
            this.lblLuong.Name = "lblLuong";
            this.lblLuong.Size = new System.Drawing.Size(99, 19);
            this.lblLuong.TabIndex = 8;
            this.lblLuong.Text = "Lương (VNĐ):";
            // 
            // txtLuong
            // 
            this.txtLuong.Font = fontInput;
            this.txtLuong.Location = new System.Drawing.Point(160, 200);
            this.txtLuong.Name = "txtLuong";
            this.txtLuong.Size = new System.Drawing.Size(250, 27);
            this.txtLuong.TabIndex = 9;
            this.txtLuong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLuong_KeyPress);
            this.txtLuong.Leave += new System.EventHandler(this.txtLuong_Leave);
            // 
            // lblVaiTro
            // 
            this.lblVaiTro.AutoSize = true;
            this.lblVaiTro.Font = fontLbl;
            this.lblVaiTro.Location = new System.Drawing.Point(20, 248);
            this.lblVaiTro.Name = "lblVaiTro";
            this.lblVaiTro.Size = new System.Drawing.Size(59, 19);
            this.lblVaiTro.TabIndex = 10;
            this.lblVaiTro.Text = "Vai trò:";
            // 
            // cmbVaiTro
            // 
            this.cmbVaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVaiTro.Font = fontInput;
            this.cmbVaiTro.FormattingEnabled = true;
            this.cmbVaiTro.Location = new System.Drawing.Point(160, 245);
            this.cmbVaiTro.Name = "cmbVaiTro";
            this.cmbVaiTro.Size = new System.Drawing.Size(250, 28);
            this.cmbVaiTro.TabIndex = 11;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = fontLbl;
            this.lblTrangThai.Location = new System.Drawing.Point(20, 293);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(81, 19);
            this.lblTrangThai.TabIndex = 12;
            this.lblTrangThai.Text = "Trạng thái:";
            // 
            // cmbTrangThai
            // 
            this.cmbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrangThai.Font = fontInput;
            this.cmbTrangThai.FormattingEnabled = true;
            this.cmbTrangThai.Location = new System.Drawing.Point(160, 290);
            this.cmbTrangThai.Name = "cmbTrangThai";
            this.cmbTrangThai.Size = new System.Drawing.Size(250, 28);
            this.cmbTrangThai.TabIndex = 13;
            // 
            // pnlBtn
            // 
            Panel pnlBtn = new Panel
            {
                Height = 70,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.White
            };
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.SeaGreen;
            this.btnLuu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(20, 10);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(394, 50);
            this.btnLuu.TabIndex = 15;
            this.btnLuu.Text = "LƯU";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.BtnLuu_Click);
            // 
            // pnlButtonContainer
            // 
            this.pnlButtonContainer.BackColor = System.Drawing.Color.White;
            this.pnlButtonContainer.Controls.Add(this.btnLuu);
            this.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonContainer.Location = new System.Drawing.Point(0, 441);
            this.pnlButtonContainer.Name = "pnlButtonContainer";
            this.pnlButtonContainer.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlButtonContainer.Size = new System.Drawing.Size(434, 70);
            this.pnlButtonContainer.TabIndex = 20;
            // 
            // frmAddEditNV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 550);
            this.Controls.Add(this.pnlButtonContainer);
            this.Controls.Add(this.cmbTrangThai);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.cmbVaiTro);
            this.Controls.Add(this.lblVaiTro);
            this.Controls.Add(this.txtLuong);
            this.Controls.Add(this.lblLuong);
            this.Controls.Add(this.txtDiaChi);
            this.Controls.Add(this.lblDiaChi);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.lblSDT);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblHoTen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddEditNV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết nhân viên";
            this.pnlButtonContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblLuong;
        private System.Windows.Forms.TextBox txtLuong;
        private System.Windows.Forms.Label lblVaiTro;
        private System.Windows.Forms.ComboBox cmbVaiTro;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cmbTrangThai;
        private System.Windows.Forms.Button btnLuu;
    }
}