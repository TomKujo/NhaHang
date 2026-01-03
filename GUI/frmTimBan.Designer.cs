namespace GUI
{
    partial class frmTimBan
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageLoai;
        private System.Windows.Forms.TabPage pageTen;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.TableLayoutPanel tblLoai;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.ComboBox cboLoai;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.TableLayoutPanel tblTen;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageLoai = new System.Windows.Forms.TabPage();
            this.pageTen = new System.Windows.Forms.TabPage();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnTim = new System.Windows.Forms.Button();
            this.tblLoai = new System.Windows.Forms.TableLayoutPanel();
            this.tblTen = new System.Windows.Forms.TableLayoutPanel();
            this.lblLoai = new System.Windows.Forms.Label();
            this.cboLoai = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();

            this.tabMain.SuspendLayout();
            this.pageLoai.SuspendLayout();
            this.pageTen.SuspendLayout();
            this.tblLoai.SuspendLayout();
            this.tblTen.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pageLoai);
            this.tabMain.Controls.Add(this.pageTen);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(400, 180);
            this.tabMain.TabIndex = 0;
            // 
            // pageLoai
            // 
            this.pageLoai.Controls.Add(this.tblLoai);
            this.pageLoai.Location = new System.Drawing.Point(4, 22);
            this.pageLoai.Name = "pageLoai";
            this.pageLoai.Padding = new System.Windows.Forms.Padding(3);
            this.pageLoai.Size = new System.Drawing.Size(392, 154);
            this.pageLoai.Text = "Theo Loại/TT";
            this.pageLoai.BackColor = Color.White;
            //
            // tblLoai
            //
            this.tblLoai.ColumnCount = 2;
            this.tblLoai.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLoai.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tblLoai.Controls.Add(this.lblLoai, 0, 0);
            this.tblLoai.Controls.Add(this.cboLoai, 1, 0);
            this.tblLoai.Controls.Add(this.lblTrangThai, 0, 1);
            this.tblLoai.Controls.Add(this.cboTrangThai, 1, 1);
            this.tblLoai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLoai.Padding = new System.Windows.Forms.Padding(20);
            this.tblLoai.RowCount = 2;
            this.tblLoai.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLoai.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            //
            // Loai, TrangThai
            //
            this.lblLoai.Text = "Loại bàn:";
            this.lblLoai.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLoai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboLoai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoai.Items.AddRange(new object[] { "Tất cả", "Thường", "VIP" });
            this.lblTrangThai.Text = "Trạng thái:";
            this.lblTrangThai.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTrangThai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboTrangThai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Có khách", "Đã cọc" });
            // 
            // pageTen
            // 
            this.pageTen.Controls.Add(this.tblTen);
            this.pageTen.Location = new System.Drawing.Point(4, 22);
            this.pageTen.Name = "pageTen";
            this.pageTen.Padding = new System.Windows.Forms.Padding(3);
            this.pageTen.Text = "Theo Tên";
            this.pageTen.BackColor = Color.White;
            //
            // tblTen
            this.tblTen.ColumnCount = 2;
            this.tblTen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tblTen.Controls.Add(this.lblTen, 0, 0);
            this.tblTen.Controls.Add(this.txtTen, 1, 0);
            this.tblTen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTen.Padding = new System.Windows.Forms.Padding(20);
            this.tblTen.RowCount = 1;
            this.tblTen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // Ten
            //
            this.lblTen.Text = "Tên bàn:";
            this.lblTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTen.Dock = System.Windows.Forms.DockStyle.Fill;

            this.txtTen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // pnlBtn
            // 
            this.pnlBtn.Controls.Add(this.btnTim);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Height = 70;
            this.pnlBtn.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            // 
            // btnTim
            //
            this.btnTim.Text = "TÌM";
            this.btnTim.BackColor = System.Drawing.Color.SeaGreen;
            this.btnTim.ForeColor = System.Drawing.Color.White;
            this.btnTim.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTim.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // frmTimBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tìm kiếm bàn";

            this.tabMain.ResumeLayout(false);
            this.pageLoai.ResumeLayout(false);
            this.pageTen.ResumeLayout(false);
            this.tblLoai.ResumeLayout(false);
            this.tblTen.ResumeLayout(false);
            this.tblTen.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}