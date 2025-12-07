namespace GUI
{
    partial class ucNhanSu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvNV;
        // Các controls đã được chuyển sang Code-Behind hoặc bị xóa
        // ... (xóa khai báo btnUnlock)

        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnAdd;
        // Xóa: private System.Windows.Forms.Button btnUnlock; 

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

            // Buttons
            this.btnSua = UIHelper.CreateButton("Sửa NV", 100, System.Drawing.Color.Blue, isPrimary: true);
            this.btnXoa = UIHelper.CreateButton("Xóa NV", 100, System.Drawing.Color.Red, isPrimary: true);
            this.btnAdd = UIHelper.CreateButton("+ Thêm NV", 100, UIHelper.PrimaryColor);
            this.btnSearch = UIHelper.CreateButton("🔍 Tìm kiếm", 100, System.Drawing.Color.Gray, isPrimary: true); // Giữ lại nút Search

            this.SuspendLayout();

            // Top Panel (Chỉ khai báo các button)
            this.btnAdd.Location = new System.Drawing.Point(20, 15);
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);

            this.btnSua.Location = new System.Drawing.Point(130, 15);
            this.btnSua.Click += new System.EventHandler(this.BtnSua_Click);

            this.btnXoa.Location = new System.Drawing.Point(240, 15);
            this.btnXoa.Click += new System.EventHandler(this.BtnXoa_Click);

            this.btnSearch.Location = new System.Drawing.Point(350, 15);
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
            this.Controls.Add(CreateTopPanel());
            this.Name = "ucNhanSu";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
        }

        #endregion
    }
}