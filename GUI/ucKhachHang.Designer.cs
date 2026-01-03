namespace GUI
{
    partial class ucKhachHang
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.DataGridView dgvKhachHang;
        private System.Windows.Forms.Button btnThem, btnSua, btnXoa, btnTim;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.dgvKhachHang = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnTim = new System.Windows.Forms.Button();

            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            //
            // StyleButton
            //
            UIHelper.StyleButton(btnThem, true); btnThem.Text = "Thêm KH"; btnThem.Size = new System.Drawing.Size(120, 35); btnThem.Location = new System.Drawing.Point(15, 12);
            UIHelper.StyleButton(btnSua, false); btnSua.Text = "Sửa KH"; btnSua.Size = new System.Drawing.Size(100, 35); btnSua.Location = new System.Drawing.Point(145, 12);
            UIHelper.StyleButton(btnXoa, false); btnXoa.Text = "Xóa KH"; btnXoa.Size = new System.Drawing.Size(100, 35); btnXoa.Location = new System.Drawing.Point(255, 12);
            UIHelper.StyleButton(btnTim, false); btnTim.Text = "Tìm Kiếm"; btnTim.Size = new System.Drawing.Size(120, 35); btnTim.Location = new System.Drawing.Point(365, 12);

            this.pnlHeader.Controls.AddRange(new System.Windows.Forms.Control[] { btnThem, btnSua, btnXoa, btnTim });
            //
            // dgvKhachHang
            //
            UIHelper.StyleDataGridView(dgvKhachHang);
            this.dgvKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;

            this.Controls.Add(this.dgvKhachHang);
            this.Controls.Add(this.pnlHeader);
            this.Size = new System.Drawing.Size(900, 600);
            this.ResumeLayout(false);
        }
    }
}