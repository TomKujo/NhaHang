using System.Windows.Forms.DataVisualization;
    
namespace GUI
{
    partial class ucDoanhThu
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNam;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.Button btnXem;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabLoiNhuan;
        private System.Windows.Forms.TabPage tabDoanhThu;
        private System.Windows.Forms.TabPage tabChiPhi;

        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoiNhuan;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartChiPhi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblNam = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.btnXem = new System.Windows.Forms.Button();

            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLoiNhuan = new System.Windows.Forms.TabPage();
            this.tabDoanhThu = new System.Windows.Forms.TabPage();
            this.tabChiPhi = new System.Windows.Forms.TabPage();

            this.chartLoiNhuan = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDoanhThu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartChiPhi = new System.Windows.Forms.DataVisualization.Charting.Chart();

            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.BackColor = System.Drawing.Color.WhiteSmoke;

            this.lblTitle.Text = "BÁO CÁO TÀI CHÍNH";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);

            this.lblNam.Text = "Chọn năm:";
            this.lblNam.Location = new System.Drawing.Point(300, 22);
            this.lblNam.AutoSize = true;

            this.numYear.Location = new System.Drawing.Point(380, 20);
            this.numYear.Maximum = 2050;
            this.numYear.Minimum = 2020;
            this.numYear.Value = System.DateTime.Now.Year;
            this.numYear.Width = 80;
            //
            // btnXem
            //
            UIHelper.StyleButton(this.btnXem, true);
            this.btnXem.Text = "Xem Báo Cáo";
            this.btnXem.Width = 120;
            this.btnXem.Location = new System.Drawing.Point(480, 15);
            this.btnXem.Click += new System.EventHandler(this.btnXem_Click);

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblNam);
            this.pnlHeader.Controls.Add(this.numYear);
            this.pnlHeader.Controls.Add(this.btnXem);
            //
            // tabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Controls.Add(this.tabLoiNhuan);
            this.tabControl.Controls.Add(this.tabDoanhThu);
            this.tabControl.Controls.Add(this.tabChiPhi);
            //
            // tabLoiNhuan
            //
            this.tabLoiNhuan.Text = "Lợi Nhuận";
            this.tabLoiNhuan.Controls.Add(this.chartLoiNhuan);
            //
            // tabDoanhThu
            //
            this.tabDoanhThu.Text = "Doanh Thu";
            this.tabDoanhThu.Controls.Add(this.chartDoanhThu);
            //
            // tabChiPhi
            //
            this.tabChiPhi.Text = "Chi Phí";
            this.tabChiPhi.Controls.Add(this.chartChiPhi);
            //
            // SetupChart
            //
            SetupChart(this.chartLoiNhuan, "Biểu đồ Lợi Nhuận (VNĐ)", System.Drawing.Color.Green);
            SetupChart(this.chartDoanhThu, "Biểu đồ Doanh Thu (VNĐ)", System.Drawing.Color.Blue);
            SetupChart(this.chartChiPhi, "Biểu đồ Chi Phí (VNĐ)", System.Drawing.Color.Red);

            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlHeader);
            this.Size = new System.Drawing.Size(900, 600);
            this.BackColor = System.Drawing.Color.White;
        }

        private void SetupChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string titleStr, System.Drawing.Color color)
        {
            chart.Dock = System.Windows.Forms.DockStyle.Fill;

            var area = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            area.AxisX.LabelStyle.Enabled = false;
            area.AxisX.Title = "Tháng";
            area.AxisX.Interval = 1;
            area.AxisY.LabelStyle.Format = "{0:N0}";
            chart.ChartAreas.Add(area);

            var title = new System.Windows.Forms.DataVisualization.Charting.Title();
            title.Text = titleStr;
            title.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            chart.Titles.Add(title);

            var legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            chart.Legends.Add(legend);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series();
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "{0:N0}";
            series.Color = color;
            series.Name = "Giá trị";
            chart.Series.Add(series);
        }
    }
}