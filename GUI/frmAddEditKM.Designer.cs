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
        private Label lblTenKM;
        private TextBox txtTenKM;
        private Label lblMoTa;
        private TextBox txtMoTa;
        private Label lblDiemCan;
        private TextBox txtDiemCanThiet;
        private Label lblGiaTriGiam;
        private TextBox txtGiaTriGiam;
        private Label lblLoaiGiam;
        private ComboBox cboLoaiGiam;
        private Label lblNgayKetThuc;
        private DateTimePicker dtpNgayKetThuc;
        private CheckBox chkKhongHetHan;
        private Button btnSave;
        private TableLayoutPanel tblMain;


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
            this.lblTenKM = new System.Windows.Forms.Label();
            this.txtTenKM = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblDiemCan = new System.Windows.Forms.Label();
            this.txtDiemCanThiet = new System.Windows.Forms.TextBox();
            this.lblGiaTriGiam = new System.Windows.Forms.Label();
            this.txtGiaTriGiam = new System.Windows.Forms.TextBox();
            this.lblLoaiGiam = new System.Windows.Forms.Label();
            this.cboLoaiGiam = new System.Windows.Forms.ComboBox();
            this.lblNgayKetThuc = new System.Windows.Forms.Label();
            this.dtpNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.chkKhongHetHan = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            //
            // frmAddEditKM
            //
            this.ClientSize = new System.Drawing.Size(420, 430);
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.btnSave);
            this.Name = "frmAddEditKM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm/Sửa Khuyến Mãi";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            //
            // tblMain
            //
            this.tblMain.ColumnCount = 2;
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Padding = new Padding(15, 10, 15, 10);
            this.tblMain.RowCount = 6;
            this.tblMain.RowStyles.Clear();
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            this.lblTenKM.Text = "Tên KM:";
            AddControl(this.lblTenKM, 0, 0, ContentAlignment.MiddleRight); AddControl(this.txtTenKM, 1, 0);

            this.lblMoTa.Text = "Mô Tả:"; this.txtMoTa.Multiline = true; this.txtMoTa.Height = 90;
            AddControl(this.lblMoTa, 0, 1, ContentAlignment.MiddleRight); AddControl(this.txtMoTa, 1, 1);

            this.lblDiemCan.Text = "Điểm Cần:";
            AddControl(this.lblDiemCan, 0, 2, ContentAlignment.MiddleRight); AddControl(this.txtDiemCanThiet, 1, 2);

            this.lblGiaTriGiam.Text = "Giá Trị Giảm:";
            AddControl(this.lblGiaTriGiam, 0, 3, ContentAlignment.MiddleRight); AddControl(this.txtGiaTriGiam, 1, 3);

            this.lblLoaiGiam.Text = "Loại Giảm:";
            AddControl(this.lblLoaiGiam, 0, 4, ContentAlignment.MiddleRight); AddControl(this.cboLoaiGiam, 1, 4);

            this.lblNgayKetThuc.Text = "Hết hạn:";
            this.dtpNgayKetThuc.Format = DateTimePickerFormat.Custom;
            this.dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy HH:mm";

            Panel pnlDate = new Panel { Dock = DockStyle.Fill };
            this.dtpNgayKetThuc.Width = 160;
            this.chkKhongHetHan.Text = "Vô hạn";
            this.chkKhongHetHan.Location = new Point(170, 0);
            pnlDate.Controls.AddRange(new Control[] { dtpNgayKetThuc, chkKhongHetHan });

            AddControl(this.lblNgayKetThuc, 0, 5, ContentAlignment.MiddleRight);
            AddControl(pnlDate, 1, 5);
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
            // btnSave
            // 
            btnSave.Dock = DockStyle.Fill;
            btnSave.Text = "LƯU";
            UIHelper.StyleButton(btnSave, true);
            btnSave.BackColor = Color.SeaGreen;
            pnlBtn.Controls.Add(btnSave);
            this.Controls.Add(pnlBtn);

            void AddControl(Control c, int col, int row, ContentAlignment? alignment = null)
            {
                this.tblMain.Controls.Add(c, col, row);
                c.Margin = new Padding(3, 8, 3, 3);
                if (c is Label lbl && alignment.HasValue) lbl.TextAlign = alignment.Value;
                else if (c is ComboBox || c is TextBox || c is NumericUpDown || c is DateTimePicker || c is CheckBox) c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }
        }
        #endregion
    }
}