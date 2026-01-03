using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUI
{
    partial class frmTimNV
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTen = new System.Windows.Forms.TabPage();
            this.btnTimTen = new System.Windows.Forms.Button();
            this.txtTuKhoa = new System.Windows.Forms.TextBox();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.tabTrangThai = new System.Windows.Forms.TabPage();
            this.btnTimStatus = new System.Windows.Forms.Button();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabTen.SuspendLayout();
            this.tabTrangThai.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTen);
            this.tabControl1.Controls.Add(this.tabTrangThai);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 180);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTen
            // 
            this.tabTen.Controls.Add(this.btnTimTen);
            this.tabTen.Controls.Add(this.txtTuKhoa);
            this.tabTen.Controls.Add(this.lblInstruction);
            this.tabTen.Location = new System.Drawing.Point(4, 22);
            this.tabTen.Name = "tabTen";
            this.tabTen.Padding = new System.Windows.Forms.Padding(3);
            this.tabTen.Size = new System.Drawing.Size(392, 154);
            this.tabTen.TabIndex = 0;
            this.tabTen.Text = "Tên";
            this.tabTen.BackColor = Color.White;
            // 
            // pnlBtnTen
            // 
            Panel pnlBtnTen = new Panel
            {
                Height = 70,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.White
            };
            // 
            // btnTimTen
            // 
            btnTimTen.Dock = DockStyle.Fill;
            btnTimTen.Text = "TÌM";
            btnTimTen.BackColor = Color.SeaGreen;
            btnTimTen.ForeColor = Color.WhiteSmoke;
            btnTimTen.FlatStyle = FlatStyle.Flat;
            btnTimTen.FlatAppearance.BorderSize = 0;
            btnTimTen.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            pnlBtnTen.Controls.Add(btnTimTen);
            tabTen.Controls.Add(pnlBtnTen);
            this.btnTimTen.Click += new System.EventHandler(this.btnTimTen_Click);
            // 
            // txtTuKhoa
            // 
            this.txtTuKhoa.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTuKhoa.Location = new System.Drawing.Point(30, 60);
            this.txtTuKhoa.Name = "txtTuKhoa";
            this.txtTuKhoa.Size = new System.Drawing.Size(380, 32);
            this.txtTuKhoa.TabIndex = 1;
            this.txtTuKhoa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTuKhoa_KeyDown);
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInstruction.Location = new System.Drawing.Point(30, 30);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(192, 19);
            this.lblInstruction.TabIndex = 0;
            this.lblInstruction.Text = "Tên nhân viên:";
            // 
            // tabTrangThai
            // 
            this.tabTrangThai.Controls.Add(this.btnTimStatus);
            this.tabTrangThai.Controls.Add(this.cboStatus);
            this.tabTrangThai.Controls.Add(this.lblStatus);
            this.tabTrangThai.Location = new System.Drawing.Point(4, 22);
            this.tabTrangThai.Name = "tabTrangThai";
            this.tabTrangThai.Padding = new System.Windows.Forms.Padding(3);
            this.tabTrangThai.Size = new System.Drawing.Size(392, 154);
            this.tabTrangThai.TabIndex = 1;
            this.tabTrangThai.Text = "Trạng Thái";
            this.tabTrangThai.BackColor = Color.White;
            // 
            // pnlBtnStatus
            // 
            Panel pnlBtnStatus = new Panel
            {
                Height = 70,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.White
            };

            // 
            // btnTimStatus
            // 
            btnTimStatus.Dock = DockStyle.Fill;
            btnTimStatus.Text = "TÌM";
            btnTimStatus.BackColor = Color.SeaGreen;
            btnTimStatus.ForeColor = Color.WhiteSmoke;
            btnTimStatus.FlatStyle = FlatStyle.Flat;
            btnTimStatus.FlatAppearance.BorderSize = 0;
            btnTimStatus.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            this.btnTimStatus.Click += new System.EventHandler(this.btnTimStatus_Click);
            pnlBtnStatus.Controls.Add(btnTimStatus);
            tabTrangThai.Controls.Add(pnlBtnStatus);
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] { "Kích hoạt", "Khóa" });
            this.cboStatus.Location = new System.Drawing.Point(30, 60);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(380, 33);
            this.cboStatus.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.Location = new System.Drawing.Point(30, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Trạng thái TK:";
            // 
            // frmTim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 220);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tìm kiếm nhân viên";
            this.tabControl1.ResumeLayout(false);
            this.tabTen.ResumeLayout(false);
            this.tabTen.PerformLayout();
            this.tabTrangThai.ResumeLayout(false);
            this.tabTrangThai.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTen;
        private System.Windows.Forms.TabPage tabTrangThai;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.TextBox txtTuKhoa;
        private System.Windows.Forms.Button btnTimTen;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnTimStatus;
    }
}