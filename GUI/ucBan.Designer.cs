namespace GUI
{
    partial class ucBan
    {
        private System.ComponentModel.IContainer components = null;

        public System.Windows.Forms.FlowLayoutPanel flpBan;
        public System.Windows.Forms.Label lblTitleBan;
        public System.Windows.Forms.Label lblTongTien;

        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Button btnAdd;

        public System.Windows.Forms.DataGridView dgvOrder;
        public System.Windows.Forms.Button btnGoiMon;
        public System.Windows.Forms.Button btnThanhToan;

        public System.Windows.Forms.DataGridView dgvBookingList;
        public System.Windows.Forms.Button btnBookNew;
        public System.Windows.Forms.Button btnEditBooking;
        public System.Windows.Forms.Button btnCancelBooking;

        public System.Windows.Forms.TextBox txtEditMaBan;
        public System.Windows.Forms.TextBox txtEditTenBan;
        public System.Windows.Forms.ComboBox cboEditLoaiBan;
        public System.Windows.Forms.Button btnSaveBan;
        public System.Windows.Forms.Button btnDeleteBan;

        public System.Windows.Forms.Panel pnlRight;
        public System.Windows.Forms.TabControl tabRight;
        public System.Windows.Forms.TabPage tabInfo;
        public System.Windows.Forms.Timer timerUpdate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = UIHelper.SecondaryColor;

            System.Windows.Forms.TableLayoutPanel layout = new System.Windows.Forms.TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Fill, ColumnCount = 2 };
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            System.Windows.Forms.Panel pnlLeft = UIHelper.CreatePanel(System.Windows.Forms.DockStyle.Fill, 0, new System.Windows.Forms.Padding(5));
            System.Windows.Forms.Panel pnlTool = UIHelper.CreatePanel(System.Windows.Forms.DockStyle.Top, 45);

            btnAdd = UIHelper.CreateButton("Thêm Bàn", 100, UIHelper.PrimaryColor);
            btnAdd.Location = new System.Drawing.Point(0, 0);

            btnSearch = UIHelper.CreateButton("Tìm Kiếm", 100, System.Drawing.Color.White, false);
            btnSearch.Location = new System.Drawing.Point(110, 0);

            pnlTool.Controls.AddRange(new System.Windows.Forms.Control[] { btnAdd, btnSearch });

            flpBan = new System.Windows.Forms.FlowLayoutPanel { Dock = System.Windows.Forms.DockStyle.Fill, AutoScroll = true, BackColor = System.Drawing.Color.White };
            pnlLeft.Controls.AddRange(new System.Windows.Forms.Control[] { flpBan, pnlTool });

            pnlRight = UIHelper.CreatePanel(System.Windows.Forms.DockStyle.Fill, 0, new System.Windows.Forms.Padding(5));
            pnlRight.Visible = false;

            lblTitleBan = UIHelper.CreateLabel("", System.Windows.Forms.DockStyle.Top, new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold), null, System.Drawing.ContentAlignment.MiddleCenter);
            lblTitleBan.Height = 40; lblTitleBan.BackColor = System.Drawing.Color.AliceBlue;

            tabRight = new System.Windows.Forms.TabControl { Dock = System.Windows.Forms.DockStyle.Fill };

            System.Windows.Forms.TabPage tabOrder = new System.Windows.Forms.TabPage("Gọi Món / Thanh Toán");
            dgvOrder = UIHelper.CreateDataGridView();
            dgvOrder.RowTemplate.Height = 35;
            dgvOrder.AllowUserToResizeRows = false;

            System.Windows.Forms.Panel pnlBotOrder = UIHelper.CreatePanel(System.Windows.Forms.DockStyle.Bottom, 100);
            lblTongTien = UIHelper.CreateLabel("0 VNĐ", System.Windows.Forms.DockStyle.Top, new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold), System.Drawing.Color.Red, System.Drawing.ContentAlignment.MiddleRight);

            btnGoiMon = UIHelper.CreateButton("GỌI MÓN", 100, System.Drawing.Color.DodgerBlue);
            btnGoiMon.Location = new System.Drawing.Point(10, 40);

            btnThanhToan = UIHelper.CreateButton("THANH TOÁN", 120, System.Drawing.Color.OrangeRed);
            btnThanhToan.Height = 50; btnThanhToan.Location = new System.Drawing.Point(230, 30);

            pnlBotOrder.Controls.AddRange(new System.Windows.Forms.Control[] { lblTongTien, btnGoiMon, btnThanhToan });
            tabOrder.Controls.AddRange(new System.Windows.Forms.Control[] { dgvOrder, pnlBotOrder });

            System.Windows.Forms.TabPage tabBooking = new System.Windows.Forms.TabPage("Đặt bàn");
            dgvBookingList = UIHelper.CreateDataGridView();

            System.Windows.Forms.Panel pnlTopBooking = UIHelper.CreatePanel(System.Windows.Forms.DockStyle.Top, 50);

            btnBookNew = UIHelper.CreateButton("ĐẶT MỚI", 100, System.Drawing.Color.Teal);
            btnBookNew.Location = new System.Drawing.Point(10, 5);

            btnEditBooking = UIHelper.CreateButton("SỬA LỊCH", 100, System.Drawing.Color.Goldenrod);
            btnEditBooking.Location = new System.Drawing.Point(120, 5);

            btnCancelBooking = UIHelper.CreateButton("HỦY LỊCH", 100, System.Drawing.Color.Red);
            btnCancelBooking.Location = new System.Drawing.Point(230, 5);

            pnlTopBooking.Controls.AddRange(new System.Windows.Forms.Control[] { btnBookNew, btnEditBooking, btnCancelBooking });
            tabBooking.Controls.AddRange(new System.Windows.Forms.Control[] { dgvBookingList, pnlTopBooking });

            System.Windows.Forms.TabPage tabInfo = new System.Windows.Forms.TabPage("Chi tiết bàn");
            tabInfo.BackColor = System.Drawing.Color.White;

            System.Windows.Forms.Label l0 = new System.Windows.Forms.Label { Text = "Mã bàn:", Top = 20, Left = 20 };
            txtEditMaBan = new System.Windows.Forms.TextBox { Top = 45, Left = 20, Width = 200, BackColor = System.Drawing.Color.WhiteSmoke, ReadOnly = true };

            System.Windows.Forms.Label l1 = new System.Windows.Forms.Label { Text = "Tên bàn:", Top = 80, Left = 20 };
            txtEditTenBan = new System.Windows.Forms.TextBox { Top = 105, Left = 20, Width = 200 };

            System.Windows.Forms.Label l2 = new System.Windows.Forms.Label { Text = "Loại bàn:", Top = 140, Left = 20 };
            cboEditLoaiBan = new System.Windows.Forms.ComboBox { Top = 165, Left = 20, Width = 200, DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
            cboEditLoaiBan.Items.AddRange(new object[] { "Thường", "VIP" });

            btnSaveBan = UIHelper.CreateButton("LƯU", 200, Color.SeaGreen);
            btnSaveBan.Top = 210; btnSaveBan.Left = 20;

            btnDeleteBan = UIHelper.CreateButton("XÓA", 200, UIHelper.DangerColor);
            btnDeleteBan.Top = 260;
            btnDeleteBan.Left = 20;

            tabInfo.Controls.AddRange(new System.Windows.Forms.Control[] {
                l0, txtEditMaBan, l1, txtEditTenBan, l2, cboEditLoaiBan, btnSaveBan, btnDeleteBan
            });

            tabRight.TabPages.AddRange(new System.Windows.Forms.TabPage[] { tabOrder, tabBooking, tabInfo });
            pnlRight.Controls.AddRange(new System.Windows.Forms.Control[] { tabRight, lblTitleBan });

            layout.Controls.Add(pnlLeft, 0, 0);
            layout.Controls.Add(pnlRight, 1, 0);
            this.Controls.Add(layout);

            timerUpdate = new System.Windows.Forms.Timer(this.components) { Interval = 60000 };

        }

        #endregion

    }
}