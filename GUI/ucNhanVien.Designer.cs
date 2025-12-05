namespace GUI
{
    partial class ucNhanSu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvNV;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblInputTitle;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnSua; // Nút Sửa
        private System.Windows.Forms.Button btnXoa; // Nút Xóa (Đã có, giữ lại)
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUnlock; // Nút Mở Khóa

        // Control cho chức năng tìm kiếm
        private System.Windows.Forms.Button btnSearch;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.dgvNV = UIHelper.CreateDataGridView();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.lblInputTitle = UIHelper.CreateLabel("THÔNG TIN NHÂN VIÊN", System.Windows.Forms.DockStyle.Top);

            // Input Controls
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.txtDiaChi = new System.Windows.Forms.TextBox();

            // Buttons
            this.btnLuu = UIHelper.CreateButton("Lưu lại", 100, UIHelper.PrimaryColor);
            this.btnSua = UIHelper.CreateButton("Sửa NV", 100, System.Drawing.Color.Blue, isPrimary: true);
            this.btnXoa = UIHelper.CreateButton("Xóa NV", 100, System.Drawing.Color.Red, isPrimary: true);
            this.btnAdd = UIHelper.CreateButton("+ Thêm NV", 100, UIHelper.PrimaryColor);
            this.btnUnlock = UIHelper.CreateButton("Mở khóa TK", 120, System.Drawing.Color.Green, isPrimary: true);
            this.btnSearch = UIHelper.CreateButton("🔍 Tìm kiếm", 100, System.Drawing.Color.Gray, isPrimary: true);


            // Cấu hình pnlInput
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblInputTitle
            //
            this.lblInputTitle.Height = 60;
            this.lblInputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInputTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblInputTitle.ForeColor = UIHelper.PrimaryColor;

            // 
            // pnlInput (Form Nhập liệu)
            // 
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlInput.Width = 300; // Chiều rộng mặc định khi hiển thị
            this.pnlInput.BackColor = System.Drawing.Color.White;
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Thêm các controls vào pnlInput theo thứ tự ngược
            this.pnlInput.Controls.Add(UIHelper.CreatePanel(System.Windows.Forms.DockStyle.None, 60, new System.Windows.Forms.Padding(20, 10, 20, 0)));
            this.pnlInput.Controls.Add(CreateInputGroup("Số điện thoại:", 180, out txtSDT));
            this.pnlInput.Controls.Add(CreateInputGroup("Địa chỉ:", 110, out txtDiaChi));
            this.pnlInput.Controls.Add(CreateInputGroup("Họ và tên:", 40, out txtTenNV));
            this.pnlInput.Controls.Add(this.lblInputTitle);

            // Giao diện Button trong pnlInput
            this.pnlInput.Controls.Add(this.btnLuu);
            this.btnLuu.Location = new System.Drawing.Point(20, 260);
            this.btnLuu.Click += new System.EventHandler(this.BtnLuu_Click);

            this.pnlInput.Controls.Add(this.btnUnlock);
            this.btnUnlock.Location = new System.Drawing.Point(140, 260);
            this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);


            // 
            // Top Panel (Không tạo trong Designer do đã dùng hàm)
            // Chỉ khai báo các button
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 15);
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);

            this.btnSua.Location = new System.Drawing.Point(190, 15);
            this.btnSua.Click += new System.EventHandler(this.BtnSua_Click);

            this.btnXoa.Location = new System.Drawing.Point(310, 15);
            this.btnXoa.Click += new System.EventHandler(this.BtnXoa_Click);

            this.btnSearch.Location = new System.Drawing.Point(430, 15);
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);


            // 
            // dgvNV
            // 
            this.dgvNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvNV_CellClick);

            // 
            // ucNhanSu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvNV);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(CreateTopPanel()); // Tái tạo Top Panel bằng code
            this.Name = "ucNhanSu";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlInput.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}