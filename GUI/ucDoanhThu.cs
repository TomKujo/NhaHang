using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucDoanhThu : UserControl
    {
        private ServiceBLL bll = new ServiceBLL();

        public ucDoanhThu()
        {
            InitializeComponent();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadData((int)numYear.Value);
        }

        private void LoadData(int year)
        {
            List<BaoCaoDTO> data = bll.GetReport(year);

            FillChart(chartLoiNhuan, data, x => x.Thang, y => y.LoiNhuan, "Lợi Nhuận");

            FillChart(chartDoanhThu, data, x => x.Thang, y => y.DoanhThu, "Doanh Thu");

            chartChiPhi.Series.Clear();

            Series sNhap = new Series("Phí Nhập Hàng");
            sNhap.ChartType = SeriesChartType.StackedColumn;
            sNhap.Color = System.Drawing.Color.Orange;
            sNhap.IsValueShownAsLabel = false;
            sNhap.LabelFormat = "{0:N0}";

            Series sLuong = new Series("Lương NV");
            sLuong.ChartType = SeriesChartType.StackedColumn;
            sLuong.Color = System.Drawing.Color.Red;
            sLuong.IsValueShownAsLabel = false;

            foreach (var item in data)
            {
                sNhap.Points.AddXY("T" + item.Thang, item.ChiPhiNhap);
                sLuong.Points.AddXY("T" + item.Thang, item.LuongNhanVien);
            }

            chartChiPhi.Series.Add(sNhap);
            chartChiPhi.Series.Add(sLuong);
            chartChiPhi.ChartAreas[0].RecalculateAxesScale();
        }

        private void FillChart(Chart chart, List<BaoCaoDTO> list, Func<BaoCaoDTO, object> xVal, Func<BaoCaoDTO, decimal> yVal, string seriesName)
        {
            chart.Series.Clear();
            Series s = new Series(seriesName);
            s.ChartType = SeriesChartType.Column;
            s.IsValueShownAsLabel = false;
            s.LabelFormat = "#,##0";

            if (seriesName == "Lợi Nhuận") s.Color = System.Drawing.Color.ForestGreen;
            if (seriesName == "Doanh Thu") s.Color = System.Drawing.Color.DodgerBlue;

            foreach (var item in list)
            {
                s.Points.AddXY("T" + xVal(item), yVal(item));
            }
            chart.Series.Add(s);
            chart.ChartAreas[0].RecalculateAxesScale();
        }
    }
}