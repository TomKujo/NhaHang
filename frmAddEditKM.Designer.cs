using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class frmAddEditKM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo Controls
        private Label lblMaKM;
        private TextBox txtMaKM;
        private Label lblTenKM;
        private TextBox txtTenKM;
        private Label lblMoTa;
        private TextBox txtMoTa;
        private Label lblDiemCanThiet;
        private NumericUpDown txtDiemCanThiet;
        private Label lblGiaTriGiam;
        private NumericUpDown txtGiaTriGiam;
        private Label lblLoaiGiam;
        private ComboBox cboLoaiGiam;
        private Label lblNgayKetThuc;
        private DateTimePicker dtpNgayKetThuc;
        private CheckBox chkKhongHetHan;
        private Button btnSave;
        private TableLayoutPanel tblMain;
        private Label lblTrangThai;
        private ComboBox cboTrangThai;


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
            this.lblMaKM = new System.Windows.Forms.Label();
            this.txtMaKM = new System.Windows.Forms.TextBox();
            this.lblTenKM = new System.Windows.Forms.Label();
            this.txtTenKM = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblDiemCanThiet = new System.Windows.Forms.Label();
            this.txtDiemCanThiet = new System.Windows.Forms.NumericUpDown();
            this.lblGiaTriGiam = new System.Windows.Forms.Label();
            this.txtGiaTriGiam = new System.Windows.Forms.NumericUpDown();
            this.lblLoaiGiam = new System.Windows.Forms.Label();
            this.cboLoaiGiam = new System.Windows.Forms.ComboBox();
            this.lblNgayKetThuc = new System.Windows.Forms.Label();
            this.dtpNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.chkKhongHetHan = new System.Windows.Forms.CheckBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.btnSave = UIHelper.CreateButton("LƯU", 120, Color.Teal, true);

            // 
            // frmAddEditKM
            // 
            this.ClientSize = new System.Drawing.Size(400, 550); // Chiều cao tăng lên để chứa hết controls
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditKM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm/Sửa Khuyến Mãi";

            // 
            // tblMain
            //
            this.tblMain.ColumnCount = 2;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMain.Location = new System.Drawing.Point(10, 10);
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new Padding(10);
            this.tblMain.RowCount = 9;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F)); // MoTa
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblMain.Size = new System.Drawing.Size(400, 480);
            this.tblMain.TabIndex = 0;
            this.tblMain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // --- Helper để căn lề giữa ---
            void AddControl(Control c, int col, int row, ContentAlignment? alignment = null)
            {
                this.tblMain.Controls.Add(c, col, row);
                c.Margin = new Padding(3, 8, 3, 3);
                if (c is Label lbl && alignment.HasValue) lbl.TextAlign = alignment.Value;
                else if (c is ComboBox || c is TextBox || c is NumericUpDown || c is DateTimePicker || c is CheckBox) c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }

            // --- 1. Mã KM ---
            this.lblMaKM.Text = "Mã KM:"; this.txtMaKM.ReadOnly = true; this.txtMaKM.BackColor = Color.WhiteSmoke;
            AddControl(this.lblMaKM, 0, 0, ContentAlignment.MiddleRight); AddControl(this.txtMaKM, 1, 0);

            // --- 2. Tên KM ---
            this.lblTenKM.Text = "Tên Khuyến Mãi:";
            AddControl(this.lblTenKM, 0, 1, ContentAlignment.MiddleRight); AddControl(this.txtTenKM, 1, 1);

            // --- 3. Mô Tả ---
            this.lblMoTa.Text = "Mô Tả:"; this.txtMoTa.Multiline = true; this.txtMoTa.Height = 70;
            AddControl(this.lblMoTa, 0, 2, ContentAlignment.MiddleRight); AddControl(this.txtMoTa, 1, 2);

            // --- 4. Điểm Yêu Cầu ---
            this.lblDiemCanThiet.Text = "Điểm Yêu Cầu:"; this.txtDiemCanThiet.Minimum = 1; this.txtDiemCanThiet.Maximum = 100000;
            AddControl(this.lblDiemCanThiet, 0, 3, ContentAlignment.MiddleRight); AddControl(this.txtDiemCanThiet, 1, 3);

            // --- 5. Giá Trị Giảm ---
            this.lblGiaTriGiam.Text = "Giá Trị Giảm:"; this.txtGiaTriGiam.Minimum = 1; this.txtGiaTriGiam.Maximum = 100000000;
            AddControl(this.lblGiaTriGiam, 0, 4, ContentAlignment.MiddleRight); AddControl(this.txtGiaTriGiam, 1, 4);

            // --- 6. Loại Giảm ---
            this.lblLoaiGiam.Text = "Loại Giảm:";
            AddControl(this.lblLoaiGiam, 0, 5, ContentAlignment.MiddleRight); AddControl(this.cboLoaiGiam, 1, 5);

            // --- 7. Ngày Kết Thúc ---
            this.lblNgayKetThuc.Text = "Ngày Kết Thúc:";
            AddControl(this.lblNgayKetThuc, 0, 6, ContentAlignment.MiddleRight);

            // Panel chứa DatePicker và CheckBox
            Panel pnlDate = new Panel { Dock = DockStyle.Fill, AutoSize = true };
            this.dtpNgayKetThuc.Location = new Point(0, 0);
            this.dtpNgayKetThuc.Width = 150;
            this.dtpNgayKetThuc.Format = DateTimePickerFormat.Short;

            this.chkKhongHetHan.Text = "Không hết hạn";
            this.chkKhongHetHan.Location = new Point(160, 0);
            this.chkKhongHetHan.AutoSize = true;

            pnlDate.Controls.AddRange(new Control[] { dtpNgayKetThuc, chkKhongHetHan });
            AddControl(pnlDate, 1, 6);

            // --- 8. Trạng Thái ---
            this.lblTrangThai.Text = "Trạng Thái:";
            this.cboTrangThai.Items.AddRange(new object[] { "Hoạt động", "Ngừng áp dụng" });
            AddControl(this.lblTrangThai, 0, 7, ContentAlignment.MiddleRight); AddControl(this.cboTrangThai, 1, 7);

            // 
            // btnSave
            // 
            this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(260, 500);
            this.btnSave.Size = new System.Drawing.Size(120, 35);
        }
        #endregion
    }
}